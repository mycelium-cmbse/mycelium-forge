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

    using Mycelium.Forge.Dal.Events;
    using Mycelium.Forge.Orm.Outbox;
    using Mycelium.Forge.Orm.Tests.Helper;

    using Npgsql;

    using NpgsqlTypes;

    using NUnit.Framework;

    [TestFixture]
    [Category("Database")]
    public class OutboxDaoTestFixture : BaseIntegrationTestClassFixture
    {
        private const string ProfileChangedEventType = "Account.ProfileChanged";

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

            var firstForAggregate1 = new OutboxMessage { AggregateId = aggregate1, EventType = ProfileChangedEventType, Payload = "{\"step\":1}" };
            var secondForAggregate1 = new OutboxMessage { AggregateId = aggregate1, EventType = ProfileChangedEventType, Payload = "{\"step\":2}" };
            var firstForAggregate2 = new OutboxMessage { AggregateId = aggregate2, EventType = ProfileChangedEventType, Payload = "{\"step\":3}" };
            var futureMessage = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = ProfileChangedEventType, Payload = "{\"future\":true}", NextAttemptAt = DateTimeOffset.UtcNow.AddMinutes(5) };
            var failedMessage = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = ProfileChangedEventType, Payload = "{\"failed\":true}", Status = OutboxMessageStatus.Failed };

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
                Assert.That(firstClaim.All(m => m.ClaimToken.HasValue && m.ClaimToken.Value != Guid.Empty), Is.True);
            }

            // While firstForAggregate1 is leased, secondForAggregate1 must remain blocked
            await using (var leasedTransaction = await this.Connection.BeginTransactionAsync())
            {
                var leasedClaim = await this.outboxDao.ClaimBatchAsync(leasedTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
                await leasedTransaction.CommitAsync();

                Assert.That(leasedClaim.Value, Is.Empty);
            }

            // Deleting firstForAggregate1 with its claim token unblocks secondForAggregate1
            var claimedFirst1 = firstClaim.First(m => m.Id == firstForAggregate1.Id);
            var claimedFirst2 = firstClaim.First(m => m.Id == firstForAggregate2.Id);

            await using (var deleteTransaction = await this.Connection.BeginTransactionAsync())
            {
                await this.outboxDao.DeleteAsync(deleteTransaction, claimedFirst1.Id, claimedFirst1.ClaimToken!.Value, CancellationToken.None);
                await this.outboxDao.DeleteAsync(deleteTransaction, claimedFirst2.Id, claimedFirst2.ClaimToken!.Value, CancellationToken.None);
                await deleteTransaction.CommitAsync();
            }

            IReadOnlyList<OutboxMessage> unblockedClaim;

            await using (var unblockedTransaction = await this.Connection.BeginTransactionAsync())
            {
                var unblockedResult = await this.outboxDao.ClaimBatchAsync(unblockedTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
                await unblockedTransaction.CommitAsync();
                unblockedClaim = unblockedResult.Value;

                using (Assert.EnterMultipleScope())
                {
                    Assert.That(unblockedClaim, Has.Count.EqualTo(1));
                    Assert.That(unblockedClaim[0].Id, Is.EqualTo(secondForAggregate1.Id));
                }
            }

            await using (var deleteSecondTransaction = await this.Connection.BeginTransactionAsync())
            {
                await this.outboxDao.DeleteAsync(deleteSecondTransaction, unblockedClaim[0].Id, unblockedClaim[0].ClaimToken!.Value, CancellationToken.None);
                await deleteSecondTransaction.CommitAsync();
            }

            // Different event types for the SAME aggregate are ordered together; marking the older one Failed unblocks the newer one.
            var sharedAggregate = Guid.NewGuid();
            var olderProfileChange = new OutboxMessage { AggregateId = sharedAggregate, EventType = ProfileChangedEventType, Payload = "{\"seq\":1}" };
            var newerAccountDelete = new OutboxMessage { AggregateId = sharedAggregate, EventType = "Account.Deleted", Payload = "{\"seq\":2}" };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, olderProfileChange, CancellationToken.None));
            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, newerAccountDelete, CancellationToken.None));

            IReadOnlyList<OutboxMessage> sharedAggregateClaim;

            await using (var multiEventClaimTransaction = await this.Connection.BeginTransactionAsync())
            {
                var multiEventClaim = await this.outboxDao.ClaimBatchAsync(multiEventClaimTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
                await multiEventClaimTransaction.CommitAsync();
                sharedAggregateClaim = multiEventClaim.Value;

                using (Assert.EnterMultipleScope())
                {
                    Assert.That(sharedAggregateClaim, Has.Count.EqualTo(1));
                    Assert.That(sharedAggregateClaim[0].Id, Is.EqualTo(olderProfileChange.Id));
                }
            }

            await using (var markOlderFailedTransaction = await this.Connection.BeginTransactionAsync())
            {
                await this.outboxDao.MarkFailedAsync(markOlderFailedTransaction, olderProfileChange.Id, sharedAggregateClaim[0].ClaimToken!.Value, "Permanent 400", CancellationToken.None);
                await markOlderFailedTransaction.CommitAsync();
            }

            IReadOnlyList<OutboxMessage> unblockedDeleteClaim;

            await using (var unblockedByFailureTransaction = await this.Connection.BeginTransactionAsync())
            {
                var unblockedByFailureResult = await this.outboxDao.ClaimBatchAsync(unblockedByFailureTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
                await unblockedByFailureTransaction.CommitAsync();
                unblockedDeleteClaim = unblockedByFailureResult.Value;

                using (Assert.EnterMultipleScope())
                {
                    Assert.That(unblockedDeleteClaim, Has.Count.EqualTo(1));
                    Assert.That(unblockedDeleteClaim[0].Id, Is.EqualTo(newerAccountDelete.Id));
                }
            }

            await using (var deleteUnblockedTransaction = await this.Connection.BeginTransactionAsync())
            {
                await this.outboxDao.DeleteAsync(deleteUnblockedTransaction, unblockedDeleteClaim[0].Id, unblockedDeleteClaim[0].ClaimToken!.Value, CancellationToken.None);
                await deleteUnblockedTransaction.CommitAsync();
            }

            // Case 3: Lease expiration allows reclaim with incremented attempts and a fresh claim token, fencing out the old owner
            var leaseMessage = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = ProfileChangedEventType, Payload = "{\"lease\":true}" };
            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, leaseMessage, CancellationToken.None));

            IReadOnlyList<OutboxMessage> firstLeaseClaim;

            await using (var shortLeaseTransaction = await this.Connection.BeginTransactionAsync())
            {
                var firstLeaseResult = await this.outboxDao.ClaimBatchAsync(shortLeaseTransaction, 10, TimeSpan.FromMilliseconds(100), CancellationToken.None);
                await shortLeaseTransaction.CommitAsync();
                firstLeaseClaim = firstLeaseResult.Value;
            }

            await Task.Delay(250);

            IReadOnlyList<OutboxMessage> reclaimed;

            await using (var reclaimTransaction = await this.Connection.BeginTransactionAsync())
            {
                var reclaimedResult = await this.outboxDao.ClaimBatchAsync(reclaimTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
                await reclaimTransaction.CommitAsync();
                reclaimed = reclaimedResult.Value;
            }

            ErrorOr<Deleted> staleOwnerDeleteResult;

            await using (var staleOwnerTransaction = await this.Connection.BeginTransactionAsync())
            {
                staleOwnerDeleteResult = await this.outboxDao.DeleteAsync(staleOwnerTransaction, leaseMessage.Id, firstLeaseClaim[0].ClaimToken!.Value, CancellationToken.None);
                await staleOwnerTransaction.CommitAsync();
            }

            using (Assert.EnterMultipleScope())
            {
                Assert.That(reclaimed, Has.Count.EqualTo(1));
                Assert.That(reclaimed[0].Id, Is.EqualTo(leaseMessage.Id));
                Assert.That(reclaimed[0].Attempts, Is.EqualTo(2));
                Assert.That(reclaimed[0].ClaimToken, Is.Not.EqualTo(firstLeaseClaim[0].ClaimToken));
                Assert.That(staleOwnerDeleteResult.IsError, Is.True);
                Assert.That(staleOwnerDeleteResult.FirstError.Type, Is.EqualTo(ErrorType.Conflict));
            }

            // Clean up before batchSize limit and UUIDv7 ordering test
            await using (var batchCleanupCommand = new NpgsqlCommand("DELETE FROM forge.outbox_message;", this.Connection))
            {
                await batchCleanupCommand.ExecuteNonQueryAsync();
            }

            // Case 4: batchSize limit respects UUIDv7 ordering across distinct aggregates
            var orderedMessage1 = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = ProfileChangedEventType, Payload = "{\"order\":1}" };
            var orderedMessage2 = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = ProfileChangedEventType, Payload = "{\"order\":2}" };
            var orderedMessage3 = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = ProfileChangedEventType, Payload = "{\"order\":3}" };

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
            var concurrentMessage1 = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = ProfileChangedEventType, Payload = "{\"worker\":1}" };
            var concurrentMessage2 = new OutboxMessage { AggregateId = Guid.NewGuid(), EventType = ProfileChangedEventType, Payload = "{\"worker\":2}" };

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

            // Case 6: Non-null LastError round-trips via MapToOutboxMessage when reclaiming a rescheduled message
            await using (var rescheduleWithErrorTransaction = await this.Connection.BeginTransactionAsync())
            {
                await this.outboxDao.RescheduleAsync(rescheduleWithErrorTransaction, worker1Claim.Value[0].Id, worker1Claim.Value[0].ClaimToken!.Value, DateTimeOffset.UtcNow.AddSeconds(-1), "Transient 503", CancellationToken.None);
                await rescheduleWithErrorTransaction.CommitAsync();
            }

            await using (var reclaimWithErrorTransaction = await this.Connection.BeginTransactionAsync())
            {
                var reclaimWithError = await this.outboxDao.ClaimBatchAsync(reclaimWithErrorTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
                await reclaimWithErrorTransaction.CommitAsync();

                using (Assert.EnterMultipleScope())
                {
                    Assert.That(reclaimWithError.IsError, Is.False);
                    Assert.That(reclaimWithError.Value, Has.Count.EqualTo(1));
                    Assert.That(reclaimWithError.Value[0].Id, Is.EqualTo(worker1Claim.Value[0].Id));
                    Assert.That(reclaimWithError.Value[0].LastError, Is.EqualTo("Transient 503"));
                }
            }

            // Case 7: Negative batchSize returns Error.Failure and canceled token throws OperationCanceledException
            await using var invalidBatchTransaction = await this.Connection.BeginTransactionAsync();
            var negativeBatchResult = await this.outboxDao.ClaimBatchAsync(invalidBatchTransaction, -1, TimeSpan.FromSeconds(30), CancellationToken.None);
            await invalidBatchTransaction.RollbackAsync();

            Assert.That(negativeBatchResult.IsError, Is.True);
            await this.AssertThrowsOperationCanceledAsync((transaction, token) => this.outboxDao.ClaimBatchAsync(transaction, 10, TimeSpan.FromSeconds(30), token));
        }

        [Test]
        public async Task VerifyDeleteAsync()
        {
            var message = new OutboxMessage
            {
                AggregateId = Guid.NewGuid(),
                EventType = ProfileChangedEventType,
                Payload = "{}"
            };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, message, CancellationToken.None));

            var claimed = await this.ClaimSingleAsync();

            ErrorOr<Deleted> wrongTokenResult;

            await using (var wrongTokenTransaction = await this.Connection.BeginTransactionAsync())
            {
                wrongTokenResult = await this.outboxDao.DeleteAsync(wrongTokenTransaction, message.Id, Guid.NewGuid(), CancellationToken.None);
                await wrongTokenTransaction.CommitAsync();
            }

            ErrorOr<Deleted> deleteExistingResult;

            await using (var deleteTransaction = await this.Connection.BeginTransactionAsync())
            {
                deleteExistingResult = await this.outboxDao.DeleteAsync(deleteTransaction, message.Id, claimed.ClaimToken!.Value, CancellationToken.None);
                await deleteTransaction.CommitAsync();
            }

            ErrorOr<Deleted> deleteMissingResult;

            await using (var missingTransaction = await this.Connection.BeginTransactionAsync())
            {
                deleteMissingResult = await this.outboxDao.DeleteAsync(missingTransaction, Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);
                await missingTransaction.CommitAsync();
            }

            await using var countCommand = new NpgsqlCommand("SELECT COUNT(*) FROM forge.outbox_message WHERE id = @id;", this.Connection);
            countCommand.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = message.Id });
            var remainingCount = (long)(await countCommand.ExecuteScalarAsync())!;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(wrongTokenResult.IsError, Is.True);
                Assert.That(wrongTokenResult.FirstError.Type, Is.EqualTo(ErrorType.Conflict));
                Assert.That(deleteExistingResult.IsError, Is.False);
                Assert.That(deleteMissingResult.IsError, Is.True);
                Assert.That(deleteMissingResult.FirstError.Type, Is.EqualTo(ErrorType.Conflict));
                Assert.That(remainingCount, Is.EqualTo(0));
            }

            await this.AssertThrowsOperationCanceledAsync((transaction, token) => this.outboxDao.DeleteAsync(transaction, message.Id, claimed.ClaimToken!.Value, token));
        }

        [Test]
        public async Task VerifyEnqueueAsync()
        {
            var aggregateId = Guid.NewGuid();

            var message = new OutboxMessage
            {
                AggregateId = aggregateId,
                EventType = ProfileChangedEventType,
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
                Assert.That(reader.GetString(1), Is.EqualTo(ProfileChangedEventType));
                Assert.That(reader.GetString(2), Does.Contain("11111111-1111-1111-1111-111111111111"));
                Assert.That(reader.GetString(3), Is.EqualTo("PENDING"));
                Assert.That(reader.GetInt32(4), Is.EqualTo(0));
                Assert.That(reader.IsDBNull(5), Is.True);
            }

            await reader.CloseAsync();

            // Typed IOutboxEvent overload serializes EventType, AggregateId, and JSON payload
            var typedAggregateId = Guid.NewGuid();
            var typedEvent = new AccountProfileChanged(typedAggregateId, "typed@example.com", "Alex", "Rivera");

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, typedEvent, CancellationToken.None));

            await using var typedReadCommand = new NpgsqlCommand("SELECT event_type, payload::text FROM forge.outbox_message WHERE aggregate_id = @aggregateId;", this.Connection);
            typedReadCommand.Parameters.Add(new NpgsqlParameter("@aggregateId", NpgsqlDbType.Uuid) { Value = typedAggregateId });

            await using var typedReader = await typedReadCommand.ExecuteReaderAsync();
            var hasTypedRow = await typedReader.ReadAsync();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(hasTypedRow, Is.True);
                Assert.That(typedReader.GetString(0), Is.EqualTo(AccountProfileChanged.EventType));
                Assert.That(typedReader.GetString(1), Does.Contain("typed@example.com"));
            }

            await typedReader.CloseAsync();

            // Invalid JSONB payload returns Error.Failure
            var invalidMessage = new OutboxMessage
            {
                AggregateId = aggregateId,
                EventType = ProfileChangedEventType,
                Payload = "{invalid-json"
            };

            await using var failTransaction = await this.Connection.BeginTransactionAsync();
            var invalidResult = await this.outboxDao.EnqueueAsync(failTransaction, invalidMessage, CancellationToken.None);
            await failTransaction.RollbackAsync();

            var unsupportedStatusMessage = new OutboxMessage
            {
                AggregateId = aggregateId,
                EventType = ProfileChangedEventType,
                Payload = "{}",
                Status = (OutboxMessageStatus)999
            };

            await using var unsupportedStatusTransaction = await this.Connection.BeginTransactionAsync();
            var unsupportedStatusResult = await this.outboxDao.EnqueueAsync(unsupportedStatusTransaction, unsupportedStatusMessage, CancellationToken.None);
            await unsupportedStatusTransaction.RollbackAsync();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidResult.IsError, Is.True);
                Assert.That(unsupportedStatusResult.IsError, Is.True);
            }

            await this.AssertThrowsOperationCanceledAsync((transaction, token) => this.outboxDao.EnqueueAsync(transaction, message, token));
        }

        [Test]
        public async Task VerifyMarkFailedAsync()
        {
            var message = new OutboxMessage
            {
                AggregateId = Guid.NewGuid(),
                EventType = ProfileChangedEventType,
                Payload = "{}"
            };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, message, CancellationToken.None));

            var claimed = await this.ClaimSingleAsync();

            ErrorOr<Updated> wrongTokenResult;

            await using (var wrongTokenTransaction = await this.Connection.BeginTransactionAsync())
            {
                wrongTokenResult = await this.outboxDao.MarkFailedAsync(wrongTokenTransaction, message.Id, Guid.NewGuid(), "Stale worker", CancellationToken.None);
                await wrongTokenTransaction.CommitAsync();
            }

            ErrorOr<Updated> markFailedResult;

            await using (var failTransaction = await this.Connection.BeginTransactionAsync())
            {
                markFailedResult = await this.outboxDao.MarkFailedAsync(failTransaction, message.Id, claimed.ClaimToken!.Value, "400 Bad Request", CancellationToken.None);
                await failTransaction.CommitAsync();
            }

            ErrorOr<Updated> notFoundResult;

            await using (var notFoundTransaction = await this.Connection.BeginTransactionAsync())
            {
                notFoundResult = await this.outboxDao.MarkFailedAsync(notFoundTransaction, Guid.NewGuid(), Guid.NewGuid(), "Missing", CancellationToken.None);
                await notFoundTransaction.CommitAsync();
            }

            await using var readCommand = new NpgsqlCommand("SELECT status, last_error, claim_token FROM forge.outbox_message WHERE id = @id;", this.Connection);
            readCommand.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = message.Id });

            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            var persistedStatus = reader.GetString(0);
            var persistedLastError = reader.GetString(1);
            var persistedClaimToken = reader.IsDBNull(2);
            await reader.CloseAsync();

            await using var claimTransaction = await this.Connection.BeginTransactionAsync();
            var claimAfterFailed = await this.outboxDao.ClaimBatchAsync(claimTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
            await claimTransaction.CommitAsync();

            var secondMessage = new OutboxMessage
            {
                AggregateId = Guid.NewGuid(),
                EventType = ProfileChangedEventType,
                Payload = "{}"
            };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, secondMessage, CancellationToken.None));
            var claimedSecond = await this.ClaimSingleAsync();

            await using (var nullErrorFailTransaction = await this.Connection.BeginTransactionAsync())
            {
                await this.outboxDao.MarkFailedAsync(nullErrorFailTransaction, secondMessage.Id, claimedSecond.ClaimToken!.Value, null, CancellationToken.None);
                await nullErrorFailTransaction.CommitAsync();
            }

            await using var nullErrorReadCommand = new NpgsqlCommand("SELECT last_error FROM forge.outbox_message WHERE id = @id;", this.Connection);
            nullErrorReadCommand.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = secondMessage.Id });
            var clearedLastError = await nullErrorReadCommand.ExecuteScalarAsync();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(wrongTokenResult.IsError, Is.True);
                Assert.That(wrongTokenResult.FirstError.Type, Is.EqualTo(ErrorType.Conflict));
                Assert.That(markFailedResult.IsError, Is.False);
                Assert.That(persistedStatus, Is.EqualTo("FAILED"));
                Assert.That(persistedLastError, Is.EqualTo("400 Bad Request"));
                Assert.That(persistedClaimToken, Is.True);
                Assert.That(claimAfterFailed.Value, Is.Empty);
                Assert.That(notFoundResult.IsError, Is.True);
                Assert.That(notFoundResult.FirstError.Type, Is.EqualTo(ErrorType.Conflict));
                Assert.That(clearedLastError, Is.EqualTo(DBNull.Value));
            }
        }

        [Test]
        public async Task VerifyRescheduleAsync()
        {
            var message = new OutboxMessage
            {
                AggregateId = Guid.NewGuid(),
                EventType = ProfileChangedEventType,
                Payload = "{}"
            };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, message, CancellationToken.None));

            var claimed = await this.ClaimSingleAsync();
            var futureAttempt = DateTimeOffset.UtcNow.AddMinutes(10);

            ErrorOr<Updated> wrongTokenResult;

            await using (var wrongTokenTransaction = await this.Connection.BeginTransactionAsync())
            {
                wrongTokenResult = await this.outboxDao.RescheduleAsync(wrongTokenTransaction, message.Id, Guid.NewGuid(), futureAttempt, "Stale worker", CancellationToken.None);
                await wrongTokenTransaction.CommitAsync();
            }

            ErrorOr<Updated> rescheduleResult;

            await using (var rescheduleTransaction = await this.Connection.BeginTransactionAsync())
            {
                rescheduleResult = await this.outboxDao.RescheduleAsync(rescheduleTransaction, message.Id, claimed.ClaimToken!.Value, futureAttempt, "Transient 503 error", CancellationToken.None);
                await rescheduleTransaction.CommitAsync();
            }

            ErrorOr<Updated> notFoundResult;

            await using (var notFoundTransaction = await this.Connection.BeginTransactionAsync())
            {
                notFoundResult = await this.outboxDao.RescheduleAsync(notFoundTransaction, Guid.NewGuid(), Guid.NewGuid(), futureAttempt, "Missing", CancellationToken.None);
                await notFoundTransaction.CommitAsync();
            }

            await using var readCommand = new NpgsqlCommand("SELECT status, next_attempt_at, last_error, claim_token FROM forge.outbox_message WHERE id = @id;", this.Connection);
            readCommand.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = message.Id });

            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            var persistedStatus = reader.GetString(0);
            var persistedNextAttempt = reader.GetFieldValue<DateTimeOffset>(1);
            var persistedLastError = reader.GetString(2);
            var persistedClaimTokenIsNull = reader.IsDBNull(3);
            await reader.CloseAsync();

            await using var claimTransaction = await this.Connection.BeginTransactionAsync();
            var claimAfterReschedule = await this.outboxDao.ClaimBatchAsync(claimTransaction, 10, TimeSpan.FromSeconds(30), CancellationToken.None);
            await claimTransaction.CommitAsync();

            var failedMessage = new OutboxMessage
            {
                AggregateId = Guid.NewGuid(),
                EventType = ProfileChangedEventType,
                Payload = "{}",
                Status = OutboxMessageStatus.Failed
            };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, failedMessage, CancellationToken.None));

            ErrorOr<Updated> rescheduleFailedResult;

            await using (var rescheduleFailedTransaction = await this.Connection.BeginTransactionAsync())
            {
                rescheduleFailedResult = await this.outboxDao.RescheduleAsync(rescheduleFailedTransaction, failedMessage.Id, Guid.NewGuid(), futureAttempt, "Should not reschedule", CancellationToken.None);
                await rescheduleFailedTransaction.CommitAsync();
            }

            await using var failedReadCommand = new NpgsqlCommand("SELECT status FROM forge.outbox_message WHERE id = @id;", this.Connection);
            failedReadCommand.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = failedMessage.Id });
            var failedStatus = (string)(await failedReadCommand.ExecuteScalarAsync())!;

            var nullErrorMessage = new OutboxMessage
            {
                AggregateId = Guid.NewGuid(),
                EventType = ProfileChangedEventType,
                Payload = "{}"
            };

            await this.Insert(transaction => this.outboxDao.EnqueueAsync(transaction, nullErrorMessage, CancellationToken.None));
            var claimedNullError = await this.ClaimSingleAsync();

            await using (var nullErrorRescheduleTransaction = await this.Connection.BeginTransactionAsync())
            {
                await this.outboxDao.RescheduleAsync(nullErrorRescheduleTransaction, nullErrorMessage.Id, claimedNullError.ClaimToken!.Value, futureAttempt, null, CancellationToken.None);
                await nullErrorRescheduleTransaction.CommitAsync();
            }

            await using var nullErrorReadCommand = new NpgsqlCommand("SELECT last_error FROM forge.outbox_message WHERE id = @id;", this.Connection);
            nullErrorReadCommand.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = nullErrorMessage.Id });
            var clearedLastError = await nullErrorReadCommand.ExecuteScalarAsync();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(wrongTokenResult.IsError, Is.True);
                Assert.That(wrongTokenResult.FirstError.Type, Is.EqualTo(ErrorType.Conflict));
                Assert.That(rescheduleResult.IsError, Is.False);
                Assert.That(persistedStatus, Is.EqualTo("PENDING"));
                Assert.That(persistedNextAttempt, Is.EqualTo(futureAttempt).Within(TimeSpan.FromSeconds(1)));
                Assert.That(persistedLastError, Is.EqualTo("Transient 503 error"));
                Assert.That(persistedClaimTokenIsNull, Is.True);
                Assert.That(claimAfterReschedule.Value, Is.Empty);
                Assert.That(notFoundResult.IsError, Is.True);
                Assert.That(notFoundResult.FirstError.Type, Is.EqualTo(ErrorType.Conflict));
                Assert.That(rescheduleFailedResult.IsError, Is.True);
                Assert.That(rescheduleFailedResult.FirstError.Type, Is.EqualTo(ErrorType.Conflict));
                Assert.That(failedStatus, Is.EqualTo("FAILED"));
                Assert.That(clearedLastError, Is.EqualTo(DBNull.Value));
            }

            await this.AssertThrowsOperationCanceledAsync((transaction, token) => this.outboxDao.RescheduleAsync(transaction, message.Id, claimed.ClaimToken!.Value, futureAttempt, "Canceled", token));
        }

        protected override Task PostSetup()
        {
            this.outboxDao = new OutboxDao(this.TestLoggerFactory.CreateLogger<OutboxDao>());
            return Task.CompletedTask;
        }

        private async Task<OutboxMessage> ClaimSingleAsync()
        {
            await using var transaction = await this.Connection.BeginTransactionAsync();
            var result = await this.outboxDao.ClaimBatchAsync(transaction, 1, TimeSpan.FromSeconds(30), CancellationToken.None);
            await transaction.CommitAsync();

            Assert.That(result.IsError, Is.False);
            Assert.That(result.Value, Has.Count.EqualTo(1));
            return result.Value[0];
        }

        private async Task AssertThrowsOperationCanceledAsync(Func<NpgsqlTransaction, CancellationToken, Task> operation)
        {
            var canceledSource = new CancellationTokenSource();
            await canceledSource.CancelAsync();
            var token = canceledSource.Token;
            var canceledTransaction = await this.Connection.BeginTransactionAsync(CancellationToken.None);

            try
            {
                Assert.That(async () => await operation(canceledTransaction, token), Throws.InstanceOf<OperationCanceledException>());
            }
            finally
            {
                await canceledTransaction.RollbackAsync(CancellationToken.None);
                await canceledTransaction.DisposeAsync();
                canceledSource.Dispose();
            }
        }
    }
}
