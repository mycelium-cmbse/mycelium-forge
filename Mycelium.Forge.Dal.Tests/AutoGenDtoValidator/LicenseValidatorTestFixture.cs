// ------------------------------------------------------------------------------------------------
// <copyright file="LicenseValidatorTestFixture.cs" company="Starion Group S.A.">
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
    public class LicenseValidatorTestFixture
    {
        private LicenseValidator validator;
        private PackageValidator packageValidator;

        [SetUp]
        public void SetUp()
        {
            this.validator = new LicenseValidator();
            this.packageValidator = new PackageValidator();
        }

        [Test]
        public async Task VerifyValidateDto()
        {
            var validSpdxLicense = new License
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Name = "Apache License 2.0",
                SpdxIdentifier = "Apache-2.0",
                LicenseText = "Apache License Version 2.0"
            };

            var validProprietaryLicense = new License
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Name = "Proprietary License",
                SpdxIdentifier = "Proprietary",
                LicenseText = "All rights reserved."
            };

            var invalidLicense = new License
            {
                Id = Guid.Empty,
                CreatedAt = default,
                ModifiedAt = default,
                Name = string.Empty,
                SpdxIdentifier = string.Empty,
                LicenseText = string.Empty
            };

            var validSpdxResult = await this.validator.ValidateDto(validSpdxLicense);
            var validProprietaryResult = await this.validator.ValidateDto(validProprietaryLicense);
            var invalidResult = await this.validator.ValidateDto(invalidLicense);
            var packageUppercaseResult = await this.packageValidator.ValidateFields(new Package { ShortName = "Apache-2.0" }, x => x.ShortName);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validSpdxResult.IsError, Is.False);
                Assert.That(validProprietaryResult.IsError, Is.False);
                Assert.That(invalidResult.IsError, Is.True);
                Assert.That(packageUppercaseResult.IsError, Is.True);
            }
        }

        [Test]
        public async Task VerifyValidateFields()
        {
            string[] validIdentifiers = ["Apache-2.0", "MIT", "BSD-3-Clause", "GPL-3.0-only", "Proprietary", "LicenseRef-Proprietary", "CC-BY-4.0", "0BSD", "A"];
            string[] invalidIdentifiers = [string.Empty, "   ", "-MIT", ".MIT", "Apache 2.0", "Apache_2.0", "GPL-3.0+", "MIT/Apache-2.0", "MIT OR Apache-2.0"];

            using (Assert.EnterMultipleScope())
            {
                foreach (var identifier in validIdentifiers)
                {
                    var result = await this.validator.ValidateFields(new License { SpdxIdentifier = identifier }, x => x.SpdxIdentifier);
                    Assert.That(result.IsError, Is.False);
                }

                foreach (var identifier in invalidIdentifiers)
                {
                    var result = await this.validator.ValidateFields(new License { SpdxIdentifier = identifier }, x => x.SpdxIdentifier);
                    Assert.That(result.IsError, Is.True);
                }

                var validNameResult = await this.validator.ValidateFields(new License { Name = "MIT License" }, x => x.Name);
                var emptyNameResult = await this.validator.ValidateFields(new License { Name = "   " }, x => x.Name);
                var validTextResult = await this.validator.ValidateFields(new License { LicenseText = "Full text" }, x => x.LicenseText);
                var emptyTextResult = await this.validator.ValidateFields(new License { LicenseText = "   " }, x => x.LicenseText);

                Assert.That(validNameResult.IsError, Is.False);
                Assert.That(emptyNameResult.IsError, Is.True);
                Assert.That(validTextResult.IsError, Is.False);
                Assert.That(emptyTextResult.IsError, Is.True);
            }
        }
    }
}
