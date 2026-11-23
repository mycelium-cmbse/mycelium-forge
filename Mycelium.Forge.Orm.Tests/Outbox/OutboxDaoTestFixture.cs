// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxDaoTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Tests.Outbox
{
    using ErrorOr;

    using Microsoft.Extensions.Logging;

    using Mycelium.Forge.Orm.Outbox;
    using Mycelium.Forge.Orm.Tests.Helper;

    using Npgsql;

    using NpgsqlTypes;

    using NUnit.Framework;

    [TestFixture]
    [Category("Database")]
    public class OutboxDaoTestFixture : BaseIntegrationTestClassFixture
    {
        private OutboxDao outboxDao;

        [Test]
        public async Task VerifyClaimBatchAsync()
        {
            // Case 1: Empty table returns empty list
            await using (var emptyTransaction = await this.Connection.BeginTransactionAsync())
            {
                var emptyClaim = await this.outboxDao.ClaimBatchAsync(emptyTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
                await emptyTransaction.CommitAsync();

                using (Assert.EnterMultipleScope())
                {
                    Assert.That(emptyClaim.IsError, Is.False);
                    Assert.That(emptyClaim.Value, Is.Empty);
                }
            }

            // Case 2: Per-aggregate UUIDv7 FIFO ordering, lease enforcement, future/failed exclusion, and deletion unblocking
            var aggregate1 = Guid.NewGuid();
            var aggregate2 = Guid.NewGuid();

            var firstForAggregate1 = new OutboxMessage { AggregateId = aggregate1, EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "{\"step\":1}" };
            var secondForAggregate1 = new OutboxMessage { AggregateId = aggregate1, EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "{\"step\":2}" };
            var firstForAggregate2 = new OutboxMessage { AggregateId = aggregate2, EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "{\"step\":3}" };
            var futureMessage = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "{\"future\":true}", NextAttemptAt = DateTimeOffset.UtcNow.AddMinutes(5) };
            var failedMessage = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "{\"failed\":true}", Status = OutboxMessageStatus.Failed };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, firstForAggregate1, CancellationToken.None));
            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, secondForAggregate1, CancellationToken.None));
            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, firstForAggregate2, CancellationToken.None));
            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, futureMessage, CancellationToken.None));
            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, failedMessage, CancellationToken.None));

            IReadOnlyList<OutboxMessage> firstClaim;

            await using (var claimTransaction = await this.Connection.BeginTransactionAsync())
            {
                var claimResult = await this.outboxDao.ClaimBatchAsync(claimTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
                await claimTransaction.CommitAsync();

                Assert.That(claimResult.IsError, Is.False);
                firstClaim = claimResult.Value;
            }

            using (Assert.EnterMultipleScope())
            {
                Assert.That(firstForAggregate1.Id.CompareTo(secondForAggregate1.Id), Is.LessThan(0));
                Assert.That(firstClaim, Has.Count.EqualTo(2));
                Assert.That(firstClaim.Select(m => m.Id), Is.EquivalentTo([firstForAggregate1.Id, firstForAggregate2.Id]));
                Assert.That(firstClaim.All(m => m.Status == OutboxMessageStatus.Pending), Is.True);
                Assert.That(firstClaim.All(m => m.Attempts == 1), Is.True);
                Assert.That(firstClaim.All(m => m.NextAttemptAt > m.CreatedAt), Is.True);
            }

            // While firstForAggregate1 is leased, secondForAggregate1 must remain blocked
            await using (var leasedTransaction = await this.Connection.BeginTransactionAsync())
            {
                var leasedClaim = await this.outboxDao.ClaimBatchAsync(leasedTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
                await leasedTransaction.CommitAsync();

                Assert.That(leasedClaim.Value, Is.Empty);
            }

            // Deleting firstForAggregate1 unblocks secondForAggregate1
            await using (var deleteTransaction = await this.Connection.BeginTransactionAsync())
            {
                await this.outboxDao.DeleteAsync(deleteTransaction, firstForAggregate1.Id, CancellationToken.None);
                await this.outboxDao.DeleteAsync(deleteTransaction, firstForAggregate2.Id, CancellationToken.None);
                await deleteTransaction.CommitAsync();
            }

            await using (var unblockedTransaction = await this.Connection.BeginTransactionAsync())
            {
                var unblockedClaim = await this.outboxDao.ClaimBatchAsync(unblockedTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
                await unblockedTransaction.CommitAsync();

                using (Assert.EnterMultipleScope())
                {
                    Assert.That(unblockedClaim.Value, Has.Count.EqualTo(1));
                    Assert.That(unblockedClaim.Value[0].Id, Is.EqualTo(secondForAggregate1.Id));
                }
            }

            await using (var deleteSecondTransaction = await this.Connection.BeginTransactionAsync())
            {
                await this.outboxDao.DeleteAsync(deleteSecondTransaction, secondForAggregate1.Id, CancellationToken.None);
                await deleteSecondTransaction.CommitAsync();
            }

            // Case 3: Lease expiration allows reclaim with incremented attempts
            var leaseMessage = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "{\"lease\":true}" };
            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, leaseMessage, CancellationToken.None));

            await using (var shortLeaseTransaction = await this.Connection.BeginTransactionAsync())
            {
                await this.outboxDao.ClaimBatchAsync(shortLeaseTransaction, 10, TimeSpan.FromMilliseconds(100), CancellationToken.None);
                await shortLeaseTransaction.CommitAsync();
            }

            await Task.Delay(250);

            await using (var reclaimTransaction = await this.Connection.BeginTransactionAsync())
            {
                var reclaimed = await this.outboxDao.ClaimBatchAsync(reclaimTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
                await reclaimTransaction.CommitAsync();

                using (Assert.EnterMultipleScope())
                {
                    Assert.That(reclaimed.Value, Has.Count.EqualTo(1));
                    Assert.That(reclaimed.Value[0].Id, Is.EqualTo(leaseMessage.Id));
                    Assert.That(reclaimed.Value[0].Attempts, Is.EqualTo(2));
                }
            }

            // Clean up before batchSize limit and UUIDv7 ordering test
            await using (var batchCleanupCommand = new NpgsqlCommand("DELETE FROM forge.outbox_message;", this.Connection))
            {
                await batchCleanupCommand.ExecuteNonQueryAsync();
            }

            // Case 4: batchSize limit respects UUIDv7 ordering across distinct aggregates
            var orderedMessage1 = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "{\"order\":1}" };
            var orderedMessage2 = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "{\"order\":2}" };
            var orderedMessage3 = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "{\"order\":3}" };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, orderedMessage1, CancellationToken.None));
            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, orderedMessage2, CancellationToken.None));
            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, orderedMessage3, CancellationToken.None));

            IReadOnlyList<OutboxMessage> limitedClaim;

            await using (var limitedTransaction = await this.Connection.BeginTransactionAsync())
            {
                var limitedResult = await this.outboxDao.ClaimBatchAsync(limitedTransaction, 2, TimeSpan.FromSeconds(30), CancellationToken.None);
                await limitedTransaction.CommitAsync();

                Assert.That(limitedResult.IsError, Is.False);
                limitedClaim = limitedResult.Value;
            }

            IReadOnlyList<OutboxMessage> remainingClaim;

            await using (var remainingTransaction = await this.Connection.BeginTransactionAsync())
            {
                var remainingResult = await this.outboxDao.ClaimBatchAsync(remainingTransaction, 2, TimeSpan.FromSeconds(30), CancellationToken.None);
                await remainingTransaction.CommitAsync();

                Assert.That(remainingResult.IsError, Is.False);
                remainingClaim = remainingResult.Value;
            }

            using (Assert.EnterMultipleScope())
            {
                Assert.That(orderedMessage1.Id.CompareTo(orderedMessage2.Id), Is.LessThan(0));
                Assert.That(orderedMessage2.Id.CompareTo(orderedMessage3.Id), Is.LessThan(0));
                Assert.That(limitedClaim, Has.Count.EqualTo(2));
                Assert.That(limitedClaim[0].Id, Is.EqualTo(orderedMessage1.Id));
                Assert.That(limitedClaim[1].Id, Is.EqualTo(orderedMessage2.Id));
                Assert.That(remainingClaim, Has.Count.EqualTo(1));
                Assert.That(remainingClaim[0].Id, Is.EqualTo(orderedMessage3.Id));
            }

            // Clean up before SKIP LOCKED test
            await using (var cleanupCommand = new NpgsqlCommand("DELETE FROM forge.outbox_message;", this.Connection))
            {
                await cleanupCommand.ExecuteNonQueryAsync();
            }

            // Case 5: Concurrent FOR UPDATE SKIP LOCKED claims distinct rows without blocking
            var concurrentMessage1 = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "{\"worker\":1}" };
            var concurrentMessage2 = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "{\"worker\":2}" };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, concurrentMessage1, CancellationToken.None));
            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, concurrentMessage2, CancellationToken.None));

            await using var secondConnection = new NpgsqlConnection(this.ConnectionString);
            await secondConnection.OpenAsync();

            await using var worker1Transaction = await this.Connection.BeginTransactionAsync();
            await using var worker2Transaction = await secondConnection.BeginTransactionAsync();

            var worker1Claim = await this.outboxDao.ClaimBatchAsync(worker1Transaction, 1, TimeSpan.FromSeconds(30), CancellationToken.None);
            var worker2Claim = await this.outboxDao.ClaimBatchAsync(worker2Transaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);

            await worker1Transaction.CommitAsync();
            await worker2Transaction.CommitAsync();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(worker1Claim.IsError, Is.False);
                Assert.That(worker2Claim.IsError, Is.False);
                Assert.That(worker1Claim.Value, Has.Count.EqualTo(1));
                Assert.That(worker2Claim.Value, Has.Count.EqualTo(1));
                Assert.That(worker1Claim.Value[0].Id, Is.Not.EqualTo(worker2Claim.Value[0].Id));
            }
        }

        [Test]
        public async Task VerifyDeleteAsync()
        {
            var message = new OutboxMessage
            {
                AggregateId = Guid.NewGuid(),
                EventType = OutboxEventTypes.AccountKeycloakSync,
                Payload = "{}"
            };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, message, CancellationToken.None));

            ErrorOr<Deleted> deleteExistingResult;

            await using (var deleteTransaction = await this.Connection.BeginTransactionAsync())
            {
                deleteExistingResult = await this.outboxDao.DeleteAsync(deleteTransaction, message.Id, CancellationToken.None);
                await deleteTransaction.CommitAsync();
            }

            ErrorOr<Deleted> deleteMissingResult;

            await using (var missingTransaction = await this.Connection.BeginTransactionAsync())
            {
                deleteMissingResult = await this.outboxDao.DeleteAsync(missingTransaction, Guid.NewGuid(), CancellationToken.None);
                await missingTransaction.CommitAsync();
            }

            await using var countCommand = new NpgsqlCommand("SELECT COUNT(*) FROM forge.outbox_message WHERE id = @id;", this.Connection);
            countCommand.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = message.Id });
            var remainingCount = (long)(await countCommand.ExecuteScalarAsync())!;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deleteExistingResult.IsError, Is.False);
                Assert.That(deleteMissingResult.IsError, Is.False);
                Assert.That(remainingCount, Is.EqualTo(0));
            }
        }

        [Test]
        public async Task VerifyEnqueueAsync()
        {
            var aggregateId = Guid.NewGuid();

            var message = new OutboxMessage
            {
                AggregateId = aggregateId,
                EventType = OutboxEventTypes.AccountKeycloakSync,
                Payload = "{\"AccountId\":\"11111111-1111-1111-1111-111111111111\"}"
            };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, message, CancellationToken.None));

            await using var readCommand = new NpgsqlCommand("SELECT aggregate_id, event_type, payload::text, status, attempts, last_error FROM forge.outbox_message WHERE id = @id;", this.Connection);
            readCommand.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = message.Id });

            await using var reader = await readCommand.ExecuteReaderAsync();
            var hasRow = await reader.ReadAsync();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(message.Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(message.CreatedAt, Is.Not.Default);
                Assert.That(hasRow, Is.True);
                Assert.That(reader.GetGuid(0), Is.EqualTo(aggregateId));
                Assert.That(reader.GetString(1), Is.EqualTo(OutboxEventTypes.AccountKeycloakSync));
                Assert.That(reader.GetString(2), Does.Contain("11111111-1111-1111-1111-111111111111"));
                Assert.That(reader.GetString(3), Is.EqualTo("PENDING"));
                Assert.That(reader.GetInt32(4), Is.EqualTo(0));
                Assert.That(reader.IsDBNull(5), Is.True);
            }

            await reader.CloseAsync();

            // Invalid JSONB payload returns Error.Failure
            var invalidMessage = new OutboxMessage
            {
                AggregateId = aggregateId,
                EventType = OutboxEventTypes.AccountKeycloakSync,
                Payload = "{invalid-json"
            };

            await using var failTransaction = await this.Connection.BeginTransactionAsync();
            var invalidResult = await this.outboxDao.EnqueueAsync(failTransaction, invalidMessage, CancellationToken.None);
            await failTransaction.RollbackAsync();

            Assert.That(invalidResult.IsError, Is.True);
        }

        [Test]
        public async Task VerifyMarkFailedAsync()
        {
            var message = new OutboxMessage
            {
                AggregateId = Guid.NewGuid(),
                EventType = OutboxEventTypes.AccountKeycloakSync,
                Payload = "{}"
            };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, message, CancellationToken.None));

            ErrorOr<Updated> markFailedResult;

            await using (var failTransaction = await this.Connection.BeginTransactionAsync())
            {
                markFailedResult = await this.outboxDao.MarkFailedAsync(failTransaction, message.Id, "400 Bad Request", CancellationToken.None);
                await failTransaction.CommitAsync();
            }

            ErrorOr<Updated> notFoundResult;

            await using (var notFoundTransaction = await this.Connection.BeginTransactionAsync())
            {
                notFoundResult = await this.outboxDao.MarkFailedAsync(notFoundTransaction, Guid.NewGuid(), "Missing", CancellationToken.None);
                await notFoundTransaction.CommitAsync();
            }

            await using var readCommand = new NpgsqlCommand("SELECT status, last_error FROM forge.outbox_message WHERE id = @id;", this.Connection);
            readCommand.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = message.Id });

            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            var persistedStatus = reader.GetString(0);
            var persistedLastError = reader.GetString(1);
            await reader.CloseAsync();

            await using var claimTransaction = await this.Connection.BeginTransactionAsync();
            var claimAfterFailed = await this.outboxDao.ClaimBatchAsync(claimTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
            await claimTransaction.CommitAsync();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(markFailedResult.IsError, Is.False);
                Assert.That(persistedStatus, Is.EqualTo("FAILED"));
                Assert.That(persistedLastError, Is.EqualTo("400 Bad Request"));
                Assert.That(claimAfterFailed.Value, Is.Empty);
                Assert.That(notFoundResult.IsError, Is.True);
                Assert.That(notFoundResult.FirstError.Type, Is.EqualTo(ErrorType.NotFound));
            }
        }

        [Test]
        public async Task VerifyRescheduleAsync()
        {
            var message = new OutboxMessage
            {
                AggregateId = Guid.NewGuid(),
                EventType = OutboxEventTypes.AccountKeycloakSync,
                Payload = "{}"
            };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, message, CancellationToken.None));

            var futureAttempt = DateTimeOffset.UtcNow.AddMinutes(10);
            ErrorOr<Updated> rescheduleResult;

            await using (var rescheduleTransaction = await this.Connection.BeginTransactionAsync())
            {
                rescheduleResult = await this.outboxDao.RescheduleAsync(rescheduleTransaction, message.Id, futureAttempt, "Transient 503 error", CancellationToken.None);
                await rescheduleTransaction.CommitAsync();
            }

            ErrorOr<Updated> notFoundResult;

            await using (var notFoundTransaction = await this.Connection.BeginTransactionAsync())
            {
                notFoundResult = await this.outboxDao.RescheduleAsync(notFoundTransaction, Guid.NewGuid(), futureAttempt, "Missing", CancellationToken.None);
                await notFoundTransaction.CommitAsync();
            }

            await using var readCommand = new NpgsqlCommand("SELECT status, next_attempt_at, last_error FROM forge.outbox_message WHERE id = @id;", this.Connection);
            readCommand.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = message.Id });

            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            var persistedStatus = reader.GetString(0);
            var persistedNextAttempt = reader.GetFieldValue<DateTimeOffset>(1);
            var persistedLastError = reader.GetString(2);
            await reader.CloseAsync();

            await using var claimTransaction = await this.Connection.BeginTransactionAsync();
            var claimAfterReschedule = await this.outboxDao.ClaimBatchAsync(claimTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
            await claimTransaction.CommitAsync();

            var failedMessage = new OutboxMessage
            {
                AggregateId = Guid.NewGuid(),
                EventType = OutboxEventTypes.AccountKeycloakSync,
                Payload = "{}",
                Status = OutboxMessageStatus.Failed
            };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, failedMessage, CancellationToken.None));

            ErrorOr<Updated> rescheduleFailedResult;

            await using (var rescheduleFailedTransaction = await this.Connection.BeginTransactionAsync())
            {
                rescheduleFailedResult = await this.outboxDao.RescheduleAsync(rescheduleFailedTransaction, failedMessage.Id, futureAttempt, "Should not reschedule", CancellationToken.None);
                await rescheduleFailedTransaction.CommitAsync();
            }

            await using var failedReadCommand = new NpgsqlCommand("SELECT status FROM forge.outbox_message WHERE id = @id;", this.Connection);
            failedReadCommand.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = failedMessage.Id });
            var failedStatus = (string)(await failedReadCommand.ExecuteScalarAsync())!;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(rescheduleResult.IsError, Is.False);
                Assert.That(persistedStatus, Is.EqualTo("PENDING"));
                Assert.That(persistedNextAttempt, Is.EqualTo(futureAttempt).Within(TimeSpan.FromSeconds(1)));
                Assert.That(persistedLastError, Is.EqualTo("Transient 503 error"));
                Assert.That(claimAfterReschedule.Value, Is.Empty);
                Assert.That(notFoundResult.IsError, Is.True);
                Assert.That(notFoundResult.FirstError.Type, Is.EqualTo(ErrorType.NotFound));
                Assert.That(rescheduleFailedResult.IsError, Is.True);
                Assert.That(rescheduleFailedResult.FirstError.Type, Is.EqualTo(ErrorType.NotFound));
                Assert.That(failedStatus, Is.EqualTo("FAILED"));
            }
        }

        protected override Task PostSetup()
        {
            this.outboxDao = new OutboxDao(this.TestLoggerFactory.CreateLogger<OutboxDao>());
            return Task.CompletedTask;
        }
    }
}
