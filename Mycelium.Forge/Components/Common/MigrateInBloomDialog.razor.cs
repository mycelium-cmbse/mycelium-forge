// ------------------------------------------------------------------------------------------------
// <copyright file="MigrateInBloomDialog.razor.cs" company="Starion Group S.A.">
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

    using Mycelium.Forge.Models;
    using Mycelium.Forge.Models.DialogResults;

    /// <summary>
    /// Represents a dialog component for migrating a package in Bloom to a selected project with a version constraint.
    /// </summary>
    public partial class MigrateInBloomDialog : ComponentBase
    {
        /// <summary>
        /// Gets or sets the cascading dialog reference used to control and close the dialog.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the package model being migrated.
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
        /// Gets or sets the event callback invoked when the user confirms migrating in Bloom.
        /// </summary>
        [Parameter]
        public EventCallback<MigrateInBloomResult> OnResult { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the dialog is cancelled or closed.
        /// </summary>
        [Parameter]
        public EventCallback OnCancel { get; set; }

        /// <summary>
        /// Gets or sets the currently selected destination project.
        /// </summary>
        public string SelectedProject { get; set; } = "Spacecraft Mission";

        /// <summary>
        /// Gets or sets the version constraint expression.
        /// </summary>
        public string VersionConstraint { get; set; } = "^1.2.0";

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
        /// Handles the open in Bloom action, emitting the selected project and version constraint before closing.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnOpenInBloomClicked()
        {
            var result = new MigrateInBloomResult
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
