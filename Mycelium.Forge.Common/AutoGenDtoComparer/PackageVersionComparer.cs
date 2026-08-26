// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionComparer.cs" company="Starion Group S.A.">
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
    /// Compares two instances of <see cref="IPackageVersion"/> and returns the property changes between them.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public partial class PackageVersionComparer : IDtoComparer<IPackageVersion>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="IPackageVersion" /> instance.</param>
        /// <param name="newDto">The updated <see cref="IPackageVersion" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(IPackageVersion oldDto, IPackageVersion newDto)
        {
            var changes = new List<PropertyChange>();

            if (oldDto == null || newDto == null)
            {
                return changes;
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.DownloadCount != newDto.DownloadCount)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.DownloadCount), oldDto.DownloadCount, newDto.DownloadCount));
            }

            if (oldDto.Listed != newDto.Listed)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.Listed), oldDto.Listed, newDto.Listed));
            }

            if (oldDto.MetaData != newDto.MetaData)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.MetaData), oldDto.MetaData, newDto.MetaData));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.PublicationDate != newDto.PublicationDate)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.PublicationDate), oldDto.PublicationDate, newDto.PublicationDate));
            }

            if (oldDto.Version != newDto.Version)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.Version), oldDto.Version, newDto.Version));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
