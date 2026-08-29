// ------------------------------------------------------------------------------------------------
// <copyright file="SqlSchemaHelperTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests.HandleBarHelpers
{
    using System;
    using System.Linq;

    using HandlebarsDotNet;
    using HandlebarsDotNet.Helpers;

    using Mycelium.Forge.Generator.HandleBarHelpers;

    using uml4net.Extensions;
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Suite of tests for the <see cref="SqlSchemaHelper" /> class.
    /// </summary>
    [TestFixture]
    public class SqlSchemaHelperTestFixture
    {
        private IHandlebars handlebars;
        private IClass accountClass;
        private IClass organizationClass;
        private IClass thingClass;

        /// <summary>
        /// Sets up the test fixture before each test execution.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.handlebars = Handlebars.CreateSharedEnvironment();
            HandlebarsHelpers.Register(this.handlebars);
            this.handlebars.RegisterSqlSchemaHelpers();

            this.accountClass = GetClass("Account");
            this.organizationClass = GetClass("Organization");
            this.thingClass = GetClass("Thing");
        }

        /// <summary>
        /// Verifies that <see cref="SqlSchemaHelper.RegisterSqlSchemaHelpers" /> registers helpers and handles null argument.
        /// </summary>
        [Test]
        public void VerifyRegisterSqlSchemaHelpers()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => SqlSchemaHelper.RegisterSqlSchemaHelpers(null!), Throws.TypeOf<ArgumentNullException>());

                var versionTemplate = this.handlebars.Compile("{{Forge.SQL.ModelVersion}}");
                Assert.That(versionTemplate(new { }), Is.EqualTo("0.1.0"));
            }
        }

        /// <summary>
        /// Verifies that WriteBasicTableDefinitions renders table DDL for classes and guards invalid context.
        /// </summary>
        [Test]
        public void VerifyWriteBasicTableDefinitions()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteBasicTableDefinitions this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<ArgumentException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Does.Contain("CREATE TABLE \"Forge\".\"Account\""));
                Assert.That(accountResult, Does.Contain("\"id\" uuid NOT NULL"));
                Assert.That(accountResult, Does.Contain("PRIMARY KEY (\"id\")"));
            }
        }

        /// <summary>
        /// Verifies that WriteBasicTableThingConstraints renders FK constraints and guards invalid context.
        /// </summary>
        [Test]
        public void VerifyWriteBasicTableThingConstraints()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteBasicTableThingConstraints this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<ArgumentException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Does.Contain("ALTER TABLE \"Forge\".\"Account\" ADD CONSTRAINT \"Account_Thing_FK_Source\" FOREIGN KEY (\"id\") REFERENCES \"Forge\".\"Thing\" (\"id\")"));
            }
        }

        /// <summary>
        /// Verifies that WriteManyToManyTableDefinitionsAndConstraints renders junction tables and guards invalid context.
        /// </summary>
        [Test]
        public void VerifyWriteManyToManyTableDefinitionsAndConstraints()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteManyToManyTableDefinitionsAndConstraints this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<ArgumentException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var orgResult = template(this.organizationClass);
                Assert.That(orgResult, Does.Contain("CREATE TABLE \"Forge\".\"Organization_administrator__Account\""));
                Assert.That(orgResult, Does.Contain("CREATE TABLE \"Forge\".\"Organization_member__Account\""));

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Is.Empty);
            }
        }

        /// <summary>
        /// Verifies that WriteNormalReferenceConstraints renders FK constraints and indexes.
        /// </summary>
        [Test]
        public void VerifyWriteNormalReferenceConstraints()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteNormalReferenceConstraints this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<ArgumentException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Does.Contain("ALTER TABLE \"Forge\".\"Account\" ADD CONSTRAINT"));
                Assert.That(accountResult, Does.Contain("CREATE INDEX"));
            }
        }

        /// <summary>
        /// Verifies that WriteUniversalAttributeIndexes renders the shared createdAt/modifiedAt indexes and guards
        /// invalid context.
        /// </summary>
        [Test]
        public void VerifyWriteUniversalAttributeIndexes()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteUniversalAttributeIndexes this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-collection"), Throws.TypeOf<ArgumentException>());

                var allClasses = GeneratorSetupFixture.XmiReaderResult.Packages
                    .SelectMany(package => package.QueryPackages())
                    .SelectMany(package => package.PackagedElement.OfType<IClass>())
                    .ToList();

                var result = template(allClasses);
                Assert.That(result, Does.Contain("CREATE INDEX \"idx_Thing_classKind_createdAt\" ON \"Forge\".\"Thing\" (\"classKind\", (\"Forge\".jsonb_to_timestamp(\"data\"->>'createdAt')))"));
                Assert.That(result, Does.Contain("CREATE INDEX \"idx_Thing_classKind_modifiedAt\" ON \"Forge\".\"Thing\" (\"classKind\", (\"Forge\".jsonb_to_timestamp(\"data\"->>'modifiedAt')))"));
            }
        }

        /// <summary>
        /// Verifies that WriteClassAttributeIndexes renders one partial index per own-or-inherited scalar
        /// attribute, skips <c>Thing</c> and abstract classes, and guards invalid context.
        /// </summary>
        [Test]
        public void VerifyWriteClassAttributeIndexes()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteClassAttributeIndexes this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<ArgumentException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var namespaceResult = template(GetClass("Namespace"));
                Assert.That(namespaceResult, Is.Empty, "an abstract class's classKind is never a real value, so it gets no indexes of its own");

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Does.Contain("CREATE INDEX \"idx_Thing_Account_status\" ON \"Forge\".\"Thing\" ((\"data\"->>'status')) WHERE \"classKind\" = 'Account'"));
                Assert.That(accountResult, Does.Contain("idx_Thing_Account_shortName"), "shortName is inherited from Namespace, not owned by Account directly");
                Assert.That(accountResult, Does.Not.Contain("idx_Thing_Account_owner"), "owner is a reference property with its own real FK column, not a JSONB attribute");
            }
        }

        /// <summary>
        /// Retrieves a class from the model by name.
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
