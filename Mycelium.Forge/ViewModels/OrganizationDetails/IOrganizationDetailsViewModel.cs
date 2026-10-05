// ------------------------------------------------------------------------------------------------
// <copyright file="IOrganizationDetailsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.OrganizationDetails
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.ViewModels.Rows;

    /// <summary>
    /// Defines the view model contract for the organization and publisher profile page.
    /// </summary>
    public interface IOrganizationDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the organization profile details.
        /// </summary>
        IOrganization Organization { get; set; }

        /// <summary>
        /// Gets or sets the collection of package rows published by the organization.
        /// </summary>
        IReadOnlyList<PackageRowViewModel> Packages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is an administrator of the organization.
        /// </summary>
        bool IsUserAdmin { get; set; }

        /// <summary>
        /// Initializes the organization view model state for the specified organization short name asynchronously.
        /// </summary>
        /// <param name="shortName">The short name of the organization.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous initialization.</returns>
        Task InitializeViewModel(string shortName, CancellationToken cancellationToken = default);
    }
}
