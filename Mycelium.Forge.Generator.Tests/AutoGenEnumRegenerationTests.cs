// ------------------------------------------------------------------------------------------------
// <copyright file="AutoGenEnumRegenerationTests.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests
{
    using System.IO;
    using System.Threading.Tasks;

    using Mycelium.Forge.Generator.Generators;

    /// <summary>
    /// Writes freshly generated enums to <c>_Forge.Common.AutoGenEnum/</c> under this test project's
    /// own build output, for visual inspection. Excluded from the default <c>dotnet test</c> run — see
    /// <see cref="AutoGenDtoRegenerationTests"/> for the same pattern applied to DTOs.
    /// </summary>
    [TestFixture]
    public class AutoGenEnumRegenerationTests
    {
        [Test]
        public void Regenerate_AutoGenEnum()
        {
            var outputDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "_Forge.Common.AutoGenEnum"));
            outputDirectory.Create();

            var generator = new UmlCoreEnumGenerator();

            Assert.That(async () => await generator.GenerateEnumerationsAsync(GeneratorSetupFixture.XmiReaderResult, outputDirectory), Throws.Nothing);
        }
    }
}
