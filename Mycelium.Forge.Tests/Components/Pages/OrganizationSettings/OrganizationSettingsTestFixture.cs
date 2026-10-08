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
    using System;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Components.Pages.OrganizationSettings;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.ViewModels.OrganizationSettings;

    [TestFixture]
    public class OrganizationSettingsTestFixture
    {
        private BunitContext context;
        private Mock<IOrganizationSettingsViewModel> viewModelMock;
        private Organization testOrganization;
        private Account testMember;
        private OrganizationInvitation testInvitation;

        private DialogService dialogService;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IOrganizationSettingsViewModel>();

            var orgId = Guid.NewGuid();
            var memberId = Guid.NewGuid();

            this.testOrganization = new Organization
            {
                Id = orgId,
                Name = "Starion Group",
                ShortName = "starion",
                Origin = "Systems engineering",
                Administrator = [memberId],
                OwnedPackage = [Guid.NewGuid()]
            };

            this.testMember = new Account
            {
                Id = memberId,
                Name = "Alex Rivera",
                ShortName = "alex.rivera"
            };

            this.testInvitation = new OrganizationInvitation
            {
                Id = Guid.NewGuid(),
                Organization = orgId,
                Target = Guid.NewGuid(),
                OrganizationInvitationKind = OrganizationInvitationKind.ADMINISTRATOR,
                Status = InvitationStatusKind.PENDING,
                ExperiesAt = DateTime.UtcNow.AddDays(2)
            };

            this.viewModelMock.Setup(x => x.Organization).Returns(this.testOrganization);
            this.viewModelMock.Setup(x => x.CurrentUserRole).Returns(OrganizationRole.Administrator);
            this.viewModelMock.Setup(x => x.CanManageOrganization).Returns(true);
            this.viewModelMock.Setup(x => x.Members).Returns([this.testMember]);
            this.viewModelMock.Setup(x => x.PendingInvitations).Returns([this.testInvitation]);
            this.viewModelMock.Setup(x => x.InvitedAccounts).Returns([]);
            this.viewModelMock.Setup(x => x.RoleOptions).Returns([OrganizationRole.Administrator, OrganizationRole.Member]);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.dialogService = this.context.Services.GetRequiredService<DialogService>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyChangeMemberRole()
        {
            var orgSettingsPage = this.context.Render<OrganizationSettings>();
            var select = orgSettingsPage.FindComponent<ForgeSelect<OrganizationRole>>();

            await orgSettingsPage.InvokeAsync(() => select.Instance.ValueChanged.InvokeAsync(OrganizationRole.Member));

            this.viewModelMock.Verify(x => x.ChangeMemberRole(this.testMember, OrganizationRole.Member), Times.Once);
        }

        [Test]
        public void VerifyGetBreadcrumbItems()
        {
            var orgSettingsPage = this.context.Render<OrganizationSettings>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(orgSettingsPage.Markup, Does.Contain("@starion"));
                Assert.That(orgSettingsPage.Markup, Does.Contain("Settings"));
            }
        }

        [Test]
        public void VerifyOnParametersSet()
        {
            var orgSettingsPage = this.context.Render<OrganizationSettings>(parameters => parameters.Add(p => p.Id, "starion"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(orgSettingsPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel("starion"), Times.Once);
            }
        }

        [Test]
        public void VerifyRemoveMember()
        {
            var orgSettingsPage = this.context.Render<OrganizationSettings>();
            var removeButton = orgSettingsPage.Find(".org-remove-member-button");

            // Fire-and-forget is intentional: opening the modal awaits DialogService.OpenAsync until dismissed; awaiting here would deadlock the test.
            _ = orgSettingsPage.InvokeAsync(() => removeButton.ClickAsync());

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
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
