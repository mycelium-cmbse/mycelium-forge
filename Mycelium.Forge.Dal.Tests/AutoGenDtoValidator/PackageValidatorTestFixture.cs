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

    [TestFixture]
    public class PackageValidatorTestFixture
    {
        private PackageValidator validator;

        [SetUp]
        public void SetUp()
        {
            this.validator = new PackageValidator();
        }

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
                Description = "A valid test package description",
                Owner = Guid.NewGuid(),
                PackageOwner = [Guid.NewGuid()],
                PackageType = Guid.NewGuid(),
                Version = [Guid.NewGuid()],
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
                Assert.That(validResult.IsError, Is.False);
                Assert.That(invalidResult.IsError, Is.True);
            }
        }

        [Test]
        public async Task VerifyValidateFields()
        {
            var packageWithValidNameOnly = new Package
            {
                Name = "ValidName",
                ShortName = string.Empty
            };

            var packageWithPackageType = new Package
            {
                PackageType = Guid.NewGuid()
            };

            var packageWithEmptyPackageType = new Package
            {
                PackageType = Guid.Empty
            };

            var nameResult = await this.validator.ValidateFields(packageWithValidNameOnly, x => x.Name);
            var shortNameResult = await this.validator.ValidateFields(packageWithValidNameOnly, x => x.ShortName);
            var validPackageTypeResult = await this.validator.ValidateFields(packageWithPackageType, x => x.PackageType);
            var invalidPackageTypeResult = await this.validator.ValidateFields(packageWithEmptyPackageType, x => x.PackageType);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nameResult.IsError, Is.False);
                Assert.That(shortNameResult.IsError, Is.True);
                Assert.That(validPackageTypeResult.IsError, Is.False);
                Assert.That(invalidPackageTypeResult.IsError, Is.True);
            }
        }
    }
}
