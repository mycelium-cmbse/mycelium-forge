// ------------------------------------------------------------------------------------------------
// <copyright file="CountryComparer.cs" company="Starion Group S.A.">
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
    /// Compares two instances of <see cref="ICountry"/> and returns the property changes between them.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public partial class CountryComparer : IDtoComparer<ICountry>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="ICountry" /> instance.</param>
        /// <param name="newDto">The updated <see cref="ICountry" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(ICountry oldDto, ICountry newDto)
        {
            var changes = new List<PropertyChange>();

            if (oldDto == null || newDto == null)
            {
                return changes;
            }

            if (oldDto.Alpha2Code != newDto.Alpha2Code)
            {
                changes.Add(new PropertyChange(nameof(ICountry.Alpha2Code), oldDto.Alpha2Code, newDto.Alpha2Code));
            }

            if (oldDto.Alpha3Code != newDto.Alpha3Code)
            {
                changes.Add(new PropertyChange(nameof(ICountry.Alpha3Code), oldDto.Alpha3Code, newDto.Alpha3Code));
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(ICountry.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(ICountry.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.Name != newDto.Name)
            {
                changes.Add(new PropertyChange(nameof(ICountry.Name), oldDto.Name, newDto.Name));
            }

            if (oldDto.NumericCode != newDto.NumericCode)
            {
                changes.Add(new PropertyChange(nameof(ICountry.NumericCode), oldDto.NumericCode, newDto.NumericCode));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
