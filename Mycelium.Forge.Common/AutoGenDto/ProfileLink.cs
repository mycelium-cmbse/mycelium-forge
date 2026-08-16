// ------------------------------------------------------------------------------------------------
// <copyright file="ProfileLink.cs" company="Starion Group S.A.">
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
    /// Represents the type of profile and the actual URI pointing to the online profile
    /// </summary>
    [Class(xmiId: "EAID_230199C8_76A7_4a35_8CD6_A3F9EEDAD982", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial class ProfileLink : IProfileLink
    {
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
        /// The type of Profile that is being referenced
        /// </summary>
        [Property(xmiId: "EAID_dst6CA9DA_2B68_4638_A050_F2C1D6256329", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IProfileLink.ProfileType")]
        public Guid ProfileType { get; set; }

        /// <summary>
        /// The actual web address pointing to the Account's profile on the platform identified by the
        /// associated ProfileType
        /// </summary>
        [Property(xmiId: "EAID_D49CC3FC_62EC_404e_9B1E_879EA59D11FD", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IProfileLink.Uri")]
        public string Uri { get; set; }

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
