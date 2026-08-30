// ------------------------------------------------------------------------------------------------
// <copyright file="AutoGenApiRegenerationTests.cs" company="Starion Group S.A.">
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
    /// Writes freshly generated Carter modules to <c>_Forge.Api.AutoGenApi/</c> under this test
    /// project's own build output, for visual inspection - see
    /// <see cref="AutoGenDtoRegenerationTests"/> for the same pattern applied to DTOs.
    /// </summary>
    [TestFixture]
    public class AutoGenApiRegenerationTests
    {
        [Test]
        public async Task Regenerate_AutoGenApi()
        {
            var outputDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "_Forge.Api.AutoGenApi"));
            outputDirectory.Create();

            var generator = new UmlCoreCarterModuleGenerator();

            Assert.That(async () => await generator.GenerateAsync(GeneratorSetupFixture.XmiReaderResult, outputDirectory), Throws.Nothing);
        }
    }
}
