// ------------------------------------------------------------------------------------------------
// <copyright file="IProfileLink.cs" company="Starion Group S.A.">
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
    public partial interface IProfileLink : IThing
    {
        /// <summary>
        /// The type of Profile that is being referenced
        /// </summary>
        [Property(xmiId: "EAID_dst6CA9DA_2B68_4638_A050_F2C1D6256329", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid ProfileType { get; set; }

        /// <summary>
        /// The actual web address pointing to the Account's profile on the platform identified by the
        /// associated ProfileType
        /// </summary>
        [Property(xmiId: "EAID_D49CC3FC_62EC_404e_9B1E_879EA59D11FD", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Uri { get; set; }

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
