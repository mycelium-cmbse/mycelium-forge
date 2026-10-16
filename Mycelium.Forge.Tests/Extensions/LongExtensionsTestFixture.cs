// ------------------------------------------------------------------------------------------------
// <copyright file="LongExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Extensions
{
    using Mycelium.Forge.Extensions;

    [TestFixture]
    public class LongExtensionsTestFixture
    {
        [Test]
        public void VerifyFormatFileSize()
        {
            var bytes = 500L.FormatFileSize();
            var kb = 2048L.FormatFileSize();
            var mb = (2L * 1024 * 1024).FormatFileSize();
            var gb = (3L * 1024 * 1024 * 1024).FormatFileSize();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(bytes, Is.EqualTo("500 B"));
                Assert.That(kb, Is.EqualTo("2 KB"));
                Assert.That(mb, Is.EqualTo("2 MB"));
                Assert.That(gb, Is.EqualTo("3 GB"));
            }
        }
    }
}
