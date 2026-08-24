// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDetailsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.PackageDetails
{
    using FluentResults;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Models.Package;
    using Mycelium.Forge.Models.Validation;

    /// <summary>
    /// Provides view model state and initialization logic for the Mycelium Forge package details page.
    /// </summary>
    public class PackageDetailsViewModel : IPackageDetailsViewModel
    {
        /// <summary>
        /// Constant representing the part definition stereotype text.
        /// </summary>
        private const string PartDefinitionStereotype = "«part def»";

        /// <summary>
        /// Constant representing the parts category label.
        /// </summary>
        private const string PartsCategory = "Parts";

        /// <summary>
        /// Gets or sets the package details and metadata.
        /// </summary>
        public PackageDetailsModel Package { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is an administrator of the package.
        /// </summary>
        public bool IsUserAdmin { get; set; } = true;

        /// <summary>
        /// Initializes the package view model state for the specified package name and organization.
        /// </summary>
        /// <param name="packageName">The name of the package.</param>
        /// <param name="organization">The organization of the package.</param>
        public void InitializeViewModel(string packageName, string organization)
        {
            var qualityChecks = new List<ValidationCheckModel>
            {
                new("Loads in a fresh workspace"),
                new("Documentation provided"),
                new("Metamodel up to date"),
                new("Dependencies resolved"),
                new("License present")
            };

            var maintainers = new List<PackageMaintainerModel>
            {
                new("Starion Group", "SG", true, PackageInvitationKind.OWNER),
                new("R. André", "RA")
            };

            var tags = new List<string>
            {
                "mission-model",
                "power"
            };

            string resolvedOrganization;

            if (string.IsNullOrWhiteSpace(organization))
            {
                resolvedOrganization = "@starion";
            }
            else
            {
                resolvedOrganization = organization.StartsWith('@') ? organization : $"@{organization}";
            }

            var resolvedName = string.IsNullOrWhiteSpace(packageName) ? "ECSS-MM-PWR" : packageName;
            var fullName = $"{resolvedOrganization}/{resolvedName}";
            var orgWithoutAt = resolvedOrganization.TrimStart('@');

            var installCommands = new Dictionary<string, string>
            {
                { "Forge CLI", $"forge add {fullName}@^1.2" },
                { "SysML v2 import", $"import {fullName}::*;" },
                { "Manifest", $"{fullName} = \"^1.2.0\"" },
                { "purl", $"pkg:forge/{fullName}@1.2.0" }
            };

            var elements = new List<PackageElementModel>
            {
                new("PowerBus", PartDefinitionStereotype, PartsCategory, "8 attributes"),
                new("Battery", PartDefinitionStereotype, PartsCategory, "12 attributes"),
                new("SolarArray", PartDefinitionStereotype, PartsCategory, "6 attributes"),
                new("PowerConditioningUnit", PartDefinitionStereotype, PartsCategory, "9 attributes"),
                new("powerOut", "«port def»", PartsCategory, "2 attributes"),
                new("BusVoltage", "«attribute def»", "Attributes", "typed by Voltage")
            };

            var dependencies = new List<PackageRelationshipModel>
            {
                new("@mycelium/ISQ-quantities-units", PageRoutes.GetPackageRoute("mycelium", "ISQ-quantities-units"), "^2.4 → 2.5.0 · MIT", false, true)
            };

            var dependents = new List<PackageRelationshipModel>
            {
                new("Spacecraft Mission", string.Empty, "project · imports v1.2.0", true),
                new("@esa/PlatformX", PageRoutes.GetPackageRoute("esa", "PlatformX"), "specializes PowerBus · v3.1.0"),
                new("@starion/SmallSat-Bus", PageRoutes.GetPackageRoute("starion", "SmallSat-Bus"), "imports v1.1.0", false, true)
            };

            var versions = new List<PackageVersionModel>
            {
                new("v1.2.0", "2 weeks ago", 3, true, "42 KB", true),
                new("v1.1.0", "2 months ago", 5, true, "40 KB"),
                new("v1.0.0", "4 months ago", 2, true, "38 KB", isUnlisted: true),
                new("v0.9.0", "5 months ago", 0, true, "35 KB")
            };

            var validationChecks = new List<ValidationCheckModel>
            {
                new("Loads in a fresh workspace", "Parsed 47 elements in 1.2s · 0 errors, 0 warnings"),
                new("Dependencies resolved", "1 dependency: @mycelium/ISQ-quantities-units ^2.4 → 2.5.0"),
                new("Metamodel conformance", "SysML v2 (2025-02) · packaged as KerML clause 10.3 kpar"),
                new("Documentation provided", "README.md present (2.1 KB)"),
                new("License present", "Apache-2.0 (SPDX)")
            };

            var validationReport = new PackageValidationReportModel(
                "Release validation passed",
                "v1.2.0 loaded and resolved cleanly in a fresh workspace. Validated 2 weeks ago.",
                "5 / 5",
                true,
                validationChecks);

            var packageDto = new Package
            {
                Name = resolvedName,
                ShortName = resolvedName.ToLowerInvariant(),
                Visibility = VisibilityKind.PUBLIC,
                CreatedAt = DateTime.UtcNow.AddDays(-14)
            };

            var packageModel = new PackageModel(
                packageDto,
                resolvedOrganization,
                "v1.2.0",
                "SysML v2",
                $"{resolvedName} mission model: Power subsystem. Part definitions for the power bus, battery, solar array, and power conditioning unit, typed by ISQ quantity kinds.",
                string.Join(" · ", tags),
                "210")
            {
                IsVerified = true,
                Role = PackageInvitationKind.OWNER,
                License = "Apache-2.0",
                Href = PageRoutes.GetPackageRoute(resolvedOrganization, resolvedName),
                Maintainers = maintainers,
                Versions = versions
            };

            this.Package = new PackageDetailsModel(packageModel)
            {
                ReleaseStatus = "Latest stable",
                Provenance = $"Published 2 weeks ago by {resolvedOrganization} · Apache-2.0 · 210 imports",
                QualityScore = "5/5 checks",
                Metamodel = "SysML v2 (2025-02)",
                RepositoryUrl = $"https://github.com/{orgWithoutAt}/{resolvedName.ToLowerInvariant()}",
                RepositoryDisplayName = $"github.com/{orgWithoutAt}/…",
                PackageUrl = $"pkg:forge/{fullName}@1.2.0",
                PackageUrlDisplayName = $"pkg:forge/{orgWithoutAt}/…",
                ReadmeDescription = $"{resolvedName} subsystem mission model following ECSS-E-ST-20C, published as a reusable SysML v2 library. It provides the electrical power architecture for early-phase spacecraft design.",
                ReadmeContents = "Part definitions: PowerBus, Battery, SolarArray, PowerConditioningUnit. Attribute definitions typed by ISQ quantity kinds (power, voltage, capacity). Interface definitions for the power distribution ports.",
                CodeUsageImport = $"import {resolvedName.Replace('-', '_')}::*;",
                CodeUsageBody = "part def MyPowerSystem :> PowerBus { }",
                QualityChecks = qualityChecks,
                Tags = tags,
                InstallCommands = installCommands,
                Elements = elements,
                Dependencies = dependencies,
                Dependents = dependents,
                ValidationReport = validationReport
            };
        }

        /// <summary>
        /// Initiates a migration of the package in Bloom to the specified target project.
        /// </summary>
        /// <param name="result">The migration parameters including destination project and version constraint.</param>
        /// <returns>A <see cref="Result" /> indicating the success or failure of the migration initiation.</returns>
        public Result MigrateInBloom(MigrateInBloomResult result)
        {
            return Result.Ok();
        }
    }
}
