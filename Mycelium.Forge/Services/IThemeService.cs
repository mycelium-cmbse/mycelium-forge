// ------------------------------------------------------------------------------------------------
// <copyright file="IThemeService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    /// <summary>
    /// Defines the contract for managing application theme state (light vs dark mode).
    /// </summary>
    public interface IThemeService
    {
        /// <summary>
        /// Gets a value indicating whether dark mode is currently active.
        /// </summary>
        bool IsDarkMode { get; }

        /// <summary>
        /// Event triggered when the active theme mode changes.
        /// </summary>
        event Action OnChange;

        /// <summary>
        /// Toggles between light and dark mode and notifies subscribers.
        /// </summary>
        void ToggleDarkMode();

        /// <summary>
        /// Sets whether dark mode should be enabled.
        /// </summary>
        /// <param name="isDark">True to enable dark mode, false for light mode.</param>
        void SetDarkMode(bool isDark);
    }
}
