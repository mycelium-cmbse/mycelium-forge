// ------------------------------------------------------------------------------------------------
// <copyright file="Documentation.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Documentation
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.ViewModels.Documentation;

    /// <summary>
    /// Code-behind logic for the Mycelium Forge documentation overview and index page.
    /// </summary>
    public partial class Documentation : ComponentBase
    {
        /// <summary>
        /// Gets or sets the documentation view model dependency.
        /// </summary>
        [Inject]
        public IDocumentationViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager for URL resolution.
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Records user feedback on the documentation page.
        /// </summary>
        /// <param name="isHelpful">A value indicating whether the page content was helpful.</param>
        public void SubmitFeedback(bool isHelpful)
        {
            this.ViewModel.RecordFeedback(isHelpful);
        }

        /// <summary>
        /// Initializes the component state and sets up view model data for overview.
        /// </summary>
        protected override void OnInitialized()
        {
            this.ViewModel.InitializeOverview();
        }
    }
}
