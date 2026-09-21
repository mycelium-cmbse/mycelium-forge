// ------------------------------------------------------------------------------------------------
// <copyright file="PackageOverviewTab.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageDetails.Tabs
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents the overview and usage README tab component for package details.
    /// </summary>
    public partial class PackageOverviewTab : ComponentBase
    {
        /// <summary>
        /// Gets or sets the package DTO.
        /// </summary>
        [Parameter]
        public IPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the currently selected package version DTO.
        /// </summary>
        [Parameter]
        public IPackageVersion Version { get; set; }

        /// <summary>
        /// Gets the code usage import statement.
        /// </summary>
        public string CodeUsageImport => InstallCommandHelper.GenerateSysMlV2Import(this.Package.ShortName);

        /// <summary>
        /// Gets the code usage example body statement.
        /// </summary>
        public string CodeUsageBody => "part def MySystem :> BaseSystem { }";

        /// <summary>
        /// Extracts section titles from the release README markdown content.
        /// </summary>
        /// <returns>A comma-separated string of section titles, or an empty string.</returns>
        public string GetReadmeContents()
        {
            if (string.IsNullOrWhiteSpace(this.Version.Readme))
            {
                return string.Empty;
            }

            var titles = this.Version.Readme
                .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .Where(line => line.StartsWith('#'))
                .Select(line => line.TrimStart('#').Trim())
                .Where(title => !string.IsNullOrWhiteSpace(title))
                .ToList();

            return titles.Count > 0 ? string.Join(", ", titles) : string.Empty;
        }
    }
}
