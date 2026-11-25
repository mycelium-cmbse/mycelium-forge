// ------------------------------------------------------------------------------------------------
// <copyright file="ScopeValidatorTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Tests.AutoGenDtoValidator
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.AutoGenDtoValidator;

    using NUnit.Framework;

    [TestFixture]
    public class ScopeValidatorTestFixture
    {
        private AccountValidator validator;

        [SetUp]
        public void SetUp()
        {
            this.validator = new AccountValidator();
        }

        [TestCase("user@example.com", true)]
        [TestCase("first.last+tag@sub.domain.co.uk", true)]
        [TestCase("", false)]
        [TestCase("plainaddress", false)]
        [TestCase("@missing-local.com", false)]
        [TestCase("user@", false)]
        [TestCase("user@domain", false)]
        [TestCase("user name@domain.com", false)]
        public void VerifyEmailFormatRegex(string email, bool expectedValid)
        {
            var isMatch = ScopeValidator<IAccount>.EmailFormatRegex().IsMatch(email);

            Assert.That(isMatch, Is.EqualTo(expectedValid));
        }

        [TestCase("http://example.com", true)]
        [TestCase("https://example.com/path?query=1", true)]
        [TestCase("", false)]
        [TestCase("   ", false)]
        [TestCase("not-a-url", false)]
        [TestCase("/relative/path", false)]
        [TestCase("http:///path", false)]
        [TestCase("https:///path", false)]
        [TestCase("ftp://example.com", false)]
        [TestCase("javascript:alert(1)", false)]
        [TestCase(null, false)]
        public void VerifyIsValidWebsite(string website, bool expectedValid)
        {
            var isValid = ScopeValidator<IAccount>.IsValidWebsite(website);

            Assert.That(isValid, Is.EqualTo(expectedValid));
        }

        [Test]
        public async Task VerifyValidateFields()
        {
            var validEmailAccount = new Account { Email = "user@example.com" };
            var emptyEmailAccount = new Account { Email = string.Empty };
            var whitespaceEmailAccount = new Account { Email = "   " };
            var invalidEmailAccount = new Account { Email = "invalid-email" };
            var nullBillingEmailAccount = new Account { BillingEmail = null };
            var emptyBillingEmailAccount = new Account { BillingEmail = string.Empty };
            var invalidBillingEmailAccount = new Account { BillingEmail = "invalid-billing-email" };
            var validBillingEmailAccount = new Account { BillingEmail = "billing@starion.eu" };
            var nullWebsiteAccount = new Account { Website = null };
            var emptyWebsiteAccount = new Account { Website = string.Empty };
            var validHttpWebsiteAccount = new Account { Website = "http://stariongroup.eu" };
            var validHttpsWebsiteAccount = new Account { Website = "https://stariongroup.eu/about" };
            var invalidSchemeWebsiteAccount = new Account { Website = "ftp://stariongroup.eu" };
            var hostlessWebsiteAccount = new Account { Website = "http:///path" };
            var invalidUrlWebsiteAccount = new Account { Website = "not-a-url" };

            var validEmailResult = await this.validator.ValidateFields(validEmailAccount, x => x.Email);
            var emptyEmailResult = await this.validator.ValidateFields(emptyEmailAccount, x => x.Email);
            var whitespaceEmailResult = await this.validator.ValidateFields(whitespaceEmailAccount, x => x.Email);
            var invalidEmailResult = await this.validator.ValidateFields(invalidEmailAccount, x => x.Email);
            var nullBillingEmailResult = await this.validator.ValidateFields(nullBillingEmailAccount, x => x.BillingEmail);
            var emptyBillingEmailResult = await this.validator.ValidateFields(emptyBillingEmailAccount, x => x.BillingEmail);
            var invalidBillingEmailResult = await this.validator.ValidateFields(invalidBillingEmailAccount, x => x.BillingEmail);
            var validBillingEmailResult = await this.validator.ValidateFields(validBillingEmailAccount, x => x.BillingEmail);
            var nullWebsiteResult = await this.validator.ValidateFields(nullWebsiteAccount, x => x.Website);
            var emptyWebsiteResult = await this.validator.ValidateFields(emptyWebsiteAccount, x => x.Website);
            var validHttpWebsiteResult = await this.validator.ValidateFields(validHttpWebsiteAccount, x => x.Website);
            var validHttpsWebsiteResult = await this.validator.ValidateFields(validHttpsWebsiteAccount, x => x.Website);
            var invalidSchemeWebsiteResult = await this.validator.ValidateFields(invalidSchemeWebsiteAccount, x => x.Website);
            var hostlessWebsiteResult = await this.validator.ValidateFields(hostlessWebsiteAccount, x => x.Website);
            var invalidUrlWebsiteResult = await this.validator.ValidateFields(invalidUrlWebsiteAccount, x => x.Website);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validEmailResult.IsError, Is.False);
                Assert.That(emptyEmailResult.IsError, Is.True);
                Assert.That(whitespaceEmailResult.IsError, Is.True);
                Assert.That(invalidEmailResult.IsError, Is.True);
                Assert.That(nullBillingEmailResult.IsError, Is.False);
                Assert.That(emptyBillingEmailResult.IsError, Is.False);
                Assert.That(invalidBillingEmailResult.IsError, Is.True);
                Assert.That(validBillingEmailResult.IsError, Is.False);
                Assert.That(nullWebsiteResult.IsError, Is.False);
                Assert.That(emptyWebsiteResult.IsError, Is.False);
                Assert.That(validHttpWebsiteResult.IsError, Is.False);
                Assert.That(validHttpsWebsiteResult.IsError, Is.False);
                Assert.That(invalidSchemeWebsiteResult.IsError, Is.True);
                Assert.That(hostlessWebsiteResult.IsError, Is.True);
                Assert.That(invalidUrlWebsiteResult.IsError, Is.True);
            }
        }
    }
}
