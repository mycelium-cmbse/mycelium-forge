// ------------------------------------------------------------------------------------------------
// <copyright file="AccountDetails.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.ViewModels.AccountDetails;

    /// <summary>
    /// Represents the account publisher profile and packages view of the Mycelium Forge registry.
    /// </summary>
    public partial class AccountDetails : ComponentBase
    {
        /// <summary>
        /// Gets or sets the account short name supplied from the URL route.
        /// </summary>
        [Parameter]
        public string ShortName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the view model for the account profile page.
        /// </summary>
        [Inject]
        public IAccountDetailsViewModel ViewModel { get; set; }

        /// <summary>
        /// Handles component parameter updates and initializes the view model with the account short name asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the initialization.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (string.IsNullOrWhiteSpace(this.ShortName))
            {
                return;
            }

            await this.ViewModel.InitializeViewModel(this.ShortName);
        }
    }
}
