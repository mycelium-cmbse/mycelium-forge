// ------------------------------------------------------------------------------------------------
// <copyright file="IAccount.cs" company="Starion Group S.A.">
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
    /// Represents a single individual, a person with their own identity in the system, distinct from any
    /// group they belong to. An Account can join one or more Organizations as a member, and may
    /// additionally be granted administrator status on an Organization it belongs to
    /// </summary>
    [Class(xmiId: "EAID_CE60B1AB_3927_4382_B362_016EA94A2C80", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial interface IAccount : IScope
    {
        /// <summary>
        /// The API keys owned or contained by the Account
        /// </summary>
        [Property(xmiId: "EAID_dst9C4D7E_699A_49fc_BE86_9A2E5D20D293", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> ApiKey { get; set; }

        /// <summary>
        /// An optional reference to the binary image data for an Account's profile picture, a pointer to where
        /// the actual image bytes live in external blob storage
        /// </summary>
        [Property(xmiId: "EAID_127F74F6_D9A7_4e84_BE10_1EBE82E92F56", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string AvatarBlobReference { get; set; }

        /// <summary>
        /// The Organization Invitiations initiated and owned by this account
        /// </summary>
        [Property(xmiId: "EAID_dstA7266E_3349_4e94_A354_31A96E27C26C", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> OwnedOrganizationInvitation { get; set; }

        /// <summary>
        /// The PackageInvitation sent by this account
        /// </summary>
        [Property(xmiId: "EAID_dstF552D7_FD16_4883_8758_17E13BC36BCC", aggregation: AggregationKind.Composite, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid OwnedPackageInvitation { get; set; }

        /// <summary>
        /// The unique identifier of the owning Forge.
        /// </summary>
        [Property(xmiId: "EAID_src2E6A77_9CEE_4ef3_A3FE_ED24C609B3DA", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Owner { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
