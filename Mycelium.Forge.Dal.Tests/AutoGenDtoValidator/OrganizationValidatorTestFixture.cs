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

    /// <summary>
    /// Test fixture for <see cref="OrganizationValidator" />.
    /// </summary>
    [TestFixture]
    public class OrganizationValidatorTestFixture
    {
        private OrganizationValidator validator;

        /// <summary>
        /// Sets up the test fixture before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.validator = new OrganizationValidator();
        }

        /// <summary>
        /// Verifies the <see cref="OrganizationValidator.ValidateDto" /> method.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
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

            var validResult = await this.validator.ValidateDto(validOrganization);

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

            var invalidResult = await this.validator.ValidateDto(invalidOrganization);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validResult.IsSuccess, Is.True);
                Assert.That(invalidResult.IsFailed, Is.True);
            }
        }

        /// <summary>
        /// Verifies the <see cref="OrganizationValidator.ValidateFields" /> method.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [Test]
        public async Task VerifyValidateFields()
        {
            var organizationWithEmailOnly = new Organization
            {
                Email = "contact@org.com",
                Name = string.Empty
            };

            var emailResult = await this.validator.ValidateFields(organizationWithEmailOnly, x => x.Email);
            var nameResult = await this.validator.ValidateFields(organizationWithEmailOnly, x => x.Name);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emailResult.IsSuccess, Is.True);
                Assert.That(nameResult.IsFailed, Is.True);
            }
        }
    }
}
