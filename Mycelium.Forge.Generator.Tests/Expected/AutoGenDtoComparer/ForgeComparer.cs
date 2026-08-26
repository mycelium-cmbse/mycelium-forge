// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeComparer.cs" company="Starion Group S.A.">
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
    /// Compares two instances of <see cref="IForge"/> and returns the property changes between them.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public partial class ForgeComparer : IDtoComparer<IForge>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="IForge" /> instance.</param>
        /// <param name="newDto">The updated <see cref="IForge" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(IForge oldDto, IForge newDto)
        {
            var changes = new List<PropertyChange>();

            if (oldDto == null || newDto == null)
            {
                return changes;
            }

            var oldAccount = oldDto.Account ?? [];
            var newAccount = newDto.Account ?? [];

            if (!oldAccount.SequenceEqual(newAccount))
            {
                changes.Add(new PropertyChange(nameof(IForge.Account), oldDto.Account, newDto.Account));
            }

            var oldAdministrator = oldDto.Administrator ?? [];
            var newAdministrator = newDto.Administrator ?? [];

            if (!oldAdministrator.SequenceEqual(newAdministrator))
            {
                changes.Add(new PropertyChange(nameof(IForge.Administrator), oldDto.Administrator, newDto.Administrator));
            }

            var oldCountry = oldDto.Country ?? [];
            var newCountry = newDto.Country ?? [];

            if (!oldCountry.SequenceEqual(newCountry))
            {
                changes.Add(new PropertyChange(nameof(IForge.Country), oldDto.Country, newDto.Country));
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(IForge.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.Description != newDto.Description)
            {
                changes.Add(new PropertyChange(nameof(IForge.Description), oldDto.Description, newDto.Description));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(IForge.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.Name != newDto.Name)
            {
                changes.Add(new PropertyChange(nameof(IForge.Name), oldDto.Name, newDto.Name));
            }

            var oldOrganization = oldDto.Organization ?? [];
            var newOrganization = newDto.Organization ?? [];

            if (!oldOrganization.SequenceEqual(newOrganization))
            {
                changes.Add(new PropertyChange(nameof(IForge.Organization), oldDto.Organization, newDto.Organization));
            }

            var oldPackageType = oldDto.PackageType ?? [];
            var newPackageType = newDto.PackageType ?? [];

            if (!oldPackageType.SequenceEqual(newPackageType))
            {
                changes.Add(new PropertyChange(nameof(IForge.PackageType), oldDto.PackageType, newDto.PackageType));
            }

            var oldProfileType = oldDto.ProfileType ?? [];
            var newProfileType = newDto.ProfileType ?? [];

            if (!oldProfileType.SequenceEqual(newProfileType))
            {
                changes.Add(new PropertyChange(nameof(IForge.ProfileType), oldDto.ProfileType, newDto.ProfileType));
            }

            if (oldDto.ShortName != newDto.ShortName)
            {
                changes.Add(new PropertyChange(nameof(IForge.ShortName), oldDto.ShortName, newDto.ShortName));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
