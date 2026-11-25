// ------------------------------------------------------------------------------------------------
// <copyright file="AddressRowTestFixture.cs" company="Starion Group S.A.">
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

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.AccountSettings;
    using Mycelium.Forge.Tests.TestHelpers;

    [TestFixture]
    public class AddressRowTestFixture
    {
        private BunitContext context;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyCallbacks()
        {
            var address = new Address
            {
                Id = Guid.NewGuid(),
                AddressLine1 = "1 Main Street",
                Locality = "Darmstadt"
            };

            Address setPrimaryAddress = null!;
            Address editedAddress = null!;
            Address deletedAddress = null!;

            var onSetPrimary = new EventCallbackFactory().Create(this, (Address a) => { setPrimaryAddress = a; });
            var onEdit = new EventCallbackFactory().Create(this, (Address a) => { editedAddress = a; });
            var onDelete = new EventCallbackFactory().Create(this, (Address a) => { deletedAddress = a; });

            var row = this.context.Render<AddressRow>(parameters =>
            {
                parameters.Add(x => x.Address, address);
                parameters.Add(x => x.CountryName, "Germany");
                parameters.Add(x => x.IsPrimary, false);
                parameters.Add(x => x.OnSetPrimary, onSetPrimary);
                parameters.Add(x => x.OnEdit, onEdit);
                parameters.Add(x => x.OnDelete, onDelete);
            });

            var setPrimaryButton = row.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId($"address-set-primary-{address.Id}"));
            var editButton = row.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId($"address-edit-{address.Id}"));
            var deleteButton = row.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId($"address-delete-{address.Id}"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(setPrimaryButton, Is.Not.Null);
                Assert.That(editButton, Is.Not.Null);
                Assert.That(deleteButton, Is.Not.Null);
            }

            await row.InvokeAsync(() => setPrimaryButton!.Find("button").ClickAsync());
            await row.InvokeAsync(() => editButton!.Find("button").ClickAsync());
            await row.InvokeAsync(() => deleteButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(setPrimaryAddress, Is.EqualTo(address));
                Assert.That(editedAddress, Is.EqualTo(address));
                Assert.That(deletedAddress, Is.EqualTo(address));
            }
        }

        [Test]
        public void VerifyRendering()
        {
            var fullAddress = new Address
            {
                Id = Guid.NewGuid(),
                AddressLine1 = "1 Main Street",
                AddressLine2 = "Suite 200",
                Locality = "Darmstadt",
                Region = "Hesse",
                PostalCode = "64283"
            };

            var primaryRow = this.context.Render<AddressRow>(parameters =>
            {
                parameters.Add(x => x.Address, fullAddress);
                parameters.Add(x => x.CountryName, "Germany");
                parameters.Add(x => x.IsPrimary, true);
            });

            var setPrimaryButtonWhenPrimary = primaryRow.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId($"address-set-primary-{fullAddress.Id}"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(primaryRow.Markup, Does.Contain("1 Main Street, Suite 200"));
                Assert.That(primaryRow.Markup, Does.Contain("Darmstadt, Hesse, 64283, Germany"));
                Assert.That(primaryRow.Markup, Does.Contain("Primary"));
                Assert.That(setPrimaryButtonWhenPrimary, Is.Null);
            }

            var minimalAddress = new Address
            {
                Id = Guid.NewGuid(),
                AddressLine1 = "10 Downing Street",
                AddressLine2 = "   ",
                Locality = "London",
                Region = string.Empty,
                PostalCode = null!
            };

            var nonPrimaryRow = this.context.Render<AddressRow>(parameters =>
            {
                parameters.Add(x => x.Address, minimalAddress);
                parameters.Add(x => x.CountryName, string.Empty);
                parameters.Add(x => x.IsPrimary, false);
            });

            var setPrimaryButtonWhenNonPrimary = nonPrimaryRow.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId($"address-set-primary-{minimalAddress.Id}"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nonPrimaryRow.Markup, Does.Contain("10 Downing Street"));
                Assert.That(nonPrimaryRow.Markup, Does.Not.Contain("10 Downing Street,"));
                Assert.That(nonPrimaryRow.Markup, Does.Contain("London"));
                Assert.That(nonPrimaryRow.Markup, Does.Not.Contain("London,"));
                Assert.That(nonPrimaryRow.Markup, Does.Not.Contain("Primary"));
                Assert.That(setPrimaryButtonWhenNonPrimary, Is.Not.Null);
            }
        }
    }
}
