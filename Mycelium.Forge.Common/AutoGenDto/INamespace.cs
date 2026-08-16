// ------------------------------------------------------------------------------------------------
// <copyright file="INamespace.cs" company="Starion Group S.A.">
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
    /// The root abstraction for anything in the system that must be uniquely addressable by name. It
    /// defines two properties, name and shortName. both mandatory and globally unique across every instance
    /// of every subtype, with no exceptions: an Organization, an Account, a Package, and the Forge
    /// singleton itself all draw from the same shared namespace of names, so none of them can ever collide
    /// with each other regardless of type
    /// </summary>
    [Class(xmiId: "EAID_195786BF_3D5E_4b05_B9B1_BBC23C3CB058", isAbstract: true, isFinalSpecialization: false, isActive: false)]
    public partial interface INamespace : IThing
    {
        /// <summary>
        /// A human readable character string in English by which a Namespace instance can be referred to.e.g.
        /// an Organization's full legal or display name, an Account's chosen display name, a Package's full
        /// name, or the Forge's own instance name. Mandatory, and globally unique across every Namespace
        /// subtype.
        /// </summary>
        [Property(xmiId: "EAID_4F9CE1B0_75B2_4f0f_96BE_251F42265A8D", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Name { get; set; }

        /// <summary>
        /// A compact, typically lowercase, URL- and path-safe identifier for a Namespace instance. Used
        /// wherever the full name would be too long or contain characters unsuitable for URLs, CLI commands, or
        /// package references (e.g. spaces, punctuation). Mandatory, and globally unique across every Namespace
        /// subtype, independently of name's uniqueness. Functions as the practical "handle" or "slug" used in
        /// addresses, links, and package coordinates, while name remains the display-facing label
        /// </summary>
        [Property(xmiId: "EAID_89C8F800_EE59_4169_853B_B38E6B1857AC", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string ShortName { get; set; }

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
