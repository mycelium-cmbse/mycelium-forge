// ------------------------------------------------------------------------------------------------
// <copyright file="AutoGenDtoRegenerationTests.cs" company="Starion Group S.A.">
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
    /// Writes freshly generated DTOs to <c>_Forge.Common.AutoGenDto/</c> under this test project's own
    /// build output, for visual inspection - matching how the uml4net/SysML2.NET code generation
    /// tutorial itself works: there is no automated idempotency assertion, a developer runs this
    /// deliberately after a model change, reviews the output by eye, and copies whatever they accept
    /// over the committed <c>Mycelium.Forge.Common/AutoGenDto/</c> by hand. Excluded from the default
    /// <c>dotnet test</c> run.
    /// </summary>
    [TestFixture]
    public class AutoGenDtoRegenerationTests
    {
        [Test]
        public async Task Regenerate_AutoGenDto()
        {
            var outputDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "_Forge.Common.AutoGenDto"));
            outputDirectory.Create();

            var generator = new UmlCoreDtoGenerator();
            
            await Assert.MultipleAsync(async () =>
            {
                Assert.That(async () => await generator.GenerateDataTransferObjectInterfacesAsync(GeneratorSetupFixture.XmiReaderResult, outputDirectory), Throws.Nothing);
                Assert.That(async () => await generator.GenerateDataTransferObjectClassesAsync(GeneratorSetupFixture.XmiReaderResult, outputDirectory), Throws.Nothing);
            });
        }
    }
}
