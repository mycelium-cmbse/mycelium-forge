// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationInvitationTestFixture.cs" company="Starion Group S.A.">
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
    public class OrganizationInvitationTestFixture
    {
        [Test]
        public void VerifyClone()
        {
            var original = new OrganizationInvitation
            {
                Id = Guid.NewGuid(),
                Organization = Guid.NewGuid(),
                Owner = Guid.NewGuid(),
                Target = Guid.NewGuid(),
                Status = InvitationStatusKind.PENDING,
                OrganizationInvitationKind = OrganizationInvitationKind.MEMBER,
                CreatedAt = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                ModifiedAt = new DateTime(2026, 1, 2, 12, 0, 0, DateTimeKind.Utc),
                ExperiesAt = new DateTime(2026, 1, 8, 10, 0, 0, DateTimeKind.Utc)
            };

            var clone = original.Clone();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(clone, Is.Not.SameAs(original));
                Assert.That(clone.Id, Is.EqualTo(original.Id));
                Assert.That(clone.Organization, Is.EqualTo(original.Organization));
                Assert.That(clone.Owner, Is.EqualTo(original.Owner));
                Assert.That(clone.Target, Is.EqualTo(original.Target));
                Assert.That(clone.Status, Is.EqualTo(original.Status));
                Assert.That(clone.OrganizationInvitationKind, Is.EqualTo(original.OrganizationInvitationKind));
                Assert.That(clone.CreatedAt, Is.EqualTo(original.CreatedAt));
                Assert.That(clone.ModifiedAt, Is.EqualTo(original.ModifiedAt));
                Assert.That(clone.ExperiesAt, Is.EqualTo(original.ExperiesAt));
            }

            clone.ExperiesAt = DateTime.UtcNow.AddDays(7);
            clone.Status = InvitationStatusKind.ACCEPTED;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(original.Status, Is.EqualTo(InvitationStatusKind.PENDING));
                Assert.That(original.ExperiesAt, Is.EqualTo(new DateTime(2026, 1, 8, 10, 0, 0, DateTimeKind.Utc)));
            }
        }
    }
}
