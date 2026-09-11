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
    using System.Runtime.InteropServices;

    using Mycelium.DesignTokens.Models;

    /// <summary>
    /// Executes the Style Dictionary process to transform DTCG design tokens into CSS stylesheets.
    /// </summary>
    public static class DesignTokenGenerator
    {
        /// <summary>
        /// Asynchronously generates CSS stylesheets from design tokens based on the provided configuration options.
        /// </summary>
        /// <param name="options">The configuration options specifying targets, script path, and directories.</param>
        /// <returns>
        /// A task returning a <see cref="DesignTokenResult" /> containing the outcome, exit code, output messages, and
        /// list of generated files.
        /// </returns>
        public static async Task<DesignTokenResult> GenerateAsync(DesignTokenGeneratorOptions options)
        {
            var nodeInstallationError = await VerifyNodeIsInstalled();

            if (!string.IsNullOrEmpty(nodeInstallationError))
            {
                return new DesignTokenResult
                {
                    IsSuccess = false,
                    ExitCode = -1,
                    StandardError = nodeInstallationError
                };
            }

            var resolvedWorkingDirectory = ResolveWorkingDirectory(options);
            var npmInstallError = await RunNpmInstallAsync(options, resolvedWorkingDirectory);

            if (!string.IsNullOrEmpty(npmInstallError))
            {
                return new DesignTokenResult
                {
                    IsSuccess = false,
                    ExitCode = -1,
                    StandardError = npmInstallError
                };
            }

            var processStartInformation = CreateProcessStartInformation(options, resolvedWorkingDirectory);

            // A cancellation token source is used to enforce a timeout for the process execution.
            using var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(options.TimeoutSeconds));
            using var process = new Process();
            process.StartInfo = processStartInformation;

            try
            {
                process.Start();

                var standardOutput = await process.StandardOutput.ReadToEndAsync(timeoutCancellationTokenSource.Token);
                var standardError = await process.StandardError.ReadToEndAsync(timeoutCancellationTokenSource.Token);
                await process.WaitForExitAsync(timeoutCancellationTokenSource.Token);

                return new DesignTokenResult
                {
                    IsSuccess = process.ExitCode == 0,
                    ExitCode = process.ExitCode,
                    StandardOutput = standardOutput,
                    StandardError = standardError,
                    GeneratedFiles = CollectGeneratedFiles(options, resolvedWorkingDirectory)
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
                    StandardError = $"The design token generation process timed out after {options.TimeoutSeconds} seconds."
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
                CreateNoWindow = true
            };
        }

        /// <summary>
        /// Scans the target output directory and gathers the paths of generated CSS files.
        /// </summary>
        /// <param name="options">The generator configuration options.</param>
        /// <param name="workingDirectory">The working directory in which execution took place.</param>
        /// <returns>A list of generated CSS file paths.</returns>
        private static List<string> CollectGeneratedFiles(DesignTokenGeneratorOptions options, string workingDirectory)
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

        /// <summary>
        /// Ensures that the Node.js package dependencies are installed before executing the script.
        /// </summary>
        /// <param name="options">The generator configuration options.</param>
        /// <param name="workingDirectory">The working directory in which execution takes place.</param>
        /// <returns>A task representing the asynchronous operation. Contains the error message, if available.</returns>
        private static async Task<string> RunNpmInstallAsync(DesignTokenGeneratorOptions options, string workingDirectory)
        {
            var scriptPath = Path.Combine(workingDirectory, options.ScriptPath);
            var scriptDirectory = Path.GetDirectoryName(scriptPath) ?? workingDirectory;
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            var processStartInformation = new ProcessStartInfo
            {
                FileName = isWindows ? "cmd.exe" : "npm",
                Arguments = isWindows ? "/c npm install" : "install",
                WorkingDirectory = scriptDirectory,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(processStartInformation);

            if (process == null)
            {
                return "Failed to start the npm process for installing dependencies.";
            }

            var standardError = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            return process.ExitCode != 0
                ? $"npm install failed with exit code {process.ExitCode}: {standardError}"
                : string.Empty;
        }

        /// <summary>
        /// Verifies that Node.js is installed on the host environment.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. Contains the error message, if available.</returns>
        private static async Task<string> VerifyNodeIsInstalled()
        {
            var processStartInformation = new ProcessStartInfo
            {
                FileName = "node",
                Arguments = "--version",
                CreateNoWindow = true
            };

            using var process = Process.Start(processStartInformation);

            if (process == null)
            {
                return "Node.js is not installed or could not be started.";
            }

            await process.WaitForExitAsync();

            return process.ExitCode != 0
                ? $"Node.js execution failed with exit code {process.ExitCode}."
                : string.Empty;
        }
    }
}
