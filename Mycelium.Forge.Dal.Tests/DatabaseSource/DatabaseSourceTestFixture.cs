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
    using System;

    using Mycelium.Forge.Dal.DatabaseSource;
    using Mycelium.Forge.Orm;

    using NUnit.Framework;

    /// <summary>
    /// Test fixture for <see cref="DatabaseSource" />.
    /// </summary>
    [TestFixture]
    public class DatabaseSourceTestFixture
    {
        /// <summary>
        /// Verifies the <see cref="DatabaseSource" /> constructor.
        /// </summary>
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

            var databaseSource = new DatabaseSource(config);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => new DatabaseSource(null), Throws.ArgumentNullException);
                Assert.That(databaseSource, Is.Not.Null);
            }
        }
    }
}
