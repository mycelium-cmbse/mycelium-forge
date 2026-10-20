// ------------------------------------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Extensions
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides extension methods for collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <param name="collection">The collection to operate on.</param>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        extension<T>(ICollection<T> collection)
        {
            /// <summary>
            /// Adds the specified item to the collection if it is not already present.
            /// </summary>
            /// <param name="item">The item to add.</param>
            /// <returns><c>true</c> if the item was added; otherwise, <c>false</c>.</returns>
            public bool AddIfNotExisting(T item)
            {
                if (collection.Contains(item))
                {
                    return false;
                }

                collection.Add(item);
                return true;
            }
        }
    }
}
