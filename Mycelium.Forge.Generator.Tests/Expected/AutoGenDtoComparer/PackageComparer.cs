// ------------------------------------------------------------------------------------------------
// <copyright file="PackageComparer.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.AutoGenDtoComparer
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Mycelium.Forge.Common.Comparers;

    /// <summary>
    /// Compares two instances of <see cref="IPackage"/> and returns the property changes between them.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public partial class PackageComparer : IDtoComparer<IPackage>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="IPackage" /> instance.</param>
        /// <param name="newDto">The updated <see cref="IPackage" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(IPackage oldDto, IPackage newDto)
        {
            var changes = new List<PropertyChange>();

            if (oldDto == null || newDto == null)
            {
                return changes;
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(IPackage.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.downloadCount != newDto.downloadCount)
            {
                changes.Add(new PropertyChange(nameof(IPackage.downloadCount), oldDto.downloadCount, newDto.downloadCount));
            }

            if (oldDto.Listed != newDto.Listed)
            {
                changes.Add(new PropertyChange(nameof(IPackage.Listed), oldDto.Listed, newDto.Listed));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(IPackage.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.Name != newDto.Name)
            {
                changes.Add(new PropertyChange(nameof(IPackage.Name), oldDto.Name, newDto.Name));
            }

            var oldPackageMaintainer = oldDto.PackageMaintainer ?? [];
            var newPackageMaintainer = newDto.PackageMaintainer ?? [];

            if (!oldPackageMaintainer.SequenceEqual(newPackageMaintainer))
            {
                changes.Add(new PropertyChange(nameof(IPackage.PackageMaintainer), oldDto.PackageMaintainer, newDto.PackageMaintainer));
            }

            var oldPackageOwner = oldDto.PackageOwner ?? [];
            var newPackageOwner = newDto.PackageOwner ?? [];

            if (!oldPackageOwner.SequenceEqual(newPackageOwner))
            {
                changes.Add(new PropertyChange(nameof(IPackage.PackageOwner), oldDto.PackageOwner, newDto.PackageOwner));
            }

            if (oldDto.PackageType != newDto.PackageType)
            {
                changes.Add(new PropertyChange(nameof(IPackage.PackageType), oldDto.PackageType, newDto.PackageType));
            }

            if (oldDto.ShortName != newDto.ShortName)
            {
                changes.Add(new PropertyChange(nameof(IPackage.ShortName), oldDto.ShortName, newDto.ShortName));
            }

            var oldVersion = oldDto.Version ?? [];
            var newVersion = newDto.Version ?? [];

            if (!oldVersion.SequenceEqual(newVersion))
            {
                changes.Add(new PropertyChange(nameof(IPackage.Version), oldDto.Version, newDto.Version));
            }

            if (oldDto.Visibility != newDto.Visibility)
            {
                changes.Add(new PropertyChange(nameof(IPackage.Visibility), oldDto.Visibility, newDto.Visibility));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
