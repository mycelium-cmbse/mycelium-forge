// ------------------------------------------------------------------------------------------------
// <copyright file="IPublishViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Publish
{
    using ErrorOr;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Models.Validation;

    /// <summary>
    /// Defines the view model contract for the Mycelium Forge package publishing wizard page.
    /// </summary>
    public interface IPublishViewModel
    {
        /// <summary>
        /// Gets the package being published.
        /// </summary>
        IPackage Package { get; }

        /// <summary>
        /// Gets the package version being published.
        /// </summary>
        IPackageVersion PackageVersion { get; }

        /// <summary>
        /// Gets the metadata of the package version being published.
        /// </summary>
        IPackageMetaData PackageMetaData { get; }

        /// <summary>
        /// Gets or sets the scope that owns the package.
        /// </summary>
        IScope SelectedScope { get; set; }

        /// <summary>
        /// Gets a value indicating whether a new version of an already existing package is being published.
        /// </summary>
        bool IsExistingPackage { get; }

        /// <summary>
        /// Gets the expected package format when publishing a new version of an existing package.
        /// </summary>
        SupportedPackageFormat ExpectedFormat { get; }

        /// <summary>
        /// Gets a value indicating whether the current authenticated user has permission to publish a new version of the existing
        /// package.
        /// </summary>
        bool CanUserReleaseExistingPackageVersion { get; }

        /// <summary>
        /// Gets the list of previously published version strings for the existing package.
        /// </summary>
        IReadOnlyList<string> ExistingVersions { get; }

        /// <summary>
        /// Gets or sets the list of pre-publishing validation rule evaluation results.
        /// </summary>
        List<ValidationCheckModel> ValidationChecks { get; set; }

        /// <summary>
        /// Gets the scopes the current account can publish to.
        /// </summary>
        IReadOnlyList<IScope> ScopeOptions { get; }

        /// <summary>
        /// Gets or sets a value indicating whether a package publish operation is currently in progress.
        /// </summary>
        bool IsPublishing { get; set; }

        /// <summary>
        /// Discards the currently staged upload file and frees associated resources.
        /// </summary>
        void DiscardStagedFile();

        /// <summary>
        /// Initializes the view model state and loads the scopes of the current account for creating a new package.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous initialization.</returns>
        Task InitializeViewModel();

        /// <summary>
        /// Initializes the view model state, loads the scopes of the current account, and loads the existing package a new version
        /// is published for.
        /// </summary>
        /// <param name="scopeShortName">The short name of the scope of the existing package.</param>
        /// <param name="packageShortName">The short name of the existing package.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous initialization.</returns>
        Task InitializeViewModel(string scopeShortName, string packageShortName);

        /// <summary>
        /// Reads the manifest of the staged artefact and maps its values onto the package metadata.
        /// </summary>
        /// <param name="stagingUploadToken">The token identifying the staged artefact.</param>
        /// <param name="artifactKind">The kind of the artefact.</param>
        /// <returns>
        /// A <see cref="Task{Boolean}" /> that is <see langword="true" /> when manifest extraction succeeded without
        /// errors; otherwise, <see langword="false" />.
        /// </returns>
        Task<bool> ExtractManifest(string stagingUploadToken, ArtifactKind artifactKind);

        /// <summary>
        /// Evaluates the pre-publishing validation checks of the package being published.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task Validate();

        /// <summary>
        /// Publishes the staged artefact and persists the package, its version, and its metadata.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Success}" /> indicating success or failure of the publish operation.</returns>
        Task<ErrorOr<Success>> Publish(CancellationToken token = default);
    }
}
