// ------------------------------------------------------------------------------------------------
// <copyright file="IProfileType.cs" company="Starion Group S.A.">
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
    /// Identifies a distinct online platform where an Scope can maintain a professional or social presence;
    /// e.g. a professional networking site, a code-hosting platform, a social media site, or an
    /// organization or personal web site.
    /// </summary>
    [Class(xmiId: "EAID_A5813AB3_8596_4a27_9755_A3EC87649CF0", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial interface IProfileType : IThing
    {
        /// <summary>
        /// An optional reference to the binary image data for a ProfileType's logo, a pointer to where the
        /// actual image bytes live in external blob storage.
        /// </summary>
        [Property(xmiId: "EAID_6268AF10_C453_4cd4_9B02_C62B44703FB8", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string LogoBlobReference { get; set; }

        /// <summary>
        /// The name of the profile type
        /// </summary>
        [Property(xmiId: "EAID_F210216A_54DC_4556_A7BC_A5AC6A2608A7", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Name { get; set; }

        /// <summary>
        /// The unique identifier of the owning Forge.
        /// </summary>
        [Property(xmiId: "EAID_src10489B_3E2E_4795_AA63_0F70D1AB3F3A", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        Guid Owner { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
