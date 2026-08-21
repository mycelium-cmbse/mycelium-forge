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
    }
}
