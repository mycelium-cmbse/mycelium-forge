// ------------------------------------------------------------------------------------------------
// <copyright file="JsonEntityBehaviorsDataLoaderTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests.DataLoaders
{
    using System;
    using System.IO;

    using Mycelium.Forge.Generator.DataLoaders;

    /// <summary>
    /// Unit test fixture for <see cref="JsonEntityBehaviorsDataLoader" />.
    /// </summary>
    [TestFixture]
    public class JsonEntityBehaviorsDataLoaderTestFixture
    {
        private JsonEntityBehaviorsDataLoader loader;

        [SetUp]
        public void SetUp()
        {
            this.loader = new JsonEntityBehaviorsDataLoader();
        }

        /// <summary>
        /// Verifies the <see cref="JsonEntityBehaviorsDataLoader.Load" /> method.
        /// </summary>
        [Test]
        public void VerifyLoad()
        {
            var jsonPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", "forge-entity-behaviors.json");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => this.loader.Load(null), Throws.TypeOf<ArgumentException>());
                Assert.That(() => this.loader.Load(string.Empty), Throws.TypeOf<ArgumentException>());
                Assert.That(() => this.loader.Load("non-existent-path.json"), Throws.TypeOf<FileNotFoundException>());

                var result = this.loader.Load(jsonPath);

                Assert.That(result, Is.Not.Null);
                Assert.That(result, Has.Count.EqualTo(6));
                Assert.That(result.ContainsKey("Address"), Is.True);
                Assert.That(result.ContainsKey("Package"), Is.True);
                Assert.That(result.ContainsKey("PackageVersion"), Is.True);

                var packageBehavior = result["Package"];
                Assert.That(packageBehavior.EntityName, Is.EqualTo("Package"));
                Assert.That(packageBehavior.BehaviorType, Is.EqualTo("OrganizationScope"));
                Assert.That(packageBehavior.Configuration.ContainsKey("PersonalCreatePermission"), Is.True);
                Assert.That(packageBehavior.Configuration["BypassPermissions"], Is.EqualTo("ViewAllOrganizations,ManageOrganizations"));
                Assert.That(packageBehavior.Configuration["bypasspermissions"], Is.EqualTo("ViewAllOrganizations,ManageOrganizations"));
            }
        }
    }
}
