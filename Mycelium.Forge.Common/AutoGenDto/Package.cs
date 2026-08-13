// ------------------------------------------------------------------------------------------------
// <copyright file="Package.cs" company="Starion Group S.A.">
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
    public partial class Package : IPackage
    {
        /// <summary>
        /// The DateTime at which the Thing has been created.
        /// </summary>
        [Property(xmiId: "EAID_C608D12F_75CD_46ed_9AEC_1E65CD83951B", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.CreatedAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Total downloads across the Package, computed as the sum of downloadCount over all its
        /// PackageVersions. Never set directly; always reflects the current totals of its versions.
        /// </summary>
        [Property(xmiId: "EAID_92CAFF82_73D8_4c5d_86A2_5768EDE59367", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: true, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackage.DownloadCount")]
        public int downloadCount { get; internal set; }

        /// <summary>
        /// Universally Unique Identifier (UUID) that uniquely identifies an instance of Thing.
        /// </summary>
        [Property(xmiId: "EAID_3A963DC1_6E7A_4925_8686_A68C8799F12E", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Whether the Package appears in search results and browsable listings. When false, it's still
        /// reachable directly (e.g. by exact name or link) but hidden from discovery.
        /// </summary>
        [Property(xmiId: "EAID_4F8A692D_12D7_4a0e_BF07_B7BD116E7E01", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackage.Listed")]
        public bool Listed { get; set; }

        /// <summary>
        /// The DateTime at which the Thing was last modified.
        /// </summary>
        [Property(xmiId: "EAID_048B19C9_AA4A_4e41_A4BD_B28426AEC937", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.ModifiedAt")]
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// A human readable character string in English by which a Namespace instance can be referred to.e.g.
        /// an Organization's full legal or display name, an Account's chosen display name, a Package's full
        /// name, or the Forge's own instance name. Mandatory, and globally unique across every Namespace
        /// subtype.
        /// </summary>
        [Property(xmiId: "EAID_4F9CE1B0_75B2_4f0f_96BE_251F42265A8D", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "INamespace.Name")]
        public string Name { get; set; }

        /// <summary>
        /// The accounts that represent the Package maintainers
        /// </summary>
        [Property(xmiId: "EAID_dst10BE38_3F02_4f31_AB54_D3C754FAC7FE", aggregation: AggregationKind.None, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackage.PackageMaintainer")]
        public List<Guid> PackageMaintainer { get; set; } = [];

        /// <summary>
        /// The accounts that represent the Package owners
        /// </summary>
        [Property(xmiId: "EAID_dst2A162D_DE42_4b01_9B5F_17A7DA9D182F", aggregation: AggregationKind.None, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackage.PackageOwner")]
        public List<Guid> PackageOwner { get; set; } = [];

        /// <summary>
        /// The referenced PackageType that denotes what kind of Package this is.
        /// </summary>
        [Property(xmiId: "EAID_dst22907F_7B04_491a_9894_18D7F240B0C8", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackage.PackageType")]
        public Guid PackageType { get; set; }

        /// <summary>
        /// A compact, typically lowercase, URL- and path-safe identifier for a Namespace instance. Used
        /// wherever the full name would be too long or contain characters unsuitable for URLs, CLI commands, or
        /// package references (e.g. spaces, punctuation). Mandatory, and globally unique across every Namespace
        /// subtype, independently of name's uniqueness. Functions as the practical "handle" or "slug" used in
        /// addresses, links, and package coordinates, while name remains the display-facing label
        /// </summary>
        [Property(xmiId: "EAID_89C8F800_EE59_4169_853B_B38E6B1857AC", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "INamespace.ShortName")]
        public string ShortName { get; set; }

        /// <summary>
        /// The PackageVersions that are owned by the Package
        /// </summary>
        [Property(xmiId: "EAID_dstEAA194_978B_4aa5_BEBC_DC3C38829711", aggregation: AggregationKind.Composite, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackage.Version")]
        public List<Guid> Version { get; set; } = [];

        /// <summary>
        /// Controls who can see and access the Package. INTERNAL is invalid when the owning Scope is an
        /// Account, per the earlier invariant, since "internal" only makes sense within an organizational
        /// boundary
        /// </summary>
        [Property(xmiId: "EAID_84CB2554_8FEB_49ee_ADC4_8AEAECB7D2D1", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackage.Visibility")]
        public VisibilityKind Visibility { get; set; }

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
