// ------------------------------------------------------------------------------------------------
// <copyright file="AutoGenOpenApiRegenerationTests.cs" company="Starion Group S.A.">
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
    /// Writes freshly generated OpenAPI component schemas to <c>_Forge.Common.AutoGenOpenApi/</c>
    /// under this test project's own build output, for visual inspection - see
    /// <see cref="AutoGenDtoRegenerationTests"/> for the same pattern applied to DTOs.
    /// </summary>
    [TestFixture]
    public class AutoGenOpenApiRegenerationTests
    {
        [Test]
        public async Task Regenerate_AutoGenOpenApi()
        {
            var outputDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "_Forge.Common.AutoGenOpenApi"));
            outputDirectory.Create();

            var generator = new UmlCoreOpenApiSchemaGenerator();

            Assert.That(async () => await generator.GenerateAsync(GeneratorSetupFixture.XmiReaderResult, outputDirectory), Throws.Nothing);
        }
    }
}
