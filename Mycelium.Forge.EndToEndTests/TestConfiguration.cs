// ------------------------------------------------------------------------------------------------
// <copyright file="TestConfiguration.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.EndToEndTests
{
    using System;

    /// <summary>
    /// Resolves the settings the end-to-end suites run against.
    /// </summary>
    /// <remarks>
    /// The end-to-end suites exercise a running Mycelium Forge host over the network rather than an
    /// in-memory test server, so that the Forge HTTP API and the web interface are verified through
    /// the same transport a real client uses. The host must therefore be started before the suites
    /// run; see the repository README for the local and CI workflows.
    /// </remarks>
    public static class TestConfiguration
    {
        /// <summary>
        /// The environment variable that overrides the address of the host under test.
        /// </summary>
        public const string BaseUrlVariable = "FORGE_BASE_URL";

        /// <summary>
        /// The address used when <see cref="BaseUrlVariable"/> is not set, matching the http profile
        /// in Properties/launchSettings.json.
        /// </summary>
        private const string DefaultBaseUrl = "http://localhost:5000";

        /// <summary>
        /// Gets the address of the Mycelium Forge host the suites run against.
        /// </summary>
        public static string BaseUrl =>
            Environment.GetEnvironmentVariable(BaseUrlVariable) is { Length: > 0 } value
                ? value
                : DefaultBaseUrl;
    }
}
