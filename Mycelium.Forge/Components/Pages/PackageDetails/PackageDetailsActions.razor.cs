// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDetailsActions.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageDetails
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageDetails.Dialogs;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.ViewModels.PackageDetails;

    /// <summary>
    /// Represents the interactive action buttons and installation commands panel for package details.
    /// </summary>
    public partial class PackageDetailsActions : ComponentBase
    {
        /// <summary>
        /// Gets or sets the dialog service used to display modal dialogs.
        /// </summary>
        [Inject]
        public DialogService DialogService { get; set; }

        /// <summary>
        /// Gets or sets the view model for the package details actions.
        /// </summary>
        [Inject]
        public IPackageDetailsActionsViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the package DTO.
        /// </summary>
        [Parameter]
        public IPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the owning organization DTO.
        /// </summary>
        [Parameter]
        public IOrganization Organization { get; set; }

        /// <summary>
        /// Gets or sets the currently selected package version DTO.
        /// </summary>
        [Parameter]
        public IPackageVersion SelectedVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is an administrator of the package.
        /// </summary>
        [Parameter]
        public bool IsUserAdmin { get; set; }

        /// <summary>
        /// Gets or sets the currently selected install method tab.
        /// </summary>
        public string SelectedInstallTab { get; set; } = InstallCommandConstants.ForgeCli;

        /// <summary>
        /// Gets the available installation method tabs.
        /// </summary>
        public IReadOnlyList<string> InstallTabs { get; } =
        [
            InstallCommandConstants.ForgeCli,
            InstallCommandConstants.SysMlV2Import,
            InstallCommandConstants.Manifest,
            InstallCommandConstants.Purl
        ];

        /// <summary>
        /// Gets the install command string for the currently selected install tab.
        /// </summary>
        /// <returns>The resolved install command string, or an empty string if not available.</returns>
        public string GetCurrentInstallCommand()
        {
            if (this.ViewModel?.InstallCommands == null)
            {
                return string.Empty;
            }

            return this.ViewModel.InstallCommands.TryGetValue(this.SelectedInstallTab, out var command)
                ? command
                : string.Empty;
        }

        /// <summary>
        /// Selects an installation method tab.
        /// </summary>
        /// <param name="tab">The name of the installation method tab.</param>
        public void SelectInstallTab(string tab)
        {
            this.SelectedInstallTab = tab;
        }

        /// <summary>
        /// Gets the CSS classes for an installation method tab trigger.
        /// </summary>
        /// <param name="tab">The installation method tab name.</param>
        /// <returns>The computed CSS class string.</returns>
        public string GetInstallTabClass(string tab)
        {
            const string baseClass = "cursor-pointer transition-colors";

            return string.Equals(this.SelectedInstallTab, tab, StringComparison.OrdinalIgnoreCase)
                ? $"{baseClass} font-semibold text-primary"
                : $"{baseClass} font-medium text-muted-foreground hover:text-foreground";
        }

        /// <summary>
        /// Opens the Add to Project dialog.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OpenAddToProjectDialog()
        {
            if (this.ViewModel?.Package == null || this.ViewModel.SelectedVersion == null)
            {
                return;
            }

            var onResult = new EventCallbackFactory().Create(this, (AddToProjectResult result) => this.HandleAddDependency(result));

            var parameters = new Dictionary<string, object>
            {
                { nameof(AddToProjectDialog.Package), this.ViewModel.Package },
                { nameof(AddToProjectDialog.PackageVersion), this.ViewModel.SelectedVersion },
                { nameof(AddToProjectDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Add to project",
                Description = $"{this.ViewModel.Package.GetFullName(this.ViewModel.Organization)} · {this.ViewModel.SelectedVersion.GetVersion()}"
            };

            await this.DialogService.OpenAsync<AddToProjectDialog>(parameters, options);
        }

        /// <summary>
        /// Handles the event when a package dependency is added to a project.
        /// </summary>
        /// <param name="result">The result containing the target project name and version constraint.</param>
        public void HandleAddDependency(AddToProjectResult result)
        {
            // Implementation pending future project integration support.
        }

        /// <summary>
        /// Opens the Migrate in Bloom dialog.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OpenMigrateInBloomDialog()
        {
            if (this.ViewModel?.Package == null || this.ViewModel.SelectedVersion == null)
            {
                return;
            }

            var onResult = new EventCallbackFactory().Create(this, (MigrateInBloomResult result) => this.HandleMigrateInBloom(result));

            var parameters = new Dictionary<string, object>
            {
                { nameof(MigrateInBloomDialog.Package), this.ViewModel.Package },
                { nameof(MigrateInBloomDialog.PackageVersion), this.ViewModel.SelectedVersion },
                { nameof(MigrateInBloomDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Migrate in Bloom",
                Description = $"{this.ViewModel.Package.GetFullName(this.ViewModel.Organization)} · {this.ViewModel.SelectedVersion.GetVersion()}"
            };

            await this.DialogService.OpenAsync<MigrateInBloomDialog>(parameters, options);
        }

        /// <summary>
        /// Handles the event when a package migration in Bloom is initiated.
        /// </summary>
        /// <param name="result">The result containing the target project name and version constraint.</param>
        public void HandleMigrateInBloom(MigrateInBloomResult result)
        {
            this.ViewModel.MigrateInBloom(result);
        }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.ViewModel.Initialize(this.Package, this.Organization, this.SelectedVersion, this.IsUserAdmin);
        }
    }
}
