// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxMessage.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Outbox
{
    /// <summary>
    /// Represents a persisted transactional outbox message stored in <c>forge.outbox_message</c>.
    /// </summary>
    public class OutboxMessage
    {
        /// <summary>
        /// Gets or sets the unique identifier of the outbox message.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the aggregate associated with this message.
        /// </summary>
        public Guid AggregateId { get; set; }

        /// <summary>
        /// Gets or sets the event type discriminator used to route the message to a handler.
        /// </summary>
        public string EventType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the JSON payload of the outbox message.
        /// </summary>
        public string Payload { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current processing status of the outbox message.
        /// </summary>
        public OutboxMessageStatus Status { get; set; } = OutboxMessageStatus.Pending;

        /// <summary>
        /// Gets or sets the number of delivery attempts made for this message.
        /// </summary>
        public int Attempts { get; set; }

        /// <summary>
        /// Gets or sets the timestamp after which the message becomes eligible to be claimed.
        /// </summary>
        public DateTimeOffset NextAttemptAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the outbox message was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the error description from the most recent failed delivery attempt.
        /// </summary>
        public string LastError { get; set; }
    }
}
