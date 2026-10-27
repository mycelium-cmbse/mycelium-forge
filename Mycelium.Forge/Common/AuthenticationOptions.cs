// ------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationOptions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Configuration options for user authentication via OpenID Connect, bound from the
    /// <see cref="SectionName" /> section of the application settings.
    /// </summary>
    public class AuthenticationOptions
    {
        /// <summary>
        /// The name of the configuration section holding the authentication settings.
        /// </summary>
        public const string SectionName = "Authentication";

        /// <summary>
        /// Gets or sets the base address of the identity provider authority.
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Gets or sets the address of the identity provider openid-configuration discovery endpoint.
        /// </summary>
        public string MetadataAddress { get; set; }

        /// <summary>
        /// Gets or sets the client identifier used to authenticate against the identity provider.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret used to authenticate against the identity provider.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HTTPS metadata is required. Defaults to <see langword="true" />.
        /// </summary>
        public bool RequireHttpsMetadata { get; set; } = true;
    }
}
