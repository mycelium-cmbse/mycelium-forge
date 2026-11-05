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
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.ViewModels.Publish;

    /// <summary>
    /// Represents the second step (package metadata) component of the package publishing wizard.
    /// </summary>
    public partial class PublishMetadataStep : ComponentBase
    {
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
        /// Gets or sets the list of individual tags for tag input binding.
        /// </summary>
        /// <remarks>
        /// Temporary adapter between string-based metadata tags and BbTagInput.
        /// TODO: Refactor PublishPackageMetadataModel.Tags to a collection and remove this adapter ASAP.
        /// </remarks>
        public IReadOnlyList<string> TagList
        {
            // TODO: Remove this temporary adapter ASAP once PublishPackageMetadataModel.Tags is refactored.
            get => string.IsNullOrWhiteSpace(this.ViewModel.Metadata.Tags)
                ? []
                : [.. this.ViewModel.Metadata.Tags.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)];
            set => this.ViewModel.Metadata.Tags = value is { Count: > 0 } ? string.Join(", ", value) : string.Empty;
        }

        /// <summary>
        /// Gets a value indicating whether proceeding to validation is disabled due to missing mandatory metadata.
        /// </summary>
        public bool IsContinueDisabled => string.IsNullOrWhiteSpace(this.ViewModel.Metadata.PackageName) ||
                                          string.IsNullOrWhiteSpace(this.ViewModel.Metadata.Version) ||
                                          string.IsNullOrWhiteSpace(this.ViewModel.Metadata.Scope);

        /// <summary>
        /// Handles changes to the package scope selection.
        /// </summary>
        /// <param name="scope">The selected scope value.</param>
        public void OnScopeChanged(string scope)
        {
            this.ViewModel.Metadata.Scope = scope ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the package name input value.
        /// </summary>
        /// <param name="packageName">The new package name value.</param>
        public void OnPackageNameChanged(string packageName)
        {
            this.ViewModel.Metadata.PackageName = packageName ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the package version input value.
        /// </summary>
        /// <param name="version">The new package version value.</param>
        public void OnVersionChanged(string version)
        {
            this.ViewModel.Metadata.Version = version ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the package description input value.
        /// </summary>
        /// <param name="description">The new description value.</param>
        public void OnDescriptionChanged(string description)
        {
            this.ViewModel.Metadata.Description = description ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the package license selection.
        /// </summary>
        /// <param name="license">The selected license identifier.</param>
        public void OnLicenseChanged(string license)
        {
            this.ViewModel.Metadata.License = license ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the package visibility selection.
        /// </summary>
        /// <param name="visibility">The selected visibility setting.</param>
        public void OnVisibilityChanged(VisibilityKind visibility)
        {
            this.ViewModel.Metadata.Visibility = visibility;
        }

        /// <summary>
        /// Handles changes to the package metamodel selection.
        /// </summary>
        /// <param name="metamodel">The selected metamodel specification.</param>
        public void OnMetamodelChanged(string metamodel)
        {
            this.ViewModel.Metadata.Metamodel = metamodel ?? string.Empty;
        }
    }
}
