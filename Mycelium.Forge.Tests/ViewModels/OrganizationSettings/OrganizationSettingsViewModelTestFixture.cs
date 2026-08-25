// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationSettingsViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.OrganizationSettings
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.ViewModels.OrganizationSettings;

    [TestFixture]
    public class OrganizationSettingsViewModelTestFixture
    {
        private OrganizationSettingsViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new OrganizationSettingsViewModel();
        }

        [Test]
        public void VerifyChangeMemberRole()
        {
            this.viewModel.InitializeViewModel("starion");
            var member = this.viewModel.Members[0];

            this.viewModel.ChangeMemberRole(member, OrganizationInvitationKind.MEMBER);

            Assert.That(member.Role, Is.EqualTo(OrganizationInvitationKind.MEMBER));
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.InitializeViewModel("starion");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Organization, Is.Not.Null);
                Assert.That(this.viewModel.CurrentUserRole, Is.EqualTo(OrganizationInvitationKind.ADMINISTRATOR));
                Assert.That(this.viewModel.Members, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.PendingInvitations, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.RoleOptions, Has.Count.EqualTo(2));
            }
        }

        [Test]
        public void VerifyRemoveMember()
        {
            this.viewModel.InitializeViewModel("starion");
            var initialCount = this.viewModel.Members.Count;
            var member = this.viewModel.Members[0];

            this.viewModel.RemoveMember(member);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Members, Has.Count.EqualTo(initialCount - 1));
                Assert.That(this.viewModel.Members, Does.Not.Contain(member));
            }
        }

        [Test]
        public void VerifyResendInvitation()
        {
            this.viewModel.InitializeViewModel("starion");
            var invitation = this.viewModel.PendingInvitations[0];

            Assert.DoesNotThrow(() => this.viewModel.ResendInvitation(invitation));
        }

        [Test]
        public void VerifyRevokeInvitation()
        {
            this.viewModel.InitializeViewModel("starion");
            var initialCount = this.viewModel.PendingInvitations.Count;
            var invitation = this.viewModel.PendingInvitations[0];

            this.viewModel.RevokeInvitation(invitation);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.PendingInvitations, Has.Count.EqualTo(initialCount - 1));
                Assert.That(this.viewModel.PendingInvitations, Does.Not.Contain(invitation));
            }
        }

        [Test]
        public void VerifyTransferOrganization()
        {
            Assert.DoesNotThrow(() => this.viewModel.TransferOrganization());
        }
    }
}
