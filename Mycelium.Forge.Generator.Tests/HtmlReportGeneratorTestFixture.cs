// ------------------------------------------------------------------------------------------------
// <copyright file="HtmlReportGeneratorTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests
{
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    using uml4net.Reporting.Drawing;
    using uml4net.Reporting.Generators;

    /// <summary>
    /// Smoke test for the <c>uml4net.Reporting</c>-based HTML meta-model report named in DD-07. This is
    /// a design-time convenience for humans inspecting the model, not a build output any runtime
    /// project depends on.
    /// </summary>
    [TestFixture]
    public class HtmlReportGeneratorTestFixture
    {
        [Test]
        public void Verify_that_the_metamodel_report_generates()
        {
            var loggerFactory = NullLoggerFactory.Instance;

            var inheritanceDiagramRenderer = new InheritanceDiagramRenderer(loggerFactory.CreateLogger<InheritanceDiagramRenderer>());
            var associationDiagramRenderer = new AssociationDiagramRenderer(loggerFactory.CreateLogger<AssociationDiagramRenderer>());
            var generator = new HtmlReportGenerator(inheritanceDiagramRenderer, associationDiagramRenderer, loggerFactory);

            var outputFile = new FileInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "AutoGenHtmlDocs", "index.html"));
            outputFile.Directory!.Create();

            Assert.That(
                () => generator.GenerateReport(
                    modelPath: new FileInfo(GeneratorSetupFixture.XmiFilePath),
                    rootDirectory: new DirectoryInfo(Path.GetDirectoryName(GeneratorSetupFixture.XmiFilePath)!),
                    rootPackageXmiId: null!,
                    rootPackageName: "mycelium-forge",
                    useStrictReading: false,
                    pathMap: new Dictionary<string, string>(),
                    outputPath: outputFile,
                    customContent: null!),
                Throws.Nothing);

            Assert.That(outputFile.Exists, Is.True);
        }
    }
}
