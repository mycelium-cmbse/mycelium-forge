// ------------------------------------------------------------------------------------------------
// <copyright file="ErrorExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using ErrorOr;

    /// <summary>
    /// Provides extension methods for <see cref="Error" /> collections.
    /// </summary>
    public static class ErrorExtensions
    {
        /// <param name="errors">The collection of errors.</param>
        extension(IEnumerable<Error> errors)
        {
            /// <summary>
            /// Joins the error descriptions into a single comma-separated string.
            /// </summary>
            /// <returns>A comma-separated string containing each error description.</returns>
            public string ToJoinedDescriptions()
            {
                return errors == null ? string.Empty : string.Join(", ", errors.Select(e => e.Description));
            }
        }
    }
}
