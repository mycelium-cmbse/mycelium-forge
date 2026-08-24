// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Documentation
{
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Models.Documentation;

    /// <summary>
    /// Provides view model state and operations for the Mycelium Forge documentation pages.
    /// </summary>
    public class DocumentationViewModel : IDocumentationViewModel
    {
        /// <summary>
        /// Gets or sets the collection of documentation navigation category groups displayed in the sidebar.
        /// </summary>
        public List<DocumentationNavGroupModel> NavGroups { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of key section feature cards.
        /// </summary>
        public List<DocumentationSectionCardModel> KeySections { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of on-page table of contents navigation items.
        /// </summary>
        public List<DocumentationTocItemModel> TableOfContents { get; set; } = [];

        /// <summary>
        /// Gets or sets the formatted last updated date string for the document.
        /// </summary>
        public string LastUpdated { get; set; } = "29 July 2026";

        /// <summary>
        /// Gets or sets a value indicating whether user feedback has been submitted.
        /// </summary>
        public bool FeedbackGiven { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user feedback was positive.
        /// </summary>
        public bool? IsHelpful { get; set; }

        /// <summary>
        /// Initializes the view model state for the overview documentation page.
        /// </summary>
        public void InitializeOverview()
        {
            this.InitializePage(
                "What is Forge",
                [..DocumentationSeed.OverviewKeySections],
                [..DocumentationSeed.OverviewTableOfContents]);
        }

        /// <summary>
        /// Initializes the view model state for the packages and kpar format documentation page.
        /// </summary>
        public void InitializePackagesAndKpar()
        {
            this.InitializePage(
                "Packages & the kpar format",
                [],
                [..DocumentationSeed.PackagesAndKparTableOfContents]);
        }

        /// <summary>
        /// Initializes the page state with the provided navigation and content structure.
        /// </summary>
        /// <param name="activeItemTitle">The title of the active navigation item.</param>
        /// <param name="keySections">The collection of key section feature cards.</param>
        /// <param name="tableOfContents">The collection of table of contents navigation items.</param>
        public void InitializePage(
            string activeItemTitle,
            List<DocumentationSectionCardModel> keySections,
            List<DocumentationTocItemModel> tableOfContents)
        {
            this.NavGroups = DocumentationSeed.BuildNavGroups(activeItemTitle);
            this.KeySections = keySections;
            this.TableOfContents = tableOfContents;
            this.LastUpdated = "29 July 2026";
            this.FeedbackGiven = false;
            this.IsHelpful = null;
        }

        /// <summary>
        /// Records user feedback indicating whether the documentation page was helpful.
        /// </summary>
        /// <param name="isHelpful">A value indicating whether the documentation page was helpful.</param>
        public void RecordFeedback(bool isHelpful)
        {
            this.FeedbackGiven = true;
            this.IsHelpful = isHelpful;
        }
    }
}
