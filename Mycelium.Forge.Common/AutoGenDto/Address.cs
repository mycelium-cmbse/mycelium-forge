// ------------------------------------------------------------------------------------------------
// <copyright file="Address.cs" company="Starion Group S.A.">
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
    public partial class Address : IAddress
    {
        /// <summary>
        /// The primary address line: street name and house/building number, or the main identifying line for
        /// addresses that don't follow a street-and-number convention. Mandatory � every Address must have at
        /// least this line.
        /// </summary>
        [Property(xmiId: "EAID_F497BB6E_C62C_4751_8AD7_2E9F6AA392A0", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IAddress.AddressLine1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// An optional second line for anything that doesn't fit on the first: apartment or suite number,
        /// floor, building name, or similar. Left empty when not needed.
        /// </summary>
        [Property(xmiId: "EAID_DCE3B0E0_26D6_4045_BC60_741D2288A6A3", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IAddress.AddressLine2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// The country the address is located in
        /// </summary>
        [Property(xmiId: "EAID_dst2EF30B_33AF_40b7_ADBC_745E4144AF15", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IAddress.Country")]
        public Guid Country { get; set; }

        /// <summary>
        /// The DateTime at which the Thing has been created.
        /// </summary>
        [Property(xmiId: "EAID_C608D12F_75CD_46ed_9AEC_1E65CD83951B", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.CreatedAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Universally Unique Identifier (UUID) that uniquely identifies an instance of Thing.
        /// </summary>
        [Property(xmiId: "EAID_3A963DC1_6E7A_4925_8686_A68C8799F12E", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The city, town, or equivalent settlement the address falls within.
        /// </summary>
        [Property(xmiId: "EAID_CB512ED7_5751_4c0e_B20A_9DEC0290ADE5", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IAddress.Locality")]
        public string Locality { get; set; }

        /// <summary>
        /// The DateTime at which the Thing was last modified.
        /// </summary>
        [Property(xmiId: "EAID_048B19C9_AA4A_4e41_A4BD_B28426AEC937", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.ModifiedAt")]
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// The postal or ZIP code. Optional, since some countries don't use postal codes at all.
        /// </summary>
        [Property(xmiId: "EAID_A77832B4_3C0A_4b88_86D5_B5814925CC2E", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IAddress.PostalCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// The state, province, or comparable administrative division. Optional, since many countries don't
        /// subdivide addresses this way.
        /// </summary>
        [Property(xmiId: "EAID_1958E6AE_4997_452b_A3C0_E8B4B04792D0", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IAddress.Region")]
        public string Region { get; set; }

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
