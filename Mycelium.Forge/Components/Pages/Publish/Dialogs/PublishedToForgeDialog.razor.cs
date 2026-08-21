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
    using Mycelium.Forge.Models.Package;

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
        /// Gets or sets the published package model.
        /// </summary>
        [Parameter]
        public PackageModel Package { get; set; }

        /// <summary>
        /// Gets or sets the publishing scope or owner identifier.
        /// </summary>
        [Parameter]
        public string Scope { get; set; } = "@starion";

        /// <summary>
        /// Gets or sets the published package name.
        /// </summary>
        [Parameter]
        public string PackageName { get; set; } = "ECSS-MM-PWR";

        /// <summary>
        /// Gets or sets the published package version string.
        /// </summary>
        [Parameter]
        public string Version { get; set; } = "v1.3.0";

        /// <summary>
        /// Gets or sets the modal dialog title.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "Published to Forge";

        /// <summary>
        /// Gets or sets an optional custom description or body message override.
        /// </summary>
        [Parameter]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an explicit target URL to navigate to when viewing the package.
        /// </summary>
        [Parameter]
        public string PackageHref { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the event callback invoked when the dialog is closed.
        /// </summary>
        [Parameter]
        public EventCallback OnClose { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the view package action is clicked.
        /// </summary>
        [Parameter]
        public EventCallback OnViewPackage { get; set; }

        /// <summary>
        /// Gets the resolved full scoped package identifier string.
        /// </summary>
        /// <returns>The scoped package name representation.</returns>
        public string GetPackageFullName()
        {
            if (this.Package != null && !string.IsNullOrEmpty(this.Package.FullName))
            {
                return this.Package.FullName;
            }

            if (!string.IsNullOrEmpty(this.Scope) && !string.IsNullOrEmpty(this.PackageName))
            {
                var normalizedScope = this.Scope.StartsWith("@", StringComparison.OrdinalIgnoreCase)
                    ? this.Scope
                    : $"@{this.Scope}";

                return $"{normalizedScope}/{this.PackageName}";
            }

            return !string.IsNullOrEmpty(this.PackageName)
                ? this.PackageName
                : "@starion/ECSS-MM-PWR";
        }

        /// <summary>
        /// Gets the formatted release version string prefixed with 'v'.
        /// </summary>
        /// <returns>The formatted version string.</returns>
        public string GetFormattedVersion()
        {
            if (this.Package != null && !string.IsNullOrEmpty(this.Package.Version))
            {
                return this.Package.Version.StartsWith("v", StringComparison.OrdinalIgnoreCase)
                    ? this.Package.Version
                    : $"v{this.Package.Version}";
            }

            if (!string.IsNullOrEmpty(this.Version))
            {
                return this.Version.StartsWith("v", StringComparison.OrdinalIgnoreCase)
                    ? this.Version
                    : $"v{this.Version}";
            }

            return "v1.3.0";
        }

        /// <summary>
        /// Gets the description message explaining that the package is live and release validation is executing.
        /// </summary>
        /// <returns>The formatted description string.</returns>
        public string GetDescriptionText()
        {
            if (!string.IsNullOrEmpty(this.Description))
            {
                return this.Description;
            }

            var packageFullName = this.GetPackageFullName();
            var formattedVersion = this.GetFormattedVersion();

            return $"{packageFullName} {formattedVersion} is live. It is discoverable and importable, and release validation is running now.";
        }

        /// <summary>
        /// Gets the resolved navigation URL for the published package details page.
        /// </summary>
        /// <returns>The target package details relative URL.</returns>
        public string GetPackageHref()
        {
            if (!string.IsNullOrEmpty(this.PackageHref))
            {
                return this.PackageHref;
            }

            if (this.Package != null && !string.IsNullOrEmpty(this.Package.Href))
            {
                return this.Package.Href;
            }

            var cleanScope = (this.Scope ?? "starion").TrimStart('@');
            var cleanPackageName = this.PackageName ?? string.Empty;

            if (!string.IsNullOrEmpty(cleanPackageName))
            {
                return PageRoutes.GetPackageRoute(cleanScope, cleanPackageName);
            }

            return PageRoutes.Packages;
        }

        /// <summary>
        /// Handles the close action, dismissing the dialog and invoking the close callback.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnCloseClicked()
        {
            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }

            await this.OnClose.InvokeAsync();
        }

        /// <summary>
        /// Handles the view package action, invoking callbacks, dismissing the dialog, and navigating to the package details.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnViewPackageClicked()
        {
            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }

            await this.OnViewPackage.InvokeAsync();

            var href = this.GetPackageHref();

            if (!string.IsNullOrEmpty(href))
            {
                this.NavigationManager.NavigateTo(href);
            }
        }
    }
}
