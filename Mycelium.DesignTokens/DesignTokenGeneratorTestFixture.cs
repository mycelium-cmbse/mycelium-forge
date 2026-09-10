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

            this.TestOutputDirectory = Path.Combine(this.ProjectDirectory, "Styles", "test-dist");

            if (Directory.Exists(this.TestOutputDirectory))
            {
                Directory.Delete(this.TestOutputDirectory, true);
            }
        }

        /// <summary>
        /// Cleans up temporary test artifacts after each test execution.
        /// </summary>
        [TearDown]
        public async Task TearDown()
        {
            if (!Directory.Exists(this.TestOutputDirectory))
            {
                return;
            }

            try
            {
                Directory.Delete(this.TestOutputDirectory, true);
            }
            catch
            {
                await Console.Error.WriteLineAsync("Failed to delete test output directory.");
            }
        }

        /// <summary>
        /// Verifies the asynchronous execution of the design token generation process.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Test]
        public async Task VerifyGenerateAsync()
        {
            var options = new DesignTokenGeneratorOptions
            {
                Target = "all",
                ScriptPath = Path.Combine("Javascript", "build-tokens.mjs"),
                OutputDirectory = Path.Combine("Styles", "test-dist"),
                TokensDirectory = Path.Combine("Styles", "tokens"),
                WorkingDirectory = this.ProjectDirectory,
                TimeoutSeconds = 30
            };

            // Generates the css files from the DTCG tokens
            var result = await this.Generator.GenerateAsync(options);

            // Verify that the expected output files exist and contain the expected content
            var forgeCombinedPath = Path.Combine(this.TestOutputDirectory, "tokens-forge.css");
            var bloomCombinedPath = Path.Combine(this.TestOutputDirectory, "tokens-bloom.css");
            var forgeLightPath = Path.Combine(this.TestOutputDirectory, "tokens-forge-light.css");
            var forgeDarkPath = Path.Combine(this.TestOutputDirectory, "tokens-forge-dark.css");

            var lightContent = File.Exists(forgeLightPath) ? await File.ReadAllTextAsync(forgeLightPath) : string.Empty;
            var darkContent = File.Exists(forgeDarkPath) ? await File.ReadAllTextAsync(forgeDarkPath) : string.Empty;
            var combinedContent = File.Exists(forgeCombinedPath) ? await File.ReadAllTextAsync(forgeCombinedPath) : string.Empty;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.ExitCode, Is.EqualTo(0));
                Assert.That(result.GeneratedFiles, Has.Count.GreaterThanOrEqualTo(6));
                Assert.That(File.Exists(forgeCombinedPath), Is.True);
                Assert.That(File.Exists(bloomCombinedPath), Is.True);
                Assert.That(File.Exists(forgeLightPath), Is.True);
                Assert.That(File.Exists(forgeDarkPath), Is.True);
                Assert.That(lightContent.Contains(":root"), Is.True);
                Assert.That(lightContent.Contains("--primary"), Is.True);
                Assert.That(lightContent.Contains("--background"), Is.True);
                Assert.That(darkContent.Contains(".dark"), Is.True);
                Assert.That(darkContent.Contains("--primary"), Is.True);
                Assert.That(darkContent.Contains("--background"), Is.True);
                Assert.That(combinedContent.Contains(":root"), Is.True);
                Assert.That(combinedContent.Contains(".dark"), Is.True);
            }
        }
    }
}
