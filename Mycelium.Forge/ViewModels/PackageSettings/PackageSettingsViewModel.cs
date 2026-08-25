// ------------------------------------------------------------------------------------------------
// <copyright file="PackageSettingsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.PackageSettings
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models.Package;

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
        /// Initializes the package settings view model state for the specified package name and organization.
        /// </summary>
        /// <param name="packageName">The name of the package.</param>
        /// <param name="organization">The organization identifier.</param>
        public void InitializeViewModel(string packageName, string organization)
        {
            var maintainers = new List<PackageMaintainerModel>
            {
                new("Starion Group", "SG", true, PackageInvitationKind.OWNER),
                new("R. André", "SG", false, PackageInvitationKind.OWNER),
                new("J. Klein", "SG")
            };

            var versions = new List<PackageVersionModel>
            {
                new("v1.2.0", "published 2 weeks ago", isLatest: true),
                new("v1.1.0", "published 2 months ago"),
                new("v1.0.0", "published 4 months ago", isUnlisted: true)
            };

            string resolvedOrganization;

            if (string.IsNullOrWhiteSpace(organization))
            {
                resolvedOrganization = "@starion";
            }
            else
            {
                resolvedOrganization = organization.StartsWith('@') ? organization : $"@{organization}";
            }

            var resolvedName = string.IsNullOrWhiteSpace(packageName) ? "ECSS-MM-PWR" : packageName;

            var packageDto = new Package
            {
                Name = resolvedName,
                ShortName = resolvedName.ToLowerInvariant(),
                Visibility = VisibilityKind.PUBLIC,
                CreatedAt = DateTime.UtcNow.AddDays(-14)
            };

            this.Package = new PackageModel(
                packageDto,
                resolvedOrganization,
                "v1.2.0",
                importCount: "210")
            {
                IsVerified = true,
                Role = PackageInvitationKind.OWNER,
                Href = PageRoutes.GetPackageRoute(resolvedOrganization, resolvedName),
                Maintainers = maintainers,
                Versions = versions
            };
        }

        /// <summary>
        /// Saves the exposed package model state.
        /// </summary>
        public void SavePackage()
        {
            // Persistence logic pending repository implementation.
        }
    }
}
