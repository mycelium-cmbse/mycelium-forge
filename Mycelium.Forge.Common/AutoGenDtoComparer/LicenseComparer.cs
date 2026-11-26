// ------------------------------------------------------------------------------------------------
// <copyright file="LicenseComparer.cs" company="Starion Group S.A.">
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
    /// Compares two instances of <see cref="ILicense"/> and returns the property changes between them.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.6.0")]
    public partial class LicenseComparer : IDtoComparer<ILicense>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="ILicense" /> instance.</param>
        /// <param name="newDto">The updated <see cref="ILicense" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(ILicense oldDto, ILicense newDto)
        {
            var changes = new List<PropertyChange>();

            if (oldDto == null || newDto == null)
            {
                return changes;
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(ILicense.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.LicenseText != newDto.LicenseText)
            {
                changes.Add(new PropertyChange(nameof(ILicense.LicenseText), oldDto.LicenseText, newDto.LicenseText));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(ILicense.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.Name != newDto.Name)
            {
                changes.Add(new PropertyChange(nameof(ILicense.Name), oldDto.Name, newDto.Name));
            }

            if (oldDto.ShortName != newDto.ShortName)
            {
                changes.Add(new PropertyChange(nameof(ILicense.ShortName), oldDto.ShortName, newDto.ShortName));
            }

            if (oldDto.SpdxIdentifier != newDto.SpdxIdentifier)
            {
                changes.Add(new PropertyChange(nameof(ILicense.SpdxIdentifier), oldDto.SpdxIdentifier, newDto.SpdxIdentifier));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
