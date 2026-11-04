// ------------------------------------------------------------------------------------------------
// <copyright file="CollectionExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Tests.Extensions
{
    using System;
    using System.Collections.Generic;

    using Mycelium.Forge.Common.Extensions;

    [TestFixture]
    public class CollectionExtensionsTestFixture
    {
        [Test]
        public void VerifyAddIfNotExisting()
        {
            var list = new List<string> { "existing" };

            // Case 1: Add new item
            var added = list.AddIfNotExisting("new-item");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(added, Is.True);
                Assert.That(list, Has.Count.EqualTo(2));
                Assert.That(list, Does.Contain("new-item"));
            }

            // Case 2: Add existing item
            var alreadyExists = list.AddIfNotExisting("existing");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(alreadyExists, Is.False);
                Assert.That(list, Has.Count.EqualTo(2));
            }
        }

        [Test]
        public void VerifyAddOrReplace()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var account1 = new Account { Id = id1, Name = "Original 1" };
            var account1Updated = new Account { Id = id1, Name = "Updated 1" };
            var account2 = new Account { Id = id2, Name = "Original 2" };

            var list = new List<Account> { account1 };

            // Case 1: Add item
            var added = list.AddOrReplace(account2);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(added, Is.True);
                Assert.That(list, Has.Count.EqualTo(2));
                Assert.That(list[1].Name, Is.EqualTo("Original 2"));
            }

            // Case 2: Replace existing item
            var updated = list.AddOrReplace(account1Updated);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(updated, Is.False);
                Assert.That(list, Has.Count.EqualTo(2));
                Assert.That(list[0].Name, Is.EqualTo("Updated 1"));
            }
        }
    }
}
