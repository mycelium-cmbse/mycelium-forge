// ------------------------------------------------------------------------------------------------
// <copyright file="IPackagePublishService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using ErrorOr;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Models;
    using Mycelium.Forge.Models.Validation;

    /// <summary>
    /// Coordinates package publishing operations including manifest extraction, domain pre-validation,
    /// artifact object storage persistence, and atomic database persistence with deferred constraints.
    /// </summary>
    public interface IPackagePublishService
    {
        /// <summary>
        /// Reads the manifest of an archive stream, maps its values onto the package entities, and resolves the package type.
        /// </summary>
        /// <param name="userContext">The contextual user information.</param>
        /// <param name="archiveStream">The archive stream containing the package files.</param>
        /// <param name="kind">The kind of artifact being extracted.</param>
        /// <param name="formatName">The format name of the artifact.</param>
        /// <param name="package">The package entity to populate.</param>
        /// <param name="packageVersion">The package version entity to populate.</param>
        /// <param name="packageMetaData">The package metadata entity to populate.</param>
        /// <param name="isExistingPackage">A value indicating whether an existing package is being published.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{IArtifactManifest}" /> containing the extracted manifest or errors.</returns>
        Task<ErrorOr<IArtifactManifest>> ExtractManifestAsync(IUserContext userContext, Stream archiveStream, ArtifactKind kind, string formatName, IPackage package, IPackageVersion packageVersion, IPackageMetaData packageMetaData, bool isExistingPackage, CancellationToken token = default);

        /// <summary>
        /// Evaluates the pre-publishing domain validation checks (schema, namespace uniqueness, semver progression, and license).
        /// </summary>
        /// <param name="userContext">The contextual user information.</param>
        /// <param name="selectedScope">The owning scope of the package.</param>
        /// <param name="package">The package entity being published.</param>
        /// <param name="packageVersion">The package version being published.</param>
        /// <param name="isExistingPackage">A value indicating whether publishing a new version of an existing package.</param>
        /// <param name="existingVersions">The collection of previously published version strings for this package.</param>
        /// <param name="beforeValidationErrors">
        /// Optional pre-validation or extraction errors to evaluate before executing domain
        /// checks.
        /// </param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A list of <see cref="ValidationCheckModel" /> rule evaluation results.</returns>
        Task<IReadOnlyList<ValidationCheckModel>> ValidateReleaseAsync(IUserContext userContext, IScope selectedScope, IPackage package, IPackageVersion packageVersion, bool isExistingPackage, IReadOnlyList<string> existingVersions, IReadOnlyList<Error> beforeValidationErrors = null, CancellationToken token = default);

        /// <summary>
        /// Stores the artifact stream into object storage and atomically persists the package, version, and metadata
        /// in the database within a transaction with deferred constraints. Rolls back object storage on failure.
        /// </summary>
        /// <param name="userContext">The contextual user information.</param>
        /// <param name="artifactStream">The stream of the artifact package file.</param>
        /// <param name="selectedScope">The owning scope of the package.</param>
        /// <param name="package">The package being created or updated.</param>
        /// <param name="packageVersion">The package version to create.</param>
        /// <param name="packageMetaData">The package metadata to create.</param>
        /// <param name="isExistingPackage">A value indicating whether the package record already exists.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating success or failure of the publication.</returns>
        Task<ErrorOr<Success>> PublishReleaseAsync(IUserContext userContext, Stream artifactStream, IScope selectedScope, IPackage package, IPackageVersion packageVersion, IPackageMetaData packageMetaData, bool isExistingPackage, CancellationToken token = default);
    }
}
