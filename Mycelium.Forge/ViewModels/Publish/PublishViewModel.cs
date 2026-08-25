// ------------------------------------------------------------------------------------------------
// <copyright file="PublishViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Publish
{
    using FluentResults;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Models.Validation;

    /// <summary>
    /// Provides view model state and operations for the Mycelium Forge package publishing wizard page.
    /// </summary>
    public class PublishViewModel : IPublishViewModel
    {
        /// <summary>
        /// Gets or sets the editable package metadata model values.
        /// </summary>
        public PublishPackageMetadataModel Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of wizard workflow steps.
        /// </summary>
        public List<PublishStepModel> Steps { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of pre-publishing validation rule evaluation results.
        /// </summary>
        public List<ValidationCheckModel> ValidationChecks { get; set; } = [];

        /// <summary>
        /// Gets the available scope publisher options.
        /// </summary>
        public IReadOnlyList<string> ScopeOptions { get; } =
        [
            "@starion",
            "@esa",
            "@omg",
            "@rhea",
            "@thales"
        ];

        /// <summary>
        /// Gets the available SPDX license identifier options.
        /// </summary>
        public IReadOnlyList<string> LicenseOptions { get; } =
        [
            "Apache-2.0",
            "MIT",
            "BSD-3-Clause",
            "GPL-3.0",
            "EPL-2.0",
            "Proprietary"
        ];

        /// <summary>
        /// Gets the available package visibility options.
        /// </summary>
        public IReadOnlyList<VisibilityKind> VisibilityOptions { get; } =
        [
            VisibilityKind.PUBLIC,
            VisibilityKind.INTERNAL,
            VisibilityKind.PRIVATE
        ];

        /// <summary>
        /// Gets the available metamodel specification options.
        /// </summary>
        public IReadOnlyList<string> MetamodelOptions { get; } =
        [
            "SysML v2 (2025-02)",
            "SysML v2 (2024-11)",
            "SysML v1.6",
            "CDP4-COMET",
            "Capella 6.1"
        ];

        /// <summary>
        /// Gets or sets a value indicating whether a package publish operation is currently in progress.
        /// </summary>
        public bool IsPublishing { get; set; }

        /// <summary>
        /// Initializes the view model state and populates default publishing form values and validation results.
        /// </summary>
        public void InitializeViewModel()
        {
            this.Metadata = new PublishPackageMetadataModel
            {
                ArtefactFileName = "ECSS-MM-PWR-1.3.0.kpar",
                ArtefactFormat = "SysML v2 (kpar)",
                Scope = "@starion",
                IsScopeVerified = true,
                PackageName = "ECSS-MM-PWR",
                Version = "1.3.0",
                Description = "ECSS mission model: Power subsystem.",
                License = "Apache-2.0",
                Visibility = VisibilityKind.PUBLIC,
                Metamodel = "SysML v2 (2025-02)",
                Tags = "mission-model, power"
            };

            this.Steps =
            [
                new PublishStepModel(1, "Package metadata", true),
                new PublishStepModel(2, "Validation"),
                new PublishStepModel(3, "Review & publish")
            ];

            this.ValidationChecks =
            [
                new ValidationCheckModel("Schema validation"),
                new ValidationCheckModel("Namespace uniqueness"),
                new ValidationCheckModel("Required metadata"),
                new ValidationCheckModel("Dependency resolution"),
                new ValidationCheckModel("License file", status: ValidationStatus.Warning),
                new ValidationCheckModel("README.md", status: ValidationStatus.Missing)
            ];

            this.IsPublishing = false;
        }

        /// <summary>
        /// Initiates the package publishing process.
        /// </summary>
        /// <returns>A <see cref="Result" /> indicating success or failure of the publish operation.</returns>
        public Result Publish()
        {
            this.IsPublishing = true;
            this.IsPublishing = false;
            return Result.Ok();
        }
    }
}
