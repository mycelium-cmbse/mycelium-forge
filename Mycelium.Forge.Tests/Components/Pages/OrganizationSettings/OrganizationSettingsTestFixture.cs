// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationSettingsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.OrganizationSettings
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.OrganizationSettings;
    using Mycelium.Forge.Models.Organization;
    using Mycelium.Forge.ViewModels.OrganizationSettings;

    [TestFixture]
    public class OrganizationSettingsTestFixture
    {
        private BunitContext context;
        private Mock<IOrganizationSettingsViewModel> viewModelMock;
        private OrganizationMemberModel testMember;
        private OrganizationInvitationModel testInvitation;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IOrganizationSettingsViewModel>();

            var org = new OrganizationModel(
                new Organization { Name = "Starion Group", ShortName = "starion", Origin = "Systems engineering" })
            {
                MemberSinceYear = 2023
            };

            this.testMember = new OrganizationMemberModel(
                new Account { Name = "Alex Rivera", ShortName = "alex.rivera" },
                OrganizationInvitationKind.ADMINISTRATOR);

            this.testInvitation = new OrganizationInvitationModel("a.novak@esa.int", OrganizationInvitationKind.ADMINISTRATOR, "Sent 2 days ago");

            this.viewModelMock.SetupGet(x => x.Organization).Returns(org);
            this.viewModelMock.SetupGet(x => x.CurrentUserRole).Returns(OrganizationInvitationKind.ADMINISTRATOR);
            this.viewModelMock.SetupGet(x => x.Members).Returns([this.testMember]);
            this.viewModelMock.SetupGet(x => x.PendingInvitations).Returns([this.testInvitation]);
            this.viewModelMock.SetupGet(x => x.RoleOptions).Returns([OrganizationInvitationKind.ADMINISTRATOR, OrganizationInvitationKind.MEMBER]);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyChangeMemberRole()
        {
            var orgSettingsPage = this.context.Render<OrganizationSettings>();

            orgSettingsPage.Instance.OnChangeMemberRole(this.testMember, OrganizationInvitationKind.MEMBER);

            this.viewModelMock.Verify(x => x.ChangeMemberRole(this.testMember, OrganizationInvitationKind.MEMBER), Times.Once);
        }

        [Test]
        public void VerifyOnParametersSet()
        {
            var orgSettingsPage = this.context.Render<OrganizationSettings>(parameters => parameters
                .Add(p => p.Id, "starion"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(orgSettingsPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel("starion"), Times.Once);
            }
        }

        [Test]
        public async Task VerifyRemoveMember()
        {
            var orgSettingsPage = this.context.Render<OrganizationSettings>();
            var removeButton = orgSettingsPage.Find(".org-remove-member-button");

            await orgSettingsPage.InvokeAsync(() => removeButton.ClickAsync());

            this.viewModelMock.Verify(x => x.RemoveMember(this.testMember), Times.Once);
        }

        [Test]
        public async Task VerifyResendInvitation()
        {
            var orgSettingsPage = this.context.Render<OrganizationSettings>();
            var resendButton = orgSettingsPage.Find(".org-resend-invitation-button");

            await orgSettingsPage.InvokeAsync(() => resendButton.ClickAsync());

            this.viewModelMock.Verify(x => x.ResendInvitation(this.testInvitation), Times.Once);
        }

        [Test]
        public async Task VerifyRevokeInvitation()
        {
            var orgSettingsPage = this.context.Render<OrganizationSettings>();
            var revokeButton = orgSettingsPage.Find(".org-revoke-invitation-button");

            await orgSettingsPage.InvokeAsync(() => revokeButton.ClickAsync());

            this.viewModelMock.Verify(x => x.RevokeInvitation(this.testInvitation), Times.Once);
        }

        [Test]
        public async Task VerifyTransferOrganization()
        {
            var orgSettingsPage = this.context.Render<OrganizationSettings>();
            var transferButton = orgSettingsPage.Find("#org-settings-transfer-button");

            await orgSettingsPage.InvokeAsync(() => transferButton.ClickAsync());

            this.viewModelMock.Verify(x => x.TransferOrganization(), Times.Once);
        }
    }
}
