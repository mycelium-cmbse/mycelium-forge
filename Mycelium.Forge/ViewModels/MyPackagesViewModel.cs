// ------------------------------------------------------------------------------------------------
// <copyright file="MyPackagesViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models;

    /// <summary>
    /// Provides view model state and operations for the My Packages page, listing packages
    /// owned or maintained by the current user across their account and organizations.
    /// </summary>
    public class MyPackagesViewModel : IMyPackagesViewModel
    {
        /// <summary>
        /// The master collection of seed package entries.
        /// </summary>
        private readonly IReadOnlyList<PackageModel> seedPackages =
        [
            new(
                "@starion/ECSS-MM-PWR",
                "/packages/starion/ECSS-MM-PWR",
                string.Empty,
                "SysML v2",
                "@starion",
                "v1.2.0",
                string.Empty,
                "210",
                false,
                VisibilityKind.PUBLIC,
                "2 weeks ago"),
            new(
                "@starion/ECSS-MM-MEC",
                "/packages/starion/ECSS-MM-MEC",
                string.Empty,
                "SysML v2",
                "@starion",
                "v1.2.0",
                string.Empty,
                "180",
                false,
                VisibilityKind.INTERNAL,
                "2 weeks ago"),
            new(
                "@mycelium/ISQ-quantities-units",
                "/packages/mycelium/ISQ-quantities-units",
                string.Empty,
                "SysML v2",
                "@mycelium",
                "v2.5.0",
                string.Empty,
                "1.2k",
                false,
                VisibilityKind.PUBLIC,
                "3 days ago",
                "Maintainer"),
            new(
                "@mycelium/Geometry3D",
                "/packages/mycelium/Geometry3D",
                string.Empty,
                "SysML v2",
                "@mycelium",
                "v1.0.0",
                string.Empty,
                "560",
                false,
                VisibilityKind.PUBLIC,
                "1 month ago",
                "Maintainer"),
            new(
                "@starion/PWR-Study-2026",
                "/packages/starion/PWR-Study-2026",
                string.Empty,
                "SysML v2",
                "@starion",
                "v0.1.0",
                string.Empty,
                "0",
                false,
                VisibilityKind.PRIVATE,
                "just now")
        ];

        /// <summary>
        /// Gets or sets the collection of packages owned or maintained by the current user.
        /// </summary>
        public IReadOnlyList<PackageModel> Packages { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and populates the packages collection.
        /// </summary>
        public void InitializeViewModel()
        {
            this.Packages = this.seedPackages;
        }
    }
}
