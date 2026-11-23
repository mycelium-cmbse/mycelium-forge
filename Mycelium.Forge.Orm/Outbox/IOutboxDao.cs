// ------------------------------------------------------------------------------------------------
// <copyright file="IOutboxDao.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Outbox
{
    using ErrorOr;

    using Npgsql;

    /// <summary>
    /// Defines data access operations for transactional outbox messages in <c>forge.outbox_message</c>.
    /// </summary>
    public interface IOutboxDao
    {
        /// <summary>
        /// Asynchronously enqueues a new <see cref="OutboxMessage" /> within the supplied database transaction.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="message">The <see cref="OutboxMessage" /> to persist.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Created}" /> indicating whether enqueueing succeeded.</returns>
        Task<ErrorOr<Created>> EnqueueAsync(NpgsqlTransaction transaction, OutboxMessage message, CancellationToken token);

        /// <summary>
        /// Asynchronously claims up to <paramref name="batchSize" /> eligible outbox messages for processing with a lease
        /// duration.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="batchSize">The maximum number of messages to claim in the batch.</param>
        /// <param name="leaseDuration">The duration for which claimed messages are leased before becoming reclaimable.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{TValue}" /> containing the claimed <see cref="OutboxMessage" /> instances.</returns>
        Task<ErrorOr<IReadOnlyList<OutboxMessage>>> ClaimBatchAsync(NpgsqlTransaction transaction, int batchSize, TimeSpan leaseDuration, CancellationToken token);

        /// <summary>
        /// Asynchronously deletes a processed outbox message by identifier.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="id">The unique identifier of the outbox message to delete.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Deleted}" /> indicating whether deletion succeeded.</returns>
        Task<ErrorOr<Deleted>> DeleteAsync(NpgsqlTransaction transaction, Guid id, CancellationToken token);

        /// <summary>
        /// Asynchronously reschedules a pending outbox message for a future retry attempt.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="id">The unique identifier of the outbox message to reschedule.</param>
        /// <param name="nextAttemptAt">The timestamp when the message should next be attempted.</param>
        /// <param name="lastError">The error description recorded from the failed attempt.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Updated}" /> indicating whether rescheduling succeeded.</returns>
        Task<ErrorOr<Updated>> RescheduleAsync(NpgsqlTransaction transaction, Guid id, DateTimeOffset nextAttemptAt, string lastError, CancellationToken token);

        /// <summary>
        /// Asynchronously marks an outbox message as permanently failed.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="id">The unique identifier of the outbox message to mark as failed.</param>
        /// <param name="lastError">The error description recorded for the permanent failure.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Updated}" /> indicating whether updating the status succeeded.</returns>
        Task<ErrorOr<Updated>> MarkFailedAsync(NpgsqlTransaction transaction, Guid id, string lastError, CancellationToken token);
    }
}
