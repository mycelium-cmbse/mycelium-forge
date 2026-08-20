// ------------------------------------------------------------------------------------------------
// <copyright file="OptionModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a selectable option or facet filter item with a display label, property group, result count, and selection
    /// state.
    /// </summary>
    public class OptionModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionModel" /> class.
        /// </summary>
        /// <param name="property">The property group name that this option belongs to.</param>
        /// <param name="label">The display label of the option.</param>
        /// <param name="count">The count of matching items for this option.</param>
        /// <param name="isChecked">A value indicating whether the option is currently selected.</param>
        /// <param name="key">The key identifier of the option.</param>
        public OptionModel(string property, string label, int count, bool isChecked = false, string key = "")
        {
            this.Property = property;
            this.Label = label;
            this.Count = count;
            this.IsChecked = isChecked;
            this.Key = string.IsNullOrEmpty(key) ? label : key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionModel" /> class.
        /// </summary>
        /// <param name="key">The key identifier of the option.</param>
        /// <param name="label">The display label of the option.</param>
        public OptionModel(string key, string label)
        {
            this.Property = string.Empty;
            this.Label = label;
            this.Count = 0;
            this.IsChecked = false;
            this.Key = key;
        }

        /// <summary>
        /// Gets or sets the property group name that this option belongs to.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the display label of the option.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the key identifier of the option.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the count of matching items for this option.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the option is currently selected.
        /// </summary>
        public bool IsChecked { get; set; }
    }
}
