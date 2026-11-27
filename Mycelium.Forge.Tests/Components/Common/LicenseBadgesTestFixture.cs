// ------------------------------------------------------------------------------------------------
// <copyright file="LicenseBadgesTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Common
{
    using System;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Common;

    [TestFixture]
    public class LicenseBadgesTestFixture
    {
        private BunitContext context;
        private License apacheLicense;
        private License mitLicense;
        private License proprietaryLicense;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();
            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.apacheLicense = new License
            {
                Id = Guid.NewGuid(),
                Name = "Apache License 2.0",
                SpdxIdentifier = "Apache-2.0",
                LicenseText = "Apache License 2.0 text"
            };

            this.mitLicense = new License
            {
                Id = Guid.NewGuid(),
                Name = "MIT License",
                SpdxIdentifier = "MIT",
                LicenseText = "MIT License text"
            };

            this.proprietaryLicense = new License
            {
                Id = Guid.NewGuid(),
                Name = "Proprietary License",
                SpdxIdentifier = "Proprietary",
                LicenseText = "Proprietary license text"
            };
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyRendering()
        {
            var emptyBadgesComponent = this.context.Render<LicenseBadges>(parameters => parameters.Add(x => x.Licenses, []));
            var nullCustomEmptyComponent = this.context.Render<LicenseBadges>(parameters => parameters.Add(x => x.Licenses, null!).Add(x => x.EmptyText, "Unspecified"));
            var populatedComponent = this.context.Render<LicenseBadges>(parameters => parameters.Add(x => x.Licenses, [this.apacheLicense, this.mitLicense, this.proprietaryLicense]).Add(x => x.ContainerClass, "custom-license-container"));

            var badges = populatedComponent.FindComponents<BbBadge>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyBadgesComponent.Markup, Does.Contain("None"));
                Assert.That(emptyBadgesComponent.FindComponents<BbBadge>(), Is.Empty);
                Assert.That(nullCustomEmptyComponent.Markup, Does.Contain("Unspecified"));
                Assert.That(populatedComponent.Find("div").ClassName, Is.EqualTo("custom-license-container"));
                Assert.That(badges, Has.Count.EqualTo(3));
                Assert.That(populatedComponent.Markup, Does.Contain("Apache-2.0"));
                Assert.That(populatedComponent.Markup, Does.Contain("MIT"));
                Assert.That(populatedComponent.Markup, Does.Contain("Proprietary"));
                Assert.That(populatedComponent.Markup, Does.Contain("View Apache-2.0 license text"));
                Assert.That(populatedComponent.Markup, Does.Contain("View MIT license text"));
                Assert.That(populatedComponent.Markup, Does.Contain("View Proprietary license text"));
            }
        }

        [Test]
        public async Task VerifyOpenLicenseTextDialog()
        {
            var dialogService = this.context.Services.GetRequiredService<DialogService>();
            var badgesComponent = this.context.Render<LicenseBadges>(parameters => parameters.Add(x => x.Licenses, [this.apacheLicense, this.mitLicense]));

            var buttons = badgesComponent.FindComponents<BbButton>();
            Assert.That(buttons, Has.Count.EqualTo(2));

            _ = badgesComponent.InvokeAsync(() => buttons[0].Find("button").ClickAsync());

            Assert.That(dialogService.Dialogs, Has.Count.EqualTo(1));

            await badgesComponent.Instance.OpenLicenseTextDialog(null!);
            Assert.That(dialogService.Dialogs, Has.Count.EqualTo(1));
        }
    }
}
