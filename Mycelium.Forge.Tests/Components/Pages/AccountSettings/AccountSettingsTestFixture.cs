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
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.AccountSettings;
    using Mycelium.Forge.Components.Pages.AccountSettings.Dialogs;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.ViewModels.AccountSettings;

    [TestFixture]
    public class AccountSettingsTestFixture
    {
        private BunitContext context;
        private Mock<IAccountSettingsViewModel> viewModelMock;
        private IRenderedComponent<BbDialogProvider> dialogProvider;
        private Account testAccount;
        private Organization adminOrganization;
        private Organization memberOrganization;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IAccountSettingsViewModel>();

            var accountId = Guid.NewGuid();

            this.testAccount = new Account
            {
                Id = accountId,
                Name = "Alex Rivera",
                ShortName = "alex.rivera",
                Email = "alex.rivera@example.com",
                Origin = "Darmstadt, Germany",
                Website = "https://stariongroup.eu",
                IsVerified = true
            };

            this.adminOrganization = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Starion Group",
                ShortName = "starion",
                Administrator = [accountId]
            };

            this.memberOrganization = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "European Space Agency",
                ShortName = "esa",
                Member = [accountId]
            };

            this.viewModelMock.Setup(x => x.Profile).Returns(this.testAccount);
            this.viewModelMock.Setup(x => x.Location).Returns("Darmstadt, Germany");
            this.viewModelMock.Setup(x => x.Organizations).Returns([this.adminOrganization, this.memberOrganization]);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.dialogProvider = this.context.Render<BbDialogProvider>();
        }

        [TearDown]
        public async Task TearDown()
        {
            this.dialogProvider.Dispose();
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyOnCreateOrganization()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();
            var transferButton = accountSettingsPage.Find("#account-settings-transfer-org-button");

            _ = accountSettingsPage.InvokeAsync(() => transferButton.ClickAsync());

            var dialog = this.dialogProvider.FindComponent<CreateOrganizationDialog>();

            var result = new CreateOrganizationResult
            {
                OrganizationName = "Starion Group",
                Scope = "starion",
                BillingEmail = "billing@stariongroup.eu"
            };

            await this.dialogProvider.InvokeAsync(() => dialog.Instance.OnResult.InvokeAsync(result));
            this.viewModelMock.Verify(x => x.CreateOrganization(It.IsAny<CreateOrganizationResult>()), Times.Once);
        }

        [Test]
        public async Task VerifyOnDeactivateAccount()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();
            var deactivateButton = accountSettingsPage.Find("#account-settings-deactivate-button");

            _ = accountSettingsPage.InvokeAsync(() => deactivateButton.ClickAsync());

            var continueButton = this.dialogProvider.FindAll("button").First(b => b.TextContent.Trim() == "Continue");
            await this.dialogProvider.InvokeAsync(() => continueButton.ClickAsync());

            this.viewModelMock.Verify(x => x.DeactivateAccount(), Times.Once);
        }

        [Test]
        public async Task VerifyOnDeleteAccount()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();
            var deleteButton = accountSettingsPage.Find("#account-settings-delete-button");

            _ = accountSettingsPage.InvokeAsync(() => deleteButton.ClickAsync());

            var deleteConfirmButton = this.dialogProvider.FindAll("button").First(b => b.TextContent.Trim() == "Delete");
            await this.dialogProvider.InvokeAsync(() => deleteConfirmButton.ClickAsync());

            this.viewModelMock.Verify(x => x.DeleteAccount(), Times.Once);
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
        public void VerifyOrganizations()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();

            var badges = accountSettingsPage.FindAll("#account-settings-organizations .badge-role");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(badges, Has.Count.EqualTo(2));
                Assert.That(badges[0].TextContent.Trim(), Is.EqualTo("Administrator"));
                Assert.That(badges[1].TextContent.Trim(), Is.EqualTo("Member"));
            }
        }

        [Test]
        public void VerifyStubMethods()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();

            var changeUsernameBtn = accountSettingsPage.Find("#account-settings-change-username-button");
            var changeEmailBtn = accountSettingsPage.Find("#account-settings-change-email-button");
            var editDisplayNameBtn = accountSettingsPage.Find("#account-settings-edit-displayname-button");
            var editLocationBtn = accountSettingsPage.Find("#account-settings-edit-location-button");
            var editWebsiteBtn = accountSettingsPage.Find("#account-settings-edit-website-button");

            Assert.That(async () =>
            {
                await accountSettingsPage.InvokeAsync(() => changeUsernameBtn.ClickAsync());
                await accountSettingsPage.InvokeAsync(() => changeEmailBtn.ClickAsync());
                await accountSettingsPage.InvokeAsync(() => editDisplayNameBtn.ClickAsync());
                await accountSettingsPage.InvokeAsync(() => editLocationBtn.ClickAsync());
                await accountSettingsPage.InvokeAsync(() => editWebsiteBtn.ClickAsync());
            }, Throws.Nothing);
        }
    }
}
