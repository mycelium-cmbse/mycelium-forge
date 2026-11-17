// ------------------------------------------------------------------------------------------------
// <copyright file="PackageTargetSummary.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Models
{
    using System;

    /// <summary>
    /// Represents minimal display metadata for a package referenced as an invitation target.
    /// </summary>
    public sealed record PackageTargetSummary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageTargetSummary" /> record from an <see cref="IPackage" /> instance.
        /// </summary>
        /// <param name="package">The source package entity.</param>
        public PackageTargetSummary(IPackage package)
        {
            this.Id = package.Id;
            this.Name = package.Name;
            this.ShortName = package.ShortName;
            this.Owner = package.Owner;
        }

        /// <summary>
        /// Gets the unique identifier of the package.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets the display name of the package.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the short name segment of the package coordinate.
        /// </summary>
        public string ShortName { get; init; }

        /// <summary>
        /// Gets the unique identifier of the owning scope.
        /// </summary>
        public Guid Owner { get; init; }
    }
}
