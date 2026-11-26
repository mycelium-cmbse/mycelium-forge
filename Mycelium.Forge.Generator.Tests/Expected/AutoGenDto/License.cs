// ------------------------------------------------------------------------------------------------
// <copyright file="License.cs" company="Starion Group S.A.">
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
    /// A software license from the closed set of licenses supported by the Forge, under which
    /// PackageVersions are distributed. Users select from this set and cannot add to it.
    /// </summary>
    [Class(xmiId: "EAID_2D1413C5_3DAF_401b_8394_B797588023F3", isAbstract: false, isFinalSpecialization: false, isActive: false)]
    public partial class License : ILicense, ICloneable<License>
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
        /// The full text of the license that users see and agree to when they consume a PackageVersion
        /// distributed under it.
        /// </summary>
        [Property(xmiId: "EAID_465BBFC5_5A86_4982_8B5D_446465C2EE7D", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: false)]
        [Implements(implementation: "ILicense.LicenseText")]
        public string LicenseText { get; set; }

        /// <summary>
        /// The DateTime at which the Thing was last modified.
        /// </summary>
        [Property(xmiId: "EAID_048B19C9_AA4A_4e41_A4BD_B28426AEC937", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: false)]
        [Implements(implementation: "IThing.ModifiedAt")]
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// A human readable character string in English by which a Namespace instance can be referred to. e.g.
        /// an Organization's full legal or display name, an Account's chosen display name, a Package's full
        /// name, or the Forge's own instance name. Mandatory, and globally unique across every Namespace
        /// subtype.
        /// </summary>
        [Property(xmiId: "EAID_4F9CE1B0_75B2_4f0f_96BE_251F42265A8D", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: false)]
        [Implements(implementation: "INamespace.Name")]
        public string Name { get; set; }

        /// <summary>
        /// A compact, typically lowercase, URL- and path-safe identifier for a Namespace instance. Used
        /// wherever the full name would be too long or contain characters unsuitable for URLs, CLI commands, or
        /// package references (e.g. spaces, punctuation). Mandatory, and globally unique across every Namespace
        /// subtype, independently of name's uniqueness. Functions as the practical "handle" or "slug" used in
        /// addresses, links, and package coordinates, while name remains the display-facing label
        /// </summary>
        [Property(xmiId: "EAID_89C8F800_EE59_4169_853B_B38E6B1857AC", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: false)]
        [RedefinedByProperty("ILicense.SpdxIdentifier")]
        [Implements(implementation: "INamespace.ShortName")]
        string INamespace.ShortName
        {
            get => this.SpdxIdentifier;
            set => this.SpdxIdentifier = value;
        }

        /// <summary>
        /// The SPDX short-form license identifier (e.g. "MIT", "Apache-2.0") from the SPDX License List. It is
        /// unique within the Forge's set of Licenses.
        /// </summary>
        [Property(xmiId: "EAID_940064A6_2555_463b_A120_1FB1EADE4D55", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: false)]
        [RedefinedProperty(propertyName: "EAID_89C8F800_EE59_4169_853B_B38E6B1857AC")]
        [Implements(implementation: "ILicense.SpdxIdentifier")]
        public string SpdxIdentifier { get; set; }

        /// <summary>
        /// Creates a copy of this <see cref="License" /> with all properties copied. List properties are copied shallowly.
        /// </summary>
        /// <returns>A new <see cref="License" /> instance with the same property values.</returns>
        public License Clone()
        {
            return new License
            {
                CreatedAt = this.CreatedAt,
                Id = this.Id,
                LicenseText = this.LicenseText,
                ModifiedAt = this.ModifiedAt,
                Name = this.Name,
                SpdxIdentifier = this.SpdxIdentifier
            };
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
