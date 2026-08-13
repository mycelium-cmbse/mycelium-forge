// ------------------------------------------------------------------------------------------------
// <copyright file="IThingPartialMergeTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Tests
{
    using System;

    /// <summary>
    /// Verifies that the hand-written <c>partial interface IThing</c> in
    /// <c>Mycelium.Forge.Common/IThing.cs</c> actually merges with the generated
    /// <c>partial interface IThing</c> in <c>AutoGenDto/IThing.cs</c>, rather than the two compiling
    /// as two independent, coincidentally-named types (which would happen silently if their
    /// namespaces ever drifted apart).
    /// </summary>
    [TestFixture]
    public class IThingPartialMergeTestFixture
    {
        [Test]
        public void Verify_that_a_generated_dto_exposes_the_hand_written_default_member()
        {
            var id = Guid.NewGuid();
            var createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var account = new Account
            {
                Id = id,
                CreatedAt = createdAt,
            };

            Assert.That(account, Is.InstanceOf<IThing>());
        }
    }
}
