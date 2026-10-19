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
    /// Defines a factory for constructing SQL read visibility filters for <typeparamref name="T" /> entities.
    /// </summary>
    /// <typeparam name="T">The domain entity type implementing <see cref="IThing" />.</typeparam>
    public interface IReadFilter<T> where T : IThing
    {
        /// <summary>
        /// Creates an <see cref="ISqlFilter" /> for <typeparamref name="T" /> instances based on the specified user context.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles for authorization evaluation.</param>
        /// <returns>An <see cref="ISqlFilter" /> containing the SQL visibility predicate and parameter bindings.</returns>
        ISqlFilter FromUserContext(IUserContext userContext);
    }
}
