// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationPageMeta.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Documentation
{
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Code-behind logic for the document footer metadata and feedback rating component.
    /// </summary>
    public partial class DocumentationPageMeta : ComponentBase
    {
        /// <summary>
        /// Gets or sets the formatted date string indicating when the document was last updated.
        /// </summary>
        [Parameter]
        public string LastUpdated { get; set; } = "29 July 2026";

        /// <summary>
        /// Gets or sets a value indicating whether feedback has already been submitted.
        /// </summary>
        [Parameter]
        public bool FeedbackGiven { get; set; }

        /// <summary>
        /// Gets or sets an event callback triggered when feedback is submitted.
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnFeedback { get; set; }

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Handles user feedback button clicks and raises the event callback.
        /// </summary>
        /// <param name="isHelpful">A value indicating whether the page content was helpful.</param>
        private async Task SubmitFeedback(bool isHelpful)
        {
            this.FeedbackGiven = true;
            await this.OnFeedback.InvokeAsync(isHelpful);
        }
    }
}
