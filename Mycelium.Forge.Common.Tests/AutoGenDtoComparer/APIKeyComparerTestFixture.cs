// ------------------------------------------------------------------------------------------------
// <copyright file="APIKeyComparerTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Tests.AutoGenDtoComparer
{
    using System;
    using System.Linq;

    using Mycelium.Forge.Common.AutoGenDtoComparer;

    /// <summary>
    /// Suite of tests for the <see cref="APIKeyComparer" /> class.
    /// </summary>
    [TestFixture]
    public class APIKeyComparerTestFixture
    {
        private APIKeyComparer comparer;

        [SetUp]
        public void SetUp()
        {
            this.comparer = new APIKeyComparer();
        }

        /// <summary>
        /// Verifies that <see cref="APIKeyComparer.Compare" /> returns empty differences when either argument is null or for identical
        /// instances, and identifies modified scalar and collection properties.
        /// </summary>
        [Test]
        public void VerifyCompare()
        {
            var baseDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var permissionsGuid = Guid.NewGuid();

            var originalApiKey = new APIKey
            {
                Id = Guid.NewGuid(),
                CreatedAt = baseDate,
                ExpiresAt = baseDate.AddDays(30),
                LastUsedAt = baseDate.AddDays(1),
                ModifiedAt = baseDate.AddHours(2),
                Name = "OriginalKey",
                Permissions = permissionsGuid,
                RevokedAt = baseDate.AddDays(10),
                SecretHash = [1, 2, 3]
            };

            var identicalApiKey = new APIKey
            {
                Id = originalApiKey.Id,
                CreatedAt = originalApiKey.CreatedAt,
                ExpiresAt = originalApiKey.ExpiresAt,
                LastUsedAt = originalApiKey.LastUsedAt,
                ModifiedAt = originalApiKey.ModifiedAt,
                Name = originalApiKey.Name,
                Permissions = originalApiKey.Permissions,
                RevokedAt = originalApiKey.RevokedAt,
                SecretHash = [1, 2, 3]
            };

            var differentPermissionsGuid = Guid.NewGuid();

            var modifiedApiKey = new APIKey
            {
                Id = originalApiKey.Id,
                CreatedAt = baseDate.AddDays(1),
                ExpiresAt = baseDate.AddDays(60),
                LastUsedAt = baseDate.AddDays(5),
                ModifiedAt = baseDate.AddDays(2),
                Name = "ModifiedKey",
                Permissions = differentPermissionsGuid,
                RevokedAt = baseDate.AddDays(20),
                SecretHash = [4, 5, 6]
            };

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.comparer.Compare(null, originalApiKey), Is.Empty);
                Assert.That(this.comparer.Compare(originalApiKey, null), Is.Empty);
                Assert.That(this.comparer.Compare(null, null), Is.Empty);
            }

            var noChanges = this.comparer.Compare(originalApiKey, identicalApiKey).ToList();
            Assert.That(noChanges, Is.Empty);

            var changes = this.comparer.Compare(originalApiKey, modifiedApiKey).ToList();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(changes, Has.Count.EqualTo(8));

                var createdAtChange = changes.Single(change => change.PropertyName == nameof(IAPIKey.CreatedAt));
                Assert.That(createdAtChange.OldValue, Is.EqualTo(originalApiKey.CreatedAt));
                Assert.That(createdAtChange.NewValue, Is.EqualTo(modifiedApiKey.CreatedAt));

                var expiresAtChange = changes.Single(change => change.PropertyName == nameof(IAPIKey.ExpiresAt));
                Assert.That(expiresAtChange.OldValue, Is.EqualTo(originalApiKey.ExpiresAt));
                Assert.That(expiresAtChange.NewValue, Is.EqualTo(modifiedApiKey.ExpiresAt));

                var lastUsedAtChange = changes.Single(change => change.PropertyName == nameof(IAPIKey.LastUsedAt));
                Assert.That(lastUsedAtChange.OldValue, Is.EqualTo(originalApiKey.LastUsedAt));
                Assert.That(lastUsedAtChange.NewValue, Is.EqualTo(modifiedApiKey.LastUsedAt));

                var modifiedAtChange = changes.Single(change => change.PropertyName == nameof(IAPIKey.ModifiedAt));
                Assert.That(modifiedAtChange.OldValue, Is.EqualTo(originalApiKey.ModifiedAt));
                Assert.That(modifiedAtChange.NewValue, Is.EqualTo(modifiedApiKey.ModifiedAt));

                var nameChange = changes.Single(change => change.PropertyName == nameof(IAPIKey.Name));
                Assert.That(nameChange.OldValue, Is.EqualTo(originalApiKey.Name));
                Assert.That(nameChange.NewValue, Is.EqualTo(modifiedApiKey.Name));

                var permissionsChange = changes.Single(change => change.PropertyName == nameof(IAPIKey.Permissions));
                Assert.That(permissionsChange.OldValue, Is.EqualTo(originalApiKey.Permissions));
                Assert.That(permissionsChange.NewValue, Is.EqualTo(modifiedApiKey.Permissions));

                var revokedAtChange = changes.Single(change => change.PropertyName == nameof(IAPIKey.RevokedAt));
                Assert.That(revokedAtChange.OldValue, Is.EqualTo(originalApiKey.RevokedAt));
                Assert.That(revokedAtChange.NewValue, Is.EqualTo(modifiedApiKey.RevokedAt));

                var secretHashChange = changes.Single(change => change.PropertyName == nameof(IAPIKey.SecretHash));
                Assert.That(secretHashChange.OldValue, Is.EqualTo(originalApiKey.SecretHash));
                Assert.That(secretHashChange.NewValue, Is.EqualTo(modifiedApiKey.SecretHash));
            }
        }
    }
}
