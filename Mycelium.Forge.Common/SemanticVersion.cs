// ------------------------------------------------------------------------------------------------
// <copyright file="SemanticVersion.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides regular expressions and utilities for SemVer 2.0.0 semantic versions.
    /// </summary>
    public static partial class SemanticVersion
    {
        /// <summary>
        /// Gets the regular expression of the SemVer 2.0.0 grammar published at https://semver.org.
        /// </summary>
        /// <returns>The <see cref="Regex" />.</returns>
        [GeneratedRegex(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$")]
        public static partial Regex VersionFormatRegex();
    }
}
