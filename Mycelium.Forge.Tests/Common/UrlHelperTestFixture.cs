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
        /// Verifies that <see cref="UrlHelper.GetAuthLoginUrl" /> generates the cookie auth login endpoint URL.
        /// </summary>
        [Test]
        public void VerifyGetAuthLoginUrl()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That("/my-packages".GetAuthLoginUrl(), Is.EqualTo($"{PageRoutes.AuthLogin}?{UrlParameterNames.ReturnUrl}=%2Fmy-packages"));
                Assert.That("//evil.com".GetAuthLoginUrl(), Is.EqualTo($"{PageRoutes.AuthLogin}?{UrlParameterNames.ReturnUrl}=%2F"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="UrlHelper.GetSafeReturnUrl" /> safely resolves local URLs and applies
        /// fallbacks.
        /// </summary>
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

        /// <summary>
        /// Verifies that <see cref="UrlHelper.GetSignInUrl" />
        /// generates the proper login URL.
        /// </summary>
        [Test]
        public void VerifyGetSignInUrl()
        {
            string? nullUrl = null;

            using (Assert.EnterMultipleScope())
            {
                Assert.That("/my-packages".GetSignInUrl(), Is.EqualTo($"{PageRoutes.Login}?{UrlParameterNames.ReturnUrl}=%2Fmy-packages"));
                Assert.That(nullUrl.GetSignInUrl(), Is.EqualTo(PageRoutes.Login));
                Assert.That("//evil.com".GetSignInUrl(), Is.EqualTo(PageRoutes.Login));
                Assert.That(PageRoutes.Login.GetSignInUrl(), Is.EqualTo(PageRoutes.Login));
            }
        }

        /// <summary>
        /// Verifies that <see cref="UrlHelper.IsLocalUrl" /> correctly validates local URLs and rejects open redirect
        /// targets.
        /// </summary>
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
