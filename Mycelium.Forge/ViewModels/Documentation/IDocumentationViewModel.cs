// ------------------------------------------------------------------------------------------------
// <copyright file="IDocumentationViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Documentation
{
    using Mycelium.Forge.Models.Documentation;

    /// <summary>
    /// Defines the view model contract for the Mycelium Forge documentation pages.
    /// </summary>
    public interface IDocumentationViewModel
    {
        /// <summary>
        /// Gets or sets the collection of documentation navigation category groups displayed in the sidebar.
        /// </summary>
        List<DocumentationNavGroupModel> NavGroups { get; set; }

        /// <summary>
        /// Gets or sets the collection of key section feature cards.
        /// </summary>
        List<DocumentationSectionCardModel> KeySections { get; set; }

        /// <summary>
        /// Gets or sets the collection of on-page table of contents navigation items.
        /// </summary>
        List<DocumentationTocItemModel> TableOfContents { get; set; }

        /// <summary>
        /// Gets or sets the formatted last updated date string for the document.
        /// </summary>
        string LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user feedback has been submitted.
        /// </summary>
        bool FeedbackGiven { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user feedback was positive.
        /// </summary>
        bool? IsHelpful { get; set; }

        /// <summary>
        /// Initializes the view model state for the overview documentation page.
        /// </summary>
        void InitializeOverview();

        /// <summary>
        /// Initializes the view model state for the packages and kpar format documentation page.
        /// </summary>
        void InitializePackagesAndKpar();

        /// <summary>
        /// Records user feedback indicating whether the documentation page was helpful.
        /// </summary>
        /// <param name="isHelpful">A value indicating whether the documentation page was helpful.</param>
        void RecordFeedback(bool isHelpful);
    }
}
