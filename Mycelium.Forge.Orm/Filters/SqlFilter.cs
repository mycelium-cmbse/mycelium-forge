// ------------------------------------------------------------------------------------------------
// <copyright file="SqlFilter.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Filters
{
    using Npgsql;

    using NpgsqlTypes;

    /// <summary>
    /// Represents a composable SQL filter for query predicates and parameter bindings.
    /// </summary>
    public class SqlFilter : ISqlFilter
    {
        /// <summary>
        /// The list of PostgreSQL command parameters.
        /// </summary>
        private readonly List<NpgsqlParameter> parameters = [];

        /// <summary>
        /// The list of SQL predicate conditions.
        /// </summary>
        private readonly List<string> predicates = [];

        /// <summary>
        /// Gets an empty filter with no conditions or parameters.
        /// </summary>
        public static SqlFilter Empty => new();

        /// <summary>
        /// Gets the SQL WHERE predicate expression (without the "WHERE" keyword).
        /// </summary>
        /// <returns>The SQL predicate string, or empty if unrestricted.</returns>
        public string ToSqlPredicate()
        {
            return this.predicates.Count switch
            {
                0 => string.Empty,
                1 => this.predicates[0],
                _ => string.Join(" AND ", this.predicates.Select(predicate => $"({predicate})"))
            };
        }

        /// <summary>
        /// Applies the necessary <see cref="NpgsqlParameter" /> instances to the database command.
        /// </summary>
        /// <param name="command">The target <see cref="NpgsqlCommand" />.</param>
        public void ApplyParameters(NpgsqlCommand command)
        {
            foreach (var parameter in this.parameters)
            {
                command.Parameters.Add(new NpgsqlParameter(parameter.ParameterName, parameter.NpgsqlDbType)
                {
                    Value = parameter.Value
                });
            }
        }

        /// <summary>
        /// Appends a SQL WHERE condition (without the "WHERE" keyword).
        /// </summary>
        /// <param name="predicate">The SQL predicate string.</param>
        /// <returns>This filter instance for method chaining.</returns>
        public SqlFilter Where(string predicate)
        {
            if (!string.IsNullOrWhiteSpace(predicate))
            {
                this.predicates.Add(predicate);
            }

            return this;
        }

        /// <summary>
        /// Adds a parameter to the filter.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="dbType">The PostgreSQL database type.</param>
        /// <param name="value">The parameter value.</param>
        /// <returns>This filter instance for method chaining.</returns>
        public SqlFilter AddParameter(string name, NpgsqlDbType dbType, object value)
        {
            this.parameters.Add(new NpgsqlParameter(name, dbType) { Value = value ?? DBNull.Value });
            return this;
        }
    }
}
