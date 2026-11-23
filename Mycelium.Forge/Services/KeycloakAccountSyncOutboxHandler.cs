// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakAccountSyncOutboxHandler.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using System.Text.Json;

    using ErrorOr;

    using Microsoft.Extensions.Options;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Common.Extensions;
    using Mycelium.Forge.Dal.Services;
    using Mycelium.Forge.Orm.Outbox;

    using Polly;
    using Polly.Retry;

    /// <summary>
    /// Outbox event handler that synchronizes Forge account profile updates to Keycloak via
    /// <see cref="IKeycloakAdminClient" />.
    /// </summary>
    public class KeycloakAccountSyncOutboxHandler : IOutboxEventHandler
    {
        /// <summary>
        /// The base delay in milliseconds for immediate in-process transient retries.
        /// </summary>
        private const int ImmediateRetryBaseDelayMilliseconds = 200;

        /// <summary>
        /// The injected <see cref="IKeycloakAdminClient" /> used to update users in Keycloak.
        /// </summary>
        private readonly IKeycloakAdminClient keycloakAdminClient;

        /// <summary>
        /// The injected <see cref="ILogger{KeycloakAccountSyncOutboxHandler}" /> used for logging.
        /// </summary>
        private readonly ILogger<KeycloakAccountSyncOutboxHandler> logger;

        /// <summary>
        /// The Polly resilience pipeline used for immediate transient retries.
        /// </summary>
        private readonly ResiliencePipeline<ErrorOr<Success>> resiliencePipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeycloakAccountSyncOutboxHandler" /> class.
        /// </summary>
        /// <param name="keycloakAdminClient">The injected <see cref="IKeycloakAdminClient" />.</param>
        /// <param name="options">The injected <see cref="OutboxOptions" />.</param>
        /// <param name="logger">The injected <see cref="ILogger{KeycloakAccountSyncOutboxHandler}" />.</param>
        public KeycloakAccountSyncOutboxHandler(IKeycloakAdminClient keycloakAdminClient, IOptions<OutboxOptions> options, ILogger<KeycloakAccountSyncOutboxHandler> logger)
        {
            this.keycloakAdminClient = keycloakAdminClient;
            this.logger = logger;
            this.resiliencePipeline = BuildResiliencePipeline(options.Value.ImmediateRetryCount);
        }

        /// <summary>
        /// Gets the outbox event type handled by this instance.
        /// </summary>
        public string EventType => OutboxEventTypes.AccountKeycloakSync;

        /// <summary>
        /// Asynchronously processes the supplied <see cref="OutboxMessage" /> by updating the corresponding Keycloak user.
        /// </summary>
        /// <param name="message">The claimed <see cref="OutboxMessage" /> to process.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether synchronization succeeded.</returns>
        public async Task<ErrorOr<Success>> HandleAsync(OutboxMessage message, CancellationToken token)
        {
            KeycloakAccountSyncPayload payload;

            try
            {
                payload = JsonSerializer.Deserialize<KeycloakAccountSyncPayload>(message.Payload);
            }
            catch (JsonException exception)
            {
                this.logger.LogError(exception, "Failed to deserialize Keycloak account sync payload for outbox message {MessageId}", message.Id.SanitizeForLog());
                return Error.Validation("Outbox.InvalidPayload", exception.Message);
            }

            if (payload == null || payload.AccountId == Guid.Empty)
            {
                this.logger.LogError("Outbox message {MessageId} contains an empty or invalid Keycloak account sync payload", message.Id.SanitizeForLog());
                return Error.Validation("Outbox.InvalidPayload", "Keycloak account sync payload is missing a valid account identifier.");
            }

            return await this.resiliencePipeline.ExecuteAsync(ct => new ValueTask<ErrorOr<Success>>(this.keycloakAdminClient.UpdateUserAsync(payload.AccountId, payload.Email, payload.FirstName, payload.LastName, ct)), token);
        }

        /// <summary>
        /// Builds the Polly retry pipeline for immediate in-process transient failure retries.
        /// </summary>
        /// <param name="immediateRetryCount">The configured number of immediate retry attempts.</param>
        /// <returns>A configured <see cref="ResiliencePipeline{TResult}" /> instance.</returns>
        private static ResiliencePipeline<ErrorOr<Success>> BuildResiliencePipeline(int immediateRetryCount)
        {
            if (immediateRetryCount <= 0)
            {
                return ResiliencePipeline<ErrorOr<Success>>.Empty;
            }

            var retryOptions = new RetryStrategyOptions<ErrorOr<Success>>
            {
                MaxRetryAttempts = immediateRetryCount,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = TimeSpan.FromMilliseconds(ImmediateRetryBaseDelayMilliseconds),
                ShouldHandle = new PredicateBuilder<ErrorOr<Success>>().Handle<HttpRequestException>().Handle<TimeoutException>().HandleResult(result => result.IsError && OutboxErrorClassifier.IsTransientError(result.Errors))
            };

            var pipelineBuilder = new ResiliencePipelineBuilder<ErrorOr<Success>>();
            pipelineBuilder.AddRetry(retryOptions);
            return pipelineBuilder.Build();
        }
    }
}
