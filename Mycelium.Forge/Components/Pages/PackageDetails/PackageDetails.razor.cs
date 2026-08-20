// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDetails.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageDetails
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;

    using Mycelium.Forge.ViewModels;

    /// <summary>
    /// Represents the package details and release overview page for Mycelium Forge packages.
    /// </summary>
    public partial class PackageDetails : ComponentBase
    {
        /// <summary>
        /// Gets or sets the scope segment supplied from the URL route.
        /// </summary>
        [Parameter]
        public string Scope { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the package identifier supplied from the URL route.
        /// </summary>
        [Parameter]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the view model for the package details page.
        /// </summary>
        [Inject]
        public IPackageDetailsViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the JavaScript runtime instance for browser interactions.
        /// </summary>
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        /// <summary>
        /// Gets or sets the currently selected install method tab.
        /// </summary>
        public string SelectedInstallTab { get; set; } = "Forge CLI";

        /// <summary>
        /// Gets the available installation method tabs.
        /// </summary>
        public IReadOnlyList<string> InstallTabs { get; } =
        [
            "Forge CLI",
            "SysML v2 import",
            "Manifest",
            "purl"
        ];

        /// <summary>
        /// Gets or sets the currently selected content section tab.
        /// </summary>
        public string SelectedContentTab { get; set; } = "Overview";

        /// <summary>
        /// Gets or sets a value indicating whether the current install command was copied to the clipboard.
        /// </summary>
        public bool IsCopied { get; set; }

        /// <summary>
        /// Gets the install command string for the currently selected install tab.
        /// </summary>
        /// <returns>The resolved install command string, or an empty string if not available.</returns>
        public string GetCurrentInstallCommand()
        {
            if (this.ViewModel.Package.InstallCommands != null && this.ViewModel.Package.InstallCommands.TryGetValue(this.SelectedInstallTab, out var command))
            {
                return command;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the dynamically computed collection of visible content tabs based on available package data.
        /// </summary>
        /// <returns>A list of tab names that have available content.</returns>
        public IReadOnlyList<string> GetVisibleContentTabs()
        {
            var tabs = new List<string> { "Overview" };

            if (this.ViewModel?.Package == null)
            {
                return tabs;
            }

            if (this.ViewModel.Package.Elements is { Count: > 0 })
            {
                tabs.Add("Contents");
            }

            if (this.ViewModel.Package.Dependencies is { Count: > 0 })
            {
                tabs.Add("Dependencies");
            }

            if (this.ViewModel.Package.Dependents is { Count: > 0 })
            {
                tabs.Add("Dependents");
            }

            if (this.ViewModel.Package.Versions is { Count: > 0 })
            {
                tabs.Add("Versions");
            }

            if (this.ViewModel.Package.ValidationReport is { Checks.Count: > 0 })
            {
                tabs.Add("Validation");
            }

            return tabs;
        }

        /// <summary>
        /// Selects an installation method tab and resets the copied status flag.
        /// </summary>
        /// <param name="tab">The name of the installation method tab.</param>
        public void SelectInstallTab(string tab)
        {
            this.SelectedInstallTab = tab;
            this.IsCopied = false;
        }

        /// <summary>
        /// Selects a main content navigation tab.
        /// </summary>
        /// <param name="tab">The name of the content tab.</param>
        public void SelectContentTab(string tab)
        {
            this.SelectedContentTab = tab;
        }

        /// <summary>
        /// Gets the CSS classes for an installation method tab trigger.
        /// </summary>
        /// <param name="tab">The installation method tab name.</param>
        /// <returns>The computed CSS class string.</returns>
        public string GetInstallTabClass(string tab)
        {
            const string baseClass = "cursor-pointer transition-colors";

            return this.SelectedInstallTab == tab
                ? $"{baseClass} font-semibold text-primary"
                : $"{baseClass} font-medium text-muted-foreground hover:text-foreground";
        }

        /// <summary>
        /// Gets the CSS classes for a content section tab trigger.
        /// </summary>
        /// <param name="tab">The content section tab name.</param>
        /// <returns>The computed CSS class string.</returns>
        public string GetContentTabClass(string tab)
        {
            const string baseClass = "h-full px-4 rounded-md text-sm leading-xs transition-colors whitespace-nowrap cursor-pointer";

            return this.SelectedContentTab == tab
                ? $"{baseClass} bg-primary/10 text-primary font-semibold"
                : $"{baseClass} text-muted-foreground hover:text-foreground hover:bg-muted/50 font-medium";
        }

        /// <summary>
        /// Copies the currently active install command to the user clipboard.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous copy operation.</returns>
        public async Task CopyInstallCommandToClipboard()
        {
            try
            {
                await this.JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", this.GetCurrentInstallCommand());
                this.IsCopied = true;
            }
            catch (Exception)
            {
                this.IsCopied = false;
            }
        }

        /// <summary>
        /// Handles component parameter updates and initializes the view model with the parsed unique identifier.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            var idParsed = Guid.TryParse(this.Id, out var parsedGuid)
                ? parsedGuid
                : Guid.Empty;

            this.ViewModel.InitializeViewModel(idParsed, this.Scope);
        }
    }
}
