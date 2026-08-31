// ------------------------------------------------------------------------------------------------
// <copyright file="AccountComparer.cs" company="Starion Group S.A.">
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
    /// Compares two instances of <see cref="IAccount"/> and returns the property changes between them.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public partial class AccountComparer : IDtoComparer<IAccount>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="IAccount" /> instance.</param>
        /// <param name="newDto">The updated <see cref="IAccount" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(IAccount oldDto, IAccount newDto)
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
                changes.Add(new PropertyChange(nameof(IAccount.Address), oldDto.Address, newDto.Address));
            }

            var oldApiKey = oldDto.ApiKey ?? [];
            var newApiKey = newDto.ApiKey ?? [];

            if (!oldApiKey.SequenceEqual(newApiKey))
            {
                changes.Add(new PropertyChange(nameof(IAccount.ApiKey), oldDto.ApiKey, newDto.ApiKey));
            }

            if (oldDto.AvatarBlobReference != newDto.AvatarBlobReference)
            {
                changes.Add(new PropertyChange(nameof(IAccount.AvatarBlobReference), oldDto.AvatarBlobReference, newDto.AvatarBlobReference));
            }

            if (oldDto.BillingEmail != newDto.BillingEmail)
            {
                changes.Add(new PropertyChange(nameof(IAccount.BillingEmail), oldDto.BillingEmail, newDto.BillingEmail));
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(IAccount.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.DefaultPackageVisibility != newDto.DefaultPackageVisibility)
            {
                changes.Add(new PropertyChange(nameof(IAccount.DefaultPackageVisibility), oldDto.DefaultPackageVisibility, newDto.DefaultPackageVisibility));
            }

            if (oldDto.Email != newDto.Email)
            {
                changes.Add(new PropertyChange(nameof(IAccount.Email), oldDto.Email, newDto.Email));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(IAccount.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.Name != newDto.Name)
            {
                changes.Add(new PropertyChange(nameof(IAccount.Name), oldDto.Name, newDto.Name));
            }

            if (oldDto.Origin != newDto.Origin)
            {
                changes.Add(new PropertyChange(nameof(IAccount.Origin), oldDto.Origin, newDto.Origin));
            }

            var oldOwnedOrganizationInvitation = oldDto.OwnedOrganizationInvitation ?? [];
            var newOwnedOrganizationInvitation = newDto.OwnedOrganizationInvitation ?? [];

            if (!oldOwnedOrganizationInvitation.SequenceEqual(newOwnedOrganizationInvitation))
            {
                changes.Add(new PropertyChange(nameof(IAccount.OwnedOrganizationInvitation), oldDto.OwnedOrganizationInvitation, newDto.OwnedOrganizationInvitation));
            }

            var oldOwnedPackage = oldDto.OwnedPackage ?? [];
            var newOwnedPackage = newDto.OwnedPackage ?? [];

            if (!oldOwnedPackage.SequenceEqual(newOwnedPackage))
            {
                changes.Add(new PropertyChange(nameof(IAccount.OwnedPackage), oldDto.OwnedPackage, newDto.OwnedPackage));
            }

            if (oldDto.OwnedPackageInvitation != newDto.OwnedPackageInvitation)
            {
                changes.Add(new PropertyChange(nameof(IAccount.OwnedPackageInvitation), oldDto.OwnedPackageInvitation, newDto.OwnedPackageInvitation));
            }

            if (oldDto.Owner != newDto.Owner)
            {
                changes.Add(new PropertyChange(nameof(IAccount.Owner), oldDto.Owner, newDto.Owner));
            }

            if (oldDto.PrimaryAddress != newDto.PrimaryAddress)
            {
                changes.Add(new PropertyChange(nameof(IAccount.PrimaryAddress), oldDto.PrimaryAddress, newDto.PrimaryAddress));
            }

            var oldProfileLink = oldDto.ProfileLink ?? [];
            var newProfileLink = newDto.ProfileLink ?? [];

            if (!oldProfileLink.SequenceEqual(newProfileLink))
            {
                changes.Add(new PropertyChange(nameof(IAccount.ProfileLink), oldDto.ProfileLink, newDto.ProfileLink));
            }

            if (oldDto.ShortName != newDto.ShortName)
            {
                changes.Add(new PropertyChange(nameof(IAccount.ShortName), oldDto.ShortName, newDto.ShortName));
            }

            if (oldDto.Status != newDto.Status)
            {
                changes.Add(new PropertyChange(nameof(IAccount.Status), oldDto.Status, newDto.Status));
            }

            if (oldDto.Website != newDto.Website)
            {
                changes.Add(new PropertyChange(nameof(IAccount.Website), oldDto.Website, newDto.Website));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
