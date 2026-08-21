// ------------------------------------------------------------------------------------------------
// <copyright file="Home.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.ViewModels.Home;

    /// <summary>
    /// Represents the home landing page of the Mycelium Forge registry.
    /// </summary>
    public partial class Home : ComponentBase
    {
        /// <summary>
        /// Gets or sets the view model for the home landing page.
        /// </summary>
        [Inject]
        public IHomeViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes the component lifecycle and populates the view model state.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ViewModel.InitializeViewModel();
        }
    }
}
