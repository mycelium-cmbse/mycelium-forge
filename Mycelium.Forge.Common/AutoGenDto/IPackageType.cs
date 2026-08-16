// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageType.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System;
    using System.Collections.Generic;

    using Mycelium.Forge.Common.Decorators;

    /// <summary>
    /// Identifies which packaging ecosystem a given Package (and its PackageVersions) belongs to; i.e.,
    /// which language or platform's native package format, manifest structure, and versioning convention
    /// Forge should apply when storing, indexing, and serving that package. Because different ecosystems
    /// use incompatible manifest schemas, dependency-resolution rules, and file layouts, PackageType is
    /// effectively a discriminator: it determines which parsing logic, validation rules, and
    /// protocol-specific API endpoint the Forge routes a given Package through.
    /// </summary>
    [Class(xmiId: "EAID_367F204E_758E_41a5_892F_22A589405944", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial interface IPackageType : IThing
    {
        /// <summary>
        /// A humand readable description of this PackageType
        /// </summary>
        [Property(xmiId: "EAID_DB2CC1A3_0314_471e_BE53_04A8E620A06D", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Description { get; set; }

        /// <summary>
        /// Human readable character string in English by which something can be referred to.
        /// </summary>
        [Property(xmiId: "EAID_932920D8_9B6C_4e9e_B832_DEB63E72B9E6", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Name { get; set; }

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
