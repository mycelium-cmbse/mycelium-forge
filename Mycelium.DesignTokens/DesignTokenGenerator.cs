// ------------------------------------------------------------------------------------------------
// <copyright file="DesignTokenGenerator.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.DesignTokens
{
    using System.Diagnostics;

    using Mycelium.DesignTokens.Models;

    /// <summary>
    /// Executes the Style Dictionary process to transform DTCG design tokens into CSS stylesheets.
    /// </summary>
    public class DesignTokenGenerator
    {
        /// <summary>
        /// Asynchronously generates CSS stylesheets from design tokens based on the provided configuration options.
        /// </summary>
        /// <param name="options">The configuration options specifying targets, script path, and directories.</param>
        /// <returns>
        /// A task returning a <see cref="DesignTokenResult" /> containing the outcome, exit code, output messages, and
        /// list of generated files.
        /// </returns>
        public async Task<DesignTokenResult> GenerateAsync(DesignTokenGeneratorOptions options)
        {
            var resolvedWorkingDirectory = ResolveWorkingDirectory(options);
            var processStartInformation = CreateProcessStartInformation(options, resolvedWorkingDirectory);

            using var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(options.TimeoutSeconds));
            using var process = new Process();
            process.StartInfo = processStartInformation;

            try
            {
                process.Start();

                var standardOutputTask = process.StandardOutput.ReadToEndAsync(timeoutCancellationTokenSource.Token);
                var standardErrorTask = process.StandardError.ReadToEndAsync(timeoutCancellationTokenSource.Token);

                await process.WaitForExitAsync(timeoutCancellationTokenSource.Token);
                await Task.WhenAll(standardOutputTask, standardErrorTask);

                var exitCode = process.ExitCode;
                var isSuccess = exitCode == 0;

                var generatedFiles = CollectGeneratedFiles(options, resolvedWorkingDirectory);

                return new DesignTokenResult
                {
                    IsSuccess = isSuccess,
                    ExitCode = exitCode,
                    StandardOutput = await standardOutputTask,
                    StandardError = await standardErrorTask,
                    GeneratedFiles = generatedFiles
                };
            }
            catch (OperationCanceledException) when (timeoutCancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    process.Kill(true);
                }
                catch (Exception exception)
                {
                    await Console.Error.WriteLineAsync(exception.Message);
                }

                return new DesignTokenResult
                {
                    IsSuccess = false,
                    ExitCode = -1,
                    StandardOutput = string.Empty,
                    StandardError = $"The design token generation process timed out after {options.TimeoutSeconds} seconds.",
                    GeneratedFiles = []
                };
            }
        }

        /// <summary>
        /// Resolves the working directory to use for executing the generator script.
        /// </summary>
        /// <param name="options">The generator configuration options.</param>
        /// <returns>The resolved absolute directory path.</returns>
        private static string ResolveWorkingDirectory(DesignTokenGeneratorOptions options)
        {
            if (!Directory.Exists(options.WorkingDirectory))
            {
                throw new DirectoryNotFoundException(options.WorkingDirectory);
            }

            return Path.GetFullPath(options.WorkingDirectory);
        }

        /// <summary>
        /// Creates the <see cref="ProcessStartInfo" /> configuration for invoking Node.js.
        /// </summary>
        /// <param name="options">The generator configuration options.</param>
        /// <param name="workingDirectory">The working directory for process execution.</param>
        /// <returns>A configured <see cref="ProcessStartInfo" /> instance.</returns>
        private static ProcessStartInfo CreateProcessStartInformation(DesignTokenGeneratorOptions options, string workingDirectory)
        {
            var outputDirectoryPath = Path.Combine(workingDirectory, options.OutputDirectory);
            var tokensDirectoryPath = Path.Combine(workingDirectory, options.TokensDirectory);
            var scriptPath = Path.Combine(workingDirectory, options.ScriptPath);

            var arguments = $"\"{scriptPath}\" --target={options.Target} --outDir=\"{outputDirectoryPath}\" --tokensDir=\"{tokensDirectoryPath}\"";

            return new ProcessStartInfo
            {
                FileName = "node",
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
        }

        /// <summary>
        /// Scans the target output directory and gathers the paths of generated CSS files.
        /// </summary>
        /// <param name="options">The generator configuration options.</param>
        /// <param name="workingDirectory">The working directory in which execution took place.</param>
        /// <returns>A read-only list of generated CSS file paths.</returns>
        private static IReadOnlyList<string> CollectGeneratedFiles(DesignTokenGeneratorOptions options, string workingDirectory)
        {
            var outputDirectoryPath = Path.Combine(workingDirectory, options.OutputDirectory);

            if (!Directory.Exists(outputDirectoryPath))
            {
                throw new DirectoryNotFoundException(outputDirectoryPath);
            }

            return Directory.GetFiles(outputDirectoryPath, "*.css", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFullPath)
                .ToList();
        }
    }
}
