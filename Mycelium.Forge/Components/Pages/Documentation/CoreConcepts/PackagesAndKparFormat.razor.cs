// ------------------------------------------------------------------------------------------------
// <copyright file="PackagesAndKparFormat.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Documentation.CoreConcepts
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.ViewModels.Documentation;

    /// <summary>
    /// Code-behind logic for the Packages and kpar format documentation topic page.
    /// </summary>
    public partial class PackagesAndKparFormat : ComponentBase
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
        /// Gets the manifest JSON snippet code displayed in the code block.
        /// </summary>
        public string ProjectJsonSnippet =>
            """
            {
              "name": "@mycelium/isq-quantities-units",
              "version": "1.2.0",
              "license": "Apache-2.0",
              "metamodel": "SysML v2 2025.1",
              "description": "Standard International System of Quantities and Units library package for SysML v2 models.",
              "dependencies": {}
            }
            """;

        /// <summary>
        /// Records user feedback on the documentation page.
        /// </summary>
        /// <param name="isHelpful">A value indicating whether the page content was helpful.</param>
        public void SubmitFeedback(bool isHelpful)
        {
            this.ViewModel.RecordFeedback(isHelpful);
        }

        /// <summary>
        /// Initializes the component state and sets up view model data for packages and kpar format.
        /// </summary>
        protected override void OnInitialized()
        {
            this.ViewModel.InitializePackagesAndKpar();
        }
    }
}
