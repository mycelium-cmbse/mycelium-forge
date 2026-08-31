// ------------------------------------------------------------------------------------------------
// <copyright file="IAddress.cs" company="Starion Group S.A.">
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
    /// Represents a physical or postal address.
    /// </summary>
    [Class(xmiId: "EAID_5E20ADE8_09B7_4fdd_993D_A80F2328C5AE", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial interface IAddress : IThing
    {
        /// <summary>
        /// The primary address line: street name and house/building number, or the main identifying line for
        /// addresses that don't follow a street-and-number convention. Mandatory � every Address must have at
        /// least this line.
        /// </summary>
        [Property(xmiId: "EAID_F497BB6E_C62C_4751_8AD7_2E9F6AA392A0", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string AddressLine1 { get; set; }

        /// <summary>
        /// An optional second line for anything that doesn't fit on the first: apartment or suite number,
        /// floor, building name, or similar. Left empty when not needed.
        /// </summary>
        [Property(xmiId: "EAID_DCE3B0E0_26D6_4045_BC60_741D2288A6A3", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string AddressLine2 { get; set; }

        /// <summary>
        /// The country the address is located in
        /// </summary>
        [Property(xmiId: "EAID_dst2EF30B_33AF_40b7_ADBC_745E4144AF15", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Country { get; set; }

        /// <summary>
        /// The city, town, or equivalent settlement the address falls within.
        /// </summary>
        [Property(xmiId: "EAID_CB512ED7_5751_4c0e_B20A_9DEC0290ADE5", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Locality { get; set; }

        /// <summary>
        /// The unique identifier of the owning Scope.
        /// </summary>
        [Property(xmiId: "EAID_src71ABB8_0FA0_4c8c_BB03_2A1FCAD7F07E", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Owner { get; set; }

        /// <summary>
        /// The postal or ZIP code. Optional, since some countries don't use postal codes at all.
        /// </summary>
        [Property(xmiId: "EAID_A77832B4_3C0A_4b88_86D5_B5814925CC2E", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string PostalCode { get; set; }

        /// <summary>
        /// The state, province, or comparable administrative division. Optional, since many countries don't
        /// subdivide addresses this way.
        /// </summary>
        [Property(xmiId: "EAID_1958E6AE_4997_452b_A3C0_E8B4B04792D0", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Region { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
