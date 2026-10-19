// ------------------------------------------------------------------------------------------------
// <copyright file="IReadFilter.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Filters
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines a factory for constructing SQL read visibility filters for domain entities.
    /// </summary>
    public interface IReadFilter
    {
        /// <summary>
        /// Creates an <see cref="ISqlFilter" /> for domain entity instances based on the specified user context.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles for authorization evaluation.</param>
        /// <returns>An <see cref="ISqlFilter" /> containing the SQL visibility predicate and parameter bindings.</returns>
        ISqlFilter FromUserContext(IUserContext userContext);
    }
}
