// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionValidatorTestFixture.cs" company="Starion Group S.A.">
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
    public class PackageVersionValidatorTestFixture
    {
        private PackageVersionValidator validator;

        [SetUp]
        public void SetUp()
        {
            this.validator = new PackageVersionValidator();
        }

        [TestCase("1.0.0", true)]
        [TestCase("1.0.0-alpha.1+build.5", true)]
        [TestCase("1.0", false)]
        [TestCase("01.0.0", false)]
        [TestCase("v1.0.0", false)]
        public async Task VerifyValidateDto(string version, bool expectedValid)
        {
            var packageVersion = new PackageVersion
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                DownloadCount = 0,
                Listed = true,
                MetaData = Guid.NewGuid(),
                Owner = Guid.NewGuid(),
                PublicationDate = DateTime.UtcNow,
                Version = version
            };

            var result = await this.validator.ValidateDto(packageVersion);

            if (expectedValid)
            {
                Assert.That(result.IsError, Is.False);
            }
            else
            {
                using (Assert.EnterMultipleScope())
                {
                    Assert.That(result.IsError, Is.True);
                    Assert.That(result.Errors.Any(e => e.Description == "Version must follow SemVer 2.0.0 format (e.g. 1.0.0)."), Is.True);
                }
            }
        }
    }
}
