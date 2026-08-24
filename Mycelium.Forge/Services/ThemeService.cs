// ------------------------------------------------------------------------------------------------
// <copyright file="ThemeService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using Microsoft.JSInterop;

    /// <summary>
    /// Service responsible for managing and broadcasting theme mode changes across the application.
    /// </summary>
    public class ThemeService : IThemeService
    {
        /// <summary>
        /// The JavaScript interop service used for client-side DOM theme updates.
        /// </summary>
        private readonly IJsInterop jsInterop;

        /// <summary>
        /// The logger instance for logging diagnostics and exceptions.
        /// </summary>
        private readonly ILogger<ThemeService> logger;

        /// <summary>
        /// Gets or sets a value indicating whether theme initialization from client storage has been performed.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeService" /> class.
        /// </summary>
        /// <param name="jsInterop">The JavaScript interop service.</param>
        /// <param name="logger">The logger instance.</param>
        public ThemeService(IJsInterop jsInterop, ILogger<ThemeService> logger)
        {
            this.jsInterop = jsInterop;
            this.logger = logger;
        }

        /// <summary>
        /// Gets a value indicating whether dark mode is currently active.
        /// </summary>
        public bool IsDarkMode { get; private set; }

        /// <summary>
        /// Event triggered when the active theme mode changes.
        /// </summary>
        public event Action OnChange;

        /// <summary>
        /// Initializes the theme mode from client storage or system preferences.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task InitializeThemeAsync()
        {
            if (this.isInitialized)
            {
                return;
            }

            this.isInitialized = true;

            try
            {
                var isDark = await this.jsInterop.GetDarkMode();

                if (this.IsDarkMode != isDark)
                {
                    this.IsDarkMode = isDark;
                    this.OnChange?.Invoke();
                }
            }
            catch (JSException ex)
            {
                this.logger.LogError(ex, "Failed to retrieve initial dark mode setting from JavaScript interop.");
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, "Failed to initialize dark mode because the circuit is not available.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unexpected error occurred during dark mode initialization.");
            }
        }

        /// <summary>
        /// Toggles between light and dark mode and notifies subscribers.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task ToggleDarkMode()
        {
            this.IsDarkMode = !this.IsDarkMode;
            this.OnChange?.Invoke();
            await this.UpdateDomTheme();
        }

        /// <summary>
        /// Sets whether dark mode should be enabled.
        /// </summary>
        /// <param name="isDark">True to enable dark mode, false for light mode.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task SetDarkMode(bool isDark)
        {
            if (this.IsDarkMode != isDark)
            {
                this.IsDarkMode = isDark;
                this.OnChange?.Invoke();
                await this.UpdateDomTheme();
            }
        }

        /// <summary>
        /// Updates the dark theme class on the document root element via JavaScript interop.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task UpdateDomTheme()
        {
            try
            {
                await this.jsInterop.SetDarkMode(this.IsDarkMode);
            }
            catch (JSException ex)
            {
                this.logger.LogError(ex, "Failed to update DOM dark mode via JavaScript interop.");
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, "Failed to update DOM dark mode because the circuit is not available.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unexpected error occurred while updating DOM dark mode.");
            }
        }
    }
}
