// ------------------------------------------------------------------------------------------------
// <copyright file="TagComparer.cs" company="Starion Group S.A.">
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
    /// Compares two instances of <see cref="ITag"/> and returns the property changes between them.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.6.0")]
    public partial class TagComparer : IDtoComparer<ITag>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="ITag" /> instance.</param>
        /// <param name="newDto">The updated <see cref="ITag" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(ITag oldDto, ITag newDto)
        {
            var changes = new List<PropertyChange>();

            if (oldDto == null || newDto == null)
            {
                return changes;
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(ITag.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(ITag.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.Name != newDto.Name)
            {
                changes.Add(new PropertyChange(nameof(ITag.Name), oldDto.Name, newDto.Name));
            }

            if (oldDto.Owner != newDto.Owner)
            {
                changes.Add(new PropertyChange(nameof(ITag.Owner), oldDto.Owner, newDto.Owner));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
