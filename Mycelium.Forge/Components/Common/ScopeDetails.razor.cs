// ------------------------------------------------------------------------------------------------
// <copyright file="ScopeDetails.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.ViewModels.Rows;

    /// <summary>
    /// Shared publisher profile and owned packages view for accounts and organizations.
    /// </summary>
    public partial class ScopeDetails : ComponentBase
    {
        /// <summary>
        /// Gets or sets the owning publisher scope (<see cref="IAccount" /> or <see cref="IOrganization" />).
        /// </summary>
        [Parameter]
        [EditorRequired]
        public IScope Scope { get; set; }

        /// <summary>
        /// Gets or sets the collection of package rows published by the scope.
        /// </summary>
        [Parameter]
        public IReadOnlyList<PackageRowViewModel> Packages { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether the settings navigation button should be rendered.
        /// </summary>
        [Parameter]
        public bool ShowSettingsButton { get; set; }

        /// <summary>
        /// Gets a value indicating whether a subtitle line is available for the scope header.
        /// </summary>
        private bool HasSubtitle => !string.IsNullOrWhiteSpace(this.GetSubtitle());

        /// <summary>
        /// Resolves the settings page URL for the current scope.
        /// </summary>
        /// <returns>The formatted settings route path.</returns>
        private string GetSettingsHref()
        {
            return this.Scope is IAccount
                ? PageRoutes.AccountSettings
                : PageRoutes.GetOrganizationSettingsRoute(this.Scope.ShortName);
        }

        /// <summary>
        /// Resolves the subtitle text to display under the scope title from the scope's origin or website.
        /// </summary>
        /// <returns>The subtitle string, or an empty string if none is available.</returns>
        private string GetSubtitle()
        {
            if (!string.IsNullOrWhiteSpace(this.Scope.Origin) && !string.Equals(this.Scope.Origin, "local", StringComparison.OrdinalIgnoreCase))
            {
                return this.Scope.Origin;
            }

            return this.Scope.Website ?? string.Empty;
        }

        /// <summary>
        /// Formats the package count and membership year summary line for the scope header.
        /// </summary>
        /// <returns>A formatted summary string.</returns>
        private string GetPackageSummaryText()
        {
            return $"{"package".ToQuantity(this.Packages.Count)} · member since {this.Scope.CreatedAt.Year}";
        }
    }
}
