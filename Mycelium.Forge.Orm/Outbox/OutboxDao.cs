// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxDao.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Outbox
{
    using System.Text.Json;

    using ErrorOr;

    using Mycelium.Forge.Common.Extensions;

    using Npgsql;

    using NpgsqlTypes;

    /// <summary>
    /// Data access object for transactional outbox messages in <c>forge.outbox_message</c>.
    /// </summary>
    public class OutboxDao : IOutboxDao
    {
        /// <summary>
        /// The database status value indicating an outbox message is pending delivery.
        /// </summary>
        private const string PendingStatusValue = "PENDING";

        /// <summary>
        /// The database status value indicating an outbox message has permanently failed delivery.
        /// </summary>
        private const string FailedStatusValue = "FAILED";

        /// <summary>
        /// The injected <see cref="ILogger{OutboxDao}" /> used for logging.
        /// </summary>
        private readonly ILogger<OutboxDao> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutboxDao" /> class.
        /// </summary>
        /// <param name="logger">The injected <see cref="ILogger{OutboxDao}" />.</param>
        public OutboxDao(ILogger<OutboxDao> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Asynchronously enqueues a new <see cref="OutboxMessage" /> within the supplied database transaction.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="message">The <see cref="OutboxMessage" /> to persist.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Created}" /> indicating whether enqueueing succeeded.</returns>
        public async Task<ErrorOr<Created>> EnqueueAsync(NpgsqlTransaction transaction, OutboxMessage message, CancellationToken token)
        {
            const string sql =
                """
                INSERT INTO forge.outbox_message (aggregate_id, event_type, payload, status, attempts, next_attempt_at)
                VALUES (@aggregateId, @eventType, @payload, @status, @attempts, @nextAttemptAt)
                RETURNING id, created_at;
                """;

            try
            {
                await using var command = new NpgsqlCommand(sql, transaction.Connection, transaction);
                var status = ToDbStatus(message.Status);
                var nextAttemptAt = message.NextAttemptAt == default ? DateTimeOffset.UtcNow : message.NextAttemptAt.ToUniversalTime();

                command.Parameters.Add(new NpgsqlParameter("@aggregateId", NpgsqlDbType.Uuid) { Value = message.AggregateId });
                command.Parameters.Add(new NpgsqlParameter("@eventType", NpgsqlDbType.Text) { Value = message.EventType });
                command.Parameters.Add(new NpgsqlParameter("@payload", NpgsqlDbType.Jsonb) { Value = message.Payload });
                command.Parameters.Add(new NpgsqlParameter("@status", NpgsqlDbType.Text) { Value = status });
                command.Parameters.Add(new NpgsqlParameter("@attempts", NpgsqlDbType.Integer) { Value = message.Attempts });
                command.Parameters.Add(new NpgsqlParameter("@nextAttemptAt", NpgsqlDbType.TimestampTz) { Value = nextAttemptAt });

                await using var reader = await command.ExecuteReaderAsync(token);

                if (await reader.ReadAsync(token))
                {
                    message.Id = reader.GetGuid(0);
                    message.CreatedAt = reader.GetFieldValue<DateTimeOffset>(1);
                }

                return Result.Created;
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to enqueue outbox message for aggregate {AggregateId}", message.AggregateId.SanitizeForLog());
                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously serializes and enqueues a typed <see cref="IOutboxEvent" /> within the supplied database transaction.
        /// </summary>
        /// <typeparam name="TEvent">The type of outbox event to enqueue.</typeparam>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="outboxEvent">The typed outbox event instance to persist.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Created}" /> indicating whether enqueueing succeeded.</returns>
        public Task<ErrorOr<Created>> EnqueueAsync<TEvent>(NpgsqlTransaction transaction, TEvent outboxEvent, CancellationToken token) where TEvent : IOutboxEvent
        {
            var message = new OutboxMessage
            {
                AggregateId = outboxEvent.AggregateId,
                EventType = TEvent.EventType,
                Payload = JsonSerializer.Serialize(outboxEvent)
            };

            return this.EnqueueAsync(transaction, message, token);
        }

        /// <summary>
        /// Asynchronously claims up to <paramref name="batchSize" /> eligible outbox messages for processing with a lease
        /// duration.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="batchSize">The maximum number of messages to claim in the batch.</param>
        /// <param name="leaseDuration">The duration for which claimed messages are leased before becoming reclaimable.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{TValue}" /> containing the claimed <see cref="OutboxMessage" /> instances.</returns>
        public async Task<ErrorOr<IReadOnlyList<OutboxMessage>>> ClaimBatchAsync(NpgsqlTransaction transaction, int batchSize, TimeSpan leaseDuration, CancellationToken token)
        {
            const string sql =
                """
                WITH claimed AS (
                    SELECT m.id
                    FROM forge.outbox_message m
                    WHERE m.status = 'PENDING'
                      AND m.next_attempt_at <= now()
                      AND NOT EXISTS (
                          SELECT 1
                          FROM forge.outbox_message older
                          WHERE older.aggregate_id = m.aggregate_id
                            AND older.status = 'PENDING'
                            AND older.id < m.id
                      )
                    ORDER BY m.id
                    LIMIT @batchSize
                    FOR UPDATE OF m SKIP LOCKED
                )
                UPDATE forge.outbox_message target
                SET attempts = target.attempts + 1,
                    next_attempt_at = now() + @lease,
                    claim_token = uuidv7()
                FROM claimed
                WHERE target.id = claimed.id
                RETURNING target.id, target.aggregate_id, target.event_type, target.payload::text AS payload, target.status, target.attempts, target.next_attempt_at, target.created_at, target.last_error, target.claim_token;
                """;

            try
            {
                await using var command = new NpgsqlCommand(sql, transaction.Connection, transaction);
                command.Parameters.Add(new NpgsqlParameter("@batchSize", NpgsqlDbType.Integer) { Value = batchSize });
                command.Parameters.Add(new NpgsqlParameter("@lease", NpgsqlDbType.Interval) { Value = leaseDuration });

                await using var reader = await command.ExecuteReaderAsync(token);
                List<OutboxMessage> claimedMessages = [];

                while (await reader.ReadAsync(token))
                {
                    claimedMessages.Add(MapToOutboxMessage(reader));
                }

                claimedMessages.Sort((left, right) => left.Id.CompareTo(right.Id));
                return claimedMessages;
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to claim outbox message batch");
                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously deletes a processed outbox message if the caller still holds the matching claim token.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="id">The unique identifier of the outbox message to delete.</param>
        /// <param name="claimToken">The fencing token assigned when the message was claimed.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Deleted}" /> indicating whether deletion succeeded.</returns>
        public async Task<ErrorOr<Deleted>> DeleteAsync(NpgsqlTransaction transaction, Guid id, Guid claimToken, CancellationToken token)
        {
            const string sql =
                """
                DELETE FROM forge.outbox_message
                WHERE id = @id
                  AND claim_token = @claimToken;
                """;

            try
            {
                await using var command = new NpgsqlCommand(sql, transaction.Connection, transaction);
                command.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = id });
                command.Parameters.Add(new NpgsqlParameter("@claimToken", NpgsqlDbType.Uuid) { Value = claimToken });

                var affected = await command.ExecuteNonQueryAsync(token);

                if (affected != 0)
                {
                    return Result.Deleted;
                }

                if (this.logger.IsEnabled(LogLevel.Warning))
                {
                    this.logger.LogWarning("Outbox message {MessageId} could not be deleted because lease ownership was lost or it was not found", id.SanitizeForLog());
                }

                return Error.Conflict(description: $"Outbox message '{id}' was not deleted because ownership was lost or it was not found.");
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to delete outbox message {MessageId}", id.SanitizeForLog());
                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously reschedules a pending outbox message for a future retry attempt if the caller still holds the
        /// matching claim token.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="id">The unique identifier of the outbox message to reschedule.</param>
        /// <param name="claimToken">The fencing token assigned when the message was claimed.</param>
        /// <param name="nextAttemptAt">The timestamp when the message should next be attempted.</param>
        /// <param name="lastError">The error description recorded from the failed attempt.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Updated}" /> indicating whether rescheduling succeeded.</returns>
        public Task<ErrorOr<Updated>> RescheduleAsync(NpgsqlTransaction transaction, Guid id, Guid claimToken, DateTimeOffset nextAttemptAt, string lastError, CancellationToken token)
        {
            const string sql =
                """
                UPDATE forge.outbox_message
                SET next_attempt_at = @nextAttemptAt,
                    last_error = @lastError,
                    claim_token = NULL
                WHERE id = @id
                  AND claim_token = @claimToken
                  AND status = 'PENDING';
                """;

            NpgsqlParameter[] parameters =
            [
                new("@id", NpgsqlDbType.Uuid) { Value = id },
                new("@claimToken", NpgsqlDbType.Uuid) { Value = claimToken },
                new("@nextAttemptAt", NpgsqlDbType.TimestampTz) { Value = nextAttemptAt.ToUniversalTime() },
                new("@lastError", NpgsqlDbType.Text) { Value = (object)lastError ?? DBNull.Value }
            ];

            return this.ExecuteUpdateAsync(transaction, sql, parameters, $"Outbox message '{id}' was not rescheduled because ownership was lost or it was not found.", "reschedule", id, token);
        }

        /// <summary>
        /// Asynchronously marks an outbox message as permanently failed if the caller still holds the matching claim token.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="id">The unique identifier of the outbox message to mark as failed.</param>
        /// <param name="claimToken">The fencing token assigned when the message was claimed.</param>
        /// <param name="lastError">The error description recorded for the permanent failure.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Updated}" /> indicating whether updating the status succeeded.</returns>
        public Task<ErrorOr<Updated>> MarkFailedAsync(NpgsqlTransaction transaction, Guid id, Guid claimToken, string lastError, CancellationToken token)
        {
            const string sql =
                """
                UPDATE forge.outbox_message
                SET status = @status,
                    last_error = @lastError,
                    claim_token = NULL
                WHERE id = @id
                  AND claim_token = @claimToken;
                """;

            NpgsqlParameter[] parameters =
            [
                new("@id", NpgsqlDbType.Uuid) { Value = id },
                new("@claimToken", NpgsqlDbType.Uuid) { Value = claimToken },
                new("@status", NpgsqlDbType.Text) { Value = ToDbStatus(OutboxMessageStatus.Failed) },
                new("@lastError", NpgsqlDbType.Text) { Value = (object)lastError ?? DBNull.Value }
            ];

            return this.ExecuteUpdateAsync(transaction, sql, parameters, $"Outbox message '{id}' was not marked as failed because ownership was lost or it was not found.", "mark as failed", id, token);
        }

        /// <summary>
        /// Maps the current row of the <see cref="NpgsqlDataReader" /> to an <see cref="OutboxMessage" /> instance.
        /// </summary>
        /// <param name="reader">The <see cref="NpgsqlDataReader" /> positioned at a claimed row.</param>
        /// <returns>A populated <see cref="OutboxMessage" /> instance.</returns>
        private static OutboxMessage MapToOutboxMessage(NpgsqlDataReader reader)
        {
            return new OutboxMessage
            {
                Id = (Guid)reader["id"],
                AggregateId = (Guid)reader["aggregate_id"],
                EventType = (string)reader["event_type"],
                Payload = (string)reader["payload"],
                Status = ParseStatus((string)reader["status"]),
                Attempts = (int)reader["attempts"],
                NextAttemptAt = reader.GetFieldValue<DateTimeOffset>(reader.GetOrdinal("next_attempt_at")),
                CreatedAt = reader.GetFieldValue<DateTimeOffset>(reader.GetOrdinal("created_at")),
                LastError = reader["last_error"] is DBNull ? null : (string)reader["last_error"],
                ClaimToken = reader["claim_token"] is DBNull ? null : (Guid?)reader["claim_token"]
            };
        }

        /// <summary>
        /// Converts an <see cref="OutboxMessageStatus" /> value to its PostgreSQL text representation.
        /// </summary>
        /// <param name="status">The <see cref="OutboxMessageStatus" /> value to convert.</param>
        /// <returns>The PostgreSQL status string.</returns>
        private static string ToDbStatus(OutboxMessageStatus status)
        {
            return status switch
            {
                OutboxMessageStatus.Pending => PendingStatusValue,
                OutboxMessageStatus.Failed => FailedStatusValue,
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, $"Unsupported outbox message status '{status}'.")
            };
        }

        /// <summary>
        /// Parses a PostgreSQL outbox status text value into an <see cref="OutboxMessageStatus" />.
        /// </summary>
        /// <param name="status">The PostgreSQL status string.</param>
        /// <returns>The parsed <see cref="OutboxMessageStatus" /> value.</returns>
        private static OutboxMessageStatus ParseStatus(string status)
        {
            return status switch
            {
                PendingStatusValue => OutboxMessageStatus.Pending,
                FailedStatusValue => OutboxMessageStatus.Failed,
                _ => throw new InvalidOperationException($"Unsupported outbox message status '{status}'.")
            };
        }

        /// <summary>
        /// Executes an outbox row update command and returns <see cref="Result.Updated" /> or <see cref="Error.Conflict" />.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="sql">The parameterized SQL <c>UPDATE</c> statement.</param>
        /// <param name="parameters">The typed SQL parameters to bind to the command.</param>
        /// <param name="conflictDescription">The error description returned when no rows match.</param>
        /// <param name="failureAction">The action verb logged if execution fails.</param>
        /// <param name="id">The unique identifier of the target outbox message.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Updated}" /> indicating the outcome of the update.</returns>
        private async Task<ErrorOr<Updated>> ExecuteUpdateAsync(NpgsqlTransaction transaction, string sql, NpgsqlParameter[] parameters, string conflictDescription, string failureAction, Guid id, CancellationToken token)
        {
            try
            {
                await using var command = new NpgsqlCommand(sql, transaction.Connection, transaction);
                command.Parameters.AddRange(parameters);

                var affected = await command.ExecuteNonQueryAsync(token);

                return affected == 0 ? Error.Conflict(description: conflictDescription) : Result.Updated;
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to {Action} outbox message {MessageId}", failureAction, id.SanitizeForLog());
                return Error.Failure(description: exception.Message);
            }
        }
    }
}
