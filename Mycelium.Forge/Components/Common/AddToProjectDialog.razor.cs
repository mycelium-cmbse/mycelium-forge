// ------------------------------------------------------------------------------------------------
// <copyright file="AddToProjectDialog.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models;
    using Mycelium.Forge.Models.DialogResults;

    /// <summary>
    /// Represents a dialog component for adding a package dependency constraint to a selected project.
    /// </summary>
    public partial class AddToProjectDialog : ComponentBase
    {
        /// <summary>
        /// Gets or sets the cascading dialog reference used to control and close the dialog.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the package model being added.
        /// </summary>
        [Parameter]
        public PackageModel Package { get; set; }

        /// <summary>
        /// Gets or sets the list of available target projects.
        /// </summary>
        [Parameter]
        public IReadOnlyList<string> Projects { get; set; } =
        [
            "Spacecraft Mission",
            "CubeSat Constellation",
            "Ground Station Network"
        ];

        /// <summary>
        /// Gets or sets the event callback invoked when the user confirms adding the dependency.
        /// </summary>
        [Parameter]
        public EventCallback<AddToProjectResult> OnResult { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the dialog is cancelled or closed.
        /// </summary>
        [Parameter]
        public EventCallback OnCancel { get; set; }

        /// <summary>
        /// Gets the validation manager instance handling field validation states.
        /// </summary>
        public ValidationManager ValidationManager { get; } = new ValidationManager();

        /// <summary>
        /// Gets or sets the currently selected destination project.
        /// </summary>
        public string SelectedProject { get; set; } = "Spacecraft Mission";

        /// <summary>
        /// Gets or sets the version constraint expression.
        /// </summary>
        public string VersionConstraint { get; set; } = "^1.2.0";

        /// <summary>
        /// Handles changes to the selected project value.
        /// </summary>
        /// <param name="value">The newly selected project.</param>
        public void OnProjectChanged(string value)
        {
            this.SelectedProject = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.SelectedProject));
        }

        /// <summary>
        /// Handles changes to the version constraint input value.
        /// </summary>
        /// <param name="value">The new version constraint expression.</param>
        public void OnVersionConstraintChanged(string value)
        {
            this.VersionConstraint = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.VersionConstraint));
        }

        /// <summary>
        /// Handles the cancel action, cancelling the dialog and invoking the cancel callback.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnCancelClicked()
        {
            await this.OnCancel.InvokeAsync();

            if (this.DialogReference != null)
            {
                await this.DialogReference.CancelAsync();
            }
        }

        /// <summary>
        /// Handles the add dependency action, validating and emitting the selected project and version constraint before closing.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnAddDependencyClicked()
        {
            var isValid = this.ValidationManager
                .Check(nameof(this.SelectedProject), !string.IsNullOrWhiteSpace(this.SelectedProject), "Project selection is required.")
                .Check(nameof(this.VersionConstraint), !string.IsNullOrWhiteSpace(this.VersionConstraint), "Version constraint is required.")
                .IsValid;

            if (!isValid)
            {
                return;
            }

            var result = new AddToProjectResult
            {
                ProjectName = this.SelectedProject,
                VersionConstraint = this.VersionConstraint
            };

            await this.OnResult.InvokeAsync(result);

            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok(result));
            }
        }

        /// <summary>
        /// Initializes default values based on passed parameters when component parameters are initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (this.Package != null && !string.IsNullOrWhiteSpace(this.Package.Version))
            {
                var cleanVersion = this.Package.Version.TrimStart('v', 'V');
                this.VersionConstraint = $"^{cleanVersion}";
            }

            this.SelectedProject = this.Projects[0];
        }
    }
}
