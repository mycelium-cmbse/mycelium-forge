// ------------------------------------------------------------------------------------------------
// <copyright file="ProfileLinkComparer.cs" company="Starion Group S.A.">
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
    /// Compares two instances of <see cref="IProfileLink"/> and returns the property changes between them.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public partial class ProfileLinkComparer : IDtoComparer<IProfileLink>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="IProfileLink" /> instance.</param>
        /// <param name="newDto">The updated <see cref="IProfileLink" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(IProfileLink oldDto, IProfileLink newDto)
        {
            var changes = new List<PropertyChange>();

            if (oldDto == null || newDto == null)
            {
                return changes;
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(IProfileLink.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(IProfileLink.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.Owner != newDto.Owner)
            {
                changes.Add(new PropertyChange(nameof(IProfileLink.Owner), oldDto.Owner, newDto.Owner));
            }

            if (oldDto.ProfileType != newDto.ProfileType)
            {
                changes.Add(new PropertyChange(nameof(IProfileLink.ProfileType), oldDto.ProfileType, newDto.ProfileType));
            }

            if (oldDto.Uri != newDto.Uri)
            {
                changes.Add(new PropertyChange(nameof(IProfileLink.Uri), oldDto.Uri, newDto.Uri));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
