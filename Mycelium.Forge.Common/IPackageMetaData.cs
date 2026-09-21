// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageMetaData.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Partial interface extending <see cref="IPackageMetaData" /> with metadata provenance, model element projections, dependencies, and quality checks.
    /// </summary>
    /// <remarks>
    /// This partial interface is temporary and will be removed once the domain metamodel is updated to include metadata provenance, manifest dependencies, contained model element declarations, and validation evaluation report entities in the code-generated DTO.
    /// </remarks>
    public partial interface IPackageMetaData
    {
        /// <summary>
        /// Gets or sets the metadata source provenance indicating whether metadata was declared by the artifact manifest or asserted by the publisher.
        /// </summary>
        MetadataSource MetadataSource { get; set; }

        /// <summary>
        /// Gets or sets the collection of unresolved or resolved package dependency tuples (Name, Summary, IsVerified).
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once dedicated Dependency entities are incorporated into the domain model.
        /// </remarks>
        List<(string Name, string Summary, bool IsVerified)> Dependencies { get; set; }

        /// <summary>
        /// Gets or sets the collection of model element definition tuples (Name, Kind, Category, AttributeSummary) contained within the package release.
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once dedicated ModelElement entities are incorporated into the domain model.
        /// </remarks>
        List<(string Name, string Kind, string Category, string AttributeSummary)> Elements { get; set; }

        /// <summary>
        /// Gets or sets the list of automated quality validation check evaluation tuples (Title, Detail, Passed).
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once dedicated ValidationReport entities are incorporated into the domain model.
        /// </remarks>
        List<(string Title, string Detail, bool Passed)> QualityChecks { get; set; }
    }
}
