// ------------------------------------------------------------------------------------------------
// <copyright file="PropertyHelperTestFixture.cs" company="Starion Group S.A.">
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

    using Mycelium.Forge.Generator.Extensions;

    using uml4net.Classification;
    using uml4net.Extensions;
    using uml4net.HandleBars;
    using uml4net.StructuredClassifiers;

    using PropertyHelper = Mycelium.Forge.Generator.HandleBarHelpers.PropertyHelper;

    /// <summary>
    /// Suite of tests for the <see cref="PropertyHelper" /> class.
    /// </summary>
    [TestFixture]
    public class PropertyHelperTestFixture
    {
        private IHandlebars handlebars;
        private IClass accountClass;
        private IClass thingClass;
        private IProperty accountOwnerProperty;
        private IProperty thingIdProperty;

        /// <summary>
        /// Sets up the test fixture before each test execution.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.handlebars = Handlebars.CreateSharedEnvironment();
            HandlebarsHelpers.Register(this.handlebars);
            this.handlebars.RegisterClassHelper();
            this.handlebars.RegisterPropertyHelper();
            PropertyHelper.RegisterPropertyHelper(this.handlebars);

            this.accountClass = GetClass("Account");
            this.thingClass = GetClass("Thing");
            this.accountOwnerProperty = this.accountClass.QueryDtoInterfaceProperties().Single(x => x.Name == "owner");
            this.thingIdProperty = this.thingClass.OwnedAttribute.Single(x => x.Name == "id");
        }

        /// <summary>
        /// Verifies that <see cref="PropertyHelper.RegisterPropertyHelper" /> registers helpers and executes them correctly.
        /// </summary>
        [Test]
        public void VerifyRegisterPropertyHelper()
        {
            var interfacePropertiesTemplate = this.handlebars.Compile("{{#each (Class.QueryDtoInterfaceProperties this)}}{{this.Name}} {{/each}}");
            var classPropertiesTemplate = this.handlebars.Compile("{{#each (Class.QueryDtoClassProperties this)}}{{this.Name}} {{/each}}");
            var writeInterfacePropertyTemplate = this.handlebars.Compile("{{Property.WriteForDTOInterface this}}");
            var writeClassPropertyTemplate = this.handlebars.Compile("{{#with this as |class|}}{{#each (Class.QueryDtoClassProperties class)}}{{Property.WriteForDTOClass this class}}{{/each}}{{/with}}");
            var writeDocumentationTemplate = this.handlebars.Compile("{{Property.WriteDocumentation this}}");
            var interfacePropertiesResult = interfacePropertiesTemplate(this.accountClass);
            var classPropertiesResult = classPropertiesTemplate(this.accountClass);
            var writeInterfacePropertyResult = writeInterfacePropertyTemplate(this.accountOwnerProperty);
            var writeClassPropertyResult = writeClassPropertyTemplate(this.accountClass);
            var writeOwnerDocumentationResult = writeDocumentationTemplate(this.accountOwnerProperty);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => PropertyHelper.RegisterPropertyHelper(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(interfacePropertiesResult, Does.Contain("owner"));
                Assert.That(classPropertiesResult, Does.Contain("owner"));
                Assert.That(writeInterfacePropertyResult, Does.Contain("Guid Owner { get; set; }"));
                Assert.That(writeClassPropertyResult, Does.Contain("public Guid Owner { get; set; }"));
                Assert.That(writeOwnerDocumentationResult, Does.Contain("The unique identifier of the owning Forge."));
                Assert.That(() => interfacePropertiesTemplate("not-a-class"), Throws.TypeOf<ArgumentException>());
                Assert.That(() => classPropertiesTemplate("not-a-class"), Throws.TypeOf<ArgumentException>());
                Assert.That(() => writeInterfacePropertyTemplate("not-a-property"), Throws.TypeOf<ArgumentException>());
                Assert.That(() => writeDocumentationTemplate("not-a-property"), Throws.TypeOf<ArgumentException>());
            }
        }

        /// <summary>
        /// Verifies that Decorator.WriteImplementsAttribute writes the implements attribute and guards invalid context.
        /// </summary>
        [Test]
        public void VerifyWriteImplementsAttribute()
        {
            var implementsTemplate = this.handlebars.Compile("{{Decorator.WriteImplementsAttribute this}}");
            var idResult = implementsTemplate(this.thingIdProperty);
            var ownerResult = implementsTemplate(this.accountOwnerProperty);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => implementsTemplate("not-a-property"), Throws.TypeOf<ArgumentException>());
                Assert.That(idResult, Does.Contain("[Implements(implementation: \"IThing.Id\")]"));
                Assert.That(ownerResult, Does.Contain("[Implements(implementation: \"IAccount.Owner\")]"));
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
