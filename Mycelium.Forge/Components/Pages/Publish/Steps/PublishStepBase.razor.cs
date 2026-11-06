// ------------------------------------------------------------------------------------------------
// <copyright file="PublishStepBase.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Publish.Steps
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models.Publish;

    /// <summary>
    /// Represents the two-column card layout base component for package publishing wizard steps.
    /// </summary>
    public partial class PublishStepBase : ComponentBase
    {
        /// <summary>
        /// Gets or sets the render fragment for the main form body card.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public RenderFragment Body { get; set; }

        /// <summary>
        /// Gets or sets the information card model rendered in the secondary column.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public InformationCardModel InformationCard { get; set; }

        /// <summary>
        /// Gets or sets the optional render fragment for the bottom footer section.
        /// </summary>
        [Parameter]
        public RenderFragment Footer { get; set; }
    }
}
