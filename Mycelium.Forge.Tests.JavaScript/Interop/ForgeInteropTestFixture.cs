// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeInteropTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.JavaScript.Interop
{
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// Test fixture that executes the Node.js test suite for the JavaScript interop layer.
    /// Marked with <see cref="CategoryAttribute" /> of "IgnoreOnCI" because GitHub Actions runs
    /// the Node.js test runner directly with coverage collection in a dedicated step.
    /// </summary>
    [TestFixture]
    [Category("IgnoreOnCI")]
    public class ForgeInteropTestFixture
    {
        private static readonly TimeSpan Timeout = TimeSpan.FromMinutes(2);
        private string testsDirectory;

        [SetUp]
        public void Setup()
        {
            this.testsDirectory = TestContext.CurrentContext.TestDirectory;
            var directory = new DirectoryInfo(this.testsDirectory);

            // Looks for the root directory of the project by searching for the .csproj file
            while (directory != null && !File.Exists(Path.Combine(directory.FullName, "Mycelium.Forge.Tests.JavaScript.csproj")))
            {
                directory = directory.Parent;
            }

            Assert.That(directory, Is.Not.Null);
            this.testsDirectory = directory.FullName;
        }

        [Test]
        public async Task VerifyForgeInteropJavaScriptTests()
        {
            var testFiles = Directory.GetFiles(this.testsDirectory, "*.test.mjs", SearchOption.AllDirectories);
            Assert.That(testFiles, Is.Not.Empty);

            var startInfo = new ProcessStartInfo("node")
            {
                WorkingDirectory = this.testsDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                UseShellExecute = false
            };

            startInfo.ArgumentList.Add("--test");

            foreach (var testFile in testFiles)
            {
                startInfo.ArgumentList.Add(testFile);
            }

            try
            {
                var process = Process.Start(startInfo);

                if (process == null)
                {
                    Assert.Fail("Failed to start Node.js process.");
                    return;
                }

                using (process)
                {
                    var standardOutputTask = process.StandardOutput.ReadToEndAsync();
                    var standardErrorTask = process.StandardError.ReadToEndAsync();

                    using var cancellation = new CancellationTokenSource(Timeout);
                    await process.WaitForExitAsync(cancellation.Token);

                    await Task.WhenAll(standardOutputTask, standardErrorTask);

                    await TestContext.Out.WriteLineAsync(standardOutputTask.Result);
                    await TestContext.Out.WriteLineAsync(standardErrorTask.Result);

                    Assert.That(process.ExitCode, Is.Zero);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"An error has occurred while running the JavaScript tests: {ex.Message}");
            }
        }
    }
}
