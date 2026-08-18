// ------------------------------------------------------------------------------------------------
// <copyright file="FacetItemModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a single facet option with a display label, result count, and selection state.
    /// </summary>
    public class FacetItemModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacetItemModel"/> class.
        /// </summary>
        /// <param name="label">The display label of the facet option.</param>
        /// <param name="count">The count of matching packages for this facet option.</param>
        /// <param name="isChecked">A value indicating whether the facet option is currently selected.</param>
        public FacetItemModel(string label, int count, bool isChecked = false)
        {
            this.Label = label;
            this.Count = count;
            this.IsChecked = isChecked;
        }

        /// <summary>
        /// Gets or sets the display label of the facet option.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the count of matching packages for this facet option.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the facet option is currently selected.
        /// </summary>
        public bool IsChecked { get; set; }
    }
}
