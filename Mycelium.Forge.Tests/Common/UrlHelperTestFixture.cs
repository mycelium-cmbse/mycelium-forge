// ------------------------------------------------------------------------------------------------
// <copyright file="UrlHelperTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Common
{
    using System.Threading.Tasks;

    using Bunit;

    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;

    using Mycelium.Forge.Common;

    [TestFixture]
    public class UrlHelperTestFixture
    {
        private BunitContext context;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyGetSafeReturnUrl()
        {
            string? nullUrl = null;

            using (Assert.EnterMultipleScope())
            {
                Assert.That("/my-packages".GetSafeReturnUrl(), Is.EqualTo("/my-packages"));
                Assert.That("my-packages".GetSafeReturnUrl(), Is.EqualTo("my-packages"));
                Assert.That(nullUrl.GetSafeReturnUrl(), Is.EqualTo(PageRoutes.Home));
                Assert.That("//evil.com".GetSafeReturnUrl(), Is.EqualTo(PageRoutes.Home));
                Assert.That(PageRoutes.Login.GetSafeReturnUrl(), Is.EqualTo(PageRoutes.Home));
                Assert.That("login".GetSafeReturnUrl(), Is.EqualTo(PageRoutes.Home));
                Assert.That(nullUrl.GetSafeReturnUrl("/custom"), Is.EqualTo("/custom"));
            }
        }

        [Test]
        public void VerifyGetSignInUrl()
        {
            var navigationManager = this.context.Services.GetRequiredService<NavigationManager>();
            navigationManager.NavigateTo("http://localhost/packages/detail");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(navigationManager.GetSignInUrl("/my-packages"), Is.EqualTo($"{PageRoutes.Login}?{UrlParameterNames.ReturnUrl}=%2Fmy-packages"));
                Assert.That(navigationManager.GetSignInUrl(), Is.EqualTo($"{PageRoutes.Login}?{UrlParameterNames.ReturnUrl}=packages%2Fdetail"));
                Assert.That(navigationManager.GetSignInUrl("//evil.com"), Is.EqualTo(PageRoutes.Login));
                Assert.That(navigationManager.GetSignInUrl(PageRoutes.Login), Is.EqualTo(PageRoutes.Login));
            }
        }

        [Test]
        public void VerifyIsLocalUrl()
        {
            string? nullUrl = null;

            using (Assert.EnterMultipleScope())
            {
                Assert.That("/".IsLocalUrl(), Is.True);
                Assert.That("/my-packages".IsLocalUrl(), Is.True);
                Assert.That("/packages/scope/package".IsLocalUrl(), Is.True);
                Assert.That("/packages?query=test".IsLocalUrl(), Is.True);
                Assert.That("packages/detail".IsLocalUrl(), Is.True);
                Assert.That("my-packages".IsLocalUrl(), Is.True);

                Assert.That(nullUrl.IsLocalUrl(), Is.False);
                Assert.That(string.Empty.IsLocalUrl(), Is.False);
                Assert.That("   ".IsLocalUrl(), Is.False);
                Assert.That("//evil.com".IsLocalUrl(), Is.False);
                Assert.That("//evil.com/path".IsLocalUrl(), Is.False);
                Assert.That("/\\evil.com".IsLocalUrl(), Is.False);
                Assert.That("\\evil.com".IsLocalUrl(), Is.False);
                Assert.That("\\\\evil.com".IsLocalUrl(), Is.False);
                Assert.That("http://evil.com".IsLocalUrl(), Is.False);
                Assert.That("https://evil.com".IsLocalUrl(), Is.False);
                Assert.That("javascript:alert(1)".IsLocalUrl(), Is.False);
            }
        }
    }
}
