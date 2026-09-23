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
    using Mycelium.Forge.Common;

    /// <summary>
    /// Test fixture for <see cref="UrlHelper" />.
    /// </summary>
    [TestFixture]
    public class UrlHelperTestFixture
    {
        /// <summary>
        /// Verifies that <see cref="UrlHelper.GetAuthLoginUrl(string, string)" /> generates the cookie auth login endpoint URL.
        /// </summary>
        [Test]
        public void VerifyGetAuthLoginUrl()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(UrlHelper.GetAuthLoginUrl("/my-packages"), Is.EqualTo($"{PageRoutes.AuthLogin}?{UrlParameterNames.ReturnUrl}=%2Fmy-packages"));
                Assert.That(UrlHelper.GetAuthLoginUrl("//evil.com"), Is.EqualTo($"{PageRoutes.AuthLogin}?{UrlParameterNames.ReturnUrl}=%2F"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="UrlHelper.GetSafeReturnUrl(string, string)" /> safely resolves local URLs and applies
        /// fallbacks.
        /// </summary>
        [Test]
        public void VerifyGetSafeReturnUrl()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(UrlHelper.GetSafeReturnUrl("/my-packages"), Is.EqualTo("/my-packages"));
                Assert.That(UrlHelper.GetSafeReturnUrl("my-packages"), Is.EqualTo("my-packages"));
                Assert.That(UrlHelper.GetSafeReturnUrl(null), Is.EqualTo(PageRoutes.Home));
                Assert.That(UrlHelper.GetSafeReturnUrl("//evil.com"), Is.EqualTo(PageRoutes.Home));
                Assert.That(UrlHelper.GetSafeReturnUrl(PageRoutes.Login), Is.EqualTo(PageRoutes.Home));
                Assert.That(UrlHelper.GetSafeReturnUrl("login"), Is.EqualTo(PageRoutes.Home));
                Assert.That(UrlHelper.GetSafeReturnUrl(null, "/custom"), Is.EqualTo("/custom"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="UrlHelper.GetSignInUrl(string, Microsoft.AspNetCore.Components.NavigationManager)" />
        /// generates the proper login URL.
        /// </summary>
        [Test]
        public void VerifyGetSignInUrl()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(UrlHelper.GetSignInUrl("/my-packages"), Is.EqualTo($"{PageRoutes.Login}?{UrlParameterNames.ReturnUrl}=%2Fmy-packages"));
                Assert.That(UrlHelper.GetSignInUrl(), Is.EqualTo(PageRoutes.Login));
                Assert.That(UrlHelper.GetSignInUrl("//evil.com"), Is.EqualTo(PageRoutes.Login));
                Assert.That(UrlHelper.GetSignInUrl(PageRoutes.Login), Is.EqualTo(PageRoutes.Login));
            }
        }

        /// <summary>
        /// Verifies that <see cref="UrlHelper.IsLocalUrl(string)" /> correctly validates local URLs and rejects open redirect
        /// targets.
        /// </summary>
        [Test]
        public void VerifyIsLocalUrl()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(UrlHelper.IsLocalUrl("/"), Is.True);
                Assert.That(UrlHelper.IsLocalUrl("/my-packages"), Is.True);
                Assert.That(UrlHelper.IsLocalUrl("/packages/scope/package"), Is.True);
                Assert.That(UrlHelper.IsLocalUrl("/packages?query=test"), Is.True);
                Assert.That(UrlHelper.IsLocalUrl("packages/detail"), Is.True);
                Assert.That(UrlHelper.IsLocalUrl("my-packages"), Is.True);

                Assert.That(UrlHelper.IsLocalUrl(null), Is.False);
                Assert.That(UrlHelper.IsLocalUrl(string.Empty), Is.False);
                Assert.That(UrlHelper.IsLocalUrl("   "), Is.False);
                Assert.That(UrlHelper.IsLocalUrl("//evil.com"), Is.False);
                Assert.That(UrlHelper.IsLocalUrl("//evil.com/path"), Is.False);
                Assert.That(UrlHelper.IsLocalUrl("/\\evil.com"), Is.False);
                Assert.That(UrlHelper.IsLocalUrl("\\evil.com"), Is.False);
                Assert.That(UrlHelper.IsLocalUrl("\\\\evil.com"), Is.False);
                Assert.That(UrlHelper.IsLocalUrl("http://evil.com"), Is.False);
                Assert.That(UrlHelper.IsLocalUrl("https://evil.com"), Is.False);
                Assert.That(UrlHelper.IsLocalUrl("javascript:alert(1)"), Is.False);
            }
        }
    }
}
