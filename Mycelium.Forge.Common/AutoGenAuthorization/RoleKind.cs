// ------------------------------------------------------------------------------------------------
// <copyright file="RoleKind.cs" company="Starion Group S.A.">
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
    using System.CodeDom.Compiler;

    /// <summary>
    /// Enumeration of application and domain roles in Mycelium Forge.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public enum RoleKind
    {
        /// <summary>
        /// Super-admin over the entire installation across all accounts and organizations.
        /// </summary>
        InstallationAdministrator,

        /// <summary>
        /// Internal operations team responsible for SaaS infrastructure health and maintenance.
        /// </summary>
        PlatformOperator,

        /// <summary>
        /// Owner and administrator of an Organization tenant.
        /// </summary>
        OrganizationAdministrator,

        /// <summary>
        /// Regular member of an Organization.
        /// </summary>
        OrganizationMember,

        /// <summary>
        /// Owner of a Package artefact container.
        /// </summary>
        PackageOwner,

        /// <summary>
        /// Maintainer of a Package with publishing and unlisting rights.
        /// </summary>
        PackageMaintainer,

        /// <summary>
        /// Explicit read access grant on a restricted package.
        /// </summary>
        PackageReader,

        /// <summary>
        /// Authenticated user account with personal scope and self-service capabilities.
        /// </summary>
        Account,

        /// <summary>
        /// Unauthenticated visitor or crawler.
        /// </summary>
        Anonymous
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
