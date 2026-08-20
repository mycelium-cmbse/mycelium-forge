// ------------------------------------------------------------------------------------------------
// <copyright file="IPublishViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using FluentResults;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models;

    /// <summary>
    /// Defines the view model contract for the Mycelium Forge package publishing wizard page.
    /// </summary>
    public interface IPublishViewModel
    {
        /// <summary>
        /// Gets or sets the editable package metadata model values.
        /// </summary>
        PublishPackageMetadataModel Metadata { get; set; }

        /// <summary>
        /// Gets or sets the list of wizard workflow steps.
        /// </summary>
        List<PublishStepModel> Steps { get; set; }

        /// <summary>
        /// Gets or sets the list of pre-publishing validation rule evaluation results.
        /// </summary>
        List<ValidationCheckModel> ValidationChecks { get; set; }

        /// <summary>
        /// Gets the available scope publisher options.
        /// </summary>
        IReadOnlyList<string> ScopeOptions { get; }

        /// <summary>
        /// Gets the available SPDX license identifier options.
        /// </summary>
        IReadOnlyList<string> LicenseOptions { get; }

        /// <summary>
        /// Gets the available package visibility options.
        /// </summary>
        IReadOnlyList<VisibilityKind> VisibilityOptions { get; }

        /// <summary>
        /// Gets the available metamodel specification options.
        /// </summary>
        IReadOnlyList<string> MetamodelOptions { get; }

        /// <summary>
        /// Gets or sets a value indicating whether a package publish operation is currently in progress.
        /// </summary>
        bool IsPublishing { get; set; }

        /// <summary>
        /// Initializes the view model state and populates default publishing form values and validation results.
        /// </summary>
        void InitializeViewModel();

        /// <summary>
        /// Initiates the package publishing process.
        /// </summary>
        /// <returns>A <see cref="Result" /> indicating success or failure of the publish operation.</returns>
        Result Publish();
    }
}
