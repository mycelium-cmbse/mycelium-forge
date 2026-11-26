// ------------------------------------------------------------------------------------------------
// <copyright file="ILicense.cs" company="Starion Group S.A.">
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
    public partial interface ILicense : INamespace
    {
        /// <summary>
        /// The full text of the license that users see and agree to when they consume a PackageVersion
        /// distributed under it.
        /// </summary>
        [Property(xmiId: "EAID_465BBFC5_5A86_4982_8B5D_446465C2EE7D", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: false)]
        string LicenseText { get; set; }

        /// <summary>
        /// The SPDX short-form license identifier (e.g. "MIT", "Apache-2.0") from the SPDX License List. It is
        /// unique within the Forge's set of Licenses.
        /// </summary>
        [Property(xmiId: "EAID_940064A6_2555_463b_A120_1FB1EADE4D55", aggregation: AggregationKind.None, lowerValue: 1, upperValue: 1, isOrdered: false, isReadOnly: false, isDerived: false, isDerivedUnion: false, isUnique: true, defaultValue: null, isOwnerEnd: false)]
        [RedefinedProperty(propertyName: "EAID_89C8F800_EE59_4169_853B_B38E6B1857AC")]
        string SpdxIdentifier { get; set; }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
