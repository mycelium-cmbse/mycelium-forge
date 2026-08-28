// ------------------------------------------------------------------------------------------------
// <copyright file="DatabaseConfigTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Config
{
    using Mycelium.Forge.Orm;

    /// <summary>
    /// Suite of tests for the <see cref="DatabaseConfig" /> class.
    /// </summary>
    [TestFixture]
    public class DatabaseConfigTestFixture
    {
        private DatabaseConfig databaseConfig;

        /// <summary>
        /// Sets up the test context before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.databaseConfig = new DatabaseConfig();
        }

        /// <summary>
        /// Verifies that <see cref="DatabaseConfig.BuildConnectionString" /> generates a valid connection string.
        /// </summary>
        [Test]
        public void VerifyBuildConnectionString()
        {
            this.databaseConfig.Host = "localhost";
            this.databaseConfig.Port = 5432;
            this.databaseConfig.Database = "forge";
            this.databaseConfig.Username = "forge";
            this.databaseConfig.Password = "forge-dev-password";

            var connectionString = this.databaseConfig.BuildConnectionString();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(connectionString, Does.Contain("Host=localhost"));
                Assert.That(connectionString, Does.Contain("Port=5432"));
                Assert.That(connectionString, Does.Contain("Database=forge"));
                Assert.That(connectionString, Does.Contain("Username=forge"));
                Assert.That(connectionString, Does.Contain("Password=forge-dev-password"));
            }
        }

        /// <summary>
        /// Verifies that properties can be get and set correctly with default values.
        /// </summary>
        [Test]
        public void VerifyProperties()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.databaseConfig.Host, Is.Empty);
                Assert.That(this.databaseConfig.Port, Is.Zero);
                Assert.That(this.databaseConfig.Database, Is.Empty);
                Assert.That(this.databaseConfig.Username, Is.Empty);
                Assert.That(this.databaseConfig.Password, Is.Empty);
            }

            this.databaseConfig.Host = "db.example.com";
            this.databaseConfig.Port = 5433;
            this.databaseConfig.Database = "custom_forge";
            this.databaseConfig.Username = "admin";
            this.databaseConfig.Password = "secret";

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.databaseConfig.Host, Is.EqualTo("db.example.com"));
                Assert.That(this.databaseConfig.Port, Is.EqualTo(5433));
                Assert.That(this.databaseConfig.Database, Is.EqualTo("custom_forge"));
                Assert.That(this.databaseConfig.Username, Is.EqualTo("admin"));
                Assert.That(this.databaseConfig.Password, Is.EqualTo("secret"));
            }
        }
    }
}
