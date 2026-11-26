// ------------------------------------------------------------------------------------------------
// <copyright file="ITag.cs" company="Starion Group S.A.">
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
    /// A short, free-text label a user can attach to a Package to describe what it relates to, used for
    /// search, filtering, and facet counts. It's normalised (lowercased, spaces→hyphens) and deduplicated
    /// on write.
    /// </summary>
    [Class(xmiId: "EAID_B267D33A_7D03_4de8_BC66_066A0EC0DF2D", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial interface ITag : IThing
    {
        /// <summary>
        /// the tag's text value, stored as a String. Unique after normalisation; what's displayed and matched
        /// against in search/autocomplete
        /// </summary>
        [Property(xmiId: "EAID_969CFA62_079A_4551_A43D_88F5EA5FF592", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: false)]
        string Name { get; set; }

        /// <summary>
        /// The unique identifier of the owning Forge.
        /// </summary>
        [Property(xmiId: "EAID_src13B896_A2F7_494b_8A14_ABD1D4CD5362", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: true)]
        Guid Owner { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
