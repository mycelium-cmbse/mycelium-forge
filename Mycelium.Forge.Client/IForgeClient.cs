// ------------------------------------------------------------------------------------------------
// <copyright file="IForgeClient.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Client
{
    using FluentResults;

    /// <summary>
    /// Defines the programmatic surface of the Mycelium Forge package registry.
    /// </summary>
    /// <remarks>
    /// SSS-FG-REG-C3M requires a first-party client library, consumable by Mycelium Bloom, by CI/CD
    /// pipelines and by third-party tooling, that wraps every Forge HTTP API endpoint. The operations
    /// below are the seven that requirement enumerates. Signatures are provisional until the request
    /// and response contracts are settled in docs/design.md; the parameter and return types will be
    /// drawn from Mycelium.Forge.Common once those are generated from the Enterprise Architect model.
    /// </remarks>
    public interface IForgeClient
    {
        /// <summary>
        /// Searches the registry for packages matching a free-text query.
        /// </summary>
        Task<Result> SearchAsync(string query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the manifest and metadata of a package without downloading its content.
        /// </summary>
        Task<Result> GetMetadataAsync(string packageIdentifier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the published versions of a package.
        /// </summary>
        Task<Result> ListVersionsAsync(string packageIdentifier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the kpar content of a package version.
        /// </summary>
        Task<Result> DownloadKparAsync(string packageIdentifier, string version, CancellationToken cancellationToken = default);

        /// <summary>
        /// Publishes a kpar as a new package or as a new version of an existing package.
        /// </summary>
        Task<Result> PublishKparAsync(Stream kpar, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unlists a published version, hiding it from search and new-install resolution.
        /// </summary>
        Task<Result> UnlistAsync(string packageIdentifier, string version, CancellationToken cancellationToken = default);

        /// <summary>
        /// Manages the revocable API credentials used to authenticate publishing operations.
        /// </summary>
        Task<Result> ManageApiKeysAsync(CancellationToken cancellationToken = default);
    }
}
