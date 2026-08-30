// ------------------------------------------------------------------------------------------------
// <copyright file="ReferenceTypeNameHelperTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests
{
    using System.Linq;

    using HandlebarsDotNet;
    using HandlebarsDotNet.Helpers;

    using Mycelium.Forge.Generator.HandleBarHelpers;

    using uml4net.Classification;
    using uml4net.Extensions;
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Exercises the <see cref="ReferenceTypeNameHelper"/>, against real <see cref="IProperty"/>
    /// instances pulled from the same Forge model every other generator test fixture already shares
    /// via <see cref="GeneratorSetupFixture"/> - no mocking framework is referenced by this project.
    /// </summary>
    /// <remarks>
    /// The two "happy path" branches - a property whose target class resolves, and one whose
    /// <see cref="IProperty.Type"/> is <see langword="null"/> in the model (falls back to
    /// <c>"unknown"</c>) - are already exercised indirectly by <see cref="ExpectedOutputTestFixture"/>'s
    /// golden-file comparisons, since <c>Package.PackageType</c> and <c>APIKey.Permissions</c> both
    /// appear there. What isn't reachable through the real generator pipeline is the two
    /// <see cref="HandlebarsException"/> guard clauses - every call site in the serializer template
    /// passes exactly one <see cref="IProperty"/> - so this fixture invokes the helper directly,
    /// bypassing that template guard, the same rationale already applied in
    /// <see cref="PropertyNameHelperTestFixture"/>.
    /// </remarks>
    [TestFixture]
    public class ReferenceTypeNameHelperTestFixture
    {
        private IHandlebars handlebars;

        [SetUp]
        public void SetUp()
        {
            this.handlebars = Handlebars.CreateSharedEnvironment();

            HandlebarsHelpers.Register(this.handlebars);

            this.handlebars.RegisterReferenceTypeNameHelper();
        }

        private static IProperty GetOwnedProperty(string className, string propertyName)
        {
            var umlClass = GeneratorSetupFixture.XmiReaderResult.Packages
                .SelectMany(package => package.QueryPackages())
                .SelectMany(package => package.PackagedElement.OfType<IClass>())
                .Single(x => x.Name == className);

            return umlClass.OwnedAttribute.Single(x => x.Name == propertyName);
        }

        [Test]
        public void Verify_that_WriteReferenceTypeName_writes_the_referenced_class_name()
        {
            var property = GetOwnedProperty("Package", "packageType");

            var template = this.handlebars.Compile("{{Property.WriteReferenceTypeName property}}");

            Assert.That(template(new { property }), Is.EqualTo("PackageType"));
        }

        [Test]
        public void Verify_that_WriteReferenceTypeName_writes_unknown_when_the_property_type_does_not_resolve()
        {
            var property = GetOwnedProperty("APIKey", "permissions");

            var template = this.handlebars.Compile("{{Property.WriteReferenceTypeName property}}");

            Assert.That(template(new { property }), Is.EqualTo("unknown"));
        }

        [Test]
        public void Verify_that_WriteReferenceTypeName_throws_when_given_no_arguments()
        {
            var template = this.handlebars.Compile("{{Property.WriteReferenceTypeName}}");

            Assert.That(() => template(new { }), Throws.TypeOf<HandlebarsException>());
        }

        [Test]
        public void Verify_that_WriteReferenceTypeName_throws_when_given_a_non_IProperty_argument()
        {
            var template = this.handlebars.Compile("{{Property.WriteReferenceTypeName property}}");

            Assert.That(() => template(new { property = "not-a-property" }), Throws.TypeOf<HandlebarsException>());
        }
    }
}
