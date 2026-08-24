// ------------------------------------------------------------------------------------------------
// <copyright file="AccountSettingsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.AccountSettings
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using FluentResults;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.AccountSettings;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Models.Organization;
    using Mycelium.Forge.Models.Profile;
    using Mycelium.Forge.ViewModels.AccountSettings;

    [TestFixture]
    public class AccountSettingsTestFixture
    {
        private BunitContext context;
        private Mock<IAccountSettingsViewModel> viewModelMock;
        private DialogService dialogService;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IAccountSettingsViewModel>();

            var account = new Account
            {
                Name = "Alex Rivera",
                ShortName = "alex.rivera",
                Email = "alex.rivera@example.com",
                Origin = "Darmstadt, Germany",
                Website = "https://stariongroup.eu"
            };

            var profile = new UserProfileModel(account, "Starion Group", "MBSE engineer and space enthusiast.");

            this.viewModelMock.SetupGet(x => x.Profile).Returns(profile);

            this.viewModelMock.SetupGet(x => x.Organizations).Returns(
            [
                new AccountOrganizationMembershipModel(new Organization { Name = "Starion Group", ShortName = "starion" }, OrganizationInvitationKind.ADMINISTRATOR),
                new AccountOrganizationMembershipModel(new Organization { Name = "European Space Agency", ShortName = "esa" })
            ]);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.dialogService = this.context.Services.GetRequiredService<DialogService>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyHandleCreateOrganization()
        {
            var result = new CreateOrganizationResult { OrganizationName = "New Org", Scope = "@neworg" };
            this.viewModelMock.Setup(x => x.CreateOrganization(result)).Returns(Result.Ok());

            var accountSettingsPage = this.context.Render<AccountSettings>();
            var handleResult = accountSettingsPage.Instance.HandleCreateOrganization(result);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(handleResult.IsSuccess, Is.True);
                this.viewModelMock.Verify(x => x.CreateOrganization(result), Times.Once);
            }
        }

        [Test]
        public void VerifyOnCreateOrganization()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();
            var transferButton = accountSettingsPage.Find("#account-settings-transfer-org-button");

            _ = accountSettingsPage.InvokeAsync(() => transferButton.ClickAsync());

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task VerifyOnDeactivateAccount()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();
            var deactivateButton = accountSettingsPage.Find("#account-settings-deactivate-button");

            await accountSettingsPage.InvokeAsync(() => deactivateButton.ClickAsync());

            this.viewModelMock.Verify(x => x.DeactivateAccount(), Times.Once);
        }

        [Test]
        public void VerifyOnDeleteAccount()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();
            var deleteButton = accountSettingsPage.Find("#account-settings-delete-button");

            _ = accountSettingsPage.InvokeAsync(() => deleteButton.ClickAsync());

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
        }

        [Test]
        public void VerifyOnInitialized()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(accountSettingsPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel(), Times.Once);
            }
        }

        [Test]
        public void VerifyStubMethods()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();

            var changeUsernameBtn = accountSettingsPage.Find("#account-settings-change-username-button");
            var changeEmailBtn = accountSettingsPage.Find("#account-settings-change-email-button");
            var editDisplayNameBtn = accountSettingsPage.Find("#account-settings-edit-displayname-button");
            var editCompanyBtn = accountSettingsPage.Find("#account-settings-edit-company-button");
            var editLocationBtn = accountSettingsPage.Find("#account-settings-edit-location-button");
            var editWebsiteBtn = accountSettingsPage.Find("#account-settings-edit-website-button");
            var editBiographyBtn = accountSettingsPage.Find("#account-settings-edit-biography-button");

            Assert.That(async () =>
            {
                await accountSettingsPage.InvokeAsync(() => changeUsernameBtn.ClickAsync());
                await accountSettingsPage.InvokeAsync(() => changeEmailBtn.ClickAsync());
                await accountSettingsPage.InvokeAsync(() => editDisplayNameBtn.ClickAsync());
                await accountSettingsPage.InvokeAsync(() => editCompanyBtn.ClickAsync());
                await accountSettingsPage.InvokeAsync(() => editLocationBtn.ClickAsync());
                await accountSettingsPage.InvokeAsync(() => editWebsiteBtn.ClickAsync());
                await accountSettingsPage.InvokeAsync(() => editBiographyBtn.ClickAsync());
            }, Throws.Nothing);
        }
    }
}
