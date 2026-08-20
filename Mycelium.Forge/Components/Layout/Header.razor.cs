// ------------------------------------------------------------------------------------------------
// <copyright file="Header.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Layout
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Routing;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents the top application header navigation bar for Mycelium Forge.
    /// </summary>
    public partial class Header : ComponentBase, IDisposable
    {
        /// <summary>
        /// Gets or sets the navigation manager instance.
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is logged in.
        /// </summary>
        public bool IsLoggedIn { get; set; } = true;

        /// <summary>
        /// Gets or sets the initials displayed in the user avatar when logged in.
        /// </summary>
        public string UserInitials { get; set; } = "RA";

        /// <summary>
        /// Gets or sets the bound search query value in the header search input.
        /// </summary>
        public string SearchQuery { get; set; } = string.Empty;

        /// <summary>
        /// Releases unmanaged and managed resources used by the component.
        /// </summary>
        public void Dispose()
        {
            this.NavigationManager.LocationChanged -= this.OnLocationChanged;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Computes the CSS class for a navigation link based on whether its destination matches the current URI.
        /// </summary>
        /// <param name="href">The destination relative path.</param>
        /// <returns>The CSS class string for the navigation link.</returns>
        public string GetNavLinkClass(string href)
        {
            var isActive = this.IsRouteActive(href);

            if (isActive)
            {
                return "text-sm leading-xs font-semibold text-primary";
            }

            return "text-sm leading-xs font-medium text-muted-foreground hover:text-foreground transition-colors";
        }

        /// <summary>
        /// Checks whether the specified relative path is active given the current URI.
        /// </summary>
        /// <param name="href">The relative route to evaluate.</param>
        /// <returns>True if the route is active; otherwise, false.</returns>
        public bool IsRouteActive(string href)
        {
            if (string.IsNullOrEmpty(href))
            {
                return false;
            }

            var currentPath = this.NavigationManager.ToBaseRelativePath(this.NavigationManager.Uri);
            var normalizedHref = href.TrimStart('/');

            if (string.IsNullOrEmpty(normalizedHref))
            {
                return string.IsNullOrEmpty(currentPath);
            }

            return currentPath.StartsWith(normalizedHref, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks whether the current page is the root landing page.
        /// </summary>
        /// <returns>True if the current page is the root page; otherwise, false.</returns>
        public bool IsHomePage()
        {
            var currentPath = this.NavigationManager.ToBaseRelativePath(this.NavigationManager.Uri);
            var cleanPath = currentPath.Split('?')[0].Trim('/');

            return string.IsNullOrEmpty(cleanPath) || cleanPath.Equals(PageRoutes.Home.TrimStart('/'), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Handles changes to the header search input value.
        /// </summary>
        /// <param name="value">The updated search query value.</param>
        public void OnSearchInputChanged(string value)
        {
            this.SearchQuery = value;
        }

        /// <summary>
        /// Initializes the component and subscribes to location changes.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.NavigationManager.LocationChanged += this.OnLocationChanged;
        }

        /// <summary>
        /// Handles location change events from the navigation manager.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The location change event arguments.</param>
        private void OnLocationChanged(object sender, LocationChangedEventArgs eventArgs)
        {
            this.InvokeAsync(this.StateHasChanged);
        }
    }
}
