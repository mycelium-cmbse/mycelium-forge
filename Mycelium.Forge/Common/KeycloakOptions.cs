// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakOptions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Settings used by the <c>seed</c> command to create the development users in Forge's Keycloak, bound from the
    /// <see cref="SectionName" /> section of the application settings.
    /// </summary>
    public class KeycloakOptions
    {
        /// <summary>
        /// The name of the configuration section holding the Keycloak administration settings.
        /// </summary>
        public const string SectionName = "Keycloak";

        /// <summary>
        /// Gets or sets the base URL of the Keycloak server, as reachable by the seeder.
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the realm the users are created in.
        /// </summary>
        public string Realm { get; set; } = "forge";

        /// <summary>
        /// Gets or sets the username of the Keycloak administrator.
        /// </summary>
        public string AdminUsername { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password of the Keycloak administrator.
        /// </summary>
        public string AdminPassword { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password given to every seeded development user.
        /// </summary>
        public string SeedUserPassword { get; set; } = "admin";
    }
}
