// ------------------------------------------------------------------------------------------------
// <copyright file="HomeViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using System.Collections.Generic;

    using Mycelium.Forge.Models;

    /// <summary>
    /// Provides view model state and operations for the Mycelium Forge home landing page.
    /// </summary>
    public class HomeViewModel : IHomeViewModel
    {
        /// <summary>
        /// Gets or sets the total published package count displayed in the hero section.
        /// </summary>
        public string PackageCount { get; set; } = "42";

        /// <summary>
        /// Gets or sets the total package version count displayed in the hero section.
        /// </summary>
        public string VersionCount { get; set; } = "128";

        /// <summary>
        /// Gets or sets the total registered publisher count displayed in the hero section.
        /// </summary>
        public string PublisherCount { get; set; } = "6";

        /// <summary>
        /// Gets or sets the total package import count displayed in the hero section.
        /// </summary>
        public string ImportCount { get; set; } = "2,582";

        /// <summary>
        /// Gets or sets the standard library package models.
        /// </summary>
        public IReadOnlyList<PackageModel> StandardLibraries { get; set; } = [];

        /// <summary>
        /// Gets or sets the recently updated package models.
        /// </summary>
        public IReadOnlyList<PackageModel> RecentlyUpdated { get; set; } = [];

        /// <summary>
        /// Gets or sets the most used package models.
        /// </summary>
        public IReadOnlyList<PackageModel> MostUsed { get; set; } = [];

        /// <summary>
        /// Gets or sets the package models from other MBSE tools.
        /// </summary>
        public IReadOnlyList<PackageModel> ModelsFromOtherMbseTools { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and populates the package catalog collections.
        /// </summary>
        public void InitializeViewModel()
        {
            this.StandardLibraries =
            [
                new PackageModel(
                    "SysML-v2-Standard-Library",
                    "/packages/omg/SysML-v2-Standard-Library",
                    "Official KerML and SysML v2 standard library models and metamodels.",
                    "SysML v2",
                    "omg",
                    "v2025.1",
                    "sysml2 · kerml · standard",
                    "1.2k",
                    true),
                new PackageModel(
                    "KerML-Core-Library",
                    "/packages/omg/KerML-Core-Library",
                    "Fundamental kernel modeling language root types and semantic primitives.",
                    "SysML v2",
                    "omg",
                    "v2025.1",
                    "kerml · core · semantics",
                    "842",
                    true),
                new PackageModel(
                    "ISQ-Quantities-Units",
                    "/packages/omg/ISQ-Quantities-Units",
                    "International System of Quantities and measurement units for SysML v2.",
                    "SysML v2",
                    "omg",
                    "v2025.1",
                    "isq · units · physics",
                    "630",
                    true)
            ];

            this.RecentlyUpdated =
            [
                new PackageModel(
                    "Avionics-Architecture-Base",
                    "/packages/esa/Avionics-Architecture-Base",
                    "Modular reference patterns for civil avionics system engineering and architecture.",
                    "SysML v2",
                    "esa",
                    "v1.4.0",
                    "avionics · reference · safety",
                    "320",
                    true),
                new PackageModel(
                    "Spacecraft-Telemetry-Types",
                    "/packages/starion/Spacecraft-Telemetry-Types",
                    "Standard TM/TC packet structures and telemetry frame models.",
                    "SysML v2",
                    "starion",
                    "v2.1.0",
                    "space · telemetry · tmtc",
                    "215",
                    true),
                new PackageModel(
                    "Thermal-Control-Subsystem",
                    "/packages/rhea/Thermal-Control-Subsystem",
                    "Thermal balance analysis interfaces, radiative transfer, and loop models.",
                    "SysML v2",
                    "rhea",
                    "v0.9.2",
                    "thermal · analysis · subsystems",
                    "180",
                    true)
            ];

            this.MostUsed =
            [
                new PackageModel(
                    "SysML-v2-Standard-Library",
                    "/packages/omg/SysML-v2-Standard-Library",
                    "Official KerML and SysML v2 standard library models and metamodels.",
                    "SysML v2",
                    "omg",
                    "v2025.1",
                    "sysml2 · kerml · standard",
                    "1.2k",
                    true),
                new PackageModel(
                    "ECSS-E-TM-10-25-Schema",
                    "/packages/esa/ECSS-E-TM-10-25-Schema",
                    "Space engineering model-based data exchange engineering ontology.",
                    "SysML v2",
                    "esa",
                    "v1.2.0",
                    "ecss · cdp4 · space",
                    "940",
                    true),
                new PackageModel(
                    "SI-Units-Extension",
                    "/packages/omg/SI-Units-Extension",
                    "Extended dimensions, prefixes, and conversion factors for engineering packages.",
                    "SysML v2",
                    "omg",
                    "v1.1.0",
                    "si · units · conversions",
                    "890",
                    true)
            ];

            this.ModelsFromOtherMbseTools =
            [
                new PackageModel(
                    "CDP4-COMET-Reference-Model",
                    "/packages/starion/CDP4-COMET-Reference-Model",
                    "Concurrent design platform engineering model transfer definitions and domain patterns.",
                    "CDP4-COMET",
                    "starion",
                    "v3.0.0",
                    "cdp4 · comet · concurrent",
                    "512",
                    true),
                new PackageModel(
                    "Capella-Operational-Analysis",
                    "/packages/thales/Capella-Operational-Analysis",
                    "Arcadia operational capabilities, actors, and operational process reference models.",
                    "Capella",
                    "thales",
                    "v1.0.0",
                    "capella · arcadia · operational",
                    "428"),
                new PackageModel(
                    "Capella-System-Architecture",
                    "/packages/thales/Capella-System-Architecture",
                    "System analysis and logical architecture decomposition template packages.",
                    "Capella",
                    "thales",
                    "v1.0.0",
                    "capella · arcadia · architecture",
                    "395")
            ];
        }
    }
}
