// ------------------------------------------------------------------------------------------------
// <copyright file="IOrganizationDetailsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using Mycelium.Forge.Models.Organization;
    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Defines the view model contract for the organization and publisher profile page.
    /// </summary>
    public interface IOrganizationDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the organization profile details.
        /// </summary>
        OrganizationModel Organization { get; set; }

        /// <summary>
        /// Gets or sets the collection of packages published by the organization.
        /// </summary>
        List<PackageModel> Packages { get; set; }

        /// <summary>
        /// Initializes the organization view model state for the specified organization identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the organization.</param>
        void InitializeViewModel(Guid id);
    }
}
