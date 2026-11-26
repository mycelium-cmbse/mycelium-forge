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
        private IClass licenseClass;
        private IClass packageClass;
        private IClass thingClass;

        [SetUp]
        public void SetUp()
        {
            this.handlebars = Handlebars.CreateSharedEnvironment();
            HandlebarsHelpers.Register(this.handlebars);
            this.handlebars.RegisterDaoHelper();

            this.countryClass = GetClass("Country");
            this.licenseClass = GetClass("License");
            this.packageClass = GetClass("Package");
            this.thingClass = GetClass("Thing");
        }

        [Test]
        public void VerifyCreateAsyncWriteValueTypeAndSingleReferenceProperties()
        {
            var template = this.handlebars.Compile("{{#Dao.CreateAsyncWriteValueTypeAndSingleReferenceProperties this}}");
            var packageResult = template(this.packageClass);
            var countryResult = template(this.countryClass);
            var thingResult = template(this.thingClass);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<HandlebarsException>());
                Assert.That(packageResult, Does.Contain("VALUES (@id, @classKind, @data - ARRAY['appliedTags', 'createdAt', 'modifiedAt', 'owner', 'packageMaintainer', 'packageOwner', 'packageType', 'version']);"));
                Assert.That(countryResult, Does.Contain("VALUES (@id, @classKind, @data - ARRAY['createdAt', 'modifiedAt', 'owner']);"));
                Assert.That(thingResult, Does.Contain("VALUES (@id, @classKind, @data - ARRAY['createdAt', 'modifiedAt']);"));
            }
        }

        [Test]
        public void VerifyReadAsyncWriteReadSql()
        {
            var template = this.handlebars.Compile("{{#Dao.ReadAsyncWriteReadSQL this}}");
            var licenseResult = template(this.licenseClass);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<HandlebarsException>());
                Assert.That(licenseResult, Does.Contain("(thing.data->>'spdxIdentifier') AS spdx_identifier"));
                Assert.That(licenseResult, Does.Not.Contain("shortName"));
                Assert.That(licenseResult, Does.Not.Contain("short_name"));
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
            var packageResult = template(this.packageClass);
            var countryResult = template(this.countryClass);
            var thingResult = template(this.thingClass);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<HandlebarsException>());
                Assert.That(packageResult, Does.Contain("UPDATE forge.thing SET data = @data - ARRAY['appliedTags', 'createdAt', 'modifiedAt', 'owner', 'packageMaintainer', 'packageOwner', 'packageType', 'version'] WHERE id = @id;"));
                Assert.That(countryResult, Does.Contain("UPDATE forge.thing SET data = @data - ARRAY['createdAt', 'modifiedAt', 'owner'] WHERE id = @id;"));
                Assert.That(thingResult, Does.Contain("UPDATE forge.thing SET data = @data - ARRAY['createdAt', 'modifiedAt'] WHERE id = @id;"));
            }
        }

        [Test]
        public void VerifyWriteMapToDto()
        {
            var template = this.handlebars.Compile("{{#Dao.WriteMapToDto this}}");
            var licenseResult = template(this.licenseClass);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => template("not-a-class"), Throws.TypeOf<HandlebarsException>());
                Assert.That(licenseResult, Does.Contain("SpdxIdentifier = (string)reader[\"spdx_identifier\"]"));
                Assert.That(licenseResult, Does.Not.Contain("ShortName"));
                Assert.That(licenseResult, Does.Not.Contain("short_name"));
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
