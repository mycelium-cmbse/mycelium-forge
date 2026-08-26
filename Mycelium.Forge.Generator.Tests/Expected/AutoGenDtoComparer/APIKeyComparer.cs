// ------------------------------------------------------------------------------------------------
// <copyright file="APIKeyComparer.cs" company="Starion Group S.A.">
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
    /// Compares two instances of <see cref="IAPIKey"/> and returns the property changes between them.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public partial class APIKeyComparer : IDtoComparer<IAPIKey>
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original <see cref="IAPIKey" /> instance.</param>
        /// <param name="newDto">The updated <see cref="IAPIKey" /> instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between
        /// <paramref name="oldDto" /> and <paramref name="newDto" />.
        /// </returns>
        public IEnumerable<PropertyChange> Compare(IAPIKey oldDto, IAPIKey newDto)
        {
            var changes = new List<PropertyChange>();

            if (oldDto == null || newDto == null)
            {
                return changes;
            }

            if (oldDto.CreatedAt != newDto.CreatedAt)
            {
                changes.Add(new PropertyChange(nameof(IAPIKey.CreatedAt), oldDto.CreatedAt, newDto.CreatedAt));
            }

            if (oldDto.ExpiresAt != newDto.ExpiresAt)
            {
                changes.Add(new PropertyChange(nameof(IAPIKey.ExpiresAt), oldDto.ExpiresAt, newDto.ExpiresAt));
            }

            if (oldDto.LastUsedAt != newDto.LastUsedAt)
            {
                changes.Add(new PropertyChange(nameof(IAPIKey.LastUsedAt), oldDto.LastUsedAt, newDto.LastUsedAt));
            }

            if (oldDto.ModifiedAt != newDto.ModifiedAt)
            {
                changes.Add(new PropertyChange(nameof(IAPIKey.ModifiedAt), oldDto.ModifiedAt, newDto.ModifiedAt));
            }

            if (oldDto.Name != newDto.Name)
            {
                changes.Add(new PropertyChange(nameof(IAPIKey.Name), oldDto.Name, newDto.Name));
            }

            if (oldDto.Permissions != newDto.Permissions)
            {
                changes.Add(new PropertyChange(nameof(IAPIKey.Permissions), oldDto.Permissions, newDto.Permissions));
            }

            if (oldDto.RevokedAt != newDto.RevokedAt)
            {
                changes.Add(new PropertyChange(nameof(IAPIKey.RevokedAt), oldDto.RevokedAt, newDto.RevokedAt));
            }

            var oldSecretHash = oldDto.SecretHash ?? [];
            var newSecretHash = newDto.SecretHash ?? [];

            if (!oldSecretHash.SequenceEqual(newSecretHash))
            {
                changes.Add(new PropertyChange(nameof(IAPIKey.SecretHash), oldDto.SecretHash, newDto.SecretHash));
            }

            return changes;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
