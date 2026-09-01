// ------------------------------------------------------------------------------------------------
// <copyright file="AutoGenSerializerRegenerationTests.cs" company="Starion Group S.A.">
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
    /// Writes freshly generated JSON serialisers to <c>_Forge.Serializer.Json.AutoGenSerializer/</c>
    /// under this test project's own build output, for visual inspection. Excluded from the default
    /// <c>dotnet test</c> run — see <see cref="AutoGenDtoRegenerationTests" /> for the same pattern
    /// applied to DTOs.
    /// </summary>
    [TestFixture]
    public class AutoGenSerializerRegenerationTests
    {
        /// <summary>
        /// Regenerates the JSON serializers.
        /// </summary>
        [Test]
        public void Regenerate_AutoGenSerializer()
        {
            var outputDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "_Forge.Serializer.Json.AutoGenSerializer"));
            outputDirectory.Create();

            var generator = new UmlCoreJsonDtoSerializerGenerator();

            Assert.That(async () => await generator.GenerateAsync(GeneratorSetupFixture.XmiReaderResult, outputDirectory), Throws.Nothing);
        }
    }
}
