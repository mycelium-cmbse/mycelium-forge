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
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models.Documentation;

    /// <summary>
    /// Provides centralized seed data for the documentation navigation and content structure.
    /// </summary>
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
            new()
            {
                Title = "What is Mycelium Forge?",
                TargetId = "what-is-mycelium-forge",
                Href = $"{PageRoutes.Documentation.Overview}#what-is-mycelium-forge",
                IsActive = true
            },
            new()
            {
                Title = "Key sections",
                TargetId = "key-sections",
                Href = $"{PageRoutes.Documentation.Overview}#key-sections",
                IsActive = false
            }
        ];

        /// <summary>
        /// Gets the table of contents items for the packages and kpar format documentation page.
        /// </summary>
        public static IReadOnlyList<DocumentationTocItemModel> PackagesAndKparTableOfContents { get; } =
        [
            new()
            {
                Title = "What is a package?",
                TargetId = "what-is-a-package",
                Href = $"{PageRoutes.Documentation.PackagesAndKparFormat}#what-is-a-package",
                IsActive = true
            },
            new()
            {
                Title = "The kpar archive",
                TargetId = "the-kpar-archive",
                Href = $"{PageRoutes.Documentation.PackagesAndKparFormat}#the-kpar-archive",
                IsActive = false
            },
            new()
            {
                Title = "Package identity",
                TargetId = "package-identity",
                Href = $"{PageRoutes.Documentation.PackagesAndKparFormat}#package-identity",
                IsActive = false
            }
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
                        new DocumentationNavItemModel { Title = "What is Forge", Href = PageRoutes.Documentation.Overview, IsActive = activeItemTitle == "What is Forge" },
                        new DocumentationNavItemModel { Title = "Platform overview", Href = "#", IsActive = activeItemTitle == "Platform overview" },
                        new DocumentationNavItemModel { Title = "Quickstart", Href = "#", IsActive = activeItemTitle == "Quickstart" }
                    ]
                },
                new DocumentationNavGroupModel
                {
                    Title = "CORE CONCEPTS",
                    Items =
                    [
                        new DocumentationNavItemModel { Title = "Packages & the kpar format", Href = PageRoutes.Documentation.PackagesAndKparFormat, IsActive = activeItemTitle == "Packages & the kpar format" },
                        new DocumentationNavItemModel { Title = "Supported formats", Href = "#", IsActive = activeItemTitle == "Supported formats" },
                        new DocumentationNavItemModel { Title = "Scopes & identifiers", Href = "#", IsActive = activeItemTitle == "Scopes & identifiers" },
                        new DocumentationNavItemModel { Title = "Versioning & SemVer", Href = "#", IsActive = activeItemTitle == "Versioning & SemVer" },
                        new DocumentationNavItemModel { Title = "Immutability & yanking", Href = "#", IsActive = activeItemTitle == "Immutability & yanking" },
                        new DocumentationNavItemModel { Title = "Release validation", Href = "#", IsActive = activeItemTitle == "Release validation" }
                    ]
                },
                new DocumentationNavGroupModel
                {
                    Title = "CONSUMING",
                    Items =
                    [
                        new DocumentationNavItemModel { Title = "Discover & search", Href = "#", IsActive = activeItemTitle == "Discover & search" },
                        new DocumentationNavItemModel { Title = "Import into a project", Href = "#", IsActive = activeItemTitle == "Import into a project" },
                        new DocumentationNavItemModel { Title = "The Library panel", Href = "#", IsActive = activeItemTitle == "The Library panel" },
                        new DocumentationNavItemModel { Title = "Standard libraries", Href = "#", IsActive = activeItemTitle == "Standard libraries" }
                    ]
                },
                new DocumentationNavGroupModel
                {
                    Title = "PUBLISHING",
                    Items =
                    [
                        new DocumentationNavItemModel { Title = "Prepare a package", Href = "#", IsActive = activeItemTitle == "Prepare a package" },
                        new DocumentationNavItemModel { Title = "Publish from Bloom", Href = "#", IsActive = activeItemTitle == "Publish from Bloom" },
                        new DocumentationNavItemModel { Title = "Publish with the CLI", Href = "#", IsActive = activeItemTitle == "Publish with the CLI" },
                        new DocumentationNavItemModel { Title = "Manage versions", Href = "#", IsActive = activeItemTitle == "Manage versions" }
                    ]
                },
                new DocumentationNavGroupModel
                {
                    Title = "SECURITY",
                    Items =
                    [
                        new DocumentationNavItemModel { Title = "API keys", Href = "#", IsActive = activeItemTitle == "API keys" },
                        new DocumentationNavItemModel { Title = "Trusted publishing", Href = "#", IsActive = activeItemTitle == "Trusted publishing" },
                        new DocumentationNavItemModel { Title = "Provenance", Href = "#", IsActive = activeItemTitle == "Provenance" }
                    ]
                },
                new DocumentationNavGroupModel
                {
                    Title = "CLI REFERENCE",
                    Items =
                    [
                        new DocumentationNavItemModel { Title = "Install & authenticate", Href = "#", IsActive = activeItemTitle == "Install & authenticate" },
                        new DocumentationNavItemModel { Title = "Command reference", Href = "#", IsActive = activeItemTitle == "Command reference" }
                    ]
                },
                new DocumentationNavGroupModel
                {
                    Title = "HTTP API",
                    Items =
                    [
                        new DocumentationNavItemModel { Title = "Overview", Href = "#", IsActive = activeItemTitle == "Overview" },
                        new DocumentationNavItemModel { Title = "Endpoints", Href = "#", IsActive = activeItemTitle == "Endpoints" }
                    ]
                },
                new DocumentationNavGroupModel
                {
                    Title = "GOVERNANCE",
                    Items =
                    [
                        new DocumentationNavItemModel { Title = "Ownership & maintainers", Href = "#", IsActive = activeItemTitle == "Ownership & maintainers" },
                        new DocumentationNavItemModel { Title = "Verified scopes", Href = "#", IsActive = activeItemTitle == "Verified scopes" },
                        new DocumentationNavItemModel { Title = "Roles & permissions", Href = "#", IsActive = activeItemTitle == "Roles & permissions" }
                    ]
                },
                new DocumentationNavGroupModel
                {
                    Title = "OPERATIONS",
                    Items =
                    [
                        new DocumentationNavItemModel { Title = "Self-hosting", Href = "#", IsActive = activeItemTitle == "Self-hosting" },
                        new DocumentationNavItemModel { Title = "Mirroring & proxying", Href = "#", IsActive = activeItemTitle == "Mirroring & proxying" }
                    ]
                },
                new DocumentationNavGroupModel
                {
                    Title = "REFERENCE & POLICIES",
                    Items =
                    [
                        new DocumentationNavItemModel { Title = "Terms & policies", Href = "#", IsActive = activeItemTitle == "Terms & policies" },
                        new DocumentationNavItemModel { Title = "Security", Href = "#", IsActive = activeItemTitle == "Security" }
                    ]
                },
                new DocumentationNavGroupModel
                {
                    Title = "FAQ",
                    Items =
                    [
                        new DocumentationNavItemModel { Title = "Troubleshooting", Href = "#", IsActive = activeItemTitle == "Troubleshooting" }
                    ]
                }
            ];
        }
    }
}
