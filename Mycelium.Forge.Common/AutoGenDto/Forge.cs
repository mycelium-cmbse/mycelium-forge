// ------------------------------------------------------------------------------------------------
// <copyright file="Forge.cs" company="Starion Group S.A.">
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
    /// The single, system-wide root entity that all Organizations and Accounts belong to. It is the top of
    /// the containment hierarchy, representing the entire platform instance itself rather than any one
    /// tenant, group, or user within it. Exactly one Forge exists at any time; there is no valid state in
    /// which zero or multiple Forge instances exist simultaneously
    /// </summary>
    [Class(xmiId: "EAID_17429100_7982_41d3_AD5A_241745EAC4FE", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial class Forge : IForge
    {
        /// <summary>
        /// The accounts that exist in Forge
        /// </summary>
        [Property(xmiId: "EAID_dst2E6A77_9CEE_4ef3_A3FE_ED24C609B3DA", aggregation: AggregationKind.Composite, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IForge.Account")]
        public List<Guid> Account { get; set; } = [];

        /// <summary>
        /// The administators of Forge
        /// </summary>
        [Property(xmiId: "EAID_dstA67767_8BA4_4ed8_A805_BFC11AB57F42", aggregation: AggregationKind.None, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [SubsettedProperty(propertyName: "EAID_dst2E6A77_9CEE_4ef3_A3FE_ED24C609B3DA")]
        [Implements(implementation: "IForge.Administrator")]
        public List<Guid> Administrator { get; set; } = [];

        /// <summary>
        /// The countries that are known to Forge
        /// </summary>
        [Property(xmiId: "EAID_dst695154_E9D7_48d8_887D_FAFCDDA173E1", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IForge.Country")]
        public List<Guid> Country { get; set; } = [];

        /// <summary>
        /// The DateTime at which the Thing has been created.
        /// </summary>
        [Property(xmiId: "EAID_C608D12F_75CD_46ed_9AEC_1E65CD83951B", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.CreatedAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// A humand readable description of this instance of Forge
        /// </summary>
        [Property(xmiId: "EAID_1E50A5E2_E9BC_49f3_87D5_C4D92C84B0FE", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IForge.Description")]
        public string Description { get; set; }

        /// <summary>
        /// Universally Unique Identifier (UUID) that uniquely identifies an instance of Thing.
        /// </summary>
        [Property(xmiId: "EAID_3A963DC1_6E7A_4925_8686_A68C8799F12E", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The DateTime at which the Thing was last modified.
        /// </summary>
        [Property(xmiId: "EAID_048B19C9_AA4A_4e41_A4BD_B28426AEC937", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.ModifiedAt")]
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// A human readable character string in English by which a Namespace instance can be referred to.e.g.
        /// an Organization's full legal or display name, an Account's chosen display name, a Package's full
        /// name, or the Forge's own instance name. Mandatory, and globally unique across every Namespace
        /// subtype.
        /// </summary>
        [Property(xmiId: "EAID_4F9CE1B0_75B2_4f0f_96BE_251F42265A8D", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "INamespace.Name")]
        public string Name { get; set; }

        /// <summary>
        /// The organizations that exist in Forge
        /// </summary>
        [Property(xmiId: "EAID_dst092B28_2C7D_440f_96DA_76C3459D0736", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IForge.Organization")]
        public List<Guid> Organization { get; set; } = [];

        /// <summary>
        /// The PackageTypes that are known to Forge
        /// </summary>
        [Property(xmiId: "EAID_dstF15AA0_EBC4_4186_B88D_3B78570FFDFA", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IForge.PackageType")]
        public List<Guid> PackageType { get; set; } = [];

        /// <summary>
        /// The ProfileTypes that are known to Forge
        /// </summary>
        [Property(xmiId: "EAID_dst10489B_3E2E_4795_AA63_0F70D1AB3F3A", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IForge.ProfileType")]
        public List<Guid> ProfileType { get; set; } = [];

        /// <summary>
        /// A compact, typically lowercase, URL- and path-safe identifier for a Namespace instance. Used
        /// wherever the full name would be too long or contain characters unsuitable for URLs, CLI commands, or
        /// package references (e.g. spaces, punctuation). Mandatory, and globally unique across every Namespace
        /// subtype, independently of name's uniqueness. Functions as the practical "handle" or "slug" used in
        /// addresses, links, and package coordinates, while name remains the display-facing label
        /// </summary>
        [Property(xmiId: "EAID_89C8F800_EE59_4169_853B_B38E6B1857AC", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "INamespace.ShortName")]
        public string ShortName { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
