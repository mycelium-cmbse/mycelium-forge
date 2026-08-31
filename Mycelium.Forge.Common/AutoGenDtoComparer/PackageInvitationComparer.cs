// ------------------------------------------------------------------------------------------------
// <copyright file="PackageInvitationComparer.cs" company="Starion Group S.A.">
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
    /// Compares two instances of <see cref="IPackageInvitation"/> and returns the property changes between them.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public partial class PackageInvitationComparer : IDtoComparer<IPackageInvitation>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="IPackageInvitation" /> instance.</param>
        /// <param name="newDto">The updated <see cref="IPackageInvitation" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(IPackageInvitation oldDto, IPackageInvitation newDto)
        {
            var changes = new List<PropertyChange>();

            if (oldDto == null || newDto == null)
            {
                return changes;
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(IPackageInvitation.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.ExperisAt != newDto.ExperisAt)
            {
                changes.Add(new PropertyChange(nameof(IPackageInvitation.ExperisAt), oldDto.ExperisAt, newDto.ExperisAt));
            }

            if (oldDto.isExpired != newDto.isExpired)
            {
                changes.Add(new PropertyChange(nameof(IPackageInvitation.isExpired), oldDto.isExpired, newDto.isExpired));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(IPackageInvitation.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.Owner != newDto.Owner)
            {
                changes.Add(new PropertyChange(nameof(IPackageInvitation.Owner), oldDto.Owner, newDto.Owner));
            }

            if (oldDto.Package != newDto.Package)
            {
                changes.Add(new PropertyChange(nameof(IPackageInvitation.Package), oldDto.Package, newDto.Package));
            }

            if (oldDto.PackageInvitationKind != newDto.PackageInvitationKind)
            {
                changes.Add(new PropertyChange(nameof(IPackageInvitation.PackageInvitationKind), oldDto.PackageInvitationKind, newDto.PackageInvitationKind));
            }

            if (oldDto.Status != newDto.Status)
            {
                changes.Add(new PropertyChange(nameof(IPackageInvitation.Status), oldDto.Status, newDto.Status));
            }

            if (oldDto.Target != newDto.Target)
            {
                changes.Add(new PropertyChange(nameof(IPackageInvitation.Target), oldDto.Target, newDto.Target));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
