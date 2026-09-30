// ------------------------------------------------------------------------------------------------
// <copyright file="IntegerExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Extensions
{
    using Mycelium.Forge.Extensions;

    /// <summary>
    /// Test fixture for <see cref="IntegerExtensions" />.
    /// </summary>
    [TestFixture]
    public class IntegerExtensionsTestFixture
    {
        /// <summary>
        /// Verifies that <see cref="IntegerExtensions.ToCompactMetric(int)" /> formats integer values into compact metric strings.
        /// </summary>
        /// <param name="value">The integer input value.</param>
        /// <param name="expected">The expected formatted metric string.</param>
        [Test]
        [TestCase(0, "0")]
        [TestCase(600, "600")]
        [TestCase(999, "999")]
        [TestCase(1000, "1k")]
        [TestCase(1600, "1.6k")]
        [TestCase(16000, "16k")]
        [TestCase(1000000, "1M")]
        [TestCase(1500000, "1.5M")]
        [TestCase(25000000, "25M")]
        [TestCase(1000000000, "1B")]
        [TestCase(2000000000, "2B")]
        [TestCase(-600, "-600")]
        [TestCase(-1600, "-1.6k")]
        [TestCase(-1000000, "-1M")]
        [TestCase(-1000000000, "-1B")]
        public void VerifyToCompactMetric(int value, string expected)
        {
            var actual = value.ToCompactMetric();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
