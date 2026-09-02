// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationInvitationComparer.cs" company="Starion Group S.A.">
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
    /// Compares two instances of <see cref="IOrganizationInvitation"/> and returns the property changes between them.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public partial class OrganizationInvitationComparer : IDtoComparer<IOrganizationInvitation>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="IOrganizationInvitation" /> instance.</param>
        /// <param name="newDto">The updated <see cref="IOrganizationInvitation" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(IOrganizationInvitation oldDto, IOrganizationInvitation newDto)
        {
            var changes = new List<PropertyChange>();

            if (oldDto == null || newDto == null)
            {
                return changes;
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(IOrganizationInvitation.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.ExperisAt != newDto.ExperisAt)
            {
                changes.Add(new PropertyChange(nameof(IOrganizationInvitation.ExperisAt), oldDto.ExperisAt, newDto.ExperisAt));
            }

            if (oldDto.isExpired != newDto.isExpired)
            {
                changes.Add(new PropertyChange(nameof(IOrganizationInvitation.isExpired), oldDto.isExpired, newDto.isExpired));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(IOrganizationInvitation.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.Organization != newDto.Organization)
            {
                changes.Add(new PropertyChange(nameof(IOrganizationInvitation.Organization), oldDto.Organization, newDto.Organization));
            }

            if (oldDto.OrganizationInvitationKind != newDto.OrganizationInvitationKind)
            {
                changes.Add(new PropertyChange(nameof(IOrganizationInvitation.OrganizationInvitationKind), oldDto.OrganizationInvitationKind, newDto.OrganizationInvitationKind));
            }

            if (oldDto.Owner != newDto.Owner)
            {
                changes.Add(new PropertyChange(nameof(IOrganizationInvitation.Owner), oldDto.Owner, newDto.Owner));
            }

            if (oldDto.Status != newDto.Status)
            {
                changes.Add(new PropertyChange(nameof(IOrganizationInvitation.Status), oldDto.Status, newDto.Status));
            }

            if (oldDto.Target != newDto.Target)
            {
                changes.Add(new PropertyChange(nameof(IOrganizationInvitation.Target), oldDto.Target, newDto.Target));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
