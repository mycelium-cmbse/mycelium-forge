// ------------------------------------------------------------------------------------------------
// <copyright file="GeneratorSetupFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Extensions.Logging.Abstractions;

    using uml4net.xmi;
    using uml4net.xmi.Extensions.EnterpriseArchitect.Extender;
    using uml4net.xmi.Extensions.EnterpriseArchitect.Structure.Readers;
    using uml4net.xmi.Readers;

    /// <summary>
    /// Reads the Mycelium Forge XMI model once for the whole test run, via the
    /// <c>Mycelium.Model.Forge</c> NuGet package (see <see cref="AssemblyMetadataValue"/> for how the
    /// package's MSBuild properties reach this running code).
    /// </summary>
    /// <remarks>
    /// Deliberately backed by <see cref="Lazy{T}"/> rather than an NUnit <c>[OneTimeSetUp]</c>: NUnit
    /// evaluates <c>[TestCaseSource]</c> methods (used by <see cref="AutoGenDtoIdempotencyTests"/> and
    /// <see cref="AutoGenEnumIdempotencyTests"/> to derive their test cases from the model) at test
    /// *discovery* time, which runs before any <c>[SetUpFixture]</c>'s <c>[OneTimeSetUp]</c> — so a
    /// setup-fixture-populated field would still be null when those sources first run. A lazy, self
    /// -initializing static sidesteps the ordering problem entirely: whichever caller touches
    /// <see cref="XmiReaderResult"/> first pays the one-time read cost, no matter when that happens.
    /// </remarks>
    public static class GeneratorSetupFixture
    {
        private static readonly Lazy<string> LazyXmiFilePath = new(() => AssemblyMetadataValue("MyceliumModelForgeXmiPath"));

        private static readonly Lazy<string> LazyModelVersion = new(() => AssemblyMetadataValue("MyceliumModelForgeVersion"));

        private static readonly Lazy<XmiReaderResult> LazyXmiReaderResult = new(ReadModel);

        /// <summary>
        /// The result of reading <c>mycelium-forge.xmi</c>, shared by every generator test fixture in
        /// this assembly.
        /// </summary>
        public static XmiReaderResult XmiReaderResult => LazyXmiReaderResult.Value;

        /// <summary>
        /// The absolute path to <c>mycelium-forge.xmi</c>, for the (rare) test that needs to re-read
        /// the file itself rather than reuse <see cref="XmiReaderResult"/> (e.g. <see cref="HtmlReportGeneratorTestFixture"/>).
        /// </summary>
        public static string XmiFilePath => LazyXmiFilePath.Value;

        /// <summary>
        /// The version of the <c>Mycelium.Model.Forge</c> NuGet package the model was read from (e.g.
        /// <c>0.2.0</c>), for generators that need to report the actual model version rather than a
        /// hardcoded string (see <see cref="Mycelium.Forge.Generator.Generators.UmlCoreSqlSchemaGenerator"/>'s
        /// <c>query_model_version()</c>).
        /// </summary>
        public static string ModelVersion => LazyModelVersion.Value;

        private static XmiReaderResult ReadModel()
        {
            var forgeXmiPath = XmiFilePath;

            // The Forge model is currently self-contained (no cross-model href/pathmap references into
            // Mycelium.Model.CommonPrimitives), so no PathMaps entry is needed yet. Should the model
            // start referencing shared primitives from that package, add its path here the same way
            // SysML2.NET maps pathmap://UML_LIBRARIES/... to a local file.
            using var scope = XmiReaderBuilder.Create()
                .UsingSettings(settings => settings.LocalReferenceBasePath = Path.GetDirectoryName(forgeXmiPath))
                .WithLogger(NullLoggerFactory.Instance)
                .WithExtensionContentReaderFacade<ExtensionContentReaderFacade>()
                .WithExtender<EnterpriseArchitectExtenderReader>();

            var reader = scope.Build();

            return reader.Read(forgeXmiPath);
        }

        /// <summary>
        /// Reads an MSBuild property that was bridged into this assembly's metadata at build time (an
        /// MSBuild property - whether set by a NuGet package's <c>.props</c> file, such as
        /// <c>$(MyceliumModelForgeXmiPath)</c>, or read back from the package reference itself, such as
        /// <c>$(MyceliumModelForgeVersion)</c> - is not otherwise visible to running code).
        /// </summary>
        private static string AssemblyMetadataValue(string key)
        {
            var value = Assembly.GetExecutingAssembly()
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .SingleOrDefault(a => a.Key == key)
                ?.Value;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(
                    $"AssemblyMetadataAttribute '{key}' was not found or is empty. Confirm the " +
                    "corresponding NuGet package is referenced and its MSBuild property flowed into " +
                    "the AssemblyMetadata item in Mycelium.Forge.Generator.Tests.csproj.");
            }

            return value;
        }
    }
}
