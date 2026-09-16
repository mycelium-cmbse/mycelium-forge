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
        /// The set of static Tailwind background utility classes for collaborator colors.
        /// </summary>
        /// <remarks>
        /// Declared as an array of string literals so that Tailwind's static source scanner
        /// discovers every class name at build time and emits its corresponding CSS rule into <c>app.css</c>.
        /// Dynamically constructed class strings (such as via string interpolation) cannot be evaluated
        /// by the static scanner.
        /// </remarks>
        private static readonly string[] CollaboratorColorClasses =
        [
            "bg-collaborator-c01",
            "bg-collaborator-c02",
            "bg-collaborator-c03",
            "bg-collaborator-c04",
            "bg-collaborator-c05",
            "bg-collaborator-c06",
            "bg-collaborator-c07",
            "bg-collaborator-c08",
            "bg-collaborator-c09",
            "bg-collaborator-c10",
            "bg-collaborator-c11",
            "bg-collaborator-c12"
        ];

        /// <summary>
        /// Gets or sets the collaborator full name or username.
        /// </summary>
        [Parameter]
        public string Name { get; set; } = string.Empty;

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
        /// Computes the background color CSS class for the collaborator avatar.
        /// </summary>
        /// <returns>A Tailwind utility class mapped to one of the collaborator color tokens.</returns>
        public string GetColorClass()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                return "bg-primary";
            }

            var hash = (uint)this.Name.GetHashCode();
            var index = hash % (uint)CollaboratorColorClasses.Length;
            return CollaboratorColorClasses[index];
        }
    }
}
