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
    using System.Linq;

    using Mycelium.Forge.Common.Comparers;

    /// <summary>
    /// Compares two instances of <see cref="IPackageVersion"/> and returns the property changes between them.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.6.0")]
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

            var oldApplicableLicenses = oldDto.ApplicableLicenses ?? [];
            var newApplicableLicenses = newDto.ApplicableLicenses ?? [];

            if (!oldApplicableLicenses.SequenceEqual(newApplicableLicenses))
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.ApplicableLicenses), oldDto.ApplicableLicenses, newDto.ApplicableLicenses));
            }

            if (oldDto.CheckSum != newDto.CheckSum)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.CheckSum), oldDto.CheckSum, newDto.CheckSum));
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.DownloadCount != newDto.DownloadCount)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.DownloadCount), oldDto.DownloadCount, newDto.DownloadCount));
            }

            if (oldDto.FileExtension != newDto.FileExtension)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.FileExtension), oldDto.FileExtension, newDto.FileExtension));
            }

            if (oldDto.Listed != newDto.Listed)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.Listed), oldDto.Listed, newDto.Listed));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.Owner != newDto.Owner)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.Owner), oldDto.Owner, newDto.Owner));
            }

            if (oldDto.PublicationDate != newDto.PublicationDate)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.PublicationDate), oldDto.PublicationDate, newDto.PublicationDate));
            }

            if (oldDto.Readme != newDto.Readme)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.Readme), oldDto.Readme, newDto.Readme));
            }

            if (oldDto.ReleaseNotes != newDto.ReleaseNotes)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.ReleaseNotes), oldDto.ReleaseNotes, newDto.ReleaseNotes));
            }

            if (oldDto.SizeInBytes != newDto.SizeInBytes)
            {
                changes.Add(new PropertyChange(nameof(IPackageVersion.SizeInBytes), oldDto.SizeInBytes, newDto.SizeInBytes));
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
