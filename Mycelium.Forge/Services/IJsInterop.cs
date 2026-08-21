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
    }
}
