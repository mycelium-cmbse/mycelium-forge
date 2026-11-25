// ------------------------------------------------------------------------------------------------
// <copyright file="AddressDialogTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.AccountSettings.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Components.Pages.AccountSettings.Dialogs;
    using Mycelium.Forge.Tests.TestHelpers;

    [TestFixture]
    public class AddressDialogTestFixture
    {
        private BunitContext context;
        private Mock<IDialogReference> dialogReferenceMock;
        private Country germany;
        private Country france;
        private IReadOnlyList<ICountry> countries;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.dialogReferenceMock = new Mock<IDialogReference>();
            this.dialogReferenceMock.Setup(x => x.CloseAsync(It.IsAny<DialogResult>())).Returns(Task.CompletedTask);
            this.dialogReferenceMock.Setup(x => x.CancelAsync()).Returns(Task.CompletedTask);

            this.germany = new Country
            {
                Id = Guid.NewGuid(),
                Name = "Germany"
            };

            this.france = new Country
            {
                Id = Guid.NewGuid(),
                Name = "France"
            };

            this.countries = [this.france, this.germany];
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyOnAddressLine1Changed()
        {
            var dialog = this.context.Render<AddressDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Countries, this.countries);
            });

            var line1Input = dialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("address-line-1"));
            var saveButton = dialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("address-submit-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(line1Input, Is.Not.Null);
                Assert.That(saveButton, Is.Not.Null);
            }

            await dialog.InvokeAsync(() => saveButton!.Find("button").ClickAsync());
            Assert.That(dialog.Instance.ValidationManager.HasError(nameof(Address.AddressLine1)), Is.True);

            await dialog.InvokeAsync(() => line1Input!.Instance.ValueChanged.InvokeAsync("10 Rue Alsace"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(dialog.Instance.EditedAddress.AddressLine1, Is.EqualTo("10 Rue Alsace"));
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(Address.AddressLine1)), Is.False);
            }

            await dialog.InvokeAsync(() => line1Input!.Instance.ValueChanged.InvokeAsync(null!));
            Assert.That(dialog.Instance.EditedAddress.AddressLine1, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task VerifyOnAddressLine2Changed()
        {
            var dialog = this.context.Render<AddressDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Countries, this.countries);
            });

            var line2Input = dialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("address-line-2"));
            Assert.That(line2Input, Is.Not.Null);

            await dialog.InvokeAsync(() => line2Input!.Instance.ValueChanged.InvokeAsync("Floor 2"));
            Assert.That(dialog.Instance.EditedAddress.AddressLine2, Is.EqualTo("Floor 2"));

            await dialog.InvokeAsync(() => line2Input!.Instance.ValueChanged.InvokeAsync(null!));
            Assert.That(dialog.Instance.EditedAddress.AddressLine2, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task VerifyOnLocalityChanged()
        {
            var dialog = this.context.Render<AddressDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Countries, this.countries);
            });

            var localityInput = dialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("address-locality"));
            var saveButton = dialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("address-submit-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(localityInput, Is.Not.Null);
                Assert.That(saveButton, Is.Not.Null);
            }

            await dialog.InvokeAsync(() => saveButton!.Find("button").ClickAsync());
            Assert.That(dialog.Instance.ValidationManager.HasError(nameof(Address.Locality)), Is.True);

            await dialog.InvokeAsync(() => localityInput!.Instance.ValueChanged.InvokeAsync("Toulouse"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(dialog.Instance.EditedAddress.Locality, Is.EqualTo("Toulouse"));
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(Address.Locality)), Is.False);
            }

            await dialog.InvokeAsync(() => localityInput!.Instance.ValueChanged.InvokeAsync(null!));
            Assert.That(dialog.Instance.EditedAddress.Locality, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task VerifyOnRegionChanged()
        {
            var dialog = this.context.Render<AddressDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Countries, this.countries);
            });

            var regionInput = dialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("address-region"));
            Assert.That(regionInput, Is.Not.Null);

            await dialog.InvokeAsync(() => regionInput!.Instance.ValueChanged.InvokeAsync("Occitanie"));
            Assert.That(dialog.Instance.EditedAddress.Region, Is.EqualTo("Occitanie"));

            await dialog.InvokeAsync(() => regionInput!.Instance.ValueChanged.InvokeAsync(null!));
            Assert.That(dialog.Instance.EditedAddress.Region, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task VerifyOnPostalCodeChanged()
        {
            var dialog = this.context.Render<AddressDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Countries, this.countries);
            });

            var postalCodeInput = dialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("address-postal-code"));
            Assert.That(postalCodeInput, Is.Not.Null);

            await dialog.InvokeAsync(() => postalCodeInput!.Instance.ValueChanged.InvokeAsync("31000"));
            Assert.That(dialog.Instance.EditedAddress.PostalCode, Is.EqualTo("31000"));

            await dialog.InvokeAsync(() => postalCodeInput!.Instance.ValueChanged.InvokeAsync(null!));
            Assert.That(dialog.Instance.EditedAddress.PostalCode, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task VerifyOnCountryChanged()
        {
            var dialog = this.context.Render<AddressDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Countries, this.countries);
            });

            var countrySelect = dialog.FindComponents<ForgeSelect<ICountry>>().FirstOrDefault(x => x.Instance.Id == "address-country");
            var saveButton = dialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("address-submit-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(countrySelect, Is.Not.Null);
                Assert.That(saveButton, Is.Not.Null);
            }

            await dialog.InvokeAsync(() => saveButton!.Find("button").ClickAsync());
            Assert.That(dialog.Instance.ValidationManager.HasError(nameof(Address.Country)), Is.True);

            await dialog.InvokeAsync(() => countrySelect!.Instance.ValueChanged.InvokeAsync(this.france));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(dialog.Instance.EditedAddress.Country, Is.EqualTo(this.france.Id));
                Assert.That(dialog.Instance.SelectedCountry, Is.EqualTo(this.france));
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(Address.Country)), Is.False);
            }

            await dialog.InvokeAsync(() => countrySelect!.Instance.ValueChanged.InvokeAsync(null!));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(dialog.Instance.EditedAddress.Country, Is.EqualTo(Guid.Empty));
                Assert.That(dialog.Instance.SelectedCountry, Is.Null);
            }
        }

        [Test]
        public async Task VerifyOnCancelClicked()
        {
            var onCancelCalled = false;
            var onCancel = new EventCallbackFactory().Create(this, () => { onCancelCalled = true; });

            var dialog = this.context.Render<AddressDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Countries, this.countries);
                parameters.Add(x => x.OnCancel, onCancel);
            });

            var cancelButton = dialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("address-cancel-button"));
            Assert.That(cancelButton, Is.Not.Null);

            await dialog.InvokeAsync(() => cancelButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(onCancelCalled, Is.True);
                this.dialogReferenceMock.Verify(x => x.CancelAsync(), Times.Once);
            }
        }

        [Test]
        public void VerifyOnInitialized()
        {
            // Case 1: New address
            var newDialog = this.context.Render<AddressDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Countries, this.countries);
            });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(newDialog.Instance.EditedAddress.Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(newDialog.Instance.SelectedCountry, Is.Null);
                Assert.That(newDialog.Markup, Does.Contain("Select a country"));
            }

            // Case 2: Existing address is cloned and country selected
            var existingAddress = new Address
            {
                Id = Guid.NewGuid(),
                AddressLine1 = "1 Main Street",
                Locality = "Darmstadt",
                Country = this.germany.Id
            };

            var editDialog = this.context.Render<AddressDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Address, existingAddress);
                parameters.Add(x => x.Countries, this.countries);
            });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(editDialog.Instance.EditedAddress, Is.Not.SameAs(existingAddress));
                Assert.That(editDialog.Instance.EditedAddress.Id, Is.EqualTo(existingAddress.Id));
                Assert.That(editDialog.Instance.EditedAddress.AddressLine1, Is.EqualTo("1 Main Street"));
                Assert.That(editDialog.Instance.SelectedCountry, Is.EqualTo(this.germany));
                Assert.That(editDialog.Markup, Does.Contain("Germany"));
            }
        }

        [Test]
        public async Task VerifyOnSaveClicked()
        {
            Address capturedResult = null!;
            var onResult = new EventCallbackFactory().Create(this, (Address address) => { capturedResult = address; });

            var dialog = this.context.Render<AddressDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Countries, this.countries);
                parameters.Add(x => x.OnResult, onResult);
            });

            var line1Input = dialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("address-line-1"));
            var localityInput = dialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("address-locality"));
            var countrySelect = dialog.FindComponents<ForgeSelect<ICountry>>().FirstOrDefault(x => x.Instance.Id == "address-country");
            var saveButton = dialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("address-submit-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(line1Input, Is.Not.Null);
                Assert.That(localityInput, Is.Not.Null);
                Assert.That(countrySelect, Is.Not.Null);
                Assert.That(saveButton, Is.Not.Null);
            }

            // Case 1: Missing required fields fails validation without closing
            await dialog.InvokeAsync(() => saveButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedResult, Is.Null);
                Assert.That(dialog.Instance.ValidationManager[nameof(Address.AddressLine1)], Does.Contain("Address line 1 is required."));
                Assert.That(dialog.Instance.ValidationManager[nameof(Address.Locality)], Does.Contain("City is required."));
                Assert.That(dialog.Instance.ValidationManager[nameof(Address.Country)], Does.Contain("Country is required."));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Never);
            }

            // Case 2: Valid fields invoke OnResult and close dialog
            await dialog.InvokeAsync(() => line1Input!.Instance.ValueChanged.InvokeAsync("1 Main Street"));
            await dialog.InvokeAsync(() => localityInput!.Instance.ValueChanged.InvokeAsync("Darmstadt"));
            await dialog.InvokeAsync(() => countrySelect!.Instance.ValueChanged.InvokeAsync(this.germany));
            await dialog.InvokeAsync(() => saveButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedResult, Is.Not.Null);
                Assert.That(capturedResult.AddressLine1, Is.EqualTo("1 Main Street"));
                Assert.That(capturedResult.Locality, Is.EqualTo("Darmstadt"));
                Assert.That(capturedResult.Country, Is.EqualTo(this.germany.Id));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }
        }
    }
}
