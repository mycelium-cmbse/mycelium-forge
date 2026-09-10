// ------------------------------------------------------------------------------------------------
// <copyright file="CollaboratorAvatar.razor.cs" company="Starion Group S.A.">
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
    /// Avatar component displaying user initials.
    /// </summary>
    public partial class CollaboratorAvatar : ComponentBase
    {
        /// <summary>
        /// The total number of collaborator color tokens available in the design system.
        /// </summary>
        private const int CollaboratorColorCount = 12;

        /// <summary>
        /// Gets or sets the collaborator full name or username.
        /// </summary>
        [Parameter]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the initials to display in the avatar circle.
        /// </summary>
        [Parameter]
        public string Initials { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CSS size classes for the avatar circle (default "w-7 h-7").
        /// </summary>
        [Parameter]
        public string Size { get; set; } = "w-7 h-7";

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the avatar container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Computes the initials string to display within the avatar.
        /// </summary>
        /// <returns>The resolved initials string.</returns>
        public string GetInitials()
        {
            if (!string.IsNullOrWhiteSpace(this.Initials))
            {
                return this.Initials;
            }

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                return "?";
            }

            var parts = this.Name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
            {
                return parts[0].Length >= 2
                    ? parts[0][..2].ToUpperInvariant()
                    : parts[0].ToUpperInvariant();
            }

            return $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant();
        }

        /// <summary>
        /// Computes the background color CSS class for the collaborator avatar.
        /// </summary>
        /// <returns>A Tailwind utility class mapped to one of the collaborator color tokens.</returns>
        public string GetColorClass()
        {
            var seed = !string.IsNullOrWhiteSpace(this.Name)
                ? this.Name
                : this.Initials;

            if (string.IsNullOrWhiteSpace(seed))
            {
                return "bg-primary";
            }

            var hash = Math.Abs(seed.GetHashCode());
            var index = hash % CollaboratorColorCount + 1;
            return $"bg-collaborator-c{index:D2}";
        }
    }
}
