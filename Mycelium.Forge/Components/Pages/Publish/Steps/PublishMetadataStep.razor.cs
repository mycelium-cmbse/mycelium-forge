// ------------------------------------------------------------------------------------------------
// <copyright file="PublishMetadataStep.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Publish.Steps
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.AutoGenDtoValidator;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Extractors;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.Publish;

    /// <summary>
    /// Represents the second step (package metadata) component of the package publishing wizard.
    /// </summary>
    public partial class PublishMetadataStep : ComponentBase
    {
        /// <summary>
        /// The available SPDX license identifier options.
        /// </summary>
        private static readonly IReadOnlyList<string> LicenseOptions =
        [
            "Apache-2.0",
            "MIT",
            "BSD-3-Clause",
            "GPL-3.0",
            "EPL-2.0",
            "Proprietary"
        ];

        /// <summary>
        /// The available package visibility options.
        /// </summary>
        private static readonly IReadOnlyList<VisibilityKind> VisibilityOptions = Enum.GetValues<VisibilityKind>();

        /// <summary>
        /// The guidance information card configuration for the package metadata step.
        /// </summary>
        private static readonly InformationCardModel StepInformationCard = new()
        {
            Title = "NEXT: VALIDATION",
            Items =
            [
                "Forge checks the manifest, the version and your permission on the scope.",
                "Dependencies, licence and README are checked too.",
                "Nothing is published yet. You confirm on the last step."
            ],
            HelpText = "Blocking checks stop the release. Warnings never do.",
            LinkText = "Read the publishing guide \u2192",
            LinkHref = "/docs/publishing"
        };

        /// <summary>
        /// Gets or sets the view model for the package publishing page.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public IPublishViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the staged artefact UI model.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public PublishArtefactModel Artefact { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when returning to the previous step.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public EventCallback OnBack { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when proceeding to the validation step.
        /// </summary>
        [Parameter]
        public EventCallback OnContinue { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when replacing the uploaded artefact.
        /// </summary>
        [Parameter]
        public EventCallback OnReplaceArtefact { get; set; }

        /// <summary>
        /// Gets the validation manager handling field validation states for the metadata form.
        /// </summary>
        public ValidationManager ValidationManager { get; } = new();

        /// <summary>
        /// Gets a value indicating whether the staged artefact is a Kpar archive whose metadata is declared by the manifest.
        /// </summary>
        public bool IsKpar => this.Artefact.Kind == ArtifactKind.Kpar;

        /// <summary>
        /// Gets a value indicating whether the package name field is disabled.
        /// </summary>
        public bool IsPackageNameDisabled => this.IsKpar;

        /// <summary>
        /// Gets a value indicating whether the selected scope is verified.
        /// </summary>
        public bool IsSelectedScopeVerified => this.ViewModel.SelectedScope?.IsVerified() ?? false;

        /// <summary>
        /// Gets a value indicating whether the selected scope is an organization.
        /// </summary>
        public bool IsScopeOrganization => this.ViewModel.SelectedScope is IOrganization;

        /// <summary>
        /// Gets the formatted `@scope/packageName` coordinate string for display.
        /// </summary>
        /// <returns>The formatted package coordinate string.</returns>
        public string GetScopePackageCoordinate()
        {
            return $"{this.ViewModel.SelectedScope.GetDisplayShortName()}/{this.ViewModel.Package.ShortName}";
        }

        /// <summary>
        /// Determines whether a visibility option is disabled for the currently selected scope.
        /// </summary>
        /// <param name="visibility">The visibility option to evaluate.</param>
        /// <returns><see langword="true" /> if the option is disabled; otherwise, <see langword="false" />.</returns>
        public bool IsVisibilityItemDisabled(VisibilityKind visibility)
        {
            return visibility == VisibilityKind.INTERNAL && !this.IsScopeOrganization;
        }

        /// <summary>
        /// Handles advancing to the validation step by validating all mandatory metadata fields first.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task HandleContinue()
        {
            if (this.ViewModel.IsExistingPackage)
            {
                var isExistingVersionValid = !string.IsNullOrWhiteSpace(this.ViewModel.PackageVersion.Version) &&
                                             KparManifestExtractor.SemanticVersionRegex().IsMatch(this.ViewModel.PackageVersion.Version);

                var isExistingValid = this.ValidationManager
                    .Check(nameof(IPackageVersion.Version), isExistingVersionValid, "Version must follow SemVer 2.0.0 format (e.g. 1.0.0).")
                    .IsValid;

                if (!isExistingValid)
                {
                    return;
                }

                await this.OnContinue.InvokeAsync();
                return;
            }

            var isShortNameValid = !string.IsNullOrWhiteSpace(this.ViewModel.Package.ShortName) &&
                                   PackageValidator.ShortNameFormatRegex().IsMatch(this.ViewModel.Package.ShortName);

            var isVersionValid = !string.IsNullOrWhiteSpace(this.ViewModel.PackageVersion.Version) &&
                                 KparManifestExtractor.SemanticVersionRegex().IsMatch(this.ViewModel.PackageVersion.Version);

            var isValid = this.ValidationManager
                .Check(nameof(IScope), this.ViewModel.SelectedScope != null, "Scope selection is required.")
                .Check(nameof(IPackage.Name), !string.IsNullOrWhiteSpace(this.ViewModel.Package.Name), "Package name is required.")
                .Check(nameof(IPackage.ShortName), isShortNameValid, "Short name must start and end with an alphanumeric character and contain only lowercase alphanumeric characters and hyphens.")
                .Check(nameof(IPackageVersion.Version), isVersionValid, "Version must follow SemVer 2.0.0 format (e.g. 1.0.0).")
                .Check(nameof(IPackage.Description), !string.IsNullOrWhiteSpace(this.ViewModel.Package.Description), "Description is required.")
                .IsValid;

            if (!isValid)
            {
                return;
            }

            await this.OnContinue.InvokeAsync();
        }

        /// <summary>
        /// Handles changes to the package scope selection.
        /// </summary>
        /// <param name="scope">The selected scope value.</param>
        public void OnScopeChanged(IScope scope)
        {
            this.ViewModel.SelectedScope = scope;
            this.ValidationManager.ClearError(nameof(IScope));

            if (!this.ViewModel.IsExistingPackage && scope != null)
            {
                this.ViewModel.Package.Visibility = scope.DefaultPackageVisibility;
                this.ViewModel.Package.IsVerified = scope.IsVerified();
            }
        }

        /// <summary>
        /// Handles changes to the package short name input value.
        /// </summary>
        /// <param name="shortName">The new package short name value.</param>
        public void OnPackageShortNameChanged(string shortName)
        {
            this.ViewModel.Package.ShortName = shortName;
            this.ValidationManager.ClearError(nameof(IPackage.ShortName));
        }

        /// <summary>
        /// Handles changes to the package name input value.
        /// </summary>
        /// <param name="packageName">The new package name value.</param>
        public void OnPackageNameChanged(string packageName)
        {
            this.ViewModel.Package.Name = packageName;
            this.ValidationManager.ClearError(nameof(IPackage.Name));
        }

        /// <summary>
        /// Gets the description and format guidance for the version field.
        /// </summary>
        /// <returns>The version field description string.</returns>
        private string GetVersionDescription()
        {
            return this.ViewModel.IsExistingPackage && this.ViewModel.ExistingVersions is { Count: > 0 }
                ? $"SemVer 2.0.0 format. Latest published version is {this.ViewModel.ExistingVersions[0]}"
                : "SemVer 2.0.0 format (e.g. 1.0.0).";
        }

        /// <summary>
        /// Handles changes to the package version input value.
        /// </summary>
        /// <param name="version">The new package version value.</param>
        public void OnVersionChanged(string version)
        {
            this.ViewModel.PackageVersion.Version = version;
            this.ValidationManager.ClearError(nameof(IPackageVersion.Version));
            this.ValidationManager.ClearError(nameof(IPackageVersion.ReleaseNotes));
        }

        /// <summary>
        /// Handles changes to the package release notes input value.
        /// </summary>
        /// <param name="releaseNotes">The new release notes value.</param>
        public void OnReleaseNotesChanged(string releaseNotes)
        {
            this.ViewModel.PackageVersion.ReleaseNotes = releaseNotes;
            this.ValidationManager.ClearError(nameof(IPackageVersion.ReleaseNotes));
        }

        /// <summary>
        /// Handles changes to the package description input value.
        /// </summary>
        /// <param name="description">The new description value.</param>
        public void OnDescriptionChanged(string description)
        {
            this.ViewModel.Package.Description = description;
            this.ValidationManager.ClearError(nameof(IPackage.Description));
        }

        /// <summary>
        /// Handles changes to the package license selection.
        /// </summary>
        /// <param name="license">The selected license identifier.</param>
        public void OnLicenseChanged(string license)
        {
            this.ViewModel.Package.License = license;
        }

        /// <summary>
        /// Handles changes to the package visibility selection.
        /// </summary>
        /// <param name="visibility">The selected visibility setting.</param>
        public void OnVisibilityChanged(VisibilityKind visibility)
        {
            this.ViewModel.Package.Visibility = visibility;
        }

        /// <summary>
        /// Handles changes to the package tags collection.
        /// </summary>
        /// <param name="tags">The updated list of tags.</param>
        public void OnTagsChanged(IReadOnlyList<string> tags)
        {
            this.ViewModel.Package.Tags = [.. tags ?? []];
        }
    }
}
