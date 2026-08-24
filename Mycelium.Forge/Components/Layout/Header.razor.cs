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
    using Mycelium.Forge.Services;

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
        /// Gets or sets the application theme management service.
        /// </summary>
        [Inject]
        public IThemeService ThemeService { get; set; }

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
        /// Gets or sets the currently selected documentation language code.
        /// </summary>
        public string SelectedLanguage { get; set; } = "EN";

        /// <summary>
        /// Gets the list of selectable language code options.
        /// </summary>
        public IReadOnlyList<string> LanguageOptions { get; } = ["EN", "PT"];

        /// <summary>
        /// Gets the collection of supported documentation languages.
        /// </summary>
        public Dictionary<string, string> SupportedLanguages { get; } = new()
        {
            { "EN", "English" },
            { "PT", "Português" }
        };

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Toggles between light and dark visual themes.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task ToggleDarkMode()
        {
            await this.ThemeService.ToggleDarkMode();
        }

        /// <summary>
        /// Updates the selected documentation language.
        /// </summary>
        /// <param name="languageCode">The language code (e.g. EN or PT).</param>
        public void SelectLanguage(string languageCode)
        {
            this.SelectedLanguage = languageCode;
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
        /// Checks whether the current page is a documentation page.
        /// </summary>
        /// <returns>True if the current page is under the documentation route; otherwise, false.</returns>
        public bool IsDocumentationPage()
        {
            var currentPath = this.NavigationManager.ToBaseRelativePath(this.NavigationManager.Uri);
            var normalizedDocs = PageRoutes.Documentation.Overview.TrimStart('/');

            return currentPath.StartsWith(normalizedDocs, StringComparison.OrdinalIgnoreCase);
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
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        /// unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.NavigationManager != null)
                {
                    this.NavigationManager.LocationChanged -= this.OnLocationChanged;
                }

                if (this.ThemeService != null)
                {
                    this.ThemeService.OnChange -= this.StateHasChanged;
                }
            }
        }

        /// <summary>
        /// Initializes the component and subscribes to location and theme changes.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.NavigationManager.LocationChanged += this.OnLocationChanged;
            this.ThemeService.OnChange += this.StateHasChanged;
        }

        /// <summary>
        /// Synchronizes theme preferences from the client after the first render pass.
        /// </summary>
        /// <param name="firstRender">A value indicating whether this is the first render pass.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await this.ThemeService.InitializeThemeAsync();
            }
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
