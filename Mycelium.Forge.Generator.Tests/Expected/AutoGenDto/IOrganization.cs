// ------------------------------------------------------------------------------------------------
// <copyright file="IOrganization.cs" company="Starion Group S.A.">
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
    /// Represents a group or entity that Accounts can belong to; ie a company, team, or similar collective.
    /// An organization must have at least one member that is its administrator
    /// </summary>
    [Class(xmiId: "EAID_7F7FC764_FFCB_4383_B10D_2D967C351E01", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial interface IOrganization : IScope
    {
        /// <summary>
        /// The accounts that are members of the Organization and that are Organization Administrator
        /// </summary>
        [Property(xmiId: "EAID_dst8A1FCF_E91A_4f88_88B4_62C30EE69428", aggregation: AggregationKind.None, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [SubsettedProperty(propertyName: "EAID_dstB7D7E0_93F9_4e97_A343_767C3B6CBC21")]
        List<Guid> Administrator { get; set; }

        /// <summary>
        /// An optional reference to the binary image data for an Organization's profile picture, a pointer to
        /// where the actual image bytes live in external blob storage
        /// </summary>
        [Property(xmiId: "EAID_E5B41B55_58F3_4126_95AF_CE345EBA0D36", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string LogoBlobReference { get; set; }

        /// <summary>
        /// The accounts that are members of the Organization
        /// </summary>
        [Property(xmiId: "EAID_dstB7D7E0_93F9_4e97_A343_767C3B6CBC21", aggregation: AggregationKind.None, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> Member { get; set; }

        /// <summary>
        /// The unique identifier of the owning Forge.
        /// </summary>
        [Property(xmiId: "EAID_src092B28_2C7D_440f_96DA_76C3459D0736", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Owner { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
