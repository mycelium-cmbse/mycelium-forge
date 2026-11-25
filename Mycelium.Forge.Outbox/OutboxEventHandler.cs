// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxEventHandler.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Outbox
{
    using System.Text.Json;

    using ErrorOr;

    using Microsoft.Extensions.Logging;

    using Mycelium.Forge.Common.Extensions;
    using Mycelium.Forge.Orm.Outbox;

    /// <summary>
    /// Base class for strongly typed outbox event handlers that handles JSON deserialization and payload validation before
    /// delegating to <see cref="HandleAsync(TEvent, CancellationToken)" />.
    /// </summary>
    /// <typeparam name="TEvent">The strongly typed <see cref="IOutboxEvent" /> handled by this instance.</typeparam>
    public abstract class OutboxEventHandler<TEvent> : IOutboxEventHandler where TEvent : IOutboxEvent
    {
        /// <summary>
        /// The <see cref="ILogger" /> used to log deserialization and validation errors.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutboxEventHandler{TEvent}" /> class.
        /// </summary>
        /// <param name="logger">The injected <see cref="ILogger" />.</param>
        protected OutboxEventHandler(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets the outbox event type handled by this instance.
        /// </summary>
        public string EventType => TEvent.EventType;

        /// <summary>
        /// Asynchronously deserializes the supplied <see cref="OutboxMessage" /> and invokes
        /// <see cref="HandleAsync(TEvent, CancellationToken)" />.
        /// </summary>
        /// <param name="message">The claimed <see cref="OutboxMessage" /> to process.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether processing succeeded.</returns>
        public async Task<ErrorOr<Success>> HandleAsync(OutboxMessage message, CancellationToken token)
        {
            TEvent outboxEvent;

            try
            {
                outboxEvent = JsonSerializer.Deserialize<TEvent>(message.Payload);
            }
            catch (JsonException exception)
            {
                this.logger.LogError(exception, "Failed to deserialize {EventType} payload for outbox message {MessageId}", TEvent.EventType.SanitizeForLog(), message.Id.SanitizeForLog());
                return Error.Validation("Outbox.InvalidPayload", exception.Message);
            }

            if (outboxEvent == null || outboxEvent.AggregateId == Guid.Empty)
            {
                this.logger.LogError("Outbox message {MessageId} contains an empty or invalid {EventType} payload", message.Id.SanitizeForLog(), TEvent.EventType.SanitizeForLog());
                return Error.Validation("Outbox.InvalidPayload", $"Outbox event '{TEvent.EventType}' payload is missing a valid aggregate identifier.");
            }

            return await this.HandleAsync(outboxEvent, token);
        }

        /// <summary>
        /// Asynchronously processes the deserialized <typeparamref name="TEvent" /> instance.
        /// </summary>
        /// <param name="outboxEvent">The deserialized typed event payload.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether processing succeeded.</returns>
        protected abstract Task<ErrorOr<Success>> HandleAsync(TEvent outboxEvent, CancellationToken token);
    }
}
