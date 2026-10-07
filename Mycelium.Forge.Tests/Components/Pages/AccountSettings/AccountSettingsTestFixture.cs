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
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.AccountSettings;
    using Mycelium.Forge.ViewModels.AccountSettings;

    [TestFixture]
    public class AccountSettingsTestFixture
    {
        private BunitContext context;
        private Mock<IAccountSettingsViewModel> viewModelMock;
        private DialogService dialogService;
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
            this.dialogService = this.context.Services.GetRequiredService<DialogService>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
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
        public void VerifyOnDeactivateAccount()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();
            var deactivateButton = accountSettingsPage.Find("#account-settings-deactivate-button");

            _ = accountSettingsPage.InvokeAsync(() => deactivateButton.ClickAsync());

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
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
