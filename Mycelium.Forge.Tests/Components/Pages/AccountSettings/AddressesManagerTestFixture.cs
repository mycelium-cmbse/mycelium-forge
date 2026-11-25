// ------------------------------------------------------------------------------------------------
// <copyright file="AddressesManagerTestFixture.cs" company="Starion Group S.A.">
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

    using DynamicData;

    using ErrorOr;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.AccountSettings;
    using Mycelium.Forge.Components.Pages.AccountSettings.Dialogs;
    using Mycelium.Forge.Tests.TestHelpers;
    using Mycelium.Forge.ViewModels.AccountSettings;

    [TestFixture]
    public class AddressesManagerTestFixture
    {
        private BunitContext context;
        private Mock<IAddressesManagerViewModel> viewModelMock;
        private IRenderedComponent<BbDialogProvider> dialogProvider;
        private SourceList<Address> addressesSourceList;
        private Country germany;
        private Address primaryAddress;
        private Address secondaryAddress;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.germany = new Country
            {
                Id = Guid.NewGuid(),
                Name = "Germany"
            };

            this.primaryAddress = new Address
            {
                Id = Guid.NewGuid(),
                AddressLine1 = "1 Main Street",
                Locality = "Darmstadt",
                Country = this.germany.Id
            };

            this.secondaryAddress = new Address
            {
                Id = Guid.NewGuid(),
                AddressLine1 = "2 Side Street",
                Locality = "Munich",
                Country = Guid.NewGuid()
            };

            this.addressesSourceList = new SourceList<Address>();
            this.viewModelMock = new Mock<IAddressesManagerViewModel>();
            this.viewModelMock.Setup(x => x.Addresses).Returns(this.addressesSourceList);
            this.viewModelMock.Setup(x => x.Countries).Returns([this.germany]);
            this.viewModelMock.Setup(x => x.PrimaryAddressId).Returns(this.primaryAddress.Id);
            this.viewModelMock.Setup(x => x.CreateAddress(It.IsAny<Address>())).ReturnsAsync(Result.Success);
            this.viewModelMock.Setup(x => x.UpdateAddress(It.IsAny<Address>())).ReturnsAsync(Result.Success);
            this.viewModelMock.Setup(x => x.SetPrimaryAddress(It.IsAny<Guid>())).ReturnsAsync(Result.Success);
            this.viewModelMock.Setup(x => x.DeleteAddress(It.IsAny<Guid>())).ReturnsAsync(Result.Success);

            this.dialogProvider = this.context.Render<BbDialogProvider>();
        }

        [TearDown]
        public async Task TearDown()
        {
            this.addressesSourceList.Dispose();
            this.dialogProvider.Dispose();
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyOnAddAddress()
        {
            var manager = this.context.Render<AddressesManager>(parameters => parameters.Add(x => x.ViewModel, this.viewModelMock.Object));
            var addButton = manager.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("account-settings-add-address-button"));
            Assert.That(addButton, Is.Not.Null);

            _ = manager.InvokeAsync(() => addButton!.Find("button").ClickAsync());

            var dialog = this.dialogProvider.FindComponent<AddressDialog>();

            var newAddress = new Address
            {
                Id = Guid.NewGuid(),
                AddressLine1 = "10 Space Way",
                Locality = "Darmstadt",
                Country = this.germany.Id
            };

            Assert.That(dialog.Instance.Countries, Has.Count.EqualTo(1));

            await this.dialogProvider.InvokeAsync(() => dialog.Instance.OnResult.InvokeAsync(newAddress));

            this.viewModelMock.Verify(x => x.CreateAddress(newAddress), Times.Once);
        }

        [Test]
        public async Task VerifyOnDeleteAddress()
        {
            this.addressesSourceList.Add(this.primaryAddress);
            var manager = this.context.Render<AddressesManager>(parameters => parameters.Add(x => x.ViewModel, this.viewModelMock.Object));

            var deleteButton = manager.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId($"address-delete-{this.primaryAddress.Id}"));
            Assert.That(deleteButton, Is.Not.Null);

            // Case 1: Cancel confirmation dialog
            _ = manager.InvokeAsync(() => deleteButton!.Find("button").ClickAsync());
            var cancelButton = this.dialogProvider.FindAll("button").First(b => b.TextContent.Trim() == "Cancel");
            await this.dialogProvider.InvokeAsync(() => cancelButton.ClickAsync());

            this.viewModelMock.Verify(x => x.DeleteAddress(It.IsAny<Guid>()), Times.Never);

            // Case 2: Confirm deletion
            _ = manager.InvokeAsync(() => deleteButton!.Find("button").ClickAsync());
            var confirmButton = this.dialogProvider.FindAll("button").First(b => b.TextContent.Trim() == "Delete");
            await this.dialogProvider.InvokeAsync(() => confirmButton.ClickAsync());

            this.viewModelMock.Verify(x => x.DeleteAddress(this.primaryAddress.Id), Times.Once);
        }

        [Test]
        public async Task VerifyOnEditAddress()
        {
            this.addressesSourceList.AddRange([this.primaryAddress, this.secondaryAddress]);
            var manager = this.context.Render<AddressesManager>(parameters => parameters.Add(x => x.ViewModel, this.viewModelMock.Object));

            // Case 1: Editing primary address
            var editPrimaryButton = manager.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId($"address-edit-{this.primaryAddress.Id}"));
            Assert.That(editPrimaryButton, Is.Not.Null);

            _ = manager.InvokeAsync(() => editPrimaryButton!.Find("button").ClickAsync());

            var dialog = this.dialogProvider.FindComponent<AddressDialog>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(dialog.Instance.Address, Is.EqualTo(this.primaryAddress));
                Assert.That(dialog.Instance.Countries, Has.Count.EqualTo(1));
            }

            var updatedAddress = this.primaryAddress.Clone();
            updatedAddress.Locality = "Frankfurt";

            await this.dialogProvider.InvokeAsync(() => dialog.Instance.OnResult.InvokeAsync(updatedAddress));
            await this.dialogProvider.InvokeAsync(() => dialog.Instance.DialogReference.CloseAsync(DialogResult.Ok()));

            this.viewModelMock.Verify(x => x.UpdateAddress(updatedAddress), Times.Once);

            // Case 2: Editing non-primary address
            var editSecondaryButton = manager.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId($"address-edit-{this.secondaryAddress.Id}"));
            Assert.That(editSecondaryButton, Is.Not.Null);

            _ = manager.InvokeAsync(() => editSecondaryButton!.Find("button").ClickAsync());

            var secondaryDialog = this.dialogProvider.FindComponent<AddressDialog>();
            Assert.That(secondaryDialog.Instance.Address, Is.EqualTo(this.secondaryAddress));

            var updatedSecondary = this.secondaryAddress.Clone();
            updatedSecondary.Locality = "Berlin";

            await this.dialogProvider.InvokeAsync(() => secondaryDialog.Instance.OnResult.InvokeAsync(updatedSecondary));

            this.viewModelMock.Verify(x => x.UpdateAddress(updatedSecondary), Times.Once);
        }

        [Test]
        public async Task VerifyOnSetPrimaryAddress()
        {
            this.addressesSourceList.AddRange([this.primaryAddress, this.secondaryAddress]);
            var manager = this.context.Render<AddressesManager>(parameters => parameters.Add(x => x.ViewModel, this.viewModelMock.Object));

            var setPrimaryButton = manager.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId($"address-set-primary-{this.secondaryAddress.Id}"));
            Assert.That(setPrimaryButton, Is.Not.Null);

            await manager.InvokeAsync(() => setPrimaryButton!.Find("button").ClickAsync());

            this.viewModelMock.Verify(x => x.SetPrimaryAddress(this.secondaryAddress.Id), Times.Once);
        }

        [Test]
        public async Task VerifyRendering()
        {
            // Case 1: Empty addresses
            var manager = this.context.Render<AddressesManager>(parameters =>
            {
                parameters.Add(x => x.ViewModel, this.viewModelMock.Object);
                parameters.Add(x => x.Class, "custom-addresses-class");
            });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(manager.Markup, Does.Contain("No addresses yet."));
                Assert.That(manager.Markup, Does.Contain("custom-addresses-class"));
                Assert.That(manager.FindComponents<AddressRow>(), Is.Empty);
            }

            // Case 2: Reactive SourceList populates AddressRows with country names and primary flags
            await manager.InvokeAsync(() => this.addressesSourceList.AddRange([this.primaryAddress, this.secondaryAddress]));

            var rows = manager.FindComponents<AddressRow>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(manager.Markup, Does.Not.Contain("No addresses yet."));
                Assert.That(rows, Has.Count.EqualTo(2));
                Assert.That(rows[0].Instance.IsPrimary, Is.True);
                Assert.That(rows[0].Instance.CountryName, Is.EqualTo("Germany"));
                Assert.That(rows[1].Instance.IsPrimary, Is.False);
                Assert.That(rows[1].Instance.CountryName, Is.EqualTo(string.Empty));
            }
        }
    }
}
