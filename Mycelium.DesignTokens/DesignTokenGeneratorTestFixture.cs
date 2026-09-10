// ------------------------------------------------------------------------------------------------
// <copyright file="DesignTokenGeneratorTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.DesignTokens
{
    using Mycelium.DesignTokens.Models;

    /// <summary>
    /// Suite of automated tests for the <see cref="DesignTokenGenerator" /> class.
    /// </summary>
    [TestFixture]
    [Category("IgnoreOnCI")]
    public class DesignTokenGeneratorTestFixture
    {
        /// <summary>
        /// Gets or sets the generator under test.
        /// </summary>
        public DesignTokenGenerator Generator { get; set; }

        /// <summary>
        /// Gets or sets the project root directory where source tokens and scripts reside.
        /// </summary>
        public string ProjectDirectory { get; set; }

        /// <summary>
        /// Gets or sets the temporary output directory used for test artifacts.
        /// </summary>
        public string TestOutputDirectory { get; set; }

        /// <summary>
        /// Sets up the test context before each test execution.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.Generator = new DesignTokenGenerator();

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var candidateDirectory = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", ".."));

            this.ProjectDirectory = File.Exists(Path.Combine(candidateDirectory, "Javascript", "build-tokens.mjs")) ? candidateDirectory : baseDirectory;
            this.TestOutputDirectory = Path.Combine(this.ProjectDirectory, "Styles", "dist");
        }

        /// <summary>
        /// Verifies the asynchronous execution of the design token generation process.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Test]
        public async Task GenerateCssFromDtcgTokensAndVerify()
        {
            var options = new DesignTokenGeneratorOptions
            {
                Target = "all",
                ScriptPath = Path.Combine("Javascript", "build-tokens.mjs"),
                OutputDirectory = Path.Combine("Styles", "dist"),
                TokensDirectory = Path.Combine("Styles", "tokens"),
                WorkingDirectory = this.ProjectDirectory,
                TimeoutSeconds = 30
            };

            // Generates the css files from the DTCG tokens
            var result = await this.Generator.GenerateAsync(options);

            // Verify that the expected output files exist and contain the expected content
            var forgeCombinedPath = Path.Combine(this.TestOutputDirectory, "tokens-forge.css");
            var bloomCombinedPath = Path.Combine(this.TestOutputDirectory, "tokens-bloom.css");
            var myceliumThemePath = Path.Combine(this.TestOutputDirectory, "theme-mycelium.css");

            var combinedForgeContent = File.Exists(forgeCombinedPath) ? await File.ReadAllTextAsync(forgeCombinedPath) : string.Empty;
            var combinedBloomContent = File.Exists(bloomCombinedPath) ? await File.ReadAllTextAsync(bloomCombinedPath) : string.Empty;
            var myceliumThemeContent = File.Exists(myceliumThemePath) ? await File.ReadAllTextAsync(myceliumThemePath) : string.Empty;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.ExitCode, Is.EqualTo(0));
                Assert.That(result.GeneratedFiles, Has.Count.EqualTo(3));
                Assert.That(File.Exists(forgeCombinedPath), Is.True);
                Assert.That(File.Exists(bloomCombinedPath), Is.True);
                Assert.That(File.Exists(myceliumThemePath), Is.True);
                Assert.That(combinedForgeContent.Contains(":root"), Is.True);
                Assert.That(combinedForgeContent.Contains(".dark"), Is.True);
                Assert.That(combinedForgeContent.Contains("--primary"), Is.True);
                Assert.That(combinedForgeContent.Contains("--background"), Is.True);
                Assert.That(combinedBloomContent.Contains(":root"), Is.True);
                Assert.That(combinedBloomContent.Contains(".dark"), Is.True);
                Assert.That(combinedForgeContent.Contains("--spacing-"), Is.True);
                Assert.That(myceliumThemeContent.Contains("@theme inline"), Is.True);
                Assert.That(myceliumThemeContent.Contains("--color-"), Is.True);
                Assert.That(myceliumThemeContent.Contains("--radius-"), Is.True);
                Assert.That(myceliumThemeContent.Contains("--spacing-"), Is.True);
                Assert.That(myceliumThemeContent.Contains("--text-"), Is.True);
            }
        }
    }
}
