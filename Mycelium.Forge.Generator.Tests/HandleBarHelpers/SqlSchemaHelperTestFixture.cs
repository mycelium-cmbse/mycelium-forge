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
        /// Verifies that DeleteBaseTableTriggerFunctions renders trigger functions and guards invalid context.
        /// </summary>
        [Test]
        public void VerifyDeleteBaseTableTriggerFunctions()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.DeleteBaseTableTriggerFunctions this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-collection"), Throws.TypeOf<ArgumentException>());

                var allClasses = GeneratorSetupFixture.XmiReaderResult.Packages
                    .SelectMany(package => package.QueryPackages())
                    .SelectMany(package => package.PackagedElement.OfType<IClass>())
                    .ToList();

                var result = template(allClasses);
                Assert.That(result, Does.Contain("CREATE OR REPLACE FUNCTION \"Forge\".scope_delete()"));
                Assert.That(result, Does.Contain("EXECUTE 'DELETE FROM \"Forge\".\"Scope\" WHERE id = $1' USING OLD.id;"));
            }
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
        /// Verifies that WriteBaseTableDeleteTriggers renders base table triggers and guards invalid context.
        /// </summary>
        [Test]
        public void VerifyWriteBaseTableDeleteTriggers()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteBaseTableDeleteTriggers this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<ArgumentException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Does.Contain("CREATE OR REPLACE TRIGGER trg_scope_on_account_delete"));
                Assert.That(accountResult, Does.Contain("AFTER DELETE ON \"Forge\".\"Account\""));
                Assert.That(accountResult, Does.Contain("EXECUTE FUNCTION \"Forge\".scope_delete()"));
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
                Assert.That(accountResult, Does.Contain("\"owner\" uuid NOT NULL"));
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
        /// Verifies that WriteBasicTableThingDeleteTriggers renders thing delete triggers and guards invalid context.
        /// </summary>
        [Test]
        public void VerifyWriteBasicTableThingDeleteTriggers()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteBasicTableThingDeleteTriggers this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<ArgumentException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Does.Contain("CREATE OR REPLACE TRIGGER trg_thing_delete"));
                Assert.That(accountResult, Does.Contain("AFTER DELETE ON \"Forge\".\"Account\""));
                Assert.That(accountResult, Does.Contain("EXECUTE FUNCTION \"Forge\".thing_delete()"));
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
