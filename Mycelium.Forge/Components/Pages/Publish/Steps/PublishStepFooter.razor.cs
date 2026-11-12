// ------------------------------------------------------------------------------------------------
// <copyright file="PublishStepFooter.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Publish.Steps
{
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Represents the standardized bottom action footer for wizard steps in the package publishing workflow.
    /// </summary>
    public partial class PublishStepFooter : ComponentBase
    {
        /// <summary>
        /// Gets or sets the custom footnote render fragment.
        /// </summary>
        [Parameter]
        public RenderFragment Footnote { get; set; }

        /// <summary>
        /// Gets or sets the explanatory helper text displayed on the left side of the footer.
        /// </summary>
        [Parameter]
        public string FootnoteText { get; set; }

        /// <summary>
        /// Gets or sets the button label text for the back navigation action.
        /// </summary>
        [Parameter]
        public string BackText { get; set; } = "Back";

        /// <summary>
        /// Gets or sets the button label text for the forward navigation action.
        /// </summary>
        [Parameter]
        public string ContinueText { get; set; } = "Continue";

        /// <summary>
        /// Gets or sets the HTML ID attribute for the back button element.
        /// </summary>
        [Parameter]
        public string BackId { get; set; }

        /// <summary>
        /// Gets or sets the HTML ID attribute for the continue/action button element.
        /// </summary>
        [Parameter]
        public string ContinueId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the forward navigation action button is disabled.
        /// </summary>
        [Parameter]
        public bool IsContinueDisabled { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when the back button is clicked.
        /// </summary>
        [Parameter]
        public EventCallback OnBack { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when the continue button is clicked.
        /// </summary>
        [Parameter]
        public EventCallback OnContinue { get; set; }
    }
}
