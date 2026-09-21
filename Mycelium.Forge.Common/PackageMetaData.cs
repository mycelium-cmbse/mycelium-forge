// ------------------------------------------------------------------------------------------------
// <copyright file="PackageMetaData.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System.Collections.Generic;

    /// <summary>
    /// Partial class implementation for <see cref="PackageMetaData" /> providing metadata provenance, model element
    /// projections, dependencies, and quality checks.
    /// </summary>
    /// <remarks>
    /// This partial class is temporary and will be removed once the domain metamodel is updated to include metadata
    /// provenance, manifest dependencies, contained model element declarations, and validation evaluation report entities in
    /// the code-generated DTO.
    /// </remarks>
    public partial class PackageMetaData
    {
        /// <summary>
        /// Gets or sets the metadata source provenance indicating whether metadata was declared by the artifact manifest or
        /// asserted by the publisher.
        /// </summary>
        public MetadataSource MetadataSource { get; set; } = MetadataSource.DeclaredByArtefact;

        /// <summary>
        /// Gets or sets the collection of unresolved or resolved package dependency tuples (Name, Summary, IsVerified).
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once dedicated Dependency entities are incorporated into the domain
        /// model.
        /// </remarks>
        public List<(string Name, string Summary, bool IsVerified)> Dependencies { get; set; } =
        [
            ("@mycelium/ISQ-quantities-units", "^2.4 → 2.5.0 · MIT", true)
        ];

        /// <summary>
        /// Gets or sets the collection of model element definition tuples (Name, Kind, Category, AttributeSummary) contained
        /// within the package release.
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once dedicated ModelElement entities are incorporated into the domain
        /// model.
        /// </remarks>
        public List<(string Name, string Kind, string Category, string AttributeSummary)> Elements { get; set; } =
        [
            ("PowerBus", "«part def»", "Parts", "8 attributes"),
            ("Battery", "«part def»", "Parts", "12 attributes"),
            ("SolarArray", "«part def»", "Parts", "6 attributes"),
            ("PowerConditioningUnit", "«part def»", "Parts", "9 attributes"),
            ("powerOut", "«port def»", "Parts", "2 attributes"),
            ("BusVoltage", "«attribute def»", "Attributes", "typed by Voltage")
        ];

        /// <summary>
        /// Gets or sets the list of automated quality validation check evaluation tuples (Title, Detail, Passed).
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once dedicated ValidationReport entities are incorporated into the
        /// domain model.
        /// </remarks>
        public List<(string Title, string Detail, bool Passed)> QualityChecks { get; set; } =
        [
            ("Loads in a fresh workspace", "Parsed elements cleanly in 1.2s · 0 errors, 0 warnings", true),
            ("Dependencies resolved", "1 dependency: @mycelium/ISQ-quantities-units ^2.4 → 2.5.0", true),
            ("Metamodel conformance", "SysML v2 (2025-02) · packaged as KerML clause 10.3 kpar", true),
            ("Documentation provided", "README.md present (2.1 KB)", true),
            ("License present", "Apache-2.0 (SPDX)", true)
        ];
    }
}
