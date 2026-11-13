// ------------------------------------------------------------------------------------------------
// <copyright file="PackageInvitationRowViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.Rows
{
    using System;
    using System.Linq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.ViewModels.Rows;

    [TestFixture]
    public class PackageInvitationRowViewModelTestFixture
    {
        [Test]
        public void VerifyGenerateRows()
        {
            var organizationId = Guid.NewGuid();
            var packageWithScopeId = Guid.NewGuid();
            var packageWithoutScopeId = Guid.NewGuid();
            var privatePackageId = Guid.NewGuid();

            var organization = new Organization
            {
                Id = organizationId,
                Name = "Starion Group",
                ShortName = "starion"
            };

            var packageWithScope = new Package
            {
                Id = packageWithScopeId,
                Name = "ECSS Power",
                ShortName = "ecss-power",
                Owner = organizationId
            };

            var packageWithoutScope = new Package
            {
                Id = packageWithoutScopeId,
                Name = "Standalone Tool",
                ShortName = "standalone-tool",
                Owner = Guid.NewGuid()
            };

            var ownerInvitation = new PackageInvitation
            {
                Id = Guid.NewGuid(),
                Package = packageWithScopeId,
                Target = Guid.NewGuid(),
                PackageInvitationKind = PackageInvitationKind.OWNER,
                Status = InvitationStatusKind.PENDING,
                ExperiesAt = DateTime.UtcNow.AddDays(3)
            };

            var maintainerInvitation = new PackageInvitation
            {
                Id = Guid.NewGuid(),
                Package = packageWithoutScopeId,
                Target = Guid.NewGuid(),
                PackageInvitationKind = PackageInvitationKind.MAINTAINER,
                Status = InvitationStatusKind.PENDING,
                ExperiesAt = DateTime.UtcNow.AddHours(-1)
            };

            var privateInvitation = new PackageInvitation
            {
                Id = Guid.NewGuid(),
                Package = privatePackageId,
                Target = Guid.NewGuid(),
                PackageInvitationKind = PackageInvitationKind.MAINTAINER,
                Status = InvitationStatusKind.PENDING,
                ExperiesAt = DateTime.UtcNow.AddDays(2)
            };

            var rows = PackageInvitationRowViewModel.GenerateRows([ownerInvitation, maintainerInvitation, privateInvitation], [organization, packageWithScope, packageWithoutScope]).ToList();
            var nullRelatedRows = PackageInvitationRowViewModel.GenerateRows([privateInvitation], null!).ToList();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(rows, Has.Count.EqualTo(3));

                Assert.That(rows[0].Invitation, Is.EqualTo(ownerInvitation));
                Assert.That(((IInvitationRowViewModel)rows[0]).Invitation, Is.EqualTo(ownerInvitation));
                Assert.That(rows[0].Package, Is.EqualTo(packageWithScope));
                Assert.That(rows[0].Scope, Is.EqualTo(organization));
                Assert.That(rows[0].RoleName, Is.EqualTo("Owner"));
                Assert.That(rows[0].ExpiresIn, Does.StartWith("Expires in "));

                Assert.That(rows[1].Package, Is.EqualTo(packageWithoutScope));
                Assert.That(rows[1].Scope, Is.Null);
                Assert.That(rows[1].RoleName, Is.EqualTo("Maintainer"));
                Assert.That(rows[1].ExpiresIn, Is.EqualTo("Expired"));

                Assert.That(rows[2].Package, Is.Null);
                Assert.That(rows[2].Scope, Is.Null);
                Assert.That(rows[2].RoleName, Is.EqualTo("Maintainer"));

                Assert.That(nullRelatedRows, Has.Count.EqualTo(1));
                Assert.That(nullRelatedRows[0].Package, Is.Null);
                Assert.That(nullRelatedRows[0].Scope, Is.Null);
            }
        }
    }
}
