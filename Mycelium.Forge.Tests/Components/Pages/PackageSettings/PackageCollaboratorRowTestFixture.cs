// ------------------------------------------------------------------------------------------------
// <copyright file="PackageCollaboratorRowTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageSettings
{
    using System;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageSettings;

    [TestFixture]
    public class PackageCollaboratorRowTestFixture
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
        public async Task VerifyRenderingAndMenuCallback()
        {
            var verifiedAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Alex Rivera",
                IsVerified = true
            };

            IAccount? clickedAccount = null;
            var onMenu = new EventCallbackFactory().Create(this, (IAccount account) => { clickedAccount = account; });

            var row = this.context.Render<PackageCollaboratorRow>(parameters => parameters
                .Add(x => x.Account, verifiedAccount)
                .Add(x => x.Role, "Owner")
                .Add(x => x.OnMenu, onMenu));

            var menuButton = row.Find(".package-maintainer-menu-button");
            await row.InvokeAsync(() => menuButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(row.Markup, Does.Contain("Alex Rivera"));
                Assert.That(row.Markup, Does.Contain("Owner"));
                Assert.That(clickedAccount, Is.EqualTo(verifiedAccount));
            }

            var unverifiedAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Jane Doe",
                IsVerified = false
            };

            var unverifiedRow = this.context.Render<PackageCollaboratorRow>(parameters => parameters
                .Add(x => x.Account, unverifiedAccount)
                .Add(x => x.Role, "Maintainer"));

            Assert.That(unverifiedRow.Markup, Does.Contain("Jane Doe"));
        }
    }
}
