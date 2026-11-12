// ------------------------------------------------------------------------------------------------
// <copyright file="ManyToManyPredicateBuilder.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers.BehaviorTypeHelpers
{
    using Mycelium.Forge.Generator.Extensions;

    /// <summary>
    /// Builds SQL predicates that query many-to-many junction tables.
    /// </summary>
    public static class ManyToManyPredicateBuilder
    {
        /// <summary>
        /// Builds an <c>EXISTS</c> clause that checks whether the caller account is linked through a many-to-many
        /// junction table to the row identified by <paramref name="rowAlias" />.<paramref name="rowColumn" />.
        /// </summary>
        /// <param name="ownerEntityName">The name of the entity that owns the many-to-many property.</param>
        /// <param name="propertyName">The name of the many-to-many property.</param>
        /// <param name="rowAlias">The name of the table (alias) that holds the referenced identifier.</param>
        /// <param name="rowColumn">The name of the column that holds the referenced identifier.</param>
        /// <returns>The SQL <c>EXISTS</c> clause.</returns>
        public static string BuildCallerAccountExistsClause(string ownerEntityName, string propertyName, string rowAlias, string rowColumn)
        {
            var ownerTable = ownerEntityName.ToSnakeCase();
            var junctionTable = $"{ownerTable}_{propertyName.ToSnakeCase()}__account";

            return $"EXISTS (SELECT 1 FROM forge.{junctionTable} WHERE source_{ownerTable} = {rowAlias.ToSnakeCase()}.{rowColumn.ToSnakeCase()} AND target_account = @callerAccountId)";
        }
    }
}
