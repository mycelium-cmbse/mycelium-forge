// ------------------------------------------------------------------------------------------------
// <copyright file="DatabaseMigratorIntegrationTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Tests
{
    using Mycelium.Forge.Orm.Tests.Helper;

    using Npgsql;

    using NUnit.Framework;

    [TestFixture]
    [Category("Database")]
    public class DatabaseMigratorIntegrationTestFixture : BaseIntegrationTestClassFixture
    {
        private const string InvalidConnectionString = "Host=invalid_host;Port=5432;Database=test;Username=postgres;Password=test;Timeout=1;CommandTimeout=1";
        private DatabaseMigrator databaseMigrator;

        [SetUp]
        public new async Task CleanDataBase()
        {
            await using var sqlRemoveCommand = new NpgsqlCommand("DROP SCHEMA IF EXISTS forge CASCADE; DROP TABLE IF EXISTS \"schemaversions\" CASCADE;", this.Connection);
            await sqlRemoveCommand.ExecuteNonQueryAsync();

            this.databaseMigrator = new DatabaseMigrator();
        }

        [Test]
        public void VerifyMigrate()
        {
            var initialMigrationResult = this.databaseMigrator.Migrate(this.ConnectionString);
            var secondMigrationResult = this.databaseMigrator.Migrate(this.ConnectionString);
            var invalidMigrationResult = this.databaseMigrator.Migrate(InvalidConnectionString);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(initialMigrationResult, Is.True);
                Assert.That(secondMigrationResult, Is.True);
                Assert.That(invalidMigrationResult, Is.False);
            }
        }

        [Test]
        public async Task VerifyResetDatabaseAsync()
        {
            var resetResult = await this.databaseMigrator.ResetDatabaseAsync(this.ConnectionString, CancellationToken.None);

            await using var verifyCommand = new NpgsqlCommand("SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_schema = 'forge' AND table_name = 'package');", this.Connection);
            var tableExists = (bool)(await verifyCommand.ExecuteScalarAsync())!;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(resetResult, Is.True);
                Assert.That(tableExists, Is.True);
                Assert.That(async () => await this.databaseMigrator.ResetDatabaseAsync(InvalidConnectionString), Throws.TypeOf<NpgsqlException>());
            }
        }
    }
}
