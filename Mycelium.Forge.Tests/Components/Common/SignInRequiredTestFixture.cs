// ------------------------------------------------------------------------------------------------
// <copyright file="SignInRequiredTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Common
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Common;

    /// <summary>
    /// Test fixture for <see cref="SignInRequired" />.
    /// </summary>
    [TestFixture]
    public class SignInRequiredTestFixture
    {
        private BunitContext context;

        /// <summary>
        /// Sets up the test context before each test execution.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
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
        /// Verifies that <see cref="SignInRequired.GetSignInUrl" /> generates the proper login URL.
        /// </summary>
        [Test]
        public void VerifyGetSignInUrl()
        {
            var nav = this.context.Services.GetRequiredService<NavigationManager>();
            nav.NavigateTo("/packages/detail");

            var component = this.context.Render<SignInRequired>();

            var customComponent = this.context.Render<SignInRequired>(parameters => parameters
                .Add(p => p.ReturnUrl, "custom-page"));

            var maliciousComponent = this.context.Render<SignInRequired>(parameters => parameters
                .Add(p => p.ReturnUrl, "//evil.com"));

            var loginComponent = this.context.Render<SignInRequired>(parameters => parameters
                .Add(p => p.ReturnUrl, PageRoutes.Login));

            var defaultUrl = component.Instance.GetSignInUrl();
            var customUrl = customComponent.Instance.GetSignInUrl();
            var maliciousUrl = maliciousComponent.Instance.GetSignInUrl();
            var loginUrl = loginComponent.Instance.GetSignInUrl();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(defaultUrl, Does.Contain(UrlParameterNames.ReturnUrl));
                Assert.That(customUrl, Is.EqualTo($"{PageRoutes.Login}?{UrlParameterNames.ReturnUrl}=custom-page"));
                Assert.That(maliciousUrl, Is.EqualTo(PageRoutes.Login));
                Assert.That(loginUrl, Is.EqualTo(PageRoutes.Login));
            }
        }

        /// <summary>
        /// Verifies the rendering of the <see cref="SignInRequired" /> component.
        /// </summary>
        [Test]
        public void VerifySignInRequiredRendering()
        {
            var component = this.context.Render<SignInRequired>();
            var button = component.Find("#sign-in-required-button");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(component.Instance, Is.Not.Null);
                Assert.That(button, Is.Not.Null);
                Assert.That(button.GetAttribute("href"), Does.Contain(PageRoutes.Login));
            }
        }
    }
}
