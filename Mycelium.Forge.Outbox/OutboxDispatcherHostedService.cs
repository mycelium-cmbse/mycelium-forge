// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxDispatcherHostedService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Outbox
{
    using ErrorOr;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Mycelium.Forge.Common.Extensions;
    using Mycelium.Forge.Dal.DatabaseSource;
    using Mycelium.Forge.Orm.Outbox;

    /// <summary>
    /// Hosted background service that polls the transactional outbox, dispatches claimed messages to matching
    /// <see cref="IOutboxEventHandler" /> implementations, and handles retry scheduling or failure marking.
    /// </summary>
    /// <remarks>
    /// Successfully delivered messages are deleted immediately. Messages that fail permanently (or exhaust
    /// <see cref="OutboxOptions.MaxAttempts" />) log an error and remain in <c>forge.outbox_message</c> with
    /// <see cref="OutboxMessageStatus.Failed" /> and <c>last_error</c> for manual SQL inspection or replay until
    /// dedicated admin tooling or metrics are added.
    /// </remarks>
    public class OutboxDispatcherHostedService : BackgroundService
    {
        /// <summary>
        /// The minimum jitter multiplier applied to retry delays.
        /// </summary>
        private const double JitterMinimumMultiplier = 0.8;

        /// <summary>
        /// The jitter range width added to <see cref="JitterMinimumMultiplier" />.
        /// </summary>
        private const double JitterRange = 0.4;

        /// <summary>
        /// The injected <see cref="IDatabaseSource" /> used to manage short outbox transactions.
        /// </summary>
        private readonly IDatabaseSource databaseSource;

        /// <summary>
        /// The injected <see cref="ILogger{OutboxDispatcherHostedService}" /> used for logging.
        /// </summary>
        private readonly ILogger<OutboxDispatcherHostedService> logger;

        /// <summary>
        /// The configured <see cref="OutboxOptions" /> controlling polling, batching, and retries.
        /// </summary>
        private readonly OutboxOptions options;

        /// <summary>
        /// The injected <see cref="IServiceScopeFactory" /> used to resolve scoped DAOs and handlers per polling cycle.
        /// </summary>
        private readonly IServiceScopeFactory serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutboxDispatcherHostedService" /> class.
        /// </summary>
        /// <param name="databaseSource">The injected <see cref="IDatabaseSource" />.</param>
        /// <param name="serviceScopeFactory">The injected <see cref="IServiceScopeFactory" />.</param>
        /// <param name="options">The injected <see cref="OutboxOptions" />.</param>
        /// <param name="logger">The injected <see cref="ILogger{OutboxDispatcherHostedService}" />.</param>
        public OutboxDispatcherHostedService(IDatabaseSource databaseSource, IServiceScopeFactory serviceScopeFactory, IOptions<OutboxOptions> options, ILogger<OutboxDispatcherHostedService> logger)
        {
            this.databaseSource = databaseSource;
            this.serviceScopeFactory = serviceScopeFactory;
            this.options = options.Value;
            this.logger = logger;
        }

        /// <summary>
        /// Executes the background loop that periodically claims and dispatches pending outbox messages.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="IHostedService.StopAsync(CancellationToken)" /> is called.</param>
        /// <returns>A <see cref="Task" /> representing the background execution.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!this.options.Enabled)
            {
                if (this.logger.IsEnabled(LogLevel.Information))
                {
                    this.logger.LogInformation("Outbox dispatcher is disabled.");
                }

                return;
            }

            var pollingInterval = TimeSpan.FromSeconds(this.options.PollingIntervalSeconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await this.ProcessNextBatchAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception exception)
                {
                    this.logger.LogWarning(exception, "Error occurred while processing outbox messages.");
                }

                try
                {
                    await Task.Delay(pollingInterval, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Executes the target <see cref="IOutboxEventHandler" /> while translating unexpected exceptions into errors.
        /// </summary>
        /// <param name="handler">The handler matching the outbox message event type.</param>
        /// <param name="message">The claimed <see cref="OutboxMessage" /> to process.</param>
        /// <param name="stoppingToken">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> representing the handler execution outcome.</returns>
        private static async Task<ErrorOr<Success>> ExecuteHandlerSafelyAsync(IOutboxEventHandler handler, OutboxMessage message, CancellationToken stoppingToken)
        {
            try
            {
                return await handler.HandleAsync(message, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                return Error.Failure("Outbox.HandlerException", exception.Message);
            }
        }

        /// <summary>
        /// Claims the next batch of eligible outbox messages and dispatches each message outside the claim transaction.
        /// </summary>
        /// <param name="stoppingToken">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous batch operation.</returns>
        private async Task ProcessNextBatchAsync(CancellationToken stoppingToken)
        {
            await using var scope = this.serviceScopeFactory.CreateAsyncScope();
            var outboxDao = scope.ServiceProvider.GetRequiredService<IOutboxDao>();
            var handlers = scope.ServiceProvider.GetServices<IOutboxEventHandler>().ToLookup(handler => handler.EventType, StringComparer.Ordinal);

            var batchSize = this.options.BatchSize;
            var leaseDuration = TimeSpan.FromSeconds(this.options.LeaseDurationSeconds);

            var claimResult = await this.databaseSource.ExecuteInTransactionAsync(transaction => outboxDao.ClaimBatchAsync(transaction, batchSize, leaseDuration, stoppingToken), stoppingToken);

            if (claimResult.IsError)
            {
                this.logger.LogWarning("Failed to claim outbox batch: {Errors}", claimResult.Errors.ToJoinedDescriptions().SanitizeForLog());
                return;
            }

            if (claimResult.Value.Count == 0)
            {
                return;
            }

            foreach (var message in claimResult.Value)
            {
                await this.ProcessMessageAsync(outboxDao, handlers, message, stoppingToken);
            }
        }

        /// <summary>
        /// Dispatches a single claimed outbox message to all matching handlers and updates or deletes its outbox record in a
        /// short transaction.
        /// </summary>
        /// <param name="outboxDao">The scoped <see cref="IOutboxDao" /> instance.</param>
        /// <param name="handlers">The registered <see cref="IOutboxEventHandler" /> lookup keyed by event type.</param>
        /// <param name="message">The claimed <see cref="OutboxMessage" /> to process.</param>
        /// <param name="stoppingToken">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous message processing.</returns>
        private async Task ProcessMessageAsync(IOutboxDao outboxDao, ILookup<string, IOutboxEventHandler> handlers, OutboxMessage message, CancellationToken stoppingToken)
        {
            var claimToken = message.ClaimToken ?? Guid.Empty;
            var matchingHandlers = handlers[message.EventType].ToList();

            if (matchingHandlers.Count == 0)
            {
                var missingHandlerError = $"No outbox handler registered for event type '{message.EventType}'.";
                this.logger.LogError("Outbox message {MessageId} permanently failed: {Error}", message.Id.SanitizeForLog(), missingHandlerError.SanitizeForLog());
                await this.MarkFailedInTransactionAsync(outboxDao, message.Id, claimToken, missingHandlerError, stoppingToken);
                return;
            }

            List<Error> errors = [];

            foreach (var handler in matchingHandlers)
            {
                var handleResult = await ExecuteHandlerSafelyAsync(handler, message, stoppingToken);

                if (handleResult.IsError)
                {
                    errors.AddRange(handleResult.Errors);
                }
            }

            if (errors.Count == 0)
            {
                var deleteResult = await this.databaseSource.ExecuteInTransactionAsync(transaction => outboxDao.DeleteAsync(transaction, message.Id, claimToken, stoppingToken), stoppingToken);

                if (deleteResult.IsError)
                {
                    this.LogCompletionError("delete processed", message.Id, deleteResult.Errors);
                }

                return;
            }

            var errorDescription = errors.ToJoinedDescriptions();
            var maxAttempts = this.options.MaxAttempts;

            if (OutboxErrorClassifier.IsPermanentError(errors) || message.Attempts >= maxAttempts)
            {
                this.logger.LogError("Outbox message {MessageId} ({EventType}) permanently failed on attempt {Attempts}/{MaxAttempts}: {Error}", message.Id.SanitizeForLog(), message.EventType.SanitizeForLog(), message.Attempts, maxAttempts, errorDescription.SanitizeForLog());
                await this.MarkFailedInTransactionAsync(outboxDao, message.Id, claimToken, errorDescription, stoppingToken);
                return;
            }

            var nextAttemptAt = DateTimeOffset.UtcNow.Add(this.CalculateRetryDelay(message.Attempts));
            this.logger.LogWarning("Outbox message {MessageId} ({EventType}) failed on attempt {Attempts}/{MaxAttempts}; rescheduling for {NextAttemptAt}: {Error}", message.Id.SanitizeForLog(), message.EventType.SanitizeForLog(), message.Attempts, maxAttempts, nextAttemptAt, errorDescription.SanitizeForLog());

            var rescheduleResult = await this.databaseSource.ExecuteInTransactionAsync(transaction => outboxDao.RescheduleAsync(transaction, message.Id, claimToken, nextAttemptAt, errorDescription, stoppingToken), stoppingToken);

            if (rescheduleResult.IsError)
            {
                this.LogCompletionError("reschedule", message.Id, rescheduleResult.Errors);
            }
        }

        /// <summary>
        /// Marks an outbox message as permanently failed inside a short database transaction.
        /// </summary>
        /// <param name="outboxDao">The scoped <see cref="IOutboxDao" /> instance.</param>
        /// <param name="messageId">The unique identifier of the outbox message.</param>
        /// <param name="claimToken">The fencing token assigned when the message was claimed.</param>
        /// <param name="errorDescription">The error description to persist.</param>
        /// <param name="stoppingToken">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous update operation.</returns>
        private async Task MarkFailedInTransactionAsync(IOutboxDao outboxDao, Guid messageId, Guid claimToken, string errorDescription, CancellationToken stoppingToken)
        {
            var markFailedResult = await this.databaseSource.ExecuteInTransactionAsync(transaction => outboxDao.MarkFailedAsync(transaction, messageId, claimToken, errorDescription, stoppingToken), stoppingToken);

            if (markFailedResult.IsError)
            {
                this.LogCompletionError("mark as failed", messageId, markFailedResult.Errors);
            }
        }

        /// <summary>
        /// Logs an outbox completion failure, distinguishing lost lease ownership conflicts from unexpected errors.
        /// </summary>
        /// <param name="action">The completion action being attempted.</param>
        /// <param name="messageId">The unique identifier of the outbox message.</param>
        /// <param name="errors">The errors returned by the persistence operation.</param>
        private void LogCompletionError(string action, Guid messageId, List<Error> errors)
        {
            if (errors.Exists(error => error.Type == ErrorType.Conflict))
            {
                this.logger.LogWarning("Lost lease ownership for outbox message {MessageId} before completing {Action}; skipping update", messageId.SanitizeForLog(), action);
                return;
            }

            this.logger.LogWarning("Failed to {Action} outbox message {MessageId}: {Errors}", action, messageId.SanitizeForLog(), errors.ToJoinedDescriptions().SanitizeForLog());
        }

        /// <summary>
        /// Computes an exponential backoff delay with jitter for the given attempt count.
        /// </summary>
        /// <param name="attempts">The number of attempts completed so far.</param>
        /// <returns>The delay before the next retry attempt.</returns>
        private TimeSpan CalculateRetryDelay(int attempts)
        {
            var cappedSeconds = Math.Min(this.options.MaxRetryDelaySeconds, this.options.BaseRetryDelaySeconds * Math.Pow(2, attempts - 1));
            var jitterMultiplier = JitterMinimumMultiplier + Random.Shared.NextDouble() * JitterRange;

            return TimeSpan.FromSeconds(cappedSeconds * jitterMultiplier);
        }
    }
}
