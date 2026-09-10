// ------------------------------------------------------------------------------------------------
// <copyright file="ModelElementCard.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Component representing a model element color-coded with SysML category surface, header, and border design tokens.
    /// </summary>
    public partial class ModelElementCard : ComponentBase
    {
        /// <summary>
        /// Gets or sets the model element data displayed by this card.
        /// </summary>
        [Parameter]
        public PackageElementModel Element { get; set; }

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the card container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Resolves the category slug corresponding to the element.
        /// </summary>
        /// <remarks>Mock method while the actual data is being developed.</remarks>
        /// <returns>The lowercase category slug string.</returns>
        public string GetCategorySlug()
        {
            var raw = !string.IsNullOrWhiteSpace(this.Element?.Category)
                ? this.Element.Category
                : this.Element?.Kind ?? "structure";

            return raw.Trim().ToLowerInvariant() switch
            {
                "parts" or "part" or "structure" or "structures" or "item" or "items" => "structure",
                "attributes" or "attribute" or "type" or "types" => "attributes",
                "ports" or "port" or "connection" or "connections" or "flow" or "interface" => "connections",
                "actions" or "action" or "behavior" or "behaviors" or "state" or "calc" => "behavior",
                "requirements" or "requirement" or "stakeholder" or "concern" => "requirements",
                "verification" or "test" or "tests" or "check" or "checks" => "verification",
                "allocations" or "allocation" => "allocations",
                "metadata" or "meta" or "docs" or "doc" => "metadata",
                "units" or "unit" or "scales" or "scale" or "library" or "libraries" => "library",
                "templates" or "template" or "variation" or "variations" or "choice" => "variation",
                _ => "structure"
            };
        }

        /// <summary>
        /// Computes the display name of the element's category.
        /// </summary>
        /// <returns>The category display name string.</returns>
        public string GetCategoryDisplayName()
        {
            if (!string.IsNullOrWhiteSpace(this.Element?.Category))
            {
                return this.Element.Category;
            }

            if (!string.IsNullOrWhiteSpace(this.Element?.Kind))
            {
                return this.Element.Kind;
            }

            return "Structure";
        }

        /// <summary>
        /// Computes the CSS container styling with category surface and border tokens.
        /// </summary>
        /// <returns>The Tailwind CSS utility class string.</returns>
        public string GetContainerClass()
        {
            var slug = this.GetCategorySlug();
            return $"bg-sysml-{slug}-surface border-sysml-{slug}-border";
        }

        /// <summary>
        /// Computes the CSS text styling for category headers.
        /// </summary>
        /// <returns>The Tailwind CSS utility class string.</returns>
        public string GetHeaderClass()
        {
            var slug = this.GetCategorySlug();
            return $"text-sysml-{slug}-header";
        }

        /// <summary>
        /// Computes the CSS styling classes for the category badge.
        /// </summary>
        /// <returns>The Tailwind CSS utility class string.</returns>
        public string GetBadgeClass()
        {
            var slug = this.GetCategorySlug();
            return $"border bg-sysml-{slug}-surface text-sysml-{slug}-header border-sysml-{slug}-border";
        }
    }
}
