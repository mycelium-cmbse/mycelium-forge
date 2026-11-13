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
    using System.Linq;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageSettings;

    [TestFixture]
    public class PackageCollaboratorRowTestFixture
    {
        private BunitContext context;
        private IRenderedComponent<BbPortalHost> providers;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.providers = this.context.Render<BbPortalHost>();
        }

        [TearDown]
        public async Task TearDown()
        {
            this.providers.Dispose();
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyRenderingAndMenuCallbacks()
        {
            var verifiedAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Alex Rivera",
                IsVerified = true
            };

            IAccount changeRoleAccount = null!;
            IAccount viewProfileAccount = null!;
            IAccount removeAccount = null!;

            var onChangeRole = new EventCallbackFactory().Create(this, (IAccount account) => { changeRoleAccount = account; });
            var onViewProfile = new EventCallbackFactory().Create(this, (IAccount account) => { viewProfileAccount = account; });
            var onRemove = new EventCallbackFactory().Create(this, (IAccount account) => { removeAccount = account; });

            var row = this.context.Render<PackageCollaboratorRow>(parameters =>
            {
                parameters.Add(x => x.Account, verifiedAccount);
                parameters.Add(x => x.Role, "Owner");
                parameters.Add(x => x.IsRoleChangeDisabled, false);
                parameters.Add(x => x.IsRemoveDisabled, false);
                parameters.Add(x => x.OnChangeRole, onChangeRole);
                parameters.Add(x => x.OnViewProfile, onViewProfile);
                parameters.Add(x => x.OnRemove, onRemove);
            });

            var menuButton = row.Find(".package-maintainer-menu-button");
            await row.InvokeAsync(() => menuButton.ClickAsync());

            var changeRoleItem = this.providers.Find(".package-maintainer-change-role-item");
            await this.providers.InvokeAsync(() => changeRoleItem.ClickAsync());

            await row.InvokeAsync(() => menuButton.ClickAsync());
            var viewProfileItem = this.providers.Find(".package-maintainer-view-profile-item");
            await this.providers.InvokeAsync(() => viewProfileItem.ClickAsync());

            await row.InvokeAsync(() => menuButton.ClickAsync());
            var removeItem = this.providers.Find(".package-maintainer-remove-item");
            await this.providers.InvokeAsync(() => removeItem.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(row.Markup, Does.Contain("Alex Rivera"));
                Assert.That(row.Markup, Does.Contain("Owner"));
                Assert.That(changeRoleAccount, Is.EqualTo(verifiedAccount));
                Assert.That(viewProfileAccount, Is.EqualTo(verifiedAccount));
                Assert.That(removeAccount, Is.EqualTo(verifiedAccount));
            }

            var unverifiedAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Jane Doe",
                IsVerified = false
            };

            var disabledRow = this.context.Render<PackageCollaboratorRow>(parameters =>
            {
                parameters.Add(x => x.Account, unverifiedAccount);
                parameters.Add(x => x.Role, "Maintainer");
                parameters.Add(x => x.IsRoleChangeDisabled, true);
                parameters.Add(x => x.IsRemoveDisabled, true);
            });

            var disabledMenuButton = disabledRow.Find(".package-maintainer-menu-button");
            await disabledRow.InvokeAsync(() => disabledMenuButton.ClickAsync());

            var menuItems = this.providers.FindComponents<BbDropdownMenuItem>();
            var changeRoleMenuItem = menuItems.LastOrDefault(x => HasClass(x.Instance, "package-maintainer-change-role-item"));
            var viewProfileMenuItem = menuItems.LastOrDefault(x => HasClass(x.Instance, "package-maintainer-view-profile-item"));
            var removeMenuItem = menuItems.LastOrDefault(x => HasClass(x.Instance, "package-maintainer-remove-item"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(disabledRow.Markup, Does.Contain("Jane Doe"));
                Assert.That(disabledRow.Markup, Does.Contain("Maintainer"));
                Assert.That(changeRoleMenuItem, Is.Not.Null);
                Assert.That(viewProfileMenuItem, Is.Not.Null);
                Assert.That(removeMenuItem, Is.Not.Null);
                Assert.That(changeRoleMenuItem!.Instance.Disabled, Is.True);
                Assert.That(viewProfileMenuItem!.Instance.Disabled, Is.False);
                Assert.That(removeMenuItem!.Instance.Disabled, Is.True);
            }
        }

        private static bool HasClass(BbDropdownMenuItem menuItem, string expectedClass)
        {
            var classList = menuItem.Class ?? string.Empty;
            return !string.IsNullOrWhiteSpace(classList) && classList.Split(' ', StringSplitOptions.RemoveEmptyEntries).Contains(expectedClass, StringComparer.Ordinal);
        }
    }
}
