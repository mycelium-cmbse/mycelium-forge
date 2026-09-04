// ------------------------------------------------------------------------------------------------
// <copyright file="DtoValidatorBaseTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Tests.DtoValidator
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.AutoGenDtoValidator;
    using Mycelium.Forge.Dal.DtoValidator;

    using NUnit.Framework;

    /// <summary>
    /// Test fixture for <see cref="DtoValidatorBase{T}" />.
    /// </summary>
    [TestFixture]
    public class DtoValidatorBaseTestFixture
    {
        private DtoValidatorBase<IPackage> validator;

        /// <summary>
        /// Sets up the test fixture before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.validator = new PackageValidator();
        }

        /// <summary>
        /// Verifies the <see cref="DtoValidatorBase{T}.AddCustomValidation" /> method.
        /// </summary>
        [Test]
        public void VerifyAddCustomValidation()
        {
            Assert.That(() => this.validator.AddCustomValidation(), Throws.Nothing);
        }
    }
}
