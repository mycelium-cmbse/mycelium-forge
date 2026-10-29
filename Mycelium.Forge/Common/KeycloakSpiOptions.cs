// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakSpiOptions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Settings of the Forge Keycloak extension that calls back into Forge, bound from the
    /// <see cref="SectionName" /> section of the application settings.
    /// </summary>
    public class KeycloakSpiOptions
    {
        /// <summary>
        /// The name of the configuration section holding the Keycloak extension settings.
        /// </summary>
        public const string SectionName = "KeycloakSpi";

        /// <summary>
        /// The name of the request header carrying the <see cref="Secret" />.
        /// </summary>
        public const string SecretHeaderName = "X-Forge-Secret";

        /// <summary>
        /// Gets or sets the shared secret the Keycloak extension presents to the Forge HTTP API. When empty, the
        /// endpoints that require it are disabled.
        /// </summary>
        public string Secret { get; set; } = string.Empty;
    }
}
