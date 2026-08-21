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
    /// <summary>
    /// Service responsible for managing and broadcasting theme mode changes across the application.
    /// </summary>
    public class ThemeService : IThemeService
    {
        /// <summary>
        /// Gets a value indicating whether dark mode is currently active.
        /// </summary>
        public bool IsDarkMode { get; private set; }

        /// <summary>
        /// Event triggered when the active theme mode changes.
        /// </summary>
        public event Action OnChange;

        /// <summary>
        /// Toggles between light and dark mode and notifies subscribers.
        /// </summary>
        public void ToggleDarkMode()
        {
            this.IsDarkMode = !this.IsDarkMode;
            this.OnChange?.Invoke();
        }

        /// <summary>
        /// Sets whether dark mode should be enabled.
        /// </summary>
        /// <param name="isDark">True to enable dark mode, false for light mode.</param>
        public void SetDarkMode(bool isDark)
        {
            if (this.IsDarkMode != isDark)
            {
                this.IsDarkMode = isDark;
                this.OnChange?.Invoke();
            }
        }
    }
}
