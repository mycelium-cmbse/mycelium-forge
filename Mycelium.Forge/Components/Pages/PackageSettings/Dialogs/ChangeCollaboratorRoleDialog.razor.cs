// ------------------------------------------------------------------------------------------------
// <copyright file="ChangeCollaboratorRoleDialog.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageSettings.Dialogs
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents a modal dialog for changing a collaborator's role within a package.
    /// </summary>
    public partial class ChangeCollaboratorRoleDialog : ComponentBase
    {
        /// <summary>
        /// Gets or sets the cascading dialog reference used to control and close the dialog.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the collaborator account whose role is being updated.
        /// </summary>
        [Parameter]
        public IAccount Collaborator { get; set; }

        /// <summary>
        /// Gets or sets the collaborator's current role within the package.
        /// </summary>
        [Parameter]
        public PackageInvitationKind CurrentRole { get; set; } = PackageInvitationKind.MAINTAINER;

        /// <summary>
        /// Gets or sets the event callback invoked with the newly selected role when saved.
        /// </summary>
        [Parameter]
        public EventCallback<PackageInvitationKind> OnResult { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the dialog is cancelled.
        /// </summary>
        [Parameter]
        public EventCallback OnCancel { get; set; }

        /// <summary>
        /// Gets or sets the selected package role.
        /// </summary>
        public PackageInvitationKind SelectedRole { get; set; } = PackageInvitationKind.MAINTAINER;

        /// <summary>
        /// Gets the available package role options.
        /// </summary>
        public IReadOnlyList<PackageInvitationKind> RoleOptions { get; } = Enum.GetValues<PackageInvitationKind>();

        /// <summary>
        /// Handles changes to the selected role.
        /// </summary>
        /// <param name="value">The newly selected package role.</param>
        public void OnRoleChanged(PackageInvitationKind value)
        {
            this.SelectedRole = value;
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
        /// Handles the save action, invoking the result callback with the selected role and closing the dialog.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnSaveClicked()
        {
            await this.OnResult.InvokeAsync(this.SelectedRole);

            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }
        }

        /// <summary>
        /// Initializes the selected role from the collaborator's current role.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.SelectedRole = this.CurrentRole;
        }
    }
}
