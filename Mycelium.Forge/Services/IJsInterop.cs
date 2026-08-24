// ------------------------------------------------------------------------------------------------
// <copyright file="IJsInterop.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    /// <summary>
    /// Defines JavaScript runtime interop operations for client browser capabilities.
    /// </summary>
    public interface IJsInterop
    {
        /// <summary>
        /// Copies the specified text string to the system clipboard using browser APIs.
        /// </summary>
        /// <param name="text">The text content to copy to clipboard.</param>
        /// <returns>A <see cref="Task{Boolean}" /> indicating whether the clipboard write was successful.</returns>
        Task<bool> CopyToClipboard(string text);

        /// <summary>
        /// Retrieves the current dark mode preference from local storage or system preferences.
        /// </summary>
        /// <returns>A <see cref="Task{Boolean}" /> that is <c>true</c> if dark mode is active, <c>false</c> otherwise.</returns>
        Task<bool> GetDarkMode();

        /// <summary>
        /// Applies the dark mode setting to the DOM and persists it to local storage.
        /// </summary>
        /// <param name="isDark"><c>true</c> to enable dark mode; <c>false</c> for light mode.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task SetDarkMode(bool isDark);
    }
}
