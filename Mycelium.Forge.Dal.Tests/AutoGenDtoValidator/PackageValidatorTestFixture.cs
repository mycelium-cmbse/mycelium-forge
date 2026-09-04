// ------------------------------------------------------------------------------------------------
// <copyright file="PackageValidatorTestFixture.cs" company="Starion Group S.A.">
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
    /// Test fixture for <see cref="PackageValidator" />.
    /// </summary>
    [TestFixture]
    public class PackageValidatorTestFixture
    {
        private PackageValidator validator;

        /// <summary>
        /// Sets up the test fixture before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.validator = new PackageValidator();
        }

        /// <summary>
        /// Verifies the <see cref="PackageValidator.ValidateDto" /> method.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [Test]
        public async Task VerifyValidateDto()
        {
            var validPackage = new Package
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Name = "TestPackage",
                ShortName = "test-pkg",
                Owner = Guid.NewGuid(),
                PackageOwner = [Guid.NewGuid()],
                PackageType = Guid.NewGuid(),
                Listed = true,
                Visibility = VisibilityKind.PUBLIC
            };

            var validResult = await this.validator.ValidateDto(validPackage);

            var invalidPackage = new Package
            {
                Id = Guid.Empty,
                CreatedAt = default,
                ModifiedAt = default,
                Name = string.Empty,
                ShortName = string.Empty,
                Owner = Guid.Empty,
                PackageOwner = [],
                PackageType = Guid.Empty
            };

            var invalidResult = await this.validator.ValidateDto(invalidPackage);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validResult.IsSuccess, Is.True);
                Assert.That(invalidResult.IsFailed, Is.True);
            }
        }

        /// <summary>
        /// Verifies the <see cref="PackageValidator.ValidateFields" /> method.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [Test]
        public async Task VerifyValidateFields()
        {
            var packageWithValidNameOnly = new Package
            {
                Name = "ValidName",
                ShortName = string.Empty
            };

            var nameResult = await this.validator.ValidateFields(packageWithValidNameOnly, x => x.Name);
            var shortNameResult = await this.validator.ValidateFields(packageWithValidNameOnly, x => x.ShortName);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nameResult.IsSuccess, Is.True);
                Assert.That(shortNameResult.IsFailed, Is.True);
            }
        }
    }
}
