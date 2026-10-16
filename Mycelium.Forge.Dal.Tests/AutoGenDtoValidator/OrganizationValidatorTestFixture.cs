// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationValidatorTestFixture.cs" company="Starion Group S.A.">
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
    public class OrganizationValidatorTestFixture
    {
        private OrganizationValidator validator;

        [SetUp]
        public void SetUp()
        {
            this.validator = new OrganizationValidator();
        }

        [Test]
        public async Task VerifyValidateDto()
        {
            var validOrganization = new Organization
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Name = "TestOrg",
                ShortName = "test-org",
                Email = "info@test.org",
                Origin = "local",
                Owner = Guid.NewGuid(),
                PrimaryAddress = Guid.NewGuid(),
                Administrator = [Guid.NewGuid()],
                Member = [Guid.NewGuid()],
                Status = ScopeStatusKind.ACTIVE,
                DefaultPackageVisibility = VisibilityKind.PRIVATE
            };

            var invalidOrganization = new Organization
            {
                Id = Guid.Empty,
                CreatedAt = default,
                ModifiedAt = default,
                Name = string.Empty,
                ShortName = string.Empty,
                Email = string.Empty,
                Origin = string.Empty,
                Owner = Guid.Empty,
                PrimaryAddress = Guid.Empty,
                Administrator = [],
                Member = []
            };

            var validResult = await this.validator.ValidateDto(validOrganization);
            var invalidResult = await this.validator.ValidateDto(invalidOrganization);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validResult.IsError, Is.False);
                Assert.That(invalidResult.IsError, Is.True);
            }
        }

        [Test]
        public async Task VerifyValidateFields()
        {
            var organizationWithEmailOnly = new Organization
            {
                Email = "contact@org.com",
                Name = string.Empty
            };

            var whitespaceOrg = new Organization { Name = "   " };
            var nullAddressOrg = new Organization { PrimaryAddress = null };
            var emptyAddressOrg = new Organization { PrimaryAddress = Guid.Empty };
            var emptyAdminOrg = new Organization { Administrator = [] };
            var emptyGuidAdminOrg = new Organization { Administrator = [Guid.Empty] };
            var validAdminOrg = new Organization { Administrator = [Guid.NewGuid()] };
            var invalidRegexOrg = new Organization { ShortName = "INVALID_SCOPE" };
            var validRegexOrg = new Organization { ShortName = "valid-scope.1" };

            var emailResult = await this.validator.ValidateFields(organizationWithEmailOnly, x => x.Email);
            var nameResult = await this.validator.ValidateFields(organizationWithEmailOnly, x => x.Name);
            var whitespaceNameResult = await this.validator.ValidateFields(whitespaceOrg, x => x.Name);
            var nullAddressResult = await this.validator.ValidateFields(nullAddressOrg, x => x.PrimaryAddress);
            var emptyAddressResult = await this.validator.ValidateFields(emptyAddressOrg, x => x.PrimaryAddress);
            var emptyAdminResult = await this.validator.ValidateFields(emptyAdminOrg, x => x.Administrator);
            var emptyGuidAdminResult = await this.validator.ValidateFields(emptyGuidAdminOrg, x => x.Administrator);
            var validAdminResult = await this.validator.ValidateFields(validAdminOrg, x => x.Administrator);
            var invalidRegexResult = await this.validator.ValidateFields(invalidRegexOrg, x => x.ShortName);
            var validRegexResult = await this.validator.ValidateFields(validRegexOrg, x => x.ShortName);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emailResult.IsError, Is.False);
                Assert.That(nameResult.IsError, Is.True);
                Assert.That(whitespaceNameResult.IsError, Is.True);
                Assert.That(nullAddressResult.IsError, Is.False);
                Assert.That(emptyAddressResult.IsError, Is.True);
                Assert.That(emptyAdminResult.IsError, Is.True);
                Assert.That(emptyGuidAdminResult.IsError, Is.True);
                Assert.That(validAdminResult.IsError, Is.False);
                Assert.That(invalidRegexResult.IsError, Is.True);
                Assert.That(validRegexResult.IsError, Is.False);
            }
        }
    }
}
