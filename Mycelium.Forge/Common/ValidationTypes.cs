// ------------------------------------------------------------------------------------------------
// <copyright file="ValidationTypes.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Provides constant category names for package publishing validation checks.
    /// </summary>
    public static class ValidationTypes
    {
        /// <summary>
        /// The schema validation check type.
        /// </summary>
        public const string SchemaValidation = "Schema validation";

        /// <summary>
        /// The namespace uniqueness check type.
        /// </summary>
        public const string NamespaceUniqueness = "Namespace uniqueness";

        /// <summary>
        /// The package version check type.
        /// </summary>
        public const string PackageVersion = "Package version";

        /// <summary>
        /// The license check type.
        /// </summary>
        public const string License = "License";
    }
}
