// ------------------------------------------------------------------------------------------------
// <copyright file="DatabaseSourceTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Tests.DatabaseSource
{
    using Microsoft.Extensions.Logging;

    using Moq;

    using Mycelium.Forge.Dal.DatabaseSource;
    using Mycelium.Forge.Orm;

    using NUnit.Framework;

    [TestFixture]
    public class DatabaseSourceTestFixture
    {
        private Mock<ILogger<DatabaseSource>> loggerMock;

        [SetUp]
        public void SetUp()
        {
            this.loggerMock = new Mock<ILogger<DatabaseSource>>();
        }

        [Test]
        public void VerifyConstructor()
        {
            var config = new DatabaseConfig
            {
                Host = "localhost",
                Port = 5432,
                Database = "forge",
                Username = "forge",
                Password = "password"
            };

            var databaseSource = new DatabaseSource(config, this.loggerMock.Object);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => new DatabaseSource(null, this.loggerMock.Object), Throws.ArgumentNullException);
                Assert.That(() => new DatabaseSource(config, null), Throws.ArgumentNullException);
                Assert.That(databaseSource, Is.Not.Null);
            }
        }

        [Test]
        public void VerifyExecuteInTransactionAsync()
        {
            var config = new DatabaseConfig
            {
                Host = "localhost",
                Port = 5432,
                Database = "forge",
                Username = "forge",
                Password = "password"
            };

            var databaseSource = new DatabaseSource(config, this.loggerMock.Object);

            Assert.That(async () => await databaseSource.ExecuteInTransactionAsync<bool>(null, CancellationToken.None), Throws.ArgumentNullException);
        }
    }
}
