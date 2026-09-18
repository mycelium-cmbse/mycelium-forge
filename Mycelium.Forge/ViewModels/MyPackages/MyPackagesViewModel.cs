// ------------------------------------------------------------------------------------------------
// <copyright file="MyPackagesViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.MyPackages
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Models.Package;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Provides view model state and operations for the My Packages page, listing packages
    /// owned or maintained by the current user across their account and organizations.
    /// </summary>
    public class MyPackagesViewModel : IMyPackagesViewModel
    {
        /// <summary>
        /// The (injected) <see cref="IUserService" /> used to retrieve the current user account.
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyPackagesViewModel" /> class.
        /// </summary>
        /// <param name="userService">The (injected) <see cref="IUserService" />.</param>
        public MyPackagesViewModel(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Gets or sets the collection of packages owned or maintained by the current user.
        /// </summary>
        public List<PackageModel> Packages { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and populates the packages collection.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task InitializeViewModel()
        {
            var currentUser = await this.userService.GetCurrentUser();

            this.Packages = SeedData.Packages
                .Where(p => p.PackageOwner.Contains(currentUser.Id) || p.PackageMaintainer.Contains(currentUser.Id))
                .Select(p =>
                {
                    var model = PackageModel.FromPackage(p);

                    if (p.PackageOwner.Contains(currentUser.Id))
                    {
                        model.Role = PackageInvitationKind.OWNER;
                    }
                    else if (p.PackageMaintainer.Contains(currentUser.Id))
                    {
                        model.Role = PackageInvitationKind.MAINTAINER;
                    }

                    return model;
                })
                .ToList();
        }
    }
}
