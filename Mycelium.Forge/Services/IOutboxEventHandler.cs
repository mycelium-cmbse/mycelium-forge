// ------------------------------------------------------------------------------------------------
// <copyright file="IOutboxEventHandler.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using ErrorOr;

    using Mycelium.Forge.Orm.Outbox;

    /// <summary>
    /// Defines a handler that processes transactional outbox messages for a specific event type.
    /// </summary>
    public interface IOutboxEventHandler
    {
        /// <summary>
        /// Gets the outbox event type handled by this instance.
        /// </summary>
        string EventType { get; }

        /// <summary>
        /// Asynchronously processes the supplied <see cref="OutboxMessage" />.
        /// </summary>
        /// <param name="message">The claimed <see cref="OutboxMessage" /> to process.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether processing succeeded.</returns>
        Task<ErrorOr<Success>> HandleAsync(OutboxMessage message, CancellationToken token);
    }
}
