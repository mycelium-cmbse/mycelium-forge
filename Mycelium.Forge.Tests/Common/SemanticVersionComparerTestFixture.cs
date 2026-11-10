// ------------------------------------------------------------------------------------------------
// <copyright file="SemanticVersionComparerTestFixture.cs" company="Starion Group S.A.">
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
    public class SemanticVersionComparerTestFixture
    {
        private SemanticVersionComparer comparer;

        [SetUp]
        public void SetUp()
        {
            this.comparer = SemanticVersionComparer.Instance;
        }

        [TestCase("1.0.0", "1.0.0", 0)]
        [TestCase("2.0.0", "1.0.0", 1)]
        [TestCase("1.0.0", "2.0.0", -1)]
        [TestCase("1.2.0", "1.1.0", 1)]
        [TestCase("1.1.0", "1.2.0", -1)]
        [TestCase("1.0.2", "1.0.1", 1)]
        [TestCase("1.0.1", "1.0.2", -1)]
        [TestCase("1.0.0-alpha", "1.0.0", -1)]
        [TestCase("1.0.0", "1.0.0-alpha", 1)]
        [TestCase("1.0.0-alpha", "1.0.0-alpha", 0)]
        [TestCase("1.0.0-alpha", "1.0.0-beta", -1)]
        [TestCase("1.0.0-beta", "1.0.0-alpha", 1)]
        [TestCase("1.0.0-alpha.1", "1.0.0-alpha.2", -1)]
        [TestCase("1.0.0-alpha.2", "1.0.0-alpha.1", 1)]
        [TestCase("1.0.0-alpha.1", "1.0.0-alpha.beta", -1)]
        [TestCase("1.0.0-alpha.beta", "1.0.0-alpha.1", 1)]
        [TestCase("1.0.0-alpha", "1.0.0-alpha.1", -1)]
        [TestCase("not-semver-a", "not-semver-b", -1)]
        [TestCase(null, "1.0.0", -1)]
        [TestCase("1.0.0", null, 1)]
        [TestCase(null, null, 0)]
        public void VerifyCompare(string? x, string? y, int expectedSign)
        {
            var result = this.comparer.Compare(x, y);

            switch (expectedSign)
            {
                case 0:
                    Assert.That(result, Is.Zero);
                    break;
                case > 0:
                    Assert.That(result, Is.GreaterThan(0));
                    break;
                default:
                    Assert.That(result, Is.LessThan(0));
                    break;
            }
        }

        [TestCase("2.0.0", "1.9.9", true)]
        [TestCase("1.1.0", "1.0.0", false)]
        [TestCase("1.0.1", "1.0.0", false)]
        [TestCase("1.0.0", "2.0.0", false)]
        [TestCase("invalid", "1.0.0", false)]
        [TestCase("1.0.0", "invalid", false)]
        public void VerifyIsMajorBump(string? newVersion, string? currentVersion, bool expected)
        {
            Assert.That(SemanticVersionComparer.IsMajorBump(newVersion, currentVersion), Is.EqualTo(expected));
        }
    }
}
