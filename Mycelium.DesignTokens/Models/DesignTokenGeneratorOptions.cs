// ------------------------------------------------------------------------------------------------
// <copyright file="DesignTokenGeneratorOptions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.DesignTokens.Models
{
    /// <summary>
    /// Represents configuration options for executing the design token generation process.
    /// </summary>
    public class DesignTokenGeneratorOptions
    {
        /// <summary>
        /// Gets or sets the target application for token generation (e.g. "forge", "bloom", or "all").
        /// </summary>
        public string Target { get; set; } = "all";

        /// <summary>
        /// Gets or sets the relative or absolute path to the Style Dictionary generation script.
        /// </summary>
        public string ScriptPath { get; set; } = Path.Combine("Javascript", "build-tokens.mjs");

        /// <summary>
        /// Gets or sets the output directory where generated CSS files are saved.
        /// </summary>
        public string OutputDirectory { get; set; } = Path.Combine("Styles", "dist");

        /// <summary>
        /// Gets or sets the path to the DTCG JSON tokens directory.
        /// </summary>
        public string TokensDirectory { get; set; } = Path.Combine("Styles", "tokens");

        /// <summary>
        /// Gets or sets the working directory in which the generator script is executed.
        /// </summary>
        public string WorkingDirectory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the execution timeout in seconds for the generation process. Defaults to 30 seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;
    }
}
