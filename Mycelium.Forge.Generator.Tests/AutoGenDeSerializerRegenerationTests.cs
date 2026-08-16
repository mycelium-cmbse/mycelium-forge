// ------------------------------------------------------------------------------------------------
// <copyright file="AutoGenDeSerializerRegenerationTests.cs" company="Starion Group S.A.">
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
    /// Writes freshly generated JSON deserialisers to <c>_Forge.Serializer.Json.AutoGenDeSerializer/</c>
    /// under this test project's own build output, for visual inspection. Excluded from the default
    /// <c>dotnet test</c> run — see <see cref="AutoGenDtoRegenerationTests"/> for the same pattern
    /// applied to DTOs.
    /// </summary>
    [TestFixture]
    public class AutoGenDeSerializerRegenerationTests
    {
        [Test]
        public async Task Regenerate_AutoGenDeSerializer()
        {
            var outputDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "_Forge.Serializer.Json.AutoGenDeSerializer"));
            outputDirectory.Create();

            var generator = new UmlCoreJsonDtoDeSerializerGenerator();

            Assert.That(async () => await generator.GenerateAsync(GeneratorSetupFixture.XmiReaderResult, outputDirectory), Throws.Nothing);
        }
    }
}
