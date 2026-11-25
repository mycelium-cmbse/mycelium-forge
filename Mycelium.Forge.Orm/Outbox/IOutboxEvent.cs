// ------------------------------------------------------------------------------------------------
// <copyright file="IOutboxEvent.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Outbox
{
    /// <summary>
    /// Defines a typed domain event that can be persisted in the transactional outbox and dispatched to handlers.
    /// </summary>
    public interface IOutboxEvent
    {
        /// <summary>
        /// Gets the event type discriminator used to route serialized outbox messages to matching handlers.
        /// </summary>
        static abstract string EventType { get; }

        /// <summary>
        /// Gets the unique identifier of the aggregate associated with this event, used for per-aggregate ordering.
        /// </summary>
        Guid AggregateId { get; }
    }
}
