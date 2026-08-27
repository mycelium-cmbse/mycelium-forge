// ------------------------------------------------------------------------------------------------
// <copyright file="AddressComparer.cs" company="Starion Group S.A.">
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
    /// Compares two instances of <see cref="IAddress"/> and returns the property changes between them.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public partial class AddressComparer : IDtoComparer<IAddress>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="IAddress" /> instance.</param>
        /// <param name="newDto">The updated <see cref="IAddress" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(IAddress oldDto, IAddress newDto)
        {
            var changes = new List<PropertyChange>();

            if (oldDto == null || newDto == null)
            {
                return changes;
            }

            if (oldDto.AddressLine1 != newDto.AddressLine1)
            {
                changes.Add(new PropertyChange(nameof(IAddress.AddressLine1), oldDto.AddressLine1, newDto.AddressLine1));
            }

            if (oldDto.AddressLine2 != newDto.AddressLine2)
            {
                changes.Add(new PropertyChange(nameof(IAddress.AddressLine2), oldDto.AddressLine2, newDto.AddressLine2));
            }

            if (oldDto.Country != newDto.Country)
            {
                changes.Add(new PropertyChange(nameof(IAddress.Country), oldDto.Country, newDto.Country));
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(IAddress.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.Locality != newDto.Locality)
            {
                changes.Add(new PropertyChange(nameof(IAddress.Locality), oldDto.Locality, newDto.Locality));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(IAddress.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.PostalCode != newDto.PostalCode)
            {
                changes.Add(new PropertyChange(nameof(IAddress.PostalCode), oldDto.PostalCode, newDto.PostalCode));
            }

            if (oldDto.Region != newDto.Region)
            {
                changes.Add(new PropertyChange(nameof(IAddress.Region), oldDto.Region, newDto.Region));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
