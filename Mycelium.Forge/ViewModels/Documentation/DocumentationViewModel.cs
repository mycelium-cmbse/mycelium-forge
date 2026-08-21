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
    using Mycelium.Forge.Common;
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
            this.PopulateNavGroups("What is Forge");

            this.KeySections =
            [
                new DocumentationSectionCardModel
                {
                    Title = "Getting started",
                    Description = "Install the CLI and import your first package.",
                    Href = PageRoutes.Documentation.Overview,
                    IconName = "book-open"
                },
                new DocumentationSectionCardModel
                {
                    Title = "Publish a package",
                    Description = "Prepare metadata, publish from Bloom or the CLI, pass Release Validation.",
                    Href = PageRoutes.Publish,
                    IconName = "upload"
                },
                new DocumentationSectionCardModel
                {
                    Title = "Consuming packages",
                    Description = "Discover models, browse standard libraries, and import into your projects.",
                    Href = PageRoutes.Packages,
                    IconName = "download"
                },
                new DocumentationSectionCardModel
                {
                    Title = "Core concepts",
                    Description = "kpar packaging, scopes, versioning, immutability and validation.",
                    Href = PageRoutes.Documentation.PackagesAndKparFormat,
                    IconName = "layers"
                },
                new DocumentationSectionCardModel
                {
                    Title = "CLI reference",
                    Description = "Command reference, flags, authentication and CI/CD automation.",
                    Href = PageRoutes.Documentation.Overview,
                    IconName = "terminal"
                },
                new DocumentationSectionCardModel
                {
                    Title = "HTTP API",
                    Description = "REST endpoints, OpenAPI specification, and programmatic access.",
                    Href = PageRoutes.Documentation.Overview,
                    IconName = "globe"
                }
            ];

            this.TableOfContents =
            [
                new DocumentationTocItemModel
                {
                    Title = "What is Mycelium Forge?",
                    TargetId = "what-is-mycelium-forge",
                    Href = $"{PageRoutes.Documentation.Overview}#what-is-mycelium-forge",
                    IsActive = true
                },
                new DocumentationTocItemModel
                {
                    Title = "Key sections",
                    TargetId = "key-sections",
                    Href = $"{PageRoutes.Documentation.Overview}#key-sections",
                    IsActive = false
                }
            ];

            this.LastUpdated = "29 July 2026";
            this.FeedbackGiven = false;
            this.IsHelpful = null;
        }

        /// <summary>
        /// Initializes the view model state for the packages and kpar format documentation page.
        /// </summary>
        public void InitializePackagesAndKpar()
        {
            this.PopulateNavGroups("Packages & the kpar format");

            this.KeySections = [];

            this.TableOfContents =
            [
                new DocumentationTocItemModel
                {
                    Title = "What is a package?",
                    TargetId = "what-is-a-package",
                    Href = $"{PageRoutes.Documentation.PackagesAndKparFormat}#what-is-a-package",
                    IsActive = true
                },
                new DocumentationTocItemModel
                {
                    Title = "The kpar archive",
                    TargetId = "the-kpar-archive",
                    Href = $"{PageRoutes.Documentation.PackagesAndKparFormat}#the-kpar-archive",
                    IsActive = false
                },
                new DocumentationTocItemModel
                {
                    Title = "Package identity",
                    TargetId = "package-identity",
                    Href = $"{PageRoutes.Documentation.PackagesAndKparFormat}#package-identity",
                    IsActive = false
                }
            ];

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

        /// <summary>
        /// Populates the navigation groups collection with the specified active item marked.
        /// </summary>
        /// <param name="activeItemTitle">The title of the active navigation item.</param>
        private void PopulateNavGroups(string activeItemTitle)
        {
            this.NavGroups =
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
