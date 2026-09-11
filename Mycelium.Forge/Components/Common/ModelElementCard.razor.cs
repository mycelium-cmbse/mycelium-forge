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
            return this.GetCategorySlug() switch
            {
                "attributes" => "bg-sysml-attributes-surface border-sysml-attributes-border",
                "connections" => "bg-sysml-connections-surface border-sysml-connections-border",
                "behavior" => "bg-sysml-behavior-surface border-sysml-behavior-border",
                "requirements" => "bg-sysml-requirements-surface border-sysml-requirements-border",
                "verification" => "bg-sysml-verification-surface border-sysml-verification-border",
                "allocations" => "bg-sysml-allocations-surface border-sysml-allocations-border",
                "metadata" => "bg-sysml-metadata-surface border-sysml-metadata-border",
                "library" => "bg-sysml-library-surface border-sysml-library-border",
                "variation" => "bg-sysml-variation-surface border-sysml-variation-border",
                _ => "bg-sysml-structure-surface border-sysml-structure-border"
            };
        }

        /// <summary>
        /// Computes the CSS text styling for category headers.
        /// </summary>
        /// <returns>The Tailwind CSS utility class string.</returns>
        public string GetHeaderClass()
        {
            return this.GetCategorySlug() switch
            {
                "attributes" => "text-sysml-attributes-header",
                "connections" => "text-sysml-connections-header",
                "behavior" => "text-sysml-behavior-header",
                "requirements" => "text-sysml-requirements-header",
                "verification" => "text-sysml-verification-header",
                "allocations" => "text-sysml-allocations-header",
                "metadata" => "text-sysml-metadata-header",
                "library" => "text-sysml-library-header",
                "variation" => "text-sysml-variation-header",
                _ => "text-sysml-structure-header"
            };
        }

        /// <summary>
        /// Computes the CSS styling classes for the category badge.
        /// </summary>
        /// <returns>The Tailwind CSS utility class string.</returns>
        public string GetBadgeClass()
        {
            return this.GetCategorySlug() switch
            {
                "attributes" => "border bg-sysml-attributes-surface text-sysml-attributes-header border-sysml-attributes-border",
                "connections" => "border bg-sysml-connections-surface text-sysml-connections-header border-sysml-connections-border",
                "behavior" => "border bg-sysml-behavior-surface text-sysml-behavior-header border-sysml-behavior-border",
                "requirements" => "border bg-sysml-requirements-surface text-sysml-requirements-header border-sysml-requirements-border",
                "verification" => "border bg-sysml-verification-surface text-sysml-verification-header border-sysml-verification-border",
                "allocations" => "border bg-sysml-allocations-surface text-sysml-allocations-header border-sysml-allocations-border",
                "metadata" => "border bg-sysml-metadata-surface text-sysml-metadata-header border-sysml-metadata-border",
                "library" => "border bg-sysml-library-surface text-sysml-library-header border-sysml-library-border",
                "variation" => "border bg-sysml-variation-surface text-sysml-variation-header border-sysml-variation-border",
                _ => "border bg-sysml-structure-surface text-sysml-structure-header border-sysml-structure-border"
            };
        }
    }
}
