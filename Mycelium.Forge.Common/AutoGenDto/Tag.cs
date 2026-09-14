// ------------------------------------------------------------------------------------------------
// <copyright file="Tag.cs" company="Starion Group S.A.">
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
    /// a short, free-text label a user can attach to a project to describe what it relates to, used for
    /// search, filtering, and facet counts. It's normalised (lowercased, spaces→hyphens) and deduplicated
    /// on write, but not curated or restricted to a fixed list
    /// </summary>
    [Class(xmiId: "EAID_B267D33A_7D03_4de8_BC66_066A0EC0DF2D", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial class Tag : ITag
    {
        /// <summary>
        /// Indicates whether the tag is part of the Forge's curated global tag set rather than an ad-hoc,
        /// user-created label.
        /// </summary>
        [Property(xmiId: "EAID_99DCF2DF_5692_4f89_88DD_F3CEB21DA1D5", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "ITag.IsCurated")]
        public int IsCurated { get; set; }

        /// <summary>
        /// the tag's text value, stored as a String. Unique after normalisation; what's displayed and matched
        /// against in search/autocomplete
        /// </summary>
        [Property(xmiId: "EAID_969CFA62_079A_4551_A43D_88F5EA5FF592", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: false, defaultValue: null)]
        [Implements(implementation: "ITag.Name")]
        public string Name { get; set; }

        /// <summary>
        /// The unique identifier of the owning Forge.
        /// </summary>
        [Property(xmiId: "EAID_src13B896_A2F7_494b_8A14_ABD1D4CD5362", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "ITag.Owner")]
        public Guid Owner { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
