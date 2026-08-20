// ------------------------------------------------------------------------------------------------
// <copyright file="PackageSettingsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models;

    /// <summary>
    /// Provides view model state and management logic for the Mycelium Forge package settings page.
    /// </summary>
    public class PackageSettingsViewModel : IPackageSettingsViewModel
    {
        /// <summary>
        /// Gets or sets the package model.
        /// </summary>
        public PackageModel Package { get; set; }

        /// <summary>
        /// Initializes the package settings view model state for the specified package identifier.
        /// </summary>
        /// <param name="id">The unique identifier or name of the package.</param>
        /// <param name="scope">The scope identifier.</param>
        public void InitializeViewModel(string id, string scope)
        {
            var maintainers = new List<PackageMaintainerModel>
            {
                new("Starion Group", "SG", true, "Organization · Owner"),
                new("R. André", "SG", false, "You · individual account"),
                new("J. Klein", "SG", false, "Maintainer")
            };

            var versions = new List<PackageVersionModel>
            {
                new("v1.2.0", "published 2 weeks ago", isLatest: true),
                new("v1.1.0", "published 2 months ago"),
                new("v1.0.0", "published 4 months ago", isUnlisted: true)
            };

            var resolvedScope = string.IsNullOrWhiteSpace(scope) ? "@starion" : scope.StartsWith('@') ? scope : $"@{scope}";
            var resolvedName = string.IsNullOrWhiteSpace(id) ? "ECSS-MM-PWR" : id;

            this.Package = new PackageModel(
                resolvedName,
                $"/packages/{resolvedScope.TrimStart('@')}/{resolvedName}",
                string.Empty,
                "SysML v2",
                resolvedScope,
                "v1.2.0",
                string.Empty,
                "210",
                true,
                VisibilityKind.PUBLIC,
                "2 weeks ago",
                "Owner",
                maintainers,
                versions);
        }

        /// <summary>
        /// Saves the exposed package model state.
        /// </summary>
        public void SavePackage()
        {
        }
    }
}
