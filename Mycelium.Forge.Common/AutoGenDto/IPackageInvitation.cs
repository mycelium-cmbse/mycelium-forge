// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageInvitation.cs" company="Starion Group S.A.">
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
    /// member of the Organization as either maintainer or owner.Only a package owner can create
    /// invitations.
    /// </summary>
    [Class(xmiId: "EAID_78946262_88A9_441c_B65D_B0BD9BB8420A", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial interface IPackageInvitation : IInvitation
    {
        /// <summary>
        /// The unique identifier of the owning Account.
        /// </summary>
        [Property(xmiId: "EAID_srcF552D7_FD16_4883_8758_17E13BC36BCC", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Owner { get; set; }

        /// <summary>
        /// The Package this is an invitation for
        /// </summary>
        [Property(xmiId: "EAID_dstDCE16A_F7BE_4be8_85B0_765D2E6348AF", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Package { get; set; }

        /// <summary>
        /// Denotes which role the invited Account will receive if they accept.
        /// </summary>
        [Property(xmiId: "EAID_800C2AA2_A2C7_46fd_A407_BF2690393F4F", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        PackageInvitationKind PackageInvitationKind { get; set; }

        /// <summary>
        /// The Account that is being invitited to become an owner or maintainer of the Package
        /// </summary>
        [Property(xmiId: "EAID_dst987416_2AAB_4120_9898_1EEA35FA0EF9", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Target { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
