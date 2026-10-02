// ------------------------------------------------------------------------------------------------
// <copyright file="GuidExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Tests.Extensions
{
    using System;

    using Mycelium.Forge.Common.Extensions;

    [TestFixture]
    public class GuidExtensionsTestFixture
    {
        [Test]
        public void VerifySanitizeForLog()
        {
            var guid = Guid.Parse("12345678-1234-1234-1234-123456789abc");
            var result = guid.SanitizeForLog();

            var emptyGuid = Guid.Empty;
            var emptyResult = emptyGuid.SanitizeForLog();

            var randomGuid = Guid.NewGuid();
            var randomResult = randomGuid.SanitizeForLog();
            var randomRaw = randomGuid.ToString("N");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result, Is.EqualTo("***56789abc"));
                Assert.That(emptyResult, Is.EqualTo("***00000000"));
                Assert.That(randomResult, Is.EqualTo($"***{randomRaw[^8..]}"));
            }
        }
    }
}
