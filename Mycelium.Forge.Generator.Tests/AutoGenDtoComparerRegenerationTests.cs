// ------------------------------------------------------------------------------------------------
// <copyright file="AutoGenDtoComparerRegenerationTests.cs" company="Starion Group S.A.">
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
    /// Writes freshly generated DTO comparers to <c>_Forge.Common.AutoGenDtoComparer/</c>
    /// under this test project's own build output, for visual inspection.
    /// </summary>
    [TestFixture]
    public class AutoGenDtoComparerRegenerationTests
    {
        /// <summary>
        /// Regenerates the DTO comparer classes.
        /// </summary>
        [Test]
        public void Regenerate_AutoGenDtoComparer()
        {
            var outputDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "_Forge.Common.AutoGenDtoComparer"));
            outputDirectory.Create();

            var generator = new UmlCoreDtoComparerGenerator();

            Assert.That(async () => await generator.GenerateAsync(GeneratorSetupFixture.XmiReaderResult, outputDirectory), Throws.Nothing);
        }
    }
}
