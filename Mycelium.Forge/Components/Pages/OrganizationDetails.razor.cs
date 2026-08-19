// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationDetails.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.ViewModels;

    /// <summary>
    /// Represents the organization publisher profile and packages view of the Mycelium Forge registry.
    /// </summary>
    public partial class OrganizationDetails : ComponentBase
    {
        /// <summary>
        /// Gets or sets the organization identifier supplied from the URL route.
        /// </summary>
        [Parameter]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the view model for the organization profile page.
        /// </summary>
        [Inject]
        public IOrganizationDetailsViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets the formatted metadata summary line for the organization.
        /// </summary>
        /// <returns>A formatted string with verified status, package, version, import counts, and member year.</returns>
        public string GetOrganizationMetaText()
        {
            var verifiedPrefix = this.ViewModel.Organization.IsVerified ? "Verified publisher · " : string.Empty;
            return $"{verifiedPrefix}{this.ViewModel.Organization.PackageCount} packages · {this.ViewModel.Organization.VersionCount} versions · {this.ViewModel.Organization.ImportCount} imports · member since {this.ViewModel.Organization.MemberSinceYear}";
        }

        /// <summary>
        /// Handles component parameter updates and initializes the view model with the parsed organization identifier.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            var idParsed = Guid.TryParse(this.Id, out var parsedGuid)
                ? parsedGuid
                : Guid.Empty;

            this.ViewModel.InitializeViewModel(idParsed);
        }
    }
}
