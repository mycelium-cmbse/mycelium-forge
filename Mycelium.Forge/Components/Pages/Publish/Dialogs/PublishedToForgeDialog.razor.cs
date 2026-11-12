// ------------------------------------------------------------------------------------------------
// <copyright file="PublishedToForgeDialog.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Publish.Dialogs
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents a modal dialog component confirming the successful publication of a package to Mycelium Forge.
    /// </summary>
    public partial class PublishedToForgeDialog : ComponentBase
    {
        /// <summary>
        /// Gets or sets the cascading dialog reference used to control and close the dialog.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager instance.
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the publishing scope or owner identifier.
        /// </summary>
        [Parameter]
        public string Scope { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the published package short name.
        /// </summary>
        [Parameter]
        public string PackageShortname { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the published package version string.
        /// </summary>
        [Parameter]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Handles the close action, dismissing the dialog and navigating to the my packages page.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnCloseClicked()
        {
            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }

            this.NavigationManager.NavigateTo(PageRoutes.MyPackages);
        }

        /// <summary>
        /// Handles the view package action, dismissing the dialog and navigating to the package details.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnViewPackageClicked()
        {
            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }

            this.NavigationManager.NavigateTo(PageRoutes.GetPackageRoute(this.Scope, this.PackageShortname));
        }
    }
}
