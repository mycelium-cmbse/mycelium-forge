// ------------------------------------------------------------------------------------------------
// <copyright file="IForge.cs" company="Starion Group S.A.">
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
    /// The single, system-wide root entity that all Organizations and Accounts belong to. It is the top of
    /// the containment hierarchy, representing the entire platform instance itself rather than any one
    /// tenant, group, or user within it. Exactly one Forge exists at any time; there is no valid state in
    /// which zero or multiple Forge instances exist simultaneously
    /// </summary>
    [Class(xmiId: "EAID_17429100_7982_41d3_AD5A_241745EAC4FE", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial interface IForge : INamespace
    {
        /// <summary>
        /// The accounts that exist in Forge
        /// </summary>
        [Property(xmiId: "EAID_dst2E6A77_9CEE_4ef3_A3FE_ED24C609B3DA", aggregation: AggregationKind.Composite, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> Account { get; set; }

        /// <summary>
        /// The administators of Forge
        /// </summary>
        [Property(xmiId: "EAID_dstA67767_8BA4_4ed8_A805_BFC11AB57F42", aggregation: AggregationKind.None, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [SubsettedProperty(propertyName: "EAID_dst2E6A77_9CEE_4ef3_A3FE_ED24C609B3DA")]
        List<Guid> Administrator { get; set; }

        /// <summary>
        /// The countries that are known to Forge
        /// </summary>
        [Property(xmiId: "EAID_dst695154_E9D7_48d8_887D_FAFCDDA173E1", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> Country { get; set; }

        /// <summary>
        /// A humand readable description of this instance of Forge
        /// </summary>
        [Property(xmiId: "EAID_1E50A5E2_E9BC_49f3_87D5_C4D92C84B0FE", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Description { get; set; }

        /// <summary>
        /// The organizations that exist in Forge
        /// </summary>
        [Property(xmiId: "EAID_dst092B28_2C7D_440f_96DA_76C3459D0736", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> Organization { get; set; }

        /// <summary>
        /// The PackageTypes that are known to Forge
        /// </summary>
        [Property(xmiId: "EAID_dstF15AA0_EBC4_4186_B88D_3B78570FFDFA", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> PackageType { get; set; }

        /// <summary>
        /// The ProfileTypes that are known to Forge
        /// </summary>
        [Property(xmiId: "EAID_dst10489B_3E2E_4795_AA63_0F70D1AB3F3A", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> ProfileType { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
