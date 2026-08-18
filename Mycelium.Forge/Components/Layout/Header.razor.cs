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
        /// Releases unmanaged and managed resources used by the component.
        /// </summary>
        public void Dispose()
        {
            this.NavigationManager.LocationChanged -= this.OnLocationChanged;
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
