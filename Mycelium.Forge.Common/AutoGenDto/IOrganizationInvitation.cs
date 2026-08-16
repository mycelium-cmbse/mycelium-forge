// ------------------------------------------------------------------------------------------------
// <copyright file="IOrganizationInvitation.cs" company="Starion Group S.A.">
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
    /// An invitation owned by an Account sent to another account to request that target account to become a
    /// member of the Organization as either a member or an administrator.Only an administrator can create
    /// invitations.
    /// </summary>
    [Class(xmiId: "EAID_308123F2_0EF0_4cca_B5D2_ED4A358E022B", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial interface IOrganizationInvitation : IInvitation
    {
        /// <summary>
        /// The organization this is an invitation for
        /// </summary>
        [Property(xmiId: "EAID_dstE456B8_2290_4455_80B4_5191AC199252", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Organization { get; set; }

        /// <summary>
        /// Denotes which role the invited Account will receive if they accept.
        /// </summary>
        [Property(xmiId: "EAID_42118148_45A2_4468_9905_CC3DD5C65A6D", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        OrganizationInvitationKind OrganizationInvitationKind { get; set; }

        /// <summary>
        /// The Account that is being invitited to become a member of the Organization
        /// </summary>
        [Property(xmiId: "EAID_dst018175_11F9_4022_84DE_4FA17FC3D3B2", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Target { get; set; }

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
