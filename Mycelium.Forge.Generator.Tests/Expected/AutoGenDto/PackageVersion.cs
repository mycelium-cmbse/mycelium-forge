// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersion.cs" company="Starion Group S.A.">
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
    /// Represents one specific, published release of a Package,  a single, frozen snapshot identified by
    /// its version number. Once published, a PackageVersion cannot be modified; correcting or updating
    /// anything about a release means publishing an entirely new PackageVersion, never editing an existing
    /// one. This is why everything describing "what this release actually is" lives here rather than on
    /// Package, which only carries the shared, never-changing identity.
    /// </summary>
    [Class(xmiId: "EAID_88E567A2_4C86_4df5_B815_57A992FC2912", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial class PackageVersion : IPackageVersion
    {
        /// <summary>
        /// The DateTime at which the Thing has been created.
        /// </summary>
        [Property(xmiId: "EAID_C608D12F_75CD_46ed_9AEC_1E65CD83951B", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.CreatedAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The amount of times the version has been downloaded since its creation
        /// </summary>
        [Property(xmiId: "EAID_133F00F0_42F5_4d22_AAF4_0BC7A44A6C0F", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackageVersion.DownloadCount")]
        public int DownloadCount { get; set; }

        /// <summary>
        /// Universally Unique Identifier (UUID) that uniquely identifies an instance of Thing.
        /// </summary>
        [Property(xmiId: "EAID_3A963DC1_6E7A_4925_8686_A68C8799F12E", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Whether this specific version appears in search/browse results. Lets you hide one problematic
        /// release from discovery without touching any other version.
        /// </summary>
        [Property(xmiId: "EAID_BFB9588E_A91A_4546_978A_0B8A6F67EEE5", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackageVersion.Listed")]
        public bool Listed { get; set; }

        /// <summary>
        /// the owned MetaDataDefinition
        /// </summary>
        [Property(xmiId: "EAID_dst94AFDF_F62B_4600_9B44_6076F54197E6", aggregation: AggregationKind.Composite, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackageVersion.MetaData")]
        public Guid MetaData { get; set; }

        /// <summary>
        /// The DateTime at which the Thing was last modified.
        /// </summary>
        [Property(xmiId: "EAID_048B19C9_AA4A_4e41_A4BD_B28426AEC937", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.ModifiedAt")]
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// The date at which the version was uploaded or published to Forge
        /// </summary>
        [Property(xmiId: "EAID_DFF03201_3242_41f9_B6C9_64ED075102AD", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackageVersion.PublicationDate")]
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// </summary>
        [Property(xmiId: "EAID_5E3DB552_5872_48f8_BDA3_4CB2ECE6BC69", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackageVersion.Version")]
        public string Version { get; set; }

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
