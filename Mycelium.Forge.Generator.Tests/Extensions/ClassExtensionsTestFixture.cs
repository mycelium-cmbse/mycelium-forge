// ------------------------------------------------------------------------------------------------
// <copyright file="ClassExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests.Extensions
{
    using System;
    using System.Linq;

    using uml4net.Extensions;
    using uml4net.StructuredClassifiers;

    using GeneratorClassExtensions = Mycelium.Forge.Generator.Extensions.ClassExtensions;

    /// <summary>
    /// Suite of tests for the <see cref="GeneratorClassExtensions" /> class.
    /// </summary>
    [TestFixture]
    public class ClassExtensionsTestFixture
    {
        private IClass accountClass;
        private IClass organizationClass;
        private IClass thingClass;
        private IClass scopeClass;

        /// <summary>
        /// Sets up the test fixture before each test execution.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.accountClass = GetClass("Account");
            this.organizationClass = GetClass("Organization");
            this.thingClass = GetClass("Thing");
            this.scopeClass = GetClass("Scope");
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorClassExtensions.HasThingClass" /> correctly detects Thing in the hierarchy.
        /// </summary>
        [Test]
        public void VerifyHasThingClass()
        {
            var standaloneClass = new Class { Name = "Standalone" };

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorClassExtensions.HasThingClass(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(GeneratorClassExtensions.HasThingClass(this.accountClass), Is.True);
                Assert.That(GeneratorClassExtensions.HasThingClass(this.scopeClass), Is.True);
                Assert.That(GeneratorClassExtensions.HasThingClass(this.thingClass), Is.False);
                Assert.That(GeneratorClassExtensions.HasThingClass(standaloneClass), Is.False);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorClassExtensions.IsThingClass" /> identifies if a class is the root Thing class.
        /// </summary>
        [Test]
        public void VerifyIsThingClass()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorClassExtensions.IsThingClass(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(GeneratorClassExtensions.IsThingClass(this.thingClass), Is.True);
                Assert.That(GeneratorClassExtensions.IsThingClass(this.accountClass), Is.False);
                Assert.That(GeneratorClassExtensions.IsThingClass(this.scopeClass), Is.False);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorClassExtensions.QueryAllOppositeReferencesToMe" /> returns all opposite references.
        /// </summary>
        [Test]
        public void VerifyQueryAllOppositeReferencesToMe()
        {
            var isolatedClass = new Class { Name = "Isolated" };
            var emptyResult = GeneratorClassExtensions.QueryAllOppositeReferencesToMe(isolatedClass).ToList();
            var accountReferences = GeneratorClassExtensions.QueryAllOppositeReferencesToMe(this.accountClass).ToList();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorClassExtensions.QueryAllOppositeReferencesToMe(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(emptyResult, Has.Count.EqualTo(0));
                Assert.That(accountReferences, Is.Not.Null);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorClassExtensions.QueryDerivesFrom" /> correctly evaluates inheritance hierarchy.
        /// </summary>
        [Test]
        public void VerifyQueryDerivesFrom()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorClassExtensions.QueryDerivesFrom(null!, "Thing"), Throws.TypeOf<ArgumentNullException>());
                Assert.That(() => GeneratorClassExtensions.QueryDerivesFrom(this.accountClass, null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(() => GeneratorClassExtensions.QueryDerivesFrom(this.accountClass, string.Empty), Throws.TypeOf<ArgumentException>());
                Assert.That(() => GeneratorClassExtensions.QueryDerivesFrom(this.accountClass, "   "), Throws.TypeOf<ArgumentException>());
                Assert.That(GeneratorClassExtensions.QueryDerivesFrom(this.accountClass, "Scope"), Is.True);
                Assert.That(GeneratorClassExtensions.QueryDerivesFrom(this.accountClass, "Thing"), Is.True);
                Assert.That(GeneratorClassExtensions.QueryDerivesFrom(this.accountClass, "NonExistentClass"), Is.False);
                Assert.That(GeneratorClassExtensions.QueryDerivesFrom(this.thingClass, "Thing"), Is.False);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorClassExtensions.QueryOwnedManyToManyProperties" /> returns many-to-many properties.
        /// </summary>
        [Test]
        public void VerifyQueryOwnedManyToManyProperties()
        {
            var organizationManyToMany = GeneratorClassExtensions.QueryOwnedManyToManyProperties(this.organizationClass).ToList();
            var thingManyToMany = GeneratorClassExtensions.QueryOwnedManyToManyProperties(this.thingClass).ToList();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorClassExtensions.QueryOwnedManyToManyProperties(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(organizationManyToMany, Has.Count.EqualTo(2));
                Assert.That(organizationManyToMany.Any(x => x.Name == "administrator"), Is.True);
                Assert.That(organizationManyToMany.Any(x => x.Name == "member"), Is.True);
                Assert.That(thingManyToMany, Has.Count.EqualTo(0));
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorClassExtensions.QuerySqlSingleReferenceProperties" /> returns single reference
        /// properties.
        /// </summary>
        [Test]
        public void VerifyQuerySqlSingleReferenceProperties()
        {
            var accountReferences = GeneratorClassExtensions.QuerySqlSingleReferenceProperties(this.accountClass).ToList();
            var thingReferences = GeneratorClassExtensions.QuerySqlSingleReferenceProperties(this.thingClass).ToList();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorClassExtensions.QuerySqlSingleReferenceProperties(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(accountReferences, Is.Not.Null);
                Assert.That(accountReferences.Any(x => x.Name is "owner" or "forge"), Is.True);
                Assert.That(thingReferences, Is.Not.Null);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorClassExtensions.QuerySqlTableName" /> returns the capitalized table name.
        /// </summary>
        [Test]
        public void VerifyQuerySqlTableName()
        {
            var customClass = new Class { Name = "customEntity" };

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorClassExtensions.QuerySqlTableName(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(GeneratorClassExtensions.QuerySqlTableName(this.accountClass), Is.EqualTo("Account"));
                Assert.That(GeneratorClassExtensions.QuerySqlTableName(this.organizationClass), Is.EqualTo("Organization"));
                Assert.That(GeneratorClassExtensions.QuerySqlTableName(customClass), Is.EqualTo("CustomEntity"));
            }
        }

        /// <summary>
        /// Gets a class from the shared XMI model by name.
        /// </summary>
        /// <param name="className">The name of the class.</param>
        /// <returns>The <see cref="IClass" /> instance.</returns>
        private static IClass GetClass(string className)
        {
            return GeneratorSetupFixture.XmiReaderResult.Packages
                .SelectMany(package => package.QueryPackages())
                .SelectMany(package => package.PackagedElement.OfType<IClass>())
                .Single(x => x.Name == className);
        }
    }
}
