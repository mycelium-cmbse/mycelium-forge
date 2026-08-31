// ------------------------------------------------------------------------------------------------
// <copyright file="PackageInvitation.cs" company="Starion Group S.A.">
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
    public partial class PackageInvitation : IPackageInvitation
    {
        /// <summary>
        /// The DateTime at which the Thing has been created.
        /// </summary>
        [Property(xmiId: "EAID_C608D12F_75CD_46ed_9AEC_1E65CD83951B", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.CreatedAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The DateTime at which the Invitation expires.
        /// </summary>
        [Property(xmiId: "EAID_E3F82141_2DBE_4300_B7D9_A82A0816AFB2", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IInvitation.ExperisAt")]
        public DateTime ExperisAt { get; set; }

        /// <summary>
        /// Universally Unique Identifier (UUID) that uniquely identifies an instance of Thing.
        /// </summary>
        [Property(xmiId: "EAID_3A963DC1_6E7A_4925_8686_A68C8799F12E", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// A derived Boolean indicating whether an Invitation is past its usable window; true when the current
        /// moment is later than expiresAt. Not a stored state; it's computed on demand from expiresAt alone, so
        /// it's always accurate and never needs to be actively updated or swept by a background process as time
        /// passes.
        /// </summary>
        [Property(xmiId: "EAID_E6B5A8E1_3061_45bd_B6AB_E6F39B0FDE9F", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: true, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IInvitation.IsExpired")]
        public bool isExpired { get; internal set; }

        /// <summary>
        /// The DateTime at which the Thing was last modified.
        /// </summary>
        [Property(xmiId: "EAID_048B19C9_AA4A_4e41_A4BD_B28426AEC937", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.ModifiedAt")]
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// The unique identifier of the owning Account.
        /// </summary>
        [Property(xmiId: "EAID_srcF552D7_FD16_4883_8758_17E13BC36BCC", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackageInvitation.Owner")]
        public Guid Owner { get; set; }

        /// <summary>
        /// The Package this is an invitation for
        /// </summary>
        [Property(xmiId: "EAID_dstDCE16A_F7BE_4be8_85B0_765D2E6348AF", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackageInvitation.Package")]
        public Guid Package { get; set; }

        /// <summary>
        /// Denotes which role the invited Account will receive if they accept.
        /// </summary>
        [Property(xmiId: "EAID_800C2AA2_A2C7_46fd_A407_BF2690393F4F", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackageInvitation.PackageInvitationKind")]
        public PackageInvitationKind PackageInvitationKind { get; set; }

        /// <summary>
        /// The current state of an Invitation in its lifecycle, typed as InvitationStatusKind. Governs whether
        /// the invitation can still be acted on (accepted, declined) or has already been resolved or expired
        /// </summary>
        [Property(xmiId: "EAID_1F5D5916_4806_4a92_A0C3_95CBD1DA1595", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IInvitation.Status")]
        public InvitationStatusKind Status { get; set; }

        /// <summary>
        /// The Account that is being invitited to become an owner or maintainer of the Package
        /// </summary>
        [Property(xmiId: "EAID_dst987416_2AAB_4120_9898_1EEA35FA0EF9", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IPackageInvitation.Target")]
        public Guid Target { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
