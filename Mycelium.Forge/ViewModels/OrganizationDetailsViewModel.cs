// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationDetailsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using Mycelium.Forge.Models;

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
        public IReadOnlyList<PackageRowModel> Packages { get; set; } = [];

        /// <summary>
        /// Initializes the organization view model state for the specified organization identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the organization.</param>
        public void InitializeViewModel(Guid id)
        {
            this.Organization = new OrganizationModel(
                "Starion Group",
                "SG",
                "@starion",
                "Systems engineering models and ECSS mission libraries for early-phase spacecraft design.",
                true,
                2,
                4,
                390,
                2025);

            this.Packages =
            [
                new PackageRowModel(
                    "ecss-e-st-32-10c",
                    "/packages/starion/ecss-e-st-32-10c",
                    "RF telecommunication link budget and space communication interfaces.",
                    "SysML v2",
                    "@starion",
                    "v0.3.0",
                    "comms · rf · telemetry · ecss",
                    "2 months ago",
                    "190"),
                new PackageRowModel(
                    "ecss-e-st-31-01c",
                    "/packages/starion/ecss-e-st-31-01c",
                    "Structural and mechanical engineering domain metamodels and loads analysis.",
                    "SysML v2",
                    "@starion",
                    "v1.0.0",
                    "mechanical · structures · loads · ecss",
                    "3 months ago",
                    "165")
            ];
        }
    }
}
