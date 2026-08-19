// ------------------------------------------------------------------------------------------------
// <copyright file="Organization.razor.cs" company="Starion Group S.A.">
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
    public partial class Organization : ComponentBase
    {
        /// <summary>
        /// Gets or sets the view model for the organization profile page.
        /// </summary>
        [Inject]
        public IOrganizationViewModel ViewModel { get; set; }

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
        /// Initializes the component lifecycle and populates the view model state.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ViewModel.InitializeViewModel();
        }
    }
}
