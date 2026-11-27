// ------------------------------------------------------------------------------------------------
// <copyright file="LicenseBadges.razor.cs" company="Starion Group S.A.">
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
    /// Displays one or more interactive license badges that open <see cref="LicenseTextDialog" /> on click.
    /// </summary>
    public partial class LicenseBadges : ComponentBase
    {
        /// <summary>
        /// Gets or sets the dialog service used to display the full license text modal.
        /// </summary>
        [Inject]
        public DialogService DialogService { get; set; }

        /// <summary>
        /// Gets or sets the collection of applicable licenses to display.
        /// </summary>
        [Parameter]
        public IReadOnlyList<License> Licenses { get; set; } = [];

        /// <summary>
        /// Gets or sets the fallback text displayed when no licenses are present.
        /// </summary>
        [Parameter]
        public string EmptyText { get; set; } = "None";

        /// <summary>
        /// Gets or sets the CSS classes applied to the badge list container.
        /// </summary>
        [Parameter]
        public string ContainerClass { get; set; } = "flex flex-wrap items-center justify-end gap-1.5";

        /// <summary>
        /// Opens the <see cref="LicenseTextDialog" /> showing the full legal text for the selected <see cref="License" />.
        /// </summary>
        /// <param name="license">The <see cref="License" /> to display.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OpenLicenseTextDialog(License license)
        {
            if (license == null)
            {
                return;
            }

            var parameters = new Dictionary<string, object>
            {
                { nameof(LicenseTextDialog.License), license }
            };

            var options = new DialogOpenOptions
            {
                Title = license.Name,
                Description = license.SpdxIdentifier,
                Size = DialogSize.Large
            };

            await this.DialogService.OpenAsync<LicenseTextDialog>(parameters, options);
        }
    }
}
