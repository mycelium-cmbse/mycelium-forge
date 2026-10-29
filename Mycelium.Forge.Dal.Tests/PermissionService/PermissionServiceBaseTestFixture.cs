// ------------------------------------------------------------------------------------------------
// <copyright file="PermissionServiceBaseTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Tests.PermissionService
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.AutoGenPermissionService;

    using Npgsql;

    using NUnit.Framework;

    [TestFixture]
    public class PermissionServiceBaseTestFixture
    {
        private APIKeyPermissionService permissionService;
        private NpgsqlTransaction transaction;
        private UserContext userContext;
        private DateTime timestamp;

        [SetUp]
        public void SetUp()
        {
            this.transaction = null;
            this.permissionService = new APIKeyPermissionService();
            this.timestamp = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            this.userContext = new UserContext
            {
                AccountId = Guid.NewGuid(),
                Username = "ownerUser",
                CurrentRoles = [RoleKind.Account]
            };
        }

        [Test]
        public async Task VerifyIsAllowedToCreate()
        {
            var valid = new APIKey { Id = Guid.NewGuid(), Owner = this.userContext.AccountId!.Value };
            var withCreatedAt = new APIKey { Id = Guid.NewGuid(), Owner = this.userContext.AccountId.Value, CreatedAt = this.timestamp };
            var withModifiedAt = new APIKey { Id = Guid.NewGuid(), Owner = this.userContext.AccountId.Value, ModifiedAt = this.timestamp };

            var validResult = await this.permissionService.IsAllowedToCreate(this.userContext, valid, this.transaction);
            var createdAtResult = await this.permissionService.IsAllowedToCreate(this.userContext, withCreatedAt, this.transaction);
            var modifiedAtResult = await this.permissionService.IsAllowedToCreate(this.userContext, withModifiedAt, this.transaction);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validResult.IsError, Is.False);
                Assert.That(createdAtResult.IsError, Is.False);
                Assert.That(modifiedAtResult.IsError, Is.False);
            }
        }

        [Test]
        public async Task VerifyIsAllowedToUpdate()
        {
            var existing = new APIKey { Id = Guid.NewGuid(), Owner = this.userContext.AccountId!.Value, CreatedAt = this.timestamp, ModifiedAt = this.timestamp };
            var unchanged = new APIKey { Id = existing.Id, Owner = existing.Owner, CreatedAt = this.timestamp, ModifiedAt = this.timestamp };
            var changedCreatedAt = new APIKey { Id = existing.Id, Owner = existing.Owner, CreatedAt = this.timestamp.AddDays(1), ModifiedAt = this.timestamp };
            var changedModifiedAt = new APIKey { Id = existing.Id, Owner = existing.Owner, CreatedAt = this.timestamp, ModifiedAt = this.timestamp.AddDays(1) };

            var unchangedResult = await this.permissionService.IsAllowedToUpdate(this.userContext, existing, unchanged, this.transaction);
            var createdAtResult = await this.permissionService.IsAllowedToUpdate(this.userContext, existing, changedCreatedAt, this.transaction);
            var modifiedAtResult = await this.permissionService.IsAllowedToUpdate(this.userContext, existing, changedModifiedAt, this.transaction);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unchangedResult.IsError, Is.False);
                Assert.That(createdAtResult.IsError, Is.False);
                Assert.That(modifiedAtResult.IsError, Is.False);
            }
        }
    }
}
