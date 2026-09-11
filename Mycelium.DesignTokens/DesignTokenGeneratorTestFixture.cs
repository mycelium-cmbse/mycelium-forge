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
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Determine the project root directory by traversing up the directory hierarchy
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
            var result = await DesignTokenGenerator.GenerateAsync(options);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.StandardError, Is.Null.Or.Empty);
                Assert.That(result.ExitCode, Is.Zero);
                Assert.That(result.GeneratedFiles, Has.Count.EqualTo(3));
            }

            // Verify that the expected output files exist and contain the expected content
            var forgeCombinedPath = Path.Combine(this.TestOutputDirectory, "tokens-forge.css");
            var bloomCombinedPath = Path.Combine(this.TestOutputDirectory, "tokens-bloom.css");
            var myceliumThemePath = Path.Combine(this.TestOutputDirectory, "theme-mycelium.css");

            var combinedForgeContent = File.Exists(forgeCombinedPath) ? await File.ReadAllTextAsync(forgeCombinedPath) : string.Empty;
            var combinedBloomContent = File.Exists(bloomCombinedPath) ? await File.ReadAllTextAsync(bloomCombinedPath) : string.Empty;
            var myceliumThemeContent = File.Exists(myceliumThemePath) ? await File.ReadAllTextAsync(myceliumThemePath) : string.Empty;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(File.Exists(forgeCombinedPath), Is.True);
                Assert.That(File.Exists(bloomCombinedPath), Is.True);
                Assert.That(File.Exists(myceliumThemePath), Is.True);
                Assert.That(combinedForgeContent, Does.Contain(":root"));
                Assert.That(combinedForgeContent, Does.Contain(".dark"));
                Assert.That(combinedForgeContent, Does.Contain("--primary"));
                Assert.That(combinedForgeContent, Does.Contain("--background"));
                Assert.That(combinedBloomContent, Does.Contain(":root"));
                Assert.That(combinedBloomContent, Does.Contain(".dark"));
                Assert.That(combinedForgeContent, Does.Contain("--spacing-"));
                Assert.That(myceliumThemeContent, Does.Contain("@theme inline"));
                Assert.That(myceliumThemeContent, Does.Contain("--color-"));
                Assert.That(myceliumThemeContent, Does.Contain("--radius-"));
                Assert.That(myceliumThemeContent, Does.Contain("--spacing-"));
                Assert.That(myceliumThemeContent, Does.Contain("--text-"));
            }
        }
    }
}
