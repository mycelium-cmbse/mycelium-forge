// ------------------------------------------------------------------------------------------------
// <copyright file="StringExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Tests.Extensions
{
    using Mycelium.Forge.Common.Extensions;

    [TestFixture]
    public class StringExtensionsTestFixture
    {
        [Test]
        [TestCase("hello\r\nworld", "helloworld")]
        [TestCase("hello\nworld", "helloworld")]
        [TestCase("clean text", "clean text")]
        [TestCase("", "")]
        public void VerifySanitizeForLog(string input, string expected)
        {
            var actual = input.SanitizeForLog();
            var nullResult = ((string?)null).SanitizeForLog();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual, Is.EqualTo(expected));
                Assert.That(nullResult, Is.Empty);
            }
        }

        [Test]
        [TestCase(null, null, null)]
        [TestCase("", null, null)]
        [TestCase("   ", null, null)]
        [TestCase("Alice", "Alice", null)]
        [TestCase("  Alice  ", "Alice", null)]
        [TestCase("Alice Smith", "Alice", "Smith")]
        [TestCase("Jean Luc Picard", "Jean Luc", "Picard")]
        [TestCase("  Mary   Jane  ", "Mary", "Jane")]
        public void VerifySplitFullName(string? input, string? expectedFirstName, string? expectedLastName)
        {
            var (firstName, lastName) = input.SplitFullName();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(firstName, Is.EqualTo(expectedFirstName));
                Assert.That(lastName, Is.EqualTo(expectedLastName));
            }
        }

        [Test]
        [TestCase("Name", "name")]
        [TestCase("Email", "email")]
        [TestCase("A", "a")]
        [TestCase("a", "a")]
        [TestCase("", "")]
        public void VerifyToLowerCaseFirst(string input, string expected)
        {
            var actual = input.ToLowerCaseFirst();
            var nullResult = ((string?)null).ToLowerCaseFirst();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual, Is.EqualTo(expected));
                Assert.That(nullResult, Is.Empty);
            }
        }
    }
}
