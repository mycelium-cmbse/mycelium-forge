// ------------------------------------------------------------------------------------------------
// <copyright file="SignInRequired.razor.cs" company="Starion Group S.A.">
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

    /// <summary>
    /// Represents a common placeholder view prompting the user to sign in to access protected content.
    /// </summary>
    public partial class SignInRequired : ComponentBase
    {
        /// <summary>
        /// Gets or sets the navigation manager instance.
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the return URL to redirect back to after successful authentication.
        /// </summary>
        [Parameter]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Computes the target sign-in URL with the return URL query parameter appended if available.
        /// </summary>
        /// <returns>The computed login URL string.</returns>
        public string GetSignInUrl()
        {
            return UrlHelper.GetSignInUrl(this.ReturnUrl, this.NavigationManager);
        }
    }
}
