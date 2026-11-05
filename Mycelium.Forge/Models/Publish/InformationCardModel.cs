// ------------------------------------------------------------------------------------------------
// <copyright file="InformationCardModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Publish
{
    /// <summary>
    /// Represents the content and link configuration for an informative guidance card displayed beside wizard steps.
    /// </summary>
    public class InformationCardModel
    {
        /// <summary>
        /// Gets or sets the header title of the information card.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the list of bullet check items displayed in the card.
        /// </summary>
        public IReadOnlyList<string> Items { get; set; } = [];

        /// <summary>
        /// Gets or sets the explanatory help note displayed below the checklist.
        /// </summary>
        public string HelpText { get; set; }

        /// <summary>
        /// Gets or sets the display text for the documentation link.
        /// </summary>
        public string LinkText { get; set; }

        /// <summary>
        /// Gets or sets the destination URL for the documentation link.
        /// </summary>
        public string LinkHref { get; set; }
    }
}
