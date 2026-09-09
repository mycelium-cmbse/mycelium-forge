// ------------------------------------------------------------------------------------------------
// <copyright file="ISqlFilter.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Filters
{
    using Npgsql;

    /// <summary>
    /// Represents an optional SQL filter predicate and its parameter bindings for DAO read operations.
    /// </summary>
    public interface ISqlFilter
    {
        /// <summary>
        /// Gets the SQL WHERE predicate expression (without the "WHERE" keyword).
        /// </summary>
        /// <returns>The SQL predicate string, or empty if unrestricted.</returns>
        string ToSqlPredicate();

        /// <summary>
        /// Applies the necessary <see cref="NpgsqlParameter" /> instances to the database command.
        /// </summary>
        /// <param name="command">The target <see cref="NpgsqlCommand" />.</param>
        void ApplyParameters(NpgsqlCommand command);
    }
}
