// ------------------------------------------------------------------------------------------------
// <copyright file="ScopeDetailsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.ViewModels.Rows;

    [TestFixture]
    public class ScopeDetailsTestFixture
    {
        private BunitContext context;
        private Organization organization;
        private Account accountWithWebsite;
        private Account accountWithoutSubtitle;
        private IReadOnlyList<PackageRowViewModel> packages;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            var organizationId = Guid.NewGuid();
            var packageId = Guid.NewGuid();
            var packageTypeId = Guid.NewGuid();

            this.organization = new Organization
            {
                Id = organizationId,
                Name = "Starion Group",
                ShortName = "starion",
                Origin = "Systems engineering",
                CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            this.accountWithWebsite = new Account
            {
                Id = Guid.NewGuid(),
                Name = "R. Andre",
                ShortName = "r-andre",
                Origin = "local",
                Website = "https://stariongroup.eu",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            this.accountWithoutSubtitle = new Account
            {
                Id = Guid.NewGuid(),
                Name = "S. Kramer",
                ShortName = "s-kramer",
                Origin = "local",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            var package = new Package
            {
                Id = packageId,
                Name = "ECSS-MM-PWR",
                ShortName = "ecss-mm-pwr",
                Owner = organizationId,
                PackageType = packageTypeId
            };

            var packageType = new PackageType
            {
                Id = packageTypeId,
                Name = "SysML v2"
            };

            this.packages = PackageRowViewModel.GenerateRows([package], [this.organization, packageType]);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyRendering()
        {
            var nullScopeComponent = this.context.Render<ScopeDetails>();

            var organizationComponent = this.context.Render<ScopeDetails>(parameters => parameters.Add(p => p.Scope, this.organization).Add(p => p.Packages, this.packages).Add(p => p.ShowSettingsButton, true));

            var accountWithWebsiteComponent = this.context.Render<ScopeDetails>(parameters => parameters.Add(p => p.Scope, this.accountWithWebsite).Add(p => p.Packages, []).Add(p => p.ShowSettingsButton, true));

            var accountWithoutSettingsComponent = this.context.Render<ScopeDetails>(parameters => parameters.Add(p => p.Scope, this.accountWithoutSubtitle).Add(p => p.Packages, []).Add(p => p.ShowSettingsButton, false));

            var orgSettingsButton = organizationComponent.Find("#scope-details-settings-button");
            var accountSettingsButton = accountWithWebsiteComponent.Find("#scope-details-settings-button");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nullScopeComponent.Markup.Trim(), Has.Length.LessThan(5));

                Assert.That(organizationComponent.Markup, Does.Contain("Starion Group"));
                Assert.That(organizationComponent.Markup, Does.Contain("@starion"));
                Assert.That(organizationComponent.Markup, Does.Contain("Systems engineering"));
                Assert.That(organizationComponent.Markup, Does.Contain("1 package · member since 2023"));
                Assert.That(organizationComponent.Markup, Does.Contain("ECSS-MM-PWR"));
                Assert.That(orgSettingsButton.GetAttribute("href"), Is.EqualTo(PageRoutes.GetOrganizationSettingsRoute("starion")));

                Assert.That(accountWithWebsiteComponent.Markup, Does.Contain("R. Andre"));
                Assert.That(accountWithWebsiteComponent.Markup, Does.Contain("@r-andre"));
                Assert.That(accountWithWebsiteComponent.Markup, Does.Contain("https://stariongroup.eu"));
                Assert.That(accountWithWebsiteComponent.Markup, Does.Contain("0 packages · member since 2024"));
                Assert.That(accountWithWebsiteComponent.Markup, Does.Contain("No packages found"));
                Assert.That(accountSettingsButton.GetAttribute("href"), Is.EqualTo(PageRoutes.AccountSettings));

                Assert.That(accountWithoutSettingsComponent.Markup, Does.Not.Contain("local"));
                Assert.That(accountWithoutSettingsComponent.FindAll("#scope-details-settings-button"), Has.Count.EqualTo(0));
            }
        }
    }
}
