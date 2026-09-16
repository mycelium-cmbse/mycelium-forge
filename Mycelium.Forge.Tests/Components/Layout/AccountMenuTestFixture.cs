// ------------------------------------------------------------------------------------------------
// <copyright file="AccountMenuTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Layout
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Components.Common;

    [TestFixture]
    public class AccountMenuTestFixture
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
        public async Task VerifyAccountMenuRendering()
        {
            var accountMenu = this.context.Render<AccountMenu>();

            var avatar = accountMenu.FindComponent<CollaboratorAvatar>();
            var dropdownMenu = accountMenu.FindComponent<BbDropdownMenu>();
            var dropdownTrigger = accountMenu.FindComponent<BbDropdownMenuTrigger>();
            var dropdownContent = accountMenu.FindComponent<BbDropdownMenuContent>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(accountMenu.Instance, Is.Not.Null);
                Assert.That(avatar, Is.Not.Null);
                Assert.That(dropdownMenu, Is.Not.Null);
                Assert.That(dropdownTrigger, Is.Not.Null);
                Assert.That(dropdownContent, Is.Not.Null);
            }

            var button = accountMenu.Find("#header-account-menu-trigger");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(button, Is.Not.Null);
                Assert.That(button.GetAttribute("aria-expanded"), Is.EqualTo("false"));
            }

            await accountMenu.InvokeAsync(() => button.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(button, Is.Not.Null);
                Assert.That(button.GetAttribute("aria-expanded"), Is.EqualTo("true"));
            }
        }
    }
}
