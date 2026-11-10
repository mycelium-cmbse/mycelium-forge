// ------------------------------------------------------------------------------------------------
// <copyright file="StringExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests.Extensions
{
    using Mycelium.Forge.Generator.Extensions;

    /// <summary>
    /// Suite of tests for the <see cref="StringExtensions" /> class.
    /// </summary>
    [TestFixture]
    public class StringExtensionsTestFixture
    {
        [Test]
        public void VerifyNormalizeLineEndings()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(((string)null!).NormalizeLineEndings(), Is.EqualTo(string.Empty));
                Assert.That(string.Empty.NormalizeLineEndings(), Is.EqualTo(string.Empty));
                Assert.That("line1\nline2".NormalizeLineEndings(), Is.EqualTo("line1\r\nline2"));
                Assert.That("line1\rline2".NormalizeLineEndings(), Is.EqualTo("line1\r\nline2"));
                Assert.That("line1\r\nline2".NormalizeLineEndings(), Is.EqualTo("line1\r\nline2"));
                Assert.That("a\r\nb\nc\rd".NormalizeLineEndings(), Is.EqualTo("a\r\nb\r\nc\r\nd"));
            }
        }

        [Test]
        public void VerifyToSnakeCase()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(((string)null!).ToSnakeCase(), Is.EqualTo(string.Empty));
                Assert.That(string.Empty.ToSnakeCase(), Is.EqualTo(string.Empty));
                Assert.That("Account".ToSnakeCase(), Is.EqualTo("account"));
                Assert.That("Organization".ToSnakeCase(), Is.EqualTo("organization"));
                Assert.That("APIKey".ToSnakeCase(), Is.EqualTo("api_key"));
                Assert.That("PackageMetaData".ToSnakeCase(), Is.EqualTo("package_meta_data"));
                Assert.That("PackageVersion".ToSnakeCase(), Is.EqualTo("package_version"));
                Assert.That("billingEmail".ToSnakeCase(), Is.EqualTo("billing_email"));
                Assert.That("customEntity".ToSnakeCase(), Is.EqualTo("custom_entity"));
                Assert.That("sourceOrganization".ToSnakeCase(), Is.EqualTo("source_organization"));
                Assert.That("targetAccount".ToSnakeCase(), Is.EqualTo("target_account"));
                Assert.That("already_snake_case".ToSnakeCase(), Is.EqualTo("already_snake_case"));
                Assert.That("id".ToSnakeCase(), Is.EqualTo("id"));
                Assert.That("AddressLine1".ToSnakeCase(), Is.EqualTo("address_line1"));
                Assert.That("Alpha2Code".ToSnakeCase(), Is.EqualTo("alpha2_code"));
            }
        }
    }
}
