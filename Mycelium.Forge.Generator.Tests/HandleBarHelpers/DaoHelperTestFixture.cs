// ------------------------------------------------------------------------------------------------
// <copyright file="DaoHelperTestFixture.cs" company="Starion Group S.A.">
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
    public class DaoHelperTestFixture
    {
        private IHandlebars handlebars;
        private IClass countryClass;
        private IClass packageClass;
        private IClass thingClass;

        [SetUp]
        public void SetUp()
        {
            this.handlebars = Handlebars.CreateSharedEnvironment();
            HandlebarsHelpers.Register(this.handlebars);
            this.handlebars.RegisterDaoHelper();

            this.countryClass = GetClass("Country");
            this.packageClass = GetClass("Package");
            this.thingClass = GetClass("Thing");
        }

        [Test]
        public void VerifyCreateAsyncWriteValueTypeAndSingleReferenceProperties()
        {
            var template = this.handlebars.Compile("{{#Dao.CreateAsyncWriteValueTypeAndSingleReferenceProperties this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<HandlebarsException>());

                var packageResult = template(this.packageClass);
                Assert.That(packageResult, Does.Contain("VALUES (@id, @classKind, @data - ARRAY['owner', 'packageMaintainer', 'packageOwner', 'packageType', 'version']);"));

                var countryResult = template(this.countryClass);
                Assert.That(countryResult, Does.Contain("VALUES (@id, @classKind, @data - ARRAY['owner']);"));

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Does.Contain("VALUES (@id, @classKind, @data);"));
            }
        }

        [Test]
        public void VerifyRegisterDaoHelper()
        {
            Assert.That(() => DaoHelper.RegisterDaoHelper(null!), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void VerifyUpdateAsyncWriteValueTypeAndSingleReferenceProperties()
        {
            var template = this.handlebars.Compile("{{#Dao.UpdateAsyncWriteValueTypeAndSingleReferenceProperties this}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<HandlebarsException>());

                var packageResult = template(this.packageClass);
                Assert.That(packageResult, Does.Contain("UPDATE forge.thing SET data = @data - ARRAY['owner', 'packageMaintainer', 'packageOwner', 'packageType', 'version'] WHERE id = @id;"));

                var countryResult = template(this.countryClass);
                Assert.That(countryResult, Does.Contain("UPDATE forge.thing SET data = @data - ARRAY['owner'] WHERE id = @id;"));

                var thingResult = template(this.thingClass);
                Assert.That(thingResult, Does.Contain("UPDATE forge.thing SET data = @data WHERE id = @id;"));
            }
        }

        private static IClass GetClass(string className)
        {
            return GeneratorSetupFixture.XmiReaderResult.Packages
                .SelectMany(package => package.QueryPackages())
                .SelectMany(package => package.PackagedElement.OfType<IClass>())
                .Single(x => x.Name == className);
        }
    }
}
