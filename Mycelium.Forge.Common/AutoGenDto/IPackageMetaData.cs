// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageMetaData.cs" company="Starion Group S.A.">
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
    /// </summary>
    [Class(xmiId: "EAID_5C5C60CB_80CD_4aef_9FE8_BC1E4803B3F1", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial interface IPackageMetaData : IThing
    {
        /// <summary>
        /// The unique identifier of the owning PackageVersion.
        /// </summary>
        [Property(xmiId: "EAID_src94AFDF_F62B_4600_9B44_6076F54197E6", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Owner { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
