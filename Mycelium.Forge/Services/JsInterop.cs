// ------------------------------------------------------------------------------------------------
// <copyright file="JsInterop.cs" company="Starion Group S.A.">
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
    /// Provides JavaScript runtime interop operations for client browser capabilities.
    /// </summary>
    public class JsInterop : IJsInterop
    {
        /// <summary>
        /// The JavaScript runtime instance.
        /// </summary>
        private readonly IJSRuntime jsRuntime;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsInterop" /> class.
        /// </summary>
        /// <param name="jsRuntime">The underlying <see cref="IJSRuntime" /> instance.</param>
        public JsInterop(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Copies the specified text string to the system clipboard using browser APIs.
        /// </summary>
        /// <param name="text">The text content to copy to clipboard.</param>
        /// <returns>A <see cref="Task{Boolean}" /> indicating whether the clipboard write was successful.</returns>
        public async Task<bool> CopyToClipboard(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            try
            {
                return await this.jsRuntime.InvokeAsync<bool>("forgeInterop.copyToClipboard", text);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves the current dark mode preference from local storage or system preferences.
        /// </summary>
        /// <returns>A <see cref="Task{Boolean}" /> that is <c>true</c> if dark mode is active, <c>false</c> otherwise.</returns>
        public Task<bool> GetDarkMode()
        {
            return this.jsRuntime.InvokeAsync<bool>("forgeInterop.getDarkMode").AsTask();
        }

        /// <summary>
        /// Applies the dark mode setting to the DOM and persists it to local storage.
        /// </summary>
        /// <param name="isDark"><c>true</c> to enable dark mode; <c>false</c> for light mode.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public Task SetDarkMode(bool isDark)
        {
            return this.jsRuntime.InvokeVoidAsync("forgeInterop.setDarkMode", isDark).AsTask();
        }
    }
}
