// ------------------------------------------------------------------------------------------------
// <copyright file="DesignTokenResult.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.DesignTokens.Models
{
    /// <summary>
    /// Encapsulates the execution result of the design token generation process.
    /// </summary>
    public class DesignTokenResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the token generation succeeded.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the process exit code returned by the generator script.
        /// </summary>
        public int ExitCode { get; set; }

        /// <summary>
        /// Gets or sets the standard output captured during process execution.
        /// </summary>
        public string StandardOutput { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the standard error captured during process execution.
        /// </summary>
        public string StandardError { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of file paths generated during execution.
        /// </summary>
        public IReadOnlyList<string> GeneratedFiles { get; set; } = [];
    }
}
