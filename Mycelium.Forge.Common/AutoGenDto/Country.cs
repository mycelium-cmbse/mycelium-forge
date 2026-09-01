// ------------------------------------------------------------------------------------------------
// <copyright file="Country.cs" company="Starion Group S.A.">
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
    /// A reference-data record representing one of the world's recognized countries, identified by its
    /// official ISO 3166-1 codes.
    /// </summary>
    [Class(xmiId: "EAID_A94A65F7_045C_49d9_9307_1DBFF810C3F4", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial class Country : ICountry
    {
        /// <summary>
        /// The two-letter ISO 3166-1 alpha-2 code (e.g. "US", "NL"). Serves as the natural key � globally
        /// unique, and the form most commonly used as a foreign-key reference.
        /// </summary>
        [Property(xmiId: "EAID_66754ACF_D3E7_4105_B193_C558BF782058", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "ICountry.Alpha2Code")]
        public string Alpha2Code { get; set; }

        /// <summary>
        /// The three-letter ISO 3166-1 alpha-3 code (e.g. "USA", "NLD"). A less compact but sometimes more
        /// readable alternative to alpha2, used in contexts (like some ISO and shipping standards) that prefer
        /// it.
        /// </summary>
        [Property(xmiId: "EAID_A43279BE_D2FF_46df_886C_F3AA4A6DDAF5", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "ICountry.Alpha3Code")]
        public string Alpha3Code { get; set; }

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
        /// The DateTime at which the Thing was last modified.
        /// </summary>
        [Property(xmiId: "EAID_048B19C9_AA4A_4e41_A4BD_B28426AEC937", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.ModifiedAt")]
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// The country's full official name (e.g. "The Neterlands"), used for display purposes.
        /// </summary>
        [Property(xmiId: "EAID_1C566C14_B1D0_4046_A2B0_78434ACD1F62", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "ICountry.Name")]
        public string Name { get; set; }

        /// <summary>
        /// The three-digit ISO 3166-1 numeric code (e.g. "840" for the United States), stored as a zero-padded
        /// string rather than an integer so leading zeros (like "032" for Argentina) aren't silently dropped.
        /// Useful for interoperating with older or numeric-only systems.
        /// </summary>
        [Property(xmiId: "EAID_1F1F6D7B_7983_497f_A3EF_DC0B785AA91C", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "ICountry.NumericCode")]
        public string NumericCode { get; set; }

        /// <summary>
        /// The unique identifier of the owning Forge.
        /// </summary>
        [Property(xmiId: "EAID_src695154_E9D7_48d8_887D_FAFCDDA173E1", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "ICountry.Owner")]
        public Guid Owner { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
