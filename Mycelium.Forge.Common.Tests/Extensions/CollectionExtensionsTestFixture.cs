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
    using System.Collections.Generic;

    using Mycelium.Forge.Common.Extensions;

    [TestFixture]
    public class CollectionExtensionsTestFixture
    {
        [Test]
        public void VerifyAddIfNotExisting()
        {
            var list = new List<string> { "existing" };

            var added = list.AddIfNotExisting("new-item");
            var alreadyExists = list.AddIfNotExisting("existing");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(added, Is.True);
                Assert.That(alreadyExists, Is.False);
                Assert.That(list, Has.Count.EqualTo(2));
                Assert.That(list, Does.Contain("new-item"));
            }
        }
    }
}
