// ------------------------------------------------------------------------------------------------
// <copyright file="IDtoComparer.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Comparers
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a contract for comparing two data transfer objects of type <typeparamref name="T" /> and identifying the
    /// property changes.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IThing" /> being compared.</typeparam>
    public interface IDtoComparer<in T> where T : IThing
    {
        /// <summary>
        /// Compares the specified <paramref name="oldDto" /> with <paramref name="newDto" /> and returns the collection of
        /// property differences.
        /// </summary>
        /// <param name="oldDto">The original DTO instance.</param>
        /// <param name="newDto">The updated DTO instance.</param>
        /// <returns>
        /// An <see cref="IEnumerable{PropertyChange}" /> representing the property differences between <paramref name="oldDto" />
        /// and <paramref name="newDto" />.
        /// </returns>
        IEnumerable<PropertyChange> Compare(T oldDto, T newDto);
    }
}
