// ------------------------------------------------------------------------------------------------
// <copyright file="IPackage.cs" company="Starion Group S.A.">
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
    /// A named, versioned unit of software published to the Forge, owned by exactly one Scope (an
    /// Organization or an Account) and containing one or more PackageVersions
    /// </summary>
    [Class(xmiId: "EAID_65683CA4_032F_457d_B996_A3893C4293CA", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial interface IPackage : IThing, INamespace
    {
        /// <summary>
        /// Total downloads across the Package, computed as the sum of downloadCount over all its
        /// PackageVersions. Never set directly; always reflects the current totals of its versions.
        /// </summary>
        [Property(xmiId: "EAID_92CAFF82_73D8_4c5d_86A2_5768EDE59367", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: true, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        int downloadCount { get; }

        /// <summary>
        /// Whether the Package appears in search results and browsable listings. When false, it's still
        /// reachable directly (e.g. by exact name or link) but hidden from discovery.
        /// </summary>
        [Property(xmiId: "EAID_4F8A692D_12D7_4a0e_BF07_B7BD116E7E01", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        bool Listed { get; set; }

        /// <summary>
        /// The accounts that represent the Package maintainers
        /// </summary>
        [Property(xmiId: "EAID_dst10BE38_3F02_4f31_AB54_D3C754FAC7FE", aggregation: AggregationKind.None, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> PackageMaintainer { get; set; }

        /// <summary>
        /// The accounts that represent the Package owners
        /// </summary>
        [Property(xmiId: "EAID_dst2A162D_DE42_4b01_9B5F_17A7DA9D182F", aggregation: AggregationKind.None, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> PackageOwner { get; set; }

        /// <summary>
        /// The referenced PackageType that denotes what kind of Package this is.
        /// </summary>
        [Property(xmiId: "EAID_dst22907F_7B04_491a_9894_18D7F240B0C8", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid PackageType { get; set; }

        /// <summary>
        /// The PackageVersions that are owned by the Package
        /// </summary>
        [Property(xmiId: "EAID_dstEAA194_978B_4aa5_BEBC_DC3C38829711", aggregation: AggregationKind.Composite, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> Version { get; set; }

        /// <summary>
        /// Controls who can see and access the Package. INTERNAL is invalid when the owning Scope is an
        /// Account, per the earlier invariant, since "internal" only makes sense within an organizational
        /// boundary
        /// </summary>
        [Property(xmiId: "EAID_84CB2554_8FEB_49ee_ADC4_8AEAECB7D2D1", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        VisibilityKind Visibility { get; set; }

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
