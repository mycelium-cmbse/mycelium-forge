// ------------------------------------------------------------------------------------------------
// <copyright file="AutoGenDtoValidatorRegenerationTests.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests
{
    using System.IO;

    using Mycelium.Forge.Generator.Generators;

    /// <summary>
    /// Writes freshly generated DTO validators to <c>_Forge.Dal.AutoGenDtoValidator/</c> under this test project's own
    /// build output, for visual inspection.
    /// </summary>
    [TestFixture]
    public class AutoGenDtoValidatorRegenerationTests
    {
        /// <summary>
        /// Regenerates the DTO validator classes.
        /// </summary>
        [Test]
        public void Regenerate_AutoGenDtoValidator()
        {
            var outputDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "_Forge.Dal.AutoGenDtoValidator"));
            outputDirectory.Create();

            var generator = new UmlCoreDtoValidatorGenerator();

            Assert.That(async () => await generator.GenerateDtoValidatorsAsync(GeneratorSetupFixture.XmiReaderResult, outputDirectory), Throws.Nothing);
        }
    }
}
