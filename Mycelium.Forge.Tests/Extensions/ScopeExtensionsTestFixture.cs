// ------------------------------------------------------------------------------------------------
// <copyright file="ScopeExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Extensions
{
    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;

    [TestFixture]
    public class ScopeExtensionsTestFixture
    {
        [Test]
        public void VerifyGetDisplayShortName()
        {
            // Case 1: Scope with short name prefixes with '@'
            var scope = new Organization { ShortName = "starion" };
            Assert.That(scope.GetDisplayShortName(), Is.EqualTo("@starion"));

            // Case 2: Scope with whitespace short name returns empty
            var emptyScope = new Organization { ShortName = "   " };
            Assert.That(emptyScope.GetDisplayShortName(), Is.Empty);

            // Case 3: Null scope returns empty
            IScope nullScope = null!;
            Assert.That(nullScope.GetDisplayShortName(), Is.Empty);
        }

        [Test]
        public void VerifyIsVerified()
        {
            // Case 1: Account reflection of IsVerified flag
            var verifiedAccount = new Account { IsVerified = true };
            var unverifiedAccount = new Account { IsVerified = false };

            using (Assert.EnterMultipleScope())
            {
                Assert.That(verifiedAccount.IsVerified(), Is.True);
                Assert.That(unverifiedAccount.IsVerified(), Is.False);
            }

            // Case 2: Organizations are verified by default
            var organization = new Organization();
            Assert.That(organization.IsVerified(), Is.True);

            // Case 3: Other scope types default to unverified
            var mockScope = new Mock<IScope>();
            Assert.That(mockScope.Object.IsVerified(), Is.False);
        }
    }
}
