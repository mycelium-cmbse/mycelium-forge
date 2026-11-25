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
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using DynamicData;

    using ErrorOr;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Components.Pages.AccountSettings;
    using Mycelium.Forge.Components.Pages.AccountSettings.Dialogs;
    using Mycelium.Forge.Dal.Services;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.Tests.TestHelpers;
    using Mycelium.Forge.ViewModels.AccountSettings;

    [TestFixture]
    public class AccountSettingsTestFixture
    {
        private BunitContext context;
        private Mock<IAccountSettingsViewModel> viewModelMock;
        private Mock<IAddressesManagerViewModel> addressesManagerViewModelMock;
        private IRenderedComponent<BbDialogProvider> dialogProvider;
        private Account testAccount;
        private Address primaryAddress;
        private Organization adminOrganization;
        private Organization memberOrganization;
        private SourceList<IOrganization> organizationsSourceList;
        private SourceList<Address> addressesSourceList;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IAccountSettingsViewModel>();
            this.addressesManagerViewModelMock = new Mock<IAddressesManagerViewModel>();

            var accountId = Guid.NewGuid();
            var addressId = Guid.NewGuid();
            var countryId = Guid.NewGuid();

            this.testAccount = new Account
            {
                Id = accountId,
                Name = "Alex Rivera",
                ShortName = "alex.rivera",
                Email = "alex.rivera@example.com",
                Origin = "Darmstadt, Germany",
                Website = "https://stariongroup.eu",
                PrimaryAddress = addressId,
                IsVerified = true
            };

            this.primaryAddress = new Address
            {
                Id = addressId,
                AddressLine1 = "1 Main Street",
                Locality = "Darmstadt",
                Country = countryId
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

            this.viewModelMock.Setup(x => x.Profile).Returns(() => this.testAccount);
            this.organizationsSourceList = new SourceList<IOrganization>();
            this.organizationsSourceList.AddRange([this.adminOrganization, this.memberOrganization]);
            this.viewModelMock.Setup(x => x.Organizations).Returns(this.organizationsSourceList);

            this.addressesSourceList = new SourceList<Address>();
            this.addressesSourceList.Add(this.primaryAddress);
            this.addressesManagerViewModelMock.Setup(x => x.Addresses).Returns(this.addressesSourceList);
            this.addressesManagerViewModelMock.Setup(x => x.Countries).Returns([new Country { Id = countryId, Name = "Germany" }]);
            this.addressesManagerViewModelMock.Setup(x => x.PrimaryAddressId).Returns(addressId);
            this.viewModelMock.Setup(x => x.AddressesManagerViewModel).Returns(this.addressesManagerViewModelMock.Object);

            var pendingRequestsManagerViewModelMock = new Mock<IPendingRequestsManagerViewModel>();
            pendingRequestsManagerViewModelMock.Setup(x => x.PendingInvitations).Returns([]);
            this.viewModelMock.Setup(x => x.PendingRequestsManagerViewModel).Returns(pendingRequestsManagerViewModelMock.Object);

            var scopeServiceMock = new Mock<IScopeService>();
            scopeServiceMock.Setup(x => x.ReadByShortNameAsync(It.IsAny<IUserContext>(), It.IsAny<CancellationToken>(), It.IsAny<string[]>())).ReturnsAsync(ImmutableList<IScope>.Empty);

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserContext(false)).ReturnsAsync(new UserContext { AccountId = accountId });

            this.context.Services.AddSingleton(scopeServiceMock.Object);
            this.context.Services.AddSingleton(userServiceMock.Object);
            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.dialogProvider = this.context.Render<BbDialogProvider>();
        }

        [TearDown]
        public async Task TearDown()
        {
            this.addressesSourceList.Dispose();
            this.organizationsSourceList.Dispose();
            this.dialogProvider.Dispose();
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyOnCreateOrganization()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();
            var transferButton = accountSettingsPage.Find("#account-settings-create-org-button");

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
        public async Task VerifyOnEditDisplayName()
        {
            this.viewModelMock.Setup(x => x.UpdateProfile(It.IsAny<Account>())).ReturnsAsync(Result.Success);

            var accountSettingsPage = this.context.Render<AccountSettings>();
            var editDisplayNameButton = accountSettingsPage.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("account-settings-edit-displayname-button"));
            Assert.That(editDisplayNameButton, Is.Not.Null);

            // Case 1: Confirm with trimmed display name
            _ = accountSettingsPage.InvokeAsync(() => editDisplayNameButton!.Find("button").ClickAsync());

            var dialog = this.dialogProvider.FindComponent<InputPromptDialog>();
            var input = dialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-input"));
            var saveButton = dialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-confirm-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(input, Is.Not.Null);
                Assert.That(saveButton, Is.Not.Null);
            }

            await this.dialogProvider.InvokeAsync(() => input!.Instance.ValueChanged.InvokeAsync("  Alex Updated  "));
            await this.dialogProvider.InvokeAsync(() => saveButton!.Find("button").ClickAsync());

            this.viewModelMock.Verify(x => x.UpdateProfile(It.Is<Account>(a => a.Id == this.testAccount.Id && a.Name == "Alex Updated")), Times.Once);

            // Case 2: Cancel does not update profile
            _ = accountSettingsPage.InvokeAsync(() => editDisplayNameButton!.Find("button").ClickAsync());

            var cancelDialog = this.dialogProvider.FindComponent<InputPromptDialog>();
            var cancelButton = cancelDialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-cancel-button"));
            Assert.That(cancelButton, Is.Not.Null);

            await this.dialogProvider.InvokeAsync(() => cancelButton!.Find("button").ClickAsync());

            this.viewModelMock.Verify(x => x.UpdateProfile(It.IsAny<Account>()), Times.Once);

            // Case 3: Single-word display name fails inline validation and does not update profile
            _ = accountSettingsPage.InvokeAsync(() => editDisplayNameButton!.Find("button").ClickAsync());

            var invalidDialog = this.dialogProvider.FindComponent<InputPromptDialog>();
            var singleWordInput = invalidDialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-input"));
            var saveSingleButton = invalidDialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-confirm-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(singleWordInput, Is.Not.Null);
                Assert.That(saveSingleButton, Is.Not.Null);
            }

            await this.dialogProvider.InvokeAsync(() => singleWordInput!.Instance.ValueChanged.InvokeAsync("Regis"));
            await this.dialogProvider.InvokeAsync(() => saveSingleButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidDialog.Instance.ValidationManager.HasError(nameof(InputPromptDialog.InputValue)), Is.True);
                this.viewModelMock.Verify(x => x.UpdateProfile(It.IsAny<Account>()), Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnChangeEmail()
        {
            this.viewModelMock.Setup(x => x.UpdateProfile(It.IsAny<Account>())).ReturnsAsync(Result.Success);

            var accountSettingsPage = this.context.Render<AccountSettings>();
            var changeEmailButton = accountSettingsPage.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("account-settings-change-email-button"));
            Assert.That(changeEmailButton, Is.Not.Null);

            _ = accountSettingsPage.InvokeAsync(() => changeEmailButton!.Find("button").ClickAsync());

            var dialog = this.dialogProvider.FindComponent<ChangeEmailDialog>();
            Assert.That(dialog.Instance.CurrentEmail, Is.EqualTo("alex.rivera@example.com"));

            var input = dialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("change-email-input"));
            var submitButton = dialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("change-email-submit-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(input, Is.Not.Null);
                Assert.That(submitButton, Is.Not.Null);
            }

            await this.dialogProvider.InvokeAsync(() => input!.Instance.ValueChanged.InvokeAsync("  Alex.New@Example.com "));
            await this.dialogProvider.InvokeAsync(() => submitButton!.Find("button").ClickAsync());

            this.viewModelMock.Verify(x => x.UpdateProfile(It.Is<Account>(a => a.Id == this.testAccount.Id && a.Email == "alex.new@example.com" && !a.IsVerified)), Times.Once);
        }

        [Test]
        public async Task VerifyOnEditWebsite()
        {
            this.viewModelMock.Setup(x => x.UpdateProfile(It.IsAny<Account>())).ReturnsAsync(Result.Success);

            var accountSettingsPage = this.context.Render<AccountSettings>();
            var editWebsiteButton = accountSettingsPage.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("account-settings-edit-website-button"));
            Assert.That(editWebsiteButton, Is.Not.Null);

            // Case 1: Invalid URL fails validation and does not update profile
            _ = accountSettingsPage.InvokeAsync(() => editWebsiteButton!.Find("button").ClickAsync());

            var invalidDialog = this.dialogProvider.FindComponent<InputPromptDialog>();
            var invalidInput = invalidDialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-input"));
            var saveInvalidButton = invalidDialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-confirm-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidDialog.Instance.InitialValue, Is.EqualTo("https://stariongroup.eu"));
                Assert.That(invalidInput, Is.Not.Null);
                Assert.That(saveInvalidButton, Is.Not.Null);
            }

            await this.dialogProvider.InvokeAsync(() => invalidInput!.Instance.ValueChanged.InvokeAsync("ftp://invalid.example"));
            await this.dialogProvider.InvokeAsync(() => saveInvalidButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidDialog.Instance.ValidationManager.HasError(nameof(InputPromptDialog.InputValue)), Is.True);
                this.viewModelMock.Verify(x => x.UpdateProfile(It.IsAny<Account>()), Times.Never);
            }

            // Close invalid dialog via cancel button
            var cancelButton = invalidDialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-cancel-button"));
            Assert.That(cancelButton, Is.Not.Null);
            await this.dialogProvider.InvokeAsync(() => cancelButton!.Find("button").ClickAsync());

            // Case 2: Valid URL updates profile Website
            _ = accountSettingsPage.InvokeAsync(() => editWebsiteButton!.Find("button").ClickAsync());

            var validDialog = this.dialogProvider.FindComponent<InputPromptDialog>();
            var validInput = validDialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-input"));
            var saveValidButton = validDialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-confirm-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validInput, Is.Not.Null);
                Assert.That(saveValidButton, Is.Not.Null);
            }

            await this.dialogProvider.InvokeAsync(() => validInput!.Instance.ValueChanged.InvokeAsync("https://example.org"));
            await this.dialogProvider.InvokeAsync(() => saveValidButton!.Find("button").ClickAsync());

            this.viewModelMock.Verify(x => x.UpdateProfile(It.Is<Account>(a => a.Id == this.testAccount.Id && a.Website == "https://example.org")), Times.Once);

            // Case 3: Empty website clears Website to null
            _ = accountSettingsPage.InvokeAsync(() => editWebsiteButton!.Find("button").ClickAsync());

            var clearDialog = this.dialogProvider.FindComponent<InputPromptDialog>();
            var clearInput = clearDialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-input"));
            var saveClearButton = clearDialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-confirm-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(clearInput, Is.Not.Null);
                Assert.That(saveClearButton, Is.Not.Null);
            }

            await this.dialogProvider.InvokeAsync(() => clearInput!.Instance.ValueChanged.InvokeAsync("   "));
            await this.dialogProvider.InvokeAsync(() => saveClearButton!.Find("button").ClickAsync());

            this.viewModelMock.Verify(x => x.UpdateProfile(It.Is<Account>(a => a.Id == this.testAccount.Id && a.Website == null)), Times.Once);
        }

        [Test]
        public void VerifyOnInitialized()
        {
            var accountSettingsPage = this.context.Render<AccountSettings>();
            var addressesManager = accountSettingsPage.FindComponent<AddressesManager>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(accountSettingsPage.Instance, Is.Not.Null);
                Assert.That(addressesManager, Is.Not.Null);
                Assert.That(addressesManager.Instance.ViewModel, Is.EqualTo(this.addressesManagerViewModelMock.Object));
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
    }
}
