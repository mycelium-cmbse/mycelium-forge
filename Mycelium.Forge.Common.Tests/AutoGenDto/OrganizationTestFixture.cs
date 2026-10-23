// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Tests.AutoGenDto
{
    using System;

    [TestFixture]
    public class OrganizationTestFixture
    {
        [Test]
        public void VerifyClone()
        {
            var adminId = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var addressId = Guid.NewGuid();

            var original = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Acme Corp",
                ShortName = "acme",
                Email = "contact@acme.com",
                BillingEmail = "billing@acme.com",
                DefaultPackageVisibility = VisibilityKind.INTERNAL,
                Status = ScopeStatusKind.ACTIVE,
                Origin = "local",
                Owner = Guid.NewGuid(),
                PrimaryAddress = addressId,
                CreatedAt = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                ModifiedAt = new DateTime(2026, 1, 2, 12, 0, 0, DateTimeKind.Utc),
                LogoBlobReference = "blobs/acme-logo.png",
                Website = "https://acme.com",
                Administrator = [adminId],
                Member = [adminId, memberId],
                Address = [addressId],
                OwnedPackage = [Guid.NewGuid()],
                ProfileLink = [Guid.NewGuid()]
            };

            var clone = original.Clone();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(clone, Is.Not.SameAs(original));
                Assert.That(clone.Id, Is.EqualTo(original.Id));
                Assert.That(clone.Name, Is.EqualTo(original.Name));
                Assert.That(clone.ShortName, Is.EqualTo(original.ShortName));
                Assert.That(clone.Email, Is.EqualTo(original.Email));
                Assert.That(clone.BillingEmail, Is.EqualTo(original.BillingEmail));
                Assert.That(clone.DefaultPackageVisibility, Is.EqualTo(original.DefaultPackageVisibility));
                Assert.That(clone.Status, Is.EqualTo(original.Status));
                Assert.That(clone.Origin, Is.EqualTo(original.Origin));
                Assert.That(clone.Owner, Is.EqualTo(original.Owner));
                Assert.That(clone.PrimaryAddress, Is.EqualTo(original.PrimaryAddress));
                Assert.That(clone.CreatedAt, Is.EqualTo(original.CreatedAt));
                Assert.That(clone.ModifiedAt, Is.EqualTo(original.ModifiedAt));
                Assert.That(clone.LogoBlobReference, Is.EqualTo(original.LogoBlobReference));
                Assert.That(clone.Website, Is.EqualTo(original.Website));

                Assert.That(clone.Administrator, Is.Not.SameAs(original.Administrator));
                Assert.That(clone.Administrator, Is.EqualTo(original.Administrator));
                Assert.That(clone.Member, Is.Not.SameAs(original.Member));
                Assert.That(clone.Member, Is.EqualTo(original.Member));
                Assert.That(clone.Address, Is.Not.SameAs(original.Address));
                Assert.That(clone.Address, Is.EqualTo(original.Address));
                Assert.That(clone.OwnedPackage, Is.Not.SameAs(original.OwnedPackage));
                Assert.That(clone.OwnedPackage, Is.EqualTo(original.OwnedPackage));
                Assert.That(clone.ProfileLink, Is.Not.SameAs(original.ProfileLink));
                Assert.That(clone.ProfileLink, Is.EqualTo(original.ProfileLink));
            }

            clone.Name = "Mutated Corp";
            clone.Administrator.Clear();
            clone.Member.Add(Guid.NewGuid());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(original.Name, Is.EqualTo("Acme Corp"));
                Assert.That(original.Administrator, Has.Count.EqualTo(1));
                Assert.That(original.Member, Has.Count.EqualTo(2));
            }
        }
    }
}
