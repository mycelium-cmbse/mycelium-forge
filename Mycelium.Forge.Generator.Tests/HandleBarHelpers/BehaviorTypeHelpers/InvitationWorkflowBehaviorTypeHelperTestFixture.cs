// ------------------------------------------------------------------------------------------------
// <copyright file="InvitationWorkflowBehaviorTypeHelperTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests.HandleBarHelpers.BehaviorTypeHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Mycelium.Forge.Generator.Constants;
    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;
    using Mycelium.Forge.Generator.HandleBarHelpers.BehaviorTypeHelpers;

    using uml4net.Extensions;
    using uml4net.StructuredClassifiers;

    [TestFixture]
    public class InvitationWorkflowBehaviorTypeHelperTestFixture
    {
        private InvitationWorkflowBehaviorTypeHelper helper;
        private IClass packageInvitationClass;
        private EntityPermissionDefinition definition;
        private EntityBehaviorDefinition behavior;

        [SetUp]
        public void SetUp()
        {
            this.helper = new InvitationWorkflowBehaviorTypeHelper();

            this.packageInvitationClass = GeneratorSetupFixture.XmiReaderResult.Packages.SelectMany(package => package.QueryPackages()).SelectMany(package => package.PackagedElement.OfType<IClass>()).Single(x => x.Name == "PackageInvitation");

            this.definition = new EntityPermissionDefinition
            {
                EntityName = "PackageInvitation",
                CreatePermission = "InvitePackageMaintainer",
                ReadPermission = "ViewAllPackageInvitations",
                UpdatePermission = "UpdatePackageInvitation",
                DeletePermission = "DeletePackageInvitation",
                OwnerProperty = "Owner"
            };

            this.behavior = new EntityBehaviorDefinition
            {
                EntityName = "PackageInvitation",
                BehaviorType = "InvitationWorkflow",
                Configuration = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { ConfigurationKeys.ScopeEntity, "Package" },
                    { ConfigurationKeys.ScopeRoles, "PackageOwner" },
                    { ConfigurationKeys.InviteeProperty, "Target" },
                    { ConfigurationKeys.AdminPermission, "ManagePackageTeam" }
                }
            };
        }

        [Test]
        public void VerifyBuildReadFilterPredicate()
        {
            var defaultPredicate = this.helper.BuildReadFilterPredicate(this.packageInvitationClass, this.definition, this.behavior, _ => string.Empty);

            var bypassBehavior = new EntityBehaviorDefinition
            {
                EntityName = "PackageInvitation",
                BehaviorType = "InvitationWorkflow",
                Configuration = new Dictionary<string, string>(this.behavior.Configuration, StringComparer.OrdinalIgnoreCase)
                {
                    { ConfigurationKeys.BypassPermissions, "ManagePackageTeam, ViewAllPackageInvitations" }
                }
            };

            var bypassHelper = new InvitationWorkflowBehaviorTypeHelper();
            var bypassPredicate = bypassHelper.BuildReadFilterPredicate(this.packageInvitationClass, this.definition, bypassBehavior, _ => string.Empty);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(defaultPredicate, Does.Contain("@canViewAllPackageInvitations = true"));
                Assert.That(defaultPredicate, Does.Contain("package_invitation.owner = @callerAccountId"));
                Assert.That(defaultPredicate, Does.Contain("package_invitation.target = @callerAccountId"));
                Assert.That(defaultPredicate, Does.Contain("package_package_owner__account"));
                Assert.That(bypassPredicate, Does.Contain("@canManagePackageTeam = true OR @canViewAllPackageInvitations = true"));
            }
        }

        [Test]
        public void VerifyHandlesOperation()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.helper.HandlesOperation(Operations.Create), Is.True);
                Assert.That(this.helper.HandlesOperation(Operations.Read), Is.True);
                Assert.That(this.helper.HandlesOperation(Operations.Update), Is.True);
                Assert.That(this.helper.HandlesOperation(Operations.Delete), Is.True);
            }
        }

        [Test]
        public void VerifyIsAsyncMethod()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.helper.IsAsyncMethod(Operations.Create), Is.True);
                Assert.That(this.helper.IsAsyncMethod(Operations.Read), Is.True);
                Assert.That(this.helper.IsAsyncMethod(Operations.Update), Is.True);
                Assert.That(this.helper.IsAsyncMethod(Operations.Delete), Is.True);
            }
        }

        [Test]
        public void VerifyWriteIsAllowedToCreate()
        {
            var stringBuilder = new StringBuilder();
            this.helper.WriteIsAllowedToCreate(stringBuilder, this.packageInvitationClass, this.definition, this.behavior);
            var generated = stringBuilder.ToString();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(generated, Does.Contain("PermissionKind.InvitePackageMaintainer"));
                Assert.That(generated, Does.Contain("this.packageService.ReadAsync(userContext, transaction, CancellationToken.None, [toCreate.Package])"));
                Assert.That(generated, Does.Contain("package.PackageOwner.Contains(accountId)"));
            }
        }

        [Test]
        public void VerifyWriteIsAllowedToDelete()
        {
            var stringBuilder = new StringBuilder();
            this.helper.WriteIsAllowedToDelete(stringBuilder, this.packageInvitationClass, this.definition, this.behavior);
            var generated = stringBuilder.ToString();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(generated, Does.Contain("thing.Owner == accountId"));
                Assert.That(generated, Does.Contain("PermissionKind.ManagePackageTeam"));
                Assert.That(generated, Does.Contain("this.packageService.ReadAsync(userContext, transaction, CancellationToken.None, [thing.Package])"));
                Assert.That(generated, Does.Contain("package.PackageOwner.Contains(accountId)"));
            }
        }

        [Test]
        public void VerifyWriteIsAllowedToRead()
        {
            var stringBuilder = new StringBuilder();
            this.helper.WriteIsAllowedToRead(stringBuilder, this.packageInvitationClass, this.definition, this.behavior);
            var generated = stringBuilder.ToString();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(generated, Does.Contain("PermissionKind.ViewAllPackageInvitations"));
                Assert.That(generated, Does.Contain("thing.Owner == accountId || thing.Target == accountId"));
                Assert.That(generated, Does.Contain("this.packageService.ReadAsync(userContext, transaction, CancellationToken.None, [thing.Package])"));
                Assert.That(generated, Does.Contain("package.PackageOwner.Contains(accountId)"));
            }
        }

        [Test]
        public void VerifyWriteIsAllowedToUpdate()
        {
            var stringBuilder = new StringBuilder();
            this.helper.WriteIsAllowedToUpdate(stringBuilder, this.packageInvitationClass, this.definition, this.behavior, []);
            var generated = stringBuilder.ToString();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(generated, Does.Contain("existingThing.Status != updatedThing.Status"));
                Assert.That(generated, Does.Contain("updatedThing.Status == InvitationStatusKind.REVOKED"));
                Assert.That(generated, Does.Contain("var updateScopeResult = await this.packageService.ReadAsync(userContext, transaction, CancellationToken.None, [existingThing.Package]);"));
                Assert.That(generated, Does.Contain("if (!updateScopeResult.IsError && updateScopeResult.Value.Count > 0)"));
                Assert.That(generated, Does.Contain("var package = updateScopeResult.Value[0];"));
                Assert.That(generated, Does.Contain("if (package.PackageOwner.Contains(accountId))"));
            }
        }
    }
}
