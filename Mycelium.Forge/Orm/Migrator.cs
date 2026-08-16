// ------------------------------------------------------------------------------------------------
// <copyright file="Migrator.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm
{
    using System.Reflection;

    using DbUp;

    /// <summary>
    /// Runs the embedded SQL migrations under <c>Orm/Migrations/</c> against the configured
    /// PostgreSQL database (DD-18). Invoked as an explicit, one-shot command
    /// (<c>dotnet Mycelium.Forge.dll migrate</c>) rather than at every replica's startup, per DD-03's
    /// interchangeable-replica model - see <see cref="Program"/>.
    /// </summary>
    public static class Migrator
    {
        /// <summary>
        /// Applies every not-yet-applied embedded migration script, in one transaction.
        /// </summary>
        /// <param name="connectionString">
        /// The PostgreSQL connection string to migrate.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if every pending script applied successfully; otherwise <see langword="false"/>.
        /// </returns>
        public static bool Run(string connectionString)
        {
            var upgrader = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransaction()
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.Error.WriteLine(result.Error);
            }

            return result.Successful;
        }
    }
}
