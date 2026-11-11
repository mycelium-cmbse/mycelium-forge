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

    [TestFixture]
    public class SqlSchemaHelperTestFixture
    {
        private IHandlebars handlebars;
        private IClass accountClass;
        private IClass organizationClass;
        private IClass thingClass;

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

        [Test]
        public void VerifyDeleteBaseTableTriggerFunctions()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.DeleteBaseTableTriggerFunctions this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-collection"), Throws.TypeOf<HandlebarsException>());

                var allClasses = GeneratorSetupFixture.XmiReaderResult.Packages
                    .SelectMany(package => package.QueryPackages())
                    .SelectMany(package => package.PackagedElement.OfType<IClass>())
                    .ToList();

                var result = template(allClasses);
                Assert.That(result, Does.Contain("CREATE OR REPLACE FUNCTION forge.scope_delete()"));
                Assert.That(result, Does.Contain("EXECUTE 'DELETE FROM forge.scope WHERE id = $1' USING OLD.id;"));
            }
        }

        [Test]
        public void VerifyRegisterSqlSchemaHelpers()
        {
            Assert.That(() => SqlSchemaHelper.RegisterSqlSchemaHelpers(null!), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void VerifyWriteBaseTableDeleteTriggers()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteBaseTableDeleteTriggers this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<HandlebarsException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Does.Contain("CREATE OR REPLACE TRIGGER trg_scope_on_account_delete"));
                Assert.That(accountResult, Does.Contain("AFTER DELETE ON forge.account"));
                Assert.That(accountResult, Does.Contain("EXECUTE FUNCTION forge.scope_delete()"));
            }
        }

        [Test]
        public void VerifyWriteBasicTableDefinitions()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteBasicTableDefinitions this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<HandlebarsException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Does.Contain("CREATE TABLE forge.account"));
                Assert.That(accountResult, Does.Contain("id uuid NOT NULL"));
                Assert.That(accountResult, Does.Contain("owner uuid NOT NULL"));
                Assert.That(accountResult, Does.Contain("PRIMARY KEY (id)"));
            }
        }

        [Test]
        public void VerifyWriteBasicTableThingConstraints()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteBasicTableThingConstraints this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<HandlebarsException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Does.Contain("ALTER TABLE forge.account ADD CONSTRAINT account_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id)"));
            }
        }

        [Test]
        public void VerifyWriteBasicTableThingDeleteTriggers()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteBasicTableThingDeleteTriggers this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<HandlebarsException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Does.Contain("CREATE OR REPLACE TRIGGER trg_thing_delete"));
                Assert.That(accountResult, Does.Contain("AFTER DELETE ON forge.account"));
                Assert.That(accountResult, Does.Contain("EXECUTE FUNCTION forge.thing_delete()"));
            }
        }

        [Test]
        public void VerifyWriteManyToManyTableDefinitionsAndConstraints()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteManyToManyTableDefinitionsAndConstraints this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<HandlebarsException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var orgResult = template(this.organizationClass);
                Assert.That(orgResult, Does.Contain("CREATE TABLE forge.organization_administrator__account"));
                Assert.That(orgResult, Does.Contain("CREATE TABLE forge.organization_member__account"));

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Is.Empty);
            }
        }

        [Test]
        public void VerifyWriteNormalReferenceConstraints()
        {
            var template = this.handlebars.Compile("{{#Forge.SQL.WriteNormalReferenceConstraints this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<HandlebarsException>());

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Is.Empty);

                var accountResult = template(this.accountClass);
                Assert.That(accountResult, Does.Contain("ALTER TABLE forge.account ADD CONSTRAINT"));
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
