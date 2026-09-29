// ------------------------------------------------------------------------------------------------
// <copyright file="InstallCommandHelper.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Provides helper methods for constructing installation command strings for packages.
    /// </summary>
    public static class InstallCommandHelper
    {
        /// <summary>
        /// Generates the SysML v2 import statement for the specified package short name.
        /// </summary>
        /// <param name="shortName">The package short name (slug).</param>
        /// <returns>The formatted SysML v2 import statement string.</returns>
        public static string GenerateSysMlV2Import(string shortName)
        {
            return $"import {shortName.Replace('-', '_')}::*;";
        }

        /// <summary>
        /// Generates a dictionary of install command strings for all supported installation methods.
        /// </summary>
        /// <param name="publisher">The publisher scope name without the at sign.</param>
        /// <param name="shortName">The package short name (slug).</param>
        /// <param name="version">The package version string without the 'v' prefix.</param>
        /// <returns>A dictionary mapping installation tab identifiers to command strings.</returns>
        public static Dictionary<string, string> GenerateInstallCommands(string publisher, string shortName, string version)
        {
            var fullName = $"@{publisher}/{shortName}";

            return new Dictionary<string, string>
            {
                { InstallCommandConstants.ForgeCli, $"forge add {fullName}@^{version}" },
                { InstallCommandConstants.SysMlV2Import, GenerateSysMlV2Import(shortName) },
                { InstallCommandConstants.Manifest, $"{fullName} = \"^{version}\"" },
                { InstallCommandConstants.Purl, GeneratePurl(publisher, shortName, version) }
            };
        }

        /// <summary>
        /// Generates the canonical Package URL (purl) identifier.
        /// </summary>
        /// <param name="publisher">The publisher scope name without the at sign.</param>
        /// <param name="shortName">The package short name (slug).</param>
        /// <param name="version">The package version string without the 'v' prefix.</param>
        /// <returns>The formatted Package URL string.</returns>
        public static string GeneratePurl(string publisher, string shortName, string version)
        {
            return $"pkg:forge/{publisher}/{shortName}@{version}";
        }
    }
}
