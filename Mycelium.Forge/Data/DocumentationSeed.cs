// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationSeed.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Data
{
    using System.Diagnostics.CodeAnalysis;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models.Documentation;

    /// <summary>
    /// Provides centralized seed data for the documentation navigation and content structure.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DocumentationSeed
    {
        /// <summary>
        /// Gets the collection of key section feature cards for the documentation overview page.
        /// </summary>
        public static IReadOnlyList<DocumentationSectionCardModel> OverviewKeySections { get; } =
        [
            new()
            {
                Title = "Getting started",
                Description = "Install the CLI and import your first package.",
                Href = PageRoutes.Documentation.Overview,
                IconName = "book-open"
            },
            new()
            {
                Title = "Publish a package",
                Description = "Prepare metadata, publish from Bloom or the CLI, pass Release Validation.",
                Href = PageRoutes.Publish,
                IconName = "upload"
            },
            new()
            {
                Title = "Consuming packages",
                Description = "Discover models, browse standard libraries, and import into your projects.",
                Href = PageRoutes.Packages,
                IconName = "download"
            },
            new()
            {
                Title = "Core concepts",
                Description = "kpar packaging, scopes, versioning, immutability and validation.",
                Href = PageRoutes.Documentation.PackagesAndKparFormat,
                IconName = "layers"
            },
            new()
            {
                Title = "CLI reference",
                Description = "Command reference, flags, authentication and CI/CD automation.",
                Href = PageRoutes.Documentation.Overview,
                IconName = "terminal"
            },
            new()
            {
                Title = "HTTP API",
                Description = "REST endpoints, OpenAPI specification, and programmatic access.",
                Href = PageRoutes.Documentation.Overview,
                IconName = "globe"
            }
        ];

        /// <summary>
        /// Gets the table of contents items for the documentation overview page.
        /// </summary>
        public static IReadOnlyList<DocumentationTocItemModel> OverviewTableOfContents { get; } =
        [
            CreateTocItem("What is Mycelium Forge?", "what-is-mycelium-forge", PageRoutes.Documentation.Overview, true),
            CreateTocItem("Key sections", "key-sections", PageRoutes.Documentation.Overview)
        ];

        /// <summary>
        /// Gets the table of contents items for the packages and kpar format documentation page.
        /// </summary>
        public static IReadOnlyList<DocumentationTocItemModel> PackagesAndKparTableOfContents { get; } =
        [
            CreateTocItem("What is a package?", "what-is-a-package", PageRoutes.Documentation.PackagesAndKparFormat, true),
            CreateTocItem("The kpar archive", "the-kpar-archive", PageRoutes.Documentation.PackagesAndKparFormat),
            CreateTocItem("Package identity", "package-identity", PageRoutes.Documentation.PackagesAndKparFormat)
        ];

        /// <summary>
        /// Builds the full sidebar navigation groups with the specified item marked as active.
        /// </summary>
        /// <param name="activeItemTitle">The title of the active navigation item.</param>
        /// <returns>The list of navigation group models.</returns>
        public static List<DocumentationNavGroupModel> BuildNavGroups(string activeItemTitle)
        {
            return
            [
                new DocumentationNavGroupModel
                {
                    Title = "OVERVIEW",
                    Items =
                    [
                        CreateNavItem("What is Forge", PageRoutes.Documentation.Overview, activeItemTitle),
                        CreateNavItem("Platform overview", "#", activeItemTitle),
                        CreateNavItem("Quickstart", "#", activeItemTitle)
                    ]
                },
                new DocumentationNavGroupModel
                {
                    Title = "CORE CONCEPTS",
                    Items =
                    [
                        CreateNavItem("Packages & the kpar format", PageRoutes.Documentation.PackagesAndKparFormat, activeItemTitle),
                        CreateNavItem("Supported formats", "#", activeItemTitle),
                        CreateNavItem("Scopes & identifiers", "#", activeItemTitle),
                        CreateNavItem("Versioning & SemVer", "#", activeItemTitle),
                        CreateNavItem("Immutability & yanking", "#", activeItemTitle),
                        CreateNavItem("Release validation", "#", activeItemTitle)
                    ]
                },
                CreatePlaceholderGroup("CONSUMING", activeItemTitle, "Discover & search", "Import into a project", "The Library panel", "Standard libraries"),
                CreatePlaceholderGroup("PUBLISHING", activeItemTitle, "Prepare a package", "Publish from Bloom", "Publish with the CLI", "Manage versions"),
                CreatePlaceholderGroup("SECURITY", activeItemTitle, "API keys", "Trusted publishing", "Provenance"),
                CreatePlaceholderGroup("CLI REFERENCE", activeItemTitle, "Install & authenticate", "Command reference"),
                CreatePlaceholderGroup("HTTP API", activeItemTitle, "Overview", "Endpoints"),
                CreatePlaceholderGroup("GOVERNANCE", activeItemTitle, "Ownership & maintainers", "Verified scopes", "Roles & permissions"),
                CreatePlaceholderGroup("OPERATIONS", activeItemTitle, "Self-hosting", "Mirroring & proxying"),
                CreatePlaceholderGroup("REFERENCE & POLICIES", activeItemTitle, "Terms & policies", "Security"),
                CreatePlaceholderGroup("FAQ", activeItemTitle, "Troubleshooting")
            ];
        }

        /// <summary>
        /// Creates a table of contents item model.
        /// </summary>
        /// <param name="title">The title of the section.</param>
        /// <param name="targetId">The target element identifier.</param>
        /// <param name="baseHref">The base page route.</param>
        /// <param name="isActive">A value indicating whether this item is initially active.</param>
        /// <returns>A new <see cref="DocumentationTocItemModel" />.</returns>
        private static DocumentationTocItemModel CreateTocItem(string title, string targetId, string baseHref, bool isActive = false)
        {
            return new DocumentationTocItemModel
            {
                Title = title,
                TargetId = targetId,
                Href = $"{baseHref}#{targetId}",
                IsActive = isActive
            };
        }

        /// <summary>
        /// Creates a navigation item model with the specified title, href, and active state.
        /// </summary>
        /// <param name="title">The item title.</param>
        /// <param name="href">The destination URL.</param>
        /// <param name="activeItemTitle">The currently active item title.</param>
        /// <returns>A new <see cref="DocumentationNavItemModel" />.</returns>
        private static DocumentationNavItemModel CreateNavItem(string title, string href, string activeItemTitle)
        {
            return new DocumentationNavItemModel
            {
                Title = title,
                Href = href,
                IsActive = title == activeItemTitle
            };
        }

        /// <summary>
        /// Creates a navigation group model with placeholder hash links for the given titles.
        /// </summary>
        /// <param name="groupTitle">The title of the group.</param>
        /// <param name="activeItemTitle">The title of the active item.</param>
        /// <param name="itemTitles">The titles of the navigation items in the group.</param>
        /// <returns>A new <see cref="DocumentationNavGroupModel" />.</returns>
        private static DocumentationNavGroupModel CreatePlaceholderGroup(string groupTitle, string activeItemTitle, params string[] itemTitles)
        {
            var items = new List<DocumentationNavItemModel>(itemTitles.Length);

            foreach (var itemTitle in itemTitles)
            {
                items.Add(CreateNavItem(itemTitle, "#", activeItemTitle));
            }

            return new DocumentationNavGroupModel
            {
                Title = groupTitle,
                Items = items
            };
        }
    }
}
