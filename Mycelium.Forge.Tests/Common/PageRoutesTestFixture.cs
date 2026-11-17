// ------------------------------------------------------------------------------------------------
// <copyright file="PageRoutesTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Common
{
    using Mycelium.Forge.Common;

    [TestFixture]
    public class PageRoutesTestFixture
    {
        [Test]
        public void VerifyGetAccountRoute()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(PageRoutes.GetAccountRoute("alice"), Is.EqualTo("/accounts/alice"));
                Assert.That(PageRoutes.GetAccountRoute("@alice"), Is.EqualTo("/accounts/alice"));
            }
        }
    }
}
