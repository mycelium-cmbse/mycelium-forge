// ------------------------------------------------------------------------------------------------
// <copyright file="LicenseTextDialog.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents a modal dialog displaying the full legal text of a supported <see cref="ILicense" />.
    /// </summary>
    public partial class LicenseTextDialog : ComponentBase
    {
        /// <summary>
        /// Gets or sets the cascading dialog reference used to close the dialog.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ILicense" /> whose details and full text are displayed.
        /// </summary>
        [Parameter]
        public ILicense License { get; set; }

        /// <summary>
        /// Closes the license text dialog.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnCloseClicked()
        {
            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }
        }
    }
}
