// ------------------------------------------------------------------------------------------------
// <copyright file="IInvitation.cs" company="Starion Group S.A.">
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
    /// Abstract super class from which all kinds of Invitations derive.
    /// </summary>
    [Class(xmiId: "EAID_95D42105_4640_4742_8548_6498D87AB908", isAbstract: true, isFinalSpecialization: false, isActive: false)]
    public partial interface IInvitation : IThing
    {
        /// <summary>
        /// The DateTime at which the Invitation expires.
        /// </summary>
        [Property(xmiId: "EAID_E3F82141_2DBE_4300_B7D9_A82A0816AFB2", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        DateTime ExperisAt { get; set; }

        /// <summary>
        /// A derived Boolean indicating whether an Invitation is past its usable window; true when the current
        /// moment is later than expiresAt. Not a stored state; it's computed on demand from expiresAt alone, so
        /// it's always accurate and never needs to be actively updated or swept by a background process as time
        /// passes.
        /// </summary>
        [Property(xmiId: "EAID_E6B5A8E1_3061_45bd_B6AB_E6F39B0FDE9F", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: true, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        bool isExpired { get; }

        /// <summary>
        /// The current state of an Invitation in its lifecycle, typed as InvitationStatusKind. Governs whether
        /// the invitation can still be acted on (accepted, declined) or has already been resolved or expired
        /// </summary>
        [Property(xmiId: "EAID_1F5D5916_4806_4a92_A0C3_95CBD1DA1595", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        InvitationStatusKind Status { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
