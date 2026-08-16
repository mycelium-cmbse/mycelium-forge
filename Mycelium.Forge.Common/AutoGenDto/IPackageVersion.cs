// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageVersion.cs" company="Starion Group S.A.">
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
    public partial interface IPackageVersion : IThing
    {
        /// <summary>
        /// The amount of times the version has been downloaded since its creation
        /// </summary>
        [Property(xmiId: "EAID_133F00F0_42F5_4d22_AAF4_0BC7A44A6C0F", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        int DownloadCount { get; set; }

        /// <summary>
        /// Whether this specific version appears in search/browse results. Lets you hide one problematic
        /// release from discovery without touching any other version.
        /// </summary>
        [Property(xmiId: "EAID_BFB9588E_A91A_4546_978A_0B8A6F67EEE5", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        bool Listed { get; set; }

        /// <summary>
        /// the owned MetaDataDefinition
        /// </summary>
        [Property(xmiId: "EAID_dst94AFDF_F62B_4600_9B44_6076F54197E6", aggregation: AggregationKind.Composite, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid MetaData { get; set; }

        /// <summary>
        /// The date at which the version was uploaded or published to Forge
        /// </summary>
        [Property(xmiId: "EAID_DFF03201_3242_41f9_B6C9_64ED075102AD", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        DateTime PublicationDate { get; set; }

        /// <summary>
        /// </summary>
        [Property(xmiId: "EAID_5E3DB552_5872_48f8_BDA3_4CB2ECE6BC69", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Version { get; set; }

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
