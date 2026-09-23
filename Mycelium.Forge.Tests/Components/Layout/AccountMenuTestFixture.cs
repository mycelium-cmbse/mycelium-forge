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

    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Test fixture for <see cref="AccountMenu" />.
    /// </summary>
    [TestFixture]
    public class AccountMenuTestFixture
    {
        private BunitContext context;
        private Mock<IUserService> userServiceMock;

        /// <summary>
        /// Sets up mock dependencies and test context before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();

            this.userServiceMock = new Mock<IUserService>();
            this.userServiceMock.Setup(s => s.GetCurrentUser(It.IsAny<bool>())).ReturnsAsync(SeedData.RegisAccount);
            this.userServiceMock.Setup(s => s.GetUserContext(It.IsAny<bool>())).ReturnsAsync(new UserContext { Username = "regis", CurrentRoles = [RoleKind.Account] });
            this.context.Services.AddSingleton(this.userServiceMock.Object);

            this.context.JSInterop.Mode = JSRuntimeMode.Loose;
        }

        /// <summary>
        /// Tears down the BUnit context after each test.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        /// <summary>
        /// Verifies the rendering of the <see cref="AccountMenu" /> component and expanding its dropdown.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Verifies that signing out clears authentication state and navigates to the logout route.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        [Test]
        public async Task VerifyOnSignOut()
        {
            var accountMenu = this.context.Render<AccountMenu>();
            var nav = this.context.Services.GetRequiredService<NavigationManager>();

            await accountMenu.InvokeAsync(() => accountMenu.Instance.OnSignOut());

            using (Assert.EnterMultipleScope())
            {
                this.userServiceMock.Verify(s => s.SetCurrentUser(null), Times.Once);
                Assert.That(nav.Uri, Does.Contain(PageRoutes.Logout));
            }
        }
    }
}
