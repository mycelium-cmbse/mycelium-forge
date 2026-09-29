// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDetailsActionsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.PackageDetails
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Model;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Provides view model state and action execution logic for interactive package details actions.
    /// </summary>
    public class PackageDetailsActionsViewModel : IPackageDetailsActionsViewModel
    {
        /// <summary>
        /// The (injected) <see cref="INotificationService" /> used to emit notifications.
        /// </summary>
        private readonly INotificationService notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageDetailsActionsViewModel" /> class.
        /// </summary>
        /// <param name="notificationService">The (injected) <see cref="INotificationService" />.</param>
        public PackageDetailsActionsViewModel(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        /// <summary>
        /// Gets or sets the underlying package DTO.
        /// </summary>
        public IPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the owning scope DTO.
        /// </summary>
        public IScope Owner { get; set; }

        /// <summary>
        /// Gets or sets the selected package version DTO.
        /// </summary>
        public IPackageVersion SelectedVersion { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of install commands.
        /// </summary>
        public IReadOnlyDictionary<string, string> InstallCommands { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets a value indicating whether the current user is an administrator of the package.
        /// </summary>
        public bool IsUserAdmin { get; set; } = true;

        /// <summary>
        /// Initializes the package actions view model state with provided package details.
        /// </summary>
        /// <param name="package">The package instance.</param>
        /// <param name="owner">The owning scope instance.</param>
        /// <param name="selectedVersion">The currently selected package version.</param>
        /// <param name="isUserAdmin">A value indicating whether the current user is an administrator.</param>
        public void Initialize(IPackage package, IScope owner, IPackageVersion selectedVersion, bool isUserAdmin)
        {
            this.Package = package;
            this.Owner = owner;
            this.SelectedVersion = selectedVersion;
            this.IsUserAdmin = isUserAdmin;

            var publisher = this.Owner.ShortName;
            var currentVersion = this.SelectedVersion.GetVersion();

            this.InstallCommands = InstallCommandHelper.GenerateInstallCommands(publisher, this.Package.ShortName, currentVersion);
        }

        /// <summary>
        /// Initiates a migration of the package in Bloom to the specified target project and emits a user notification.
        /// </summary>
        /// <param name="result">The migration parameters including destination project and version constraint.</param>
        public void MigrateInBloom(MigrateInBloomResult result)
        {
            this.notificationService.AddNotification(
                $"Migration of package '{this.Package.Name}' to project '{result.ProjectName}' has been initiated in Bloom.",
                "Migration Initiated",
                NotificationType.Success);
        }
    }
}
