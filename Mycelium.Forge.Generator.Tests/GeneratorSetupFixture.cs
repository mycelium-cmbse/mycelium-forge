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

    using Mycelium.Forge.Generator.Extensions;
    using Mycelium.Forge.Generator.Generators;

    using uml4net.xmi;
    using uml4net.xmi.Extensions.EnterpriseArchitect.Extender;
    using uml4net.xmi.Extensions.EnterpriseArchitect.Structure.Readers;
    using uml4net.xmi.Readers;

    /// <summary>
    /// Reads the Mycelium Forge XMI model once for the whole test run, via the
    /// <c>Mycelium.Model.Forge</c> NuGet package (see <see cref="AssemblyMetadataXmiPath" /> for how the
    /// package's MSBuild property reaches this running code).
    /// </summary>
    /// <remarks>
    /// Deliberately backed by <see cref="Lazy{T}" /> rather than an NUnit <c>[OneTimeSetUp]</c>: NUnit
    /// evaluates <c>[TestCaseSource]</c> methods (used by <see cref="AutoGenDtoIdempotencyTests" /> and
    /// <see cref="AutoGenEnumIdempotencyTests" /> to derive their test cases from the model) at test
    /// *discovery* time, which runs before any <c>[SetUpFixture]</c>'s <c>[OneTimeSetUp]</c> — so a
    /// setup-fixture-populated field would still be null when those sources first run. A lazy, self
    /// -initializing static sidesteps the ordering problem entirely: whichever caller touches
    /// <see cref="XmiReaderResult" /> first pays the one-time read cost, no matter when that happens.
    /// </remarks>
    public static class GeneratorSetupFixture
    {
        private static readonly Lazy<string> LazyXmiFilePath = new(StageModelFiles);

        private static readonly Lazy<XmiReaderResult> LazyXmiReaderResult = new(ReadModel);

        /// <summary>
        /// The result of reading <c>mycelium-forge.xmi</c>, shared by every generator test fixture in
        /// this assembly.
        /// </summary>
        public static XmiReaderResult XmiReaderResult => LazyXmiReaderResult.Value;

        /// <summary>
        /// The absolute path to <c>mycelium-forge.xmi</c>, for the (rare) test that needs to re-read
        /// the file itself rather than reuse <see cref="XmiReaderResult" /> (e.g. <see cref="HtmlReportGeneratorTestFixture" />).
        /// </summary>
        public static string XmiFilePath => LazyXmiFilePath.Value;

        /// <summary>
        /// Stages the model files into a single temporary directory so that cross-model references
        /// between <c>mycelium-forge.xmi</c> and <c>mycelium-commonprimitives.xmi</c> can be resolved
        /// by the XMI reader.
        /// </summary>
        /// <returns>The path to the staged <c>mycelium-forge.xmi</c> file.</returns>
        private static string StageModelFiles()
        {
            var forgeXmiPath = AssemblyMetadataXmiPath("MyceliumModelForgeXmiPath");
            var commonPrimitivesXmiPath = AssemblyMetadataXmiPath("MyceliumModelCommonPrimitivesXmiPath");

            var workingDirectory = Path.Combine(Path.GetTempPath(), "Mycelium_Forge_Xmi_" + Guid.NewGuid());
            Directory.CreateDirectory(workingDirectory);

            var stagedForge = Path.Combine(workingDirectory, Path.GetFileName(forgeXmiPath));
            var stagedCommon = Path.Combine(workingDirectory, Path.GetFileName(commonPrimitivesXmiPath));

            File.Copy(forgeXmiPath, stagedForge, true);
            File.Copy(commonPrimitivesXmiPath, stagedCommon, true);

            return stagedForge;
        }

        /// <summary>
        /// Reads the Forge XMI model.
        /// </summary>
        /// <returns>The <see cref="XmiReaderResult" />.</returns>
        private static XmiReaderResult ReadModel()
        {
            var forgeXmiPath = XmiFilePath;

            // The Forge model references Mycelium.Model.CommonPrimitives via relative file hrefs
            // (e.g. href="mycelium-commonprimitives.xmi#..."). In uml4net, PathMaps is only consulted
            // for URIs with the 'pathmap://' scheme (as SysML2.NET does for pathmap://UML_LIBRARIES/...).
            // For relative file references, uml4net resolves them strictly via LocalReferenceBasePath and
            // ignores PathMaps. Because NuGet extracts the two packages into separate directories on disk,
            // StageModelFiles copies them into a single temporary folder so LocalReferenceBasePath can resolve
            // cross-model references.
            using var scope = XmiReaderBuilder.Create()
                .UsingSettings(settings => settings.LocalReferenceBasePath = Path.GetDirectoryName(forgeXmiPath))
                .WithLogger(NullLoggerFactory.Instance)
                .WithExtensionContentReaderFacade<ExtensionContentReaderFacade>()
                .WithExtender<EnterpriseArchitectExtenderReader>();

            var reader = scope.Build();

            var result = reader.Read(forgeXmiPath);
            UmlHandleBarsGenerator.DefaultModelVersion = result.QueryModelVersion();

            return result;
        }

        /// <summary>
        /// Reads an MSBuild property that was bridged into this assembly's metadata at build time (an
        /// MSBuild property set by a NuGet package's <c>.props</c> file, such as
        /// <c>$(MyceliumModelForgeXmiPath)</c>, is not otherwise visible to running code).
        /// </summary>
        private static string AssemblyMetadataXmiPath(string key)
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
