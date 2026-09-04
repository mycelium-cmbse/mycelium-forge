// ------------------------------------------------------------------------------------------------
// <copyright file="AutoGenPermissionServiceRegenerationTests.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests
{
    using System.IO;

    using Mycelium.Forge.Generator.DataLoaders;
    using Mycelium.Forge.Generator.Generators;

    /// <summary>
    /// Writes freshly generated permission services to <c>_Forge.Dal.AutoGenPermissionService/</c> and
    /// authorization mappings to <c>_Forge.Common.AutoGenAuthorization/</c> under this test project's own
    /// build output, for visual inspection.
    /// </summary>
    [TestFixture]
    public class AutoGenPermissionServiceRegenerationTests
    {
        /// <summary>
        /// Regenerates the authorization mapping enum and dictionary classes.
        /// </summary>
        [Test]
        public void Regenerate_AutoGenAuthorizationMapping()
        {
            var outputDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "_Forge.Common.AutoGenAuthorization"));
            outputDirectory.Create();

            var csvPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", "forge-roles-and-permissions.csv");
            var loader = new CsvRolesDataLoader();
            var model = loader.Load(csvPath);

            var generator = new UmlCorePermissionServiceGenerator();

            Assert.That(async () => await generator.GenerateAuthorizationMappingAsync(model, outputDirectory), Throws.Nothing);
        }

        /// <summary>
        /// Regenerates the permission service interfaces and classes.
        /// </summary>
        [Test]
        public void Regenerate_AutoGenPermissionService()
        {
            var outputDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "_Forge.Dal.AutoGenPermissionService"));
            outputDirectory.Create();

            var generator = new UmlCorePermissionServiceGenerator();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(async () => await generator.GeneratePermissionServiceInterfacesAsync(GeneratorSetupFixture.XmiReaderResult, outputDirectory), Throws.Nothing);
                Assert.That(async () => await generator.GeneratePermissionServiceClassesAsync(GeneratorSetupFixture.XmiReaderResult, outputDirectory), Throws.Nothing);
            }
        }
    }
}
