// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationDetailsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.OrganizationDetails
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Models.Organization;
    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Provides view model state and operations for the Mycelium Forge organization and publisher profile page.
    /// </summary>
    public class OrganizationDetailsViewModel : IOrganizationDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the organization profile details.
        /// </summary>
        public OrganizationModel Organization { get; set; }

        /// <summary>
        /// Gets or sets the collection of packages published by the organization.
        /// </summary>
        public List<PackageModel> Packages { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether the current user is an administrator of the organization.
        /// </summary>
        public bool IsUserAdmin { get; set; } = true;

        /// <summary>
        /// Initializes the organization view model state for the specified organization identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the organization.</param>
        public void InitializeViewModel(Guid id)
        {
            this.Organization = new OrganizationModel(
                SeedData.StarionOrganization,
                SeedData.StarionOrganization.Origin,
                true,
                6,
                14,
                390);

            this.Packages =
            [
                new PackageModel(
                    new Package { Name = "ecss-e-st-32-10c", ShortName = "ecss-e-st-32-10c", Visibility = VisibilityKind.PUBLIC, CreatedAt = DateTime.UtcNow.AddDays(-60), Description = "A package for handling ECSS E-ST-32-10c specifications." },
                    "@starion",
                    "v0.3.0",
                    PackageFormatConstants.SysMlV2,
                    "comms · rf · telemetry · ecss",
                    190),
                new PackageModel(
                    new Package { Name = "ecss-e-st-31-01c", ShortName = "ecss-e-st-31-01c", Visibility = VisibilityKind.PUBLIC, CreatedAt = DateTime.UtcNow.AddDays(-90), Description = "A package for handling ECSS E-ST-31-01c specifications." },
                    "@starion",
                    "v1.0.0",
                    PackageFormatConstants.SysMlV2,
                    "mechanical · structures · loads · ecss",
                    165)
            ];
        }
    }
}
