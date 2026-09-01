// ------------------------------------------------------------------------------------------------
// <copyright file="IAPIKey.cs" company="Starion Group S.A.">
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
    /// Represents a programmatic credential that lets an Account authenticate against the Forge without a
    /// human interactively signing in. The mechanism a CI/CD pipeline or automated tool uses to publish or
    /// download Packages on that Account's behalf.
    /// </summary>
    [Class(xmiId: "EAID_795DCA68_B91A_4f22_80F4_8AB8C8705FAE", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial interface IAPIKey : IThing
    {
        /// <summary>
        /// The DateTime at which the API key expires.
        /// </summary>
        [Property(xmiId: "EAID_2D476240_E031_4fa4_B4D1_2EAF94CB7DA5", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        DateTime ExpiresAt { get; set; }

        /// <summary>
        /// The DateTime when the API key was last used.
        /// </summary>
        [Property(xmiId: "EAID_0843C514_04A0_446e_8209_4945B9C546D3", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        DateTime LastUsedAt { get; set; }

        /// <summary>
        /// a human readable name that makes it easy to identify
        /// </summary>
        [Property(xmiId: "EAID_C2F6FF93_7B10_437a_8A7B_DDCD5F120E5F", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Name { get; set; }

        /// <summary>
        /// The unique identifier of the owning Account.
        /// </summary>
        [Property(xmiId: "EAID_src9C4D7E_699A_49fc_BE86_9A2E5D20D293", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Owner { get; set; }

        /// <summary>
        /// the set of permissions that define what can be done with the API key on behalf of the Account that
        /// owns the API key.
        /// </summary>
        [Property(xmiId: "EAID_011DC793_E760_475c_B930_D338B5356217", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Permissions { get; set; }

        /// <summary>
        /// The DateTime when the API key was revoked.
        /// </summary>
        [Property(xmiId: "EAID_E8AAF47E_58DB_42e3_901D_97F7677A9793", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        DateTime RevokedAt { get; set; }

        /// <summary>
        /// </summary>
        [Property(xmiId: "EAID_796BF175_5F57_4cf9_A60D_AA7936A12A6C", aggregation: AggregationKind.None, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<byte> SecretHash { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
