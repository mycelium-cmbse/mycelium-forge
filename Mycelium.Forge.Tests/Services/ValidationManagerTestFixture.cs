// ------------------------------------------------------------------------------------------------
// <copyright file="ValidationManagerTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Services
{
    using System.Linq;

    using Mycelium.Forge.Services;

    [TestFixture]
    public class ValidationManagerTestFixture
    {
        private ValidationManager validationManager;

        [SetUp]
        public void SetUp()
        {
            this.validationManager = new ValidationManager();
        }

        [Test]
        public void VerifyCheckWithPredicate()
        {
            this.validationManager.Check("Scope", () => false, "Scope is required.");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.validationManager.HasError("Scope"), Is.True);
                Assert.That(this.validationManager.IsValid, Is.False);
            }

            this.validationManager.Check("Scope", () => true, "Scope is required.");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.validationManager.HasError("Scope"), Is.False);
                Assert.That(this.validationManager.IsValid, Is.True);
            }
        }

        [Test]
        public void VerifyClearError()
        {
            this.validationManager.Check("Password", false, "Password is too short.");
            Assert.That(this.validationManager.HasError("Password"), Is.True);

            this.validationManager.ClearError("Password");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.validationManager.HasError("Password"), Is.False);
                Assert.That(this.validationManager.IsValid, Is.True);
            }
        }

        [Test]
        public void VerifyIndexer()
        {
            var initialErrors = this.validationManager["Name"].ToList();

            this.validationManager.Check("Name", false, "Name is required.");
            var populatedErrors = this.validationManager["Name"].ToList();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(initialErrors, Has.Count.EqualTo(0));
                Assert.That(populatedErrors, Has.Count.EqualTo(1));
                Assert.That(populatedErrors[0], Is.EqualTo("Name is required."));
            }
        }

        [Test]
        public void VerifyIsValidAndHasError()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.validationManager.IsValid, Is.True);
                Assert.That(this.validationManager.HasError("Email"), Is.False);
            }

            this.validationManager.Check("Email", false, "Email is invalid.");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.validationManager.IsValid, Is.False);
                Assert.That(this.validationManager.HasError("Email"), Is.True);
            }

            this.validationManager.Check("Email", true, "Email is invalid.");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.validationManager.IsValid, Is.True);
                Assert.That(this.validationManager.HasError("Email"), Is.False);
            }
        }
    }
}
