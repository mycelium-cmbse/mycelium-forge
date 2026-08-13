// ------------------------------------------------------------------------------------------------
// <copyright file="IScope.cs" company="Starion Group S.A.">
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
    /// The common abstraction shared by Account and Organization that makes sure Packages can be stored and
    /// that names and shortnames of scopes and packages owned or contained by the scopes are globally
    /// unique.
    /// </summary>
    [Class(xmiId: "EAID_72DEAA6C_B955_498d_9BBE_A2478380CD29", isAbstract: true, isFinalSpecialization: false, isActive: false)]
    public partial interface IScope : INamespace
    {
        /// <summary>
        /// The addresses that are owned or contained by the Scope
        /// </summary>
        [Property(xmiId: "EAID_dst71ABB8_0FA0_4c8c_BB03_2A1FCAD7F07E", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> Address { get; set; }

        /// <summary>
        /// The email address to which billing information will be sent, such as invoices and reminders
        /// </summary>
        [Property(xmiId: "EAID_73BA90A1_0569_48ac_82E7_27B0421290FB", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string BillingEmail { get; set; }

        /// <summary>
        /// The default visibility of a scope, this can only be set by the administrator(s) or owner of the
        /// Scope
        /// </summary>
        [Property(xmiId: "EAID_32B9DDDF_0FF1_4922_9BFC_58D6204BBF0B", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: "PRIVATE")]
        VisibilityKind DefaultPackageVisibility { get; set; }

        /// <summary>
        /// The email address where the Scope can be reached for anything unrelated to billing.
        /// </summary>
        [Property(xmiId: "EAID_A3370745_185E_4a8b_BF80_3C9CE8B4B7CE", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Email { get; set; }

        /// <summary>
        /// Denotes whether the Scope was created local to this Forge instance, or whether it was proxied from
        /// another location. When the value states  "local", then it is local to the current Forge instance,
        /// otherwise the value needs to be a URI pointing to the location it was proxied from.
        /// </summary>
        [Property(xmiId: "EAID_3A696494_7AB4_4608_95BD_E60428D81D1B", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Origin { get; set; }

        /// <summary>
        /// The packages that are owned by the Scope
        /// </summary>
        [Property(xmiId: "EAID_dstAF0422_E33E_4c34_AA14_D530733FFD0B", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> OwnedPackage { get; set; }

        /// <summary>
        /// The primary address of the Scope
        /// </summary>
        [Property(xmiId: "EAID_dst26755C_2E18_40f0_ABC3_15487E120A45", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        [SubsettedProperty(propertyName: "EAID_dst71ABB8_0FA0_4c8c_BB03_2A1FCAD7F07E")]
        Guid PrimaryAddress { get; set; }

        /// <summary>
        /// The ProfileLinks that are owned by the Scope
        /// </summary>
        [Property(xmiId: "EAID_dst19B56E_EDEF_4ae8_9DFA_BB957BFD237E", aggregation: AggregationKind.Composite, lowerValue: 0, upperValue: int.MaxValue, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        List<Guid> ProfileLink { get; set; }

        /// <summary>
        /// The status of the Scope, when the Scope is an Account, inactive means the user that cannot login
        /// anymore. When the scope is an Organization, packages can no longer be published or uploaded, nor any
        /// PackageVersions
        /// </summary>
        [Property(xmiId: "EAID_98A41878_8269_4533_A0B2_0F3F7989F334", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: "ACTIVE")]
        ScopeStatusKind Status { get; set; }

        /// <summary>
        /// The uri of the website of the scope
        /// </summary>
        [Property(xmiId: "EAID_2F9A52BC_EA1F_4af4_B07C_0FA5005BCF26", aggregation: AggregationKind.None, lowerValue: 0, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null)]
        string Website { get; set; }

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
