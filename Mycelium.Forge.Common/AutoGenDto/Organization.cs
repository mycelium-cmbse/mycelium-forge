// ------------------------------------------------------------------------------------------------
// <copyright file="Organization.cs" company="Starion Group S.A.">
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
    public partial class Organization : IOrganization
    {
        /// <summary>
        /// The addresses that are owned or contained by the Scope
        /// </summary>
        [Property(xmiId: "EAID_dst71ABB8_0FA0_4c8c_BB03_2A1FCAD7F07E", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IScope.Address")]
        public List<Guid> Address { get; set; } = [];

        /// <summary>
        /// The accounts that are members of the Organization and that are Organization Administrator
        /// </summary>
        [Property(xmiId: "EAID_dst8A1FCF_E91A_4f88_88B4_62C30EE69428", aggregation: AggregationKind.None, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [SubsettedProperty(propertyName: "EAID_dstB7D7E0_93F9_4e97_A343_767C3B6CBC21")]
        [Implements(implementation: "IOrganization.Administrator")]
        public List<Guid> Administrator { get; set; } = [];

        /// <summary>
        /// The email address to which billing information will be sent, such as invoices and reminders
        /// </summary>
        [Property(xmiId: "EAID_73BA90A1_0569_48ac_82E7_27B0421290FB", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IScope.BillingEmail")]
        public string BillingEmail { get; set; }

        /// <summary>
        /// The DateTime at which the Thing has been created.
        /// </summary>
        [Property(xmiId: "EAID_C608D12F_75CD_46ed_9AEC_1E65CD83951B", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.CreatedAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The default visibility of a scope, this can only be set by the administrator(s) or owner of the
        /// Scope
        /// </summary>
        [Property(xmiId: "EAID_32B9DDDF_0FF1_4922_9BFC_58D6204BBF0B", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: "PRIVATE")]
        [Implements(implementation: "IScope.DefaultPackageVisibility")]
        public VisibilityKind DefaultPackageVisibility { get; set; } = VisibilityKind.PRIVATE;

        /// <summary>
        /// The email address where the Scope can be reached for anything unrelated to billing.
        /// </summary>
        [Property(xmiId: "EAID_A3370745_185E_4a8b_BF80_3C9CE8B4B7CE", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IScope.Email")]
        public string Email { get; set; }

        /// <summary>
        /// Universally Unique Identifier (UUID) that uniquely identifies an instance of Thing.
        /// </summary>
        [Property(xmiId: "EAID_3A963DC1_6E7A_4925_8686_A68C8799F12E", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IThing.Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// An optional reference to the binary image data for an Organization's profile picture, a pointer to
        /// where the actual image bytes live in external blob storage
        /// </summary>
        [Property(xmiId: "EAID_E5B41B55_58F3_4126_95AF_CE345EBA0D36", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IOrganization.LogoBlobReference")]
        public string LogoBlobReference { get; set; }

        /// <summary>
        /// The accounts that are members of the Organization
        /// </summary>
        [Property(xmiId: "EAID_dstB7D7E0_93F9_4e97_A343_767C3B6CBC21", aggregation: AggregationKind.None, lowerValue: 1, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IOrganization.Member")]
        public List<Guid> Member { get; set; } = [];

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
        /// Denotes whether the Scope was created local to this Forge instance, or whether it was proxied from
        /// another location. When the value states  "local", then it is local to the current Forge instance,
        /// otherwise the value needs to be a URI pointing to the location it was proxied from.
        /// </summary>
        [Property(xmiId: "EAID_3A696494_7AB4_4608_95BD_E60428D81D1B", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IScope.Origin")]
        public string Origin { get; set; }

        /// <summary>
        /// The packages that are owned by the Scope
        /// </summary>
        [Property(xmiId: "EAID_dstAF0422_E33E_4c34_AA14_D530733FFD0B", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IScope.OwnedPackage")]
        public List<Guid> OwnedPackage { get; set; } = [];

        /// <summary>
        /// The primary address of the Scope
        /// </summary>
        [Property(xmiId: "EAID_dst26755C_2E18_40f0_ABC3_15487E120A45", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [SubsettedProperty(propertyName: "EAID_dst71ABB8_0FA0_4c8c_BB03_2A1FCAD7F07E")]
        [Implements(implementation: "IScope.PrimaryAddress")]
        public Guid PrimaryAddress { get; set; }

        /// <summary>
        /// The ProfileLinks that are owned by the Scope
        /// </summary>
        [Property(xmiId: "EAID_dst19B56E_EDEF_4ae8_9DFA_BB957BFD237E", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IScope.ProfileLink")]
        public List<Guid> ProfileLink { get; set; } = [];

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

        /// <summary>
        /// The status of the Scope, when the Scope is an Account, inactive means the user that cannot login
        /// anymore. When the scope is an Organization, packages can no longer be published or uploaded, nor any
        /// PackageVersions
        /// </summary>
        [Property(xmiId: "EAID_98A41878_8269_4533_A0B2_0F3F7989F334", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: "ACTIVE")]
        [Implements(implementation: "IScope.Status")]
        public ScopeStatusKind Status { get; set; } = ScopeStatusKind.ACTIVE;

        /// <summary>
        /// The uri of the website of the scope
        /// </summary>
        [Property(xmiId: "EAID_2F9A52BC_EA1F_4af4_B07C_0FA5005BCF26", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [Implements(implementation: "IScope.Website")]
        public string Website { get; set; }

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
