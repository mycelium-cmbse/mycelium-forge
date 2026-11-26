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
    /// A short, free-text label a user can attach to a Package to describe what it relates to, used for
    /// search, filtering, and facet counts. It's normalised (lowercased, spaces→hyphens) and deduplicated
    /// on write.
    /// </summary>
    [Class(xmiId: "EAID_B267D33A_7D03_4de8_BC66_066A0EC0DF2D", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial class Tag : ITag, ICloneable<Tag>
    {
        /// <summary>
        /// The DateTime at which the Thing has been created.
        /// </summary>
        [Property(xmiId: "EAID_C608D12F_75CD_46ed_9AEC_1E65CD83951B", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: false)]
        [Implements(implementation: "IThing.CreatedAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Universally Unique Identifier (UUID) that uniquely identifies an instance of Thing.
        /// </summary>
        [Property(xmiId: "EAID_3A963DC1_6E7A_4925_8686_A68C8799F12E", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: false)]
        [Implements(implementation: "IThing.Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The DateTime at which the Thing was last modified.
        /// </summary>
        [Property(xmiId: "EAID_048B19C9_AA4A_4e41_A4BD_B28426AEC937", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: false)]
        [Implements(implementation: "IThing.ModifiedAt")]
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// the tag's text value, stored as a String. Unique after normalisation; what's displayed and matched
        /// against in search/autocomplete
        /// </summary>
        [Property(xmiId: "EAID_969CFA62_079A_4551_A43D_88F5EA5FF592", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: false)]
        [Implements(implementation: "ITag.Name")]
        public string Name { get; set; }

        /// <summary>
        /// The unique identifier of the owning Forge.
        /// </summary>
        [Property(xmiId: "EAID_src13B896_A2F7_494b_8A14_ABD1D4CD5362", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: true)]
        [Implements(implementation: "ITag.Owner")]
        public Guid Owner { get; set; }

        /// <summary>
        /// Creates a copy of this <see cref="Tag" /> with all properties copied. List properties are copied shallowly.
        /// </summary>
        /// <returns>A new <see cref="Tag" /> instance with the same property values.</returns>
        public Tag Clone()
        {
            return new Tag
            {
                CreatedAt = this.CreatedAt,
                Id = this.Id,
                ModifiedAt = this.ModifiedAt,
                Name = this.Name,
                Owner = this.Owner
            };
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
