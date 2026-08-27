// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationComparer.cs" company="Starion Group S.A.">
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
    /// Compares two instances of <see cref="IOrganization"/> and returns the property changes between them.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public partial class OrganizationComparer : IDtoComparer<IOrganization>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="IOrganization" /> instance.</param>
        /// <param name="newDto">The updated <see cref="IOrganization" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(IOrganization oldDto, IOrganization newDto)
        {
            var changes = new List<PropertyChange>();

            if (oldDto == null || newDto == null)
            {
                return changes;
            }

            var oldAddress = oldDto.Address ?? [];
            var newAddress = newDto.Address ?? [];

            if (!oldAddress.SequenceEqual(newAddress))
            {
                changes.Add(new PropertyChange(nameof(IOrganization.Address), oldDto.Address, newDto.Address));
            }

            var oldAdministrator = oldDto.Administrator ?? [];
            var newAdministrator = newDto.Administrator ?? [];

            if (!oldAdministrator.SequenceEqual(newAdministrator))
            {
                changes.Add(new PropertyChange(nameof(IOrganization.Administrator), oldDto.Administrator, newDto.Administrator));
            }

            if (oldDto.BillingEmail != newDto.BillingEmail)
            {
                changes.Add(new PropertyChange(nameof(IOrganization.BillingEmail), oldDto.BillingEmail, newDto.BillingEmail));
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(IOrganization.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.DefaultPackageVisibility != newDto.DefaultPackageVisibility)
            {
                changes.Add(new PropertyChange(nameof(IOrganization.DefaultPackageVisibility), oldDto.DefaultPackageVisibility, newDto.DefaultPackageVisibility));
            }

            if (oldDto.Email != newDto.Email)
            {
                changes.Add(new PropertyChange(nameof(IOrganization.Email), oldDto.Email, newDto.Email));
            }

            if (oldDto.LogoBlobReference != newDto.LogoBlobReference)
            {
                changes.Add(new PropertyChange(nameof(IOrganization.LogoBlobReference), oldDto.LogoBlobReference, newDto.LogoBlobReference));
            }

            var oldMember = oldDto.Member ?? [];
            var newMember = newDto.Member ?? [];

            if (!oldMember.SequenceEqual(newMember))
            {
                changes.Add(new PropertyChange(nameof(IOrganization.Member), oldDto.Member, newDto.Member));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(IOrganization.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.Name != newDto.Name)
            {
                changes.Add(new PropertyChange(nameof(IOrganization.Name), oldDto.Name, newDto.Name));
            }

            if (oldDto.Origin != newDto.Origin)
            {
                changes.Add(new PropertyChange(nameof(IOrganization.Origin), oldDto.Origin, newDto.Origin));
            }

            var oldOwnedPackage = oldDto.OwnedPackage ?? [];
            var newOwnedPackage = newDto.OwnedPackage ?? [];

            if (!oldOwnedPackage.SequenceEqual(newOwnedPackage))
            {
                changes.Add(new PropertyChange(nameof(IOrganization.OwnedPackage), oldDto.OwnedPackage, newDto.OwnedPackage));
            }

            if (oldDto.PrimaryAddress != newDto.PrimaryAddress)
            {
                changes.Add(new PropertyChange(nameof(IOrganization.PrimaryAddress), oldDto.PrimaryAddress, newDto.PrimaryAddress));
            }

            var oldProfileLink = oldDto.ProfileLink ?? [];
            var newProfileLink = newDto.ProfileLink ?? [];

            if (!oldProfileLink.SequenceEqual(newProfileLink))
            {
                changes.Add(new PropertyChange(nameof(IOrganization.ProfileLink), oldDto.ProfileLink, newDto.ProfileLink));
            }

            if (oldDto.ShortName != newDto.ShortName)
            {
                changes.Add(new PropertyChange(nameof(IOrganization.ShortName), oldDto.ShortName, newDto.ShortName));
            }

            if (oldDto.Status != newDto.Status)
            {
                changes.Add(new PropertyChange(nameof(IOrganization.Status), oldDto.Status, newDto.Status));
            }

            if (oldDto.Website != newDto.Website)
            {
                changes.Add(new PropertyChange(nameof(IOrganization.Website), oldDto.Website, newDto.Website));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
