// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeCodeBlock.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Code-behind logic for the syntax-styled code snippet block component.
    /// </summary>
    public partial class ForgeCodeBlock : ComponentBase
    {
        /// <summary>
        /// Gets or sets the file name or descriptor label displayed in the top bar.
        /// </summary>
        [Parameter]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source code snippet text.
        /// </summary>
        [Parameter]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;
    }
}
