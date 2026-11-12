// ------------------------------------------------------------------------------------------------
// <copyright file="StringExtensionsTestFixture.cs" company="Starion Group S.A.">
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
    public class StringExtensionsTestFixture
    {
        [Test]
        [TestCase("@starion", "starion")]
        [TestCase("starion", "starion")]
        [TestCase("  @Starion  ", "starion")]
        [TestCase("@@Starion", "starion")]
        [TestCase("", "")]
        [TestCase("   ", "")]
        public void VerifyCleanScope(string input, string expected)
        {
            var actual = input.CleanScope();
            var nullResult = ((string?)null).CleanScope();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual, Is.EqualTo(expected));
                Assert.That(nullResult, Is.Empty);
            }
        }

        [Test]
        [TestCase("Starion Group", "SG")]
        [TestCase("European Space Agency", "EA")]
        [TestCase("Single", "SI")]
        [TestCase("A", "A")]
        [TestCase("", "")]
        [TestCase("   ", "")]
        public void VerifyGetInitials(string input, string expected)
        {
            var actual = input.GetInitials();
            var nullResult = ((string?)null).GetInitials();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual, Is.EqualTo(expected));
                Assert.That(nullResult, Is.Empty);
            }
        }

        [Test]
        [TestCase("warning", 1, null, true, "1 warning")]
        [TestCase("warning", 2, null, true, "2 warnings")]
        [TestCase("warning", 0, null, true, "0 warnings")]
        [TestCase("error", 1, null, true, "1 error")]
        [TestCase("error", 3, null, true, "3 errors")]
        [TestCase("child", 2, "children", true, "2 children")]
        [TestCase("warning does", 1, "warnings do", false, "warning does")]
        [TestCase("warning does", 2, "warnings do", false, "warnings do")]
        public void VerifyToQuantity(string singular, int count, string? plural, bool includeCount, string expected)
        {
            Assert.That(singular.ToQuantity(count, plural, includeCount), Is.EqualTo(expected));
        }

        [Test]
        [TestCase("hello", "Hello")]
        [TestCase("HELLO", "Hello")]
        [TestCase("a", "A")]
        [TestCase("", "")]
        public void VerifyToUpperCaseFirst(string input, string expected)
        {
            var actual = input.ToUpperCaseFirst();
            var nullResult = ((string?)null).ToUpperCaseFirst();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual, Is.EqualTo(expected));
                Assert.That(nullResult, Is.Empty);
            }
        }
    }
}
