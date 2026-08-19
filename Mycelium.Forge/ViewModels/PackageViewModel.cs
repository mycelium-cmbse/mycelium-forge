// ------------------------------------------------------------------------------------------------
// <copyright file="PackageViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using Mycelium.Forge.Models;

    /// <summary>
    /// Provides view model state and initialization logic for the Mycelium Forge package details page.
    /// </summary>
    public class PackageViewModel : IPackageViewModel
    {
        /// <summary>
        /// Gets or sets the package details and metadata.
        /// </summary>
        public PackageDetailsModel Package { get; set; }

        /// <summary>
        /// Initializes the package view model state for the specified package unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the package.</param>
        public void InitializeViewModel(Guid id)
        {
            var qualityChecks = new List<PackageQualityCheckModel>
            {
                new("Loads in a fresh workspace"),
                new("Documentation provided"),
                new("Metamodel up to date"),
                new("Dependencies resolved"),
                new("License present")
            };

            var maintainers = new List<PackageMaintainerModel>
            {
                new("Starion Group", "SG", true),
                new("R. André", "RA")
            };

            var tags = new List<string>
            {
                "mission-model",
                "power"
            };

            var installCommands = new Dictionary<string, string>
            {
                { "Forge CLI", "forge add @starion/ECSS-MM-PWR@^1.2" },
                { "SysML v2 import", "import @starion/ECSS-MM-PWR::*;" },
                { "Manifest", "@starion/ECSS-MM-PWR = \"^1.2.0\"" },
                { "purl", "pkg:forge/@starion/ECSS-MM-PWR@1.2.0" }
            };

            var elements = new List<PackageElementModel>
            {
                new("PowerBus", "«part def»", "Parts", "8 attributes"),
                new("Battery", "«part def»", "Parts", "12 attributes"),
                new("SolarArray", "«part def»", "Parts", "6 attributes"),
                new("PowerConditioningUnit", "«part def»", "Parts", "9 attributes"),
                new("powerOut", "«port def»", "Parts", "2 attributes"),
                new("BusVoltage", "«attribute def»", "Attributes", "typed by Voltage")
            };

            var dependencies = new List<PackageDependencyModel>
            {
                new("@mycelium/ISQ-quantities-units", "/packages/mycelium/ISQ-quantities-units", "^2.4 → 2.5.0 · MIT", true)
            };

            var dependents = new List<PackageDependentModel>
            {
                new("Spacecraft Mission", string.Empty, "project · imports v1.2.0", true),
                new("@esa/PlatformX", "/packages/esa/PlatformX", "specializes PowerBus · v3.1.0"),
                new("@starion/SmallSat-Bus", "/packages/starion/SmallSat-Bus", "imports v1.1.0", false, true)
            };

            var versions = new List<PackageVersionModel>
            {
                new("v1.2.0", "Latest", "2 weeks ago", 3, true, "42 KB"),
                new("v1.1.0", string.Empty, "2 months ago", 5, true, "40 KB"),
                new("v1.0.0", string.Empty, "4 months ago", 2, true, "38 KB", true),
                new("v0.9.0", "pre", "5 months ago", 0, true, "35 KB")
            };

            var validationChecks = new List<PackageValidationCheckModel>
            {
                new("Loads in a fresh workspace", "Parsed 47 elements in 1.2s · 0 errors, 0 warnings", "Pass"),
                new("Dependencies resolved", "1 dependency: @mycelium/ISQ-quantities-units ^2.4 → 2.5.0", "Pass"),
                new("Metamodel conformance", "SysML v2 (2025-02) · packaged as KerML clause 10.3 kpar", "Pass"),
                new("Documentation provided", "README.md present (2.1 KB)", "Pass"),
                new("License present", "Apache-2.0 (SPDX)", "Pass")
            };

            var validationReport = new PackageValidationReportModel(
                "Release validation passed",
                "v1.2.0 loaded and resolved cleanly in a fresh workspace. Validated 2 weeks ago.",
                "5 / 5",
                true,
                validationChecks);

            this.Package = new PackageDetailsModel(
                "ECSS-MM-PWR",
                "@starion",
                "@starion/ECSS-MM-PWR",
                "SysML v2",
                "v1.2.0",
                "Latest stable",
                "ECSS mission model: Power subsystem. Part definitions for the power bus, battery, solar array, and power conditioning unit, typed by ISQ quantity kinds.",
                "Published 2 weeks ago by @starion · Apache-2.0 · 210 imports",
                true,
                "210",
                "5/5 checks",
                "2 weeks ago",
                "Apache-2.0",
                "SysML v2 (2025-02)",
                "https://github.com/starion/ecss-mm-pwr",
                "github.com/starion/…",
                "pkg:forge/@starion/ECSS-MM-PWR@1.2.0",
                "pkg:forge/@starion/…",
                "Power subsystem mission model following ECSS-E-ST-20C, published as a reusable SysML v2 library. It provides the electrical power architecture for early-phase spacecraft design.",
                "Part definitions: PowerBus, Battery, SolarArray, PowerConditioningUnit. Attribute definitions typed by ISQ quantity kinds (power, voltage, capacity). Interface definitions for the power distribution ports.",
                "import ECSS::MM::PWR::*;",
                "part def MyPowerSystem :> PowerBus { }",
                qualityChecks,
                maintainers,
                tags,
                installCommands,
                elements,
                dependencies,
                dependents,
                versions,
                validationReport);
        }
    }
}
