// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationDetails.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.ViewModels.OrganizationDetails;

    /// <summary>
    /// Represents the organization publisher profile and packages view of the Mycelium Forge registry.
    /// </summary>
    public partial class OrganizationDetails : ComponentBase
    {
        /// <summary>
        /// Gets or sets the organization short name supplied from the URL route.
        /// </summary>
        [Parameter]
        public string ShortName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the view model for the organization profile page.
        /// </summary>
        [Inject]
        public IOrganizationDetailsViewModel ViewModel { get; set; }

        /// <summary>
        /// Handles component parameter updates and initializes the view model with the organization short name asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the initialization.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await this.ViewModel.InitializeViewModel(this.ShortName);
        }
    }
}
