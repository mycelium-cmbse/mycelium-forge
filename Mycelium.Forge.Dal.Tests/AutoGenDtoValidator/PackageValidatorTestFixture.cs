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

            var validResult = await this.validator.ValidateDto(validPackage);
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

            var whitespaceDescPackage = new Package { Description = "   " };
            var emptyOwnerPackage = new Package { PackageOwner = [] };
            var emptyGuidOwnerPackage = new Package { PackageOwner = [Guid.Empty] };
            var validOwnerPackage = new Package { PackageOwner = [Guid.NewGuid()] };
            var invalidRegexPackage = new Package { ShortName = "INVALID_NAME" };
            var validRegexPackage = new Package { ShortName = "valid-pkg-1" };

            var nameResult = await this.validator.ValidateFields(packageWithValidNameOnly, x => x.Name);
            var shortNameResult = await this.validator.ValidateFields(packageWithValidNameOnly, x => x.ShortName);
            var validPackageTypeResult = await this.validator.ValidateFields(packageWithPackageType, x => x.PackageType);
            var invalidPackageTypeResult = await this.validator.ValidateFields(packageWithEmptyPackageType, x => x.PackageType);
            var whitespaceDescResult = await this.validator.ValidateFields(whitespaceDescPackage, x => x.Description);
            var emptyOwnerResult = await this.validator.ValidateFields(emptyOwnerPackage, x => x.PackageOwner);
            var emptyGuidOwnerResult = await this.validator.ValidateFields(emptyGuidOwnerPackage, x => x.PackageOwner);
            var validOwnerResult = await this.validator.ValidateFields(validOwnerPackage, x => x.PackageOwner);
            var invalidRegexResult = await this.validator.ValidateFields(invalidRegexPackage, x => x.ShortName);
            var validRegexResult = await this.validator.ValidateFields(validRegexPackage, x => x.ShortName);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nameResult.IsError, Is.False);
                Assert.That(shortNameResult.IsError, Is.True);
                Assert.That(validPackageTypeResult.IsError, Is.False);
                Assert.That(invalidPackageTypeResult.IsError, Is.True);
                Assert.That(whitespaceDescResult.IsError, Is.True);
                Assert.That(emptyOwnerResult.IsError, Is.True);
                Assert.That(emptyGuidOwnerResult.IsError, Is.True);
                Assert.That(validOwnerResult.IsError, Is.False);
                Assert.That(invalidRegexResult.IsError, Is.True);
                Assert.That(validRegexResult.IsError, Is.False);
            }
        }
    }
}
