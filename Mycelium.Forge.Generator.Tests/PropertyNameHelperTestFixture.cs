// ------------------------------------------------------------------------------------------------
// <copyright file="PropertyNameHelperTestFixture.cs" company="Starion Group S.A.">
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
    /// Exercises the three Handlebars helpers <see cref="PropertyNameHelper"/> registers, against
    /// real <see cref="IProperty"/> instances pulled from the same Forge model every other generator
    /// test fixture already shares via <see cref="GeneratorSetupFixture"/> - no mocking framework is
    /// referenced by this project, and the model already has properties covering every branch these
    /// helpers need (a plain string property, a derived property, and <c>IThing.Id</c>).
    /// </summary>
    /// <remarks>
    /// The two "happy path" branches that a real property can reach (capitalization, and the
    /// non-derived id/string/non-string checks) are already exercised indirectly by
    /// <see cref="ExpectedOutputTestFixture"/>'s golden-file comparisons, since the serializer/
    /// deserializer templates call these helpers for every non-derived property of every
    /// "interesting" class. What isn't reachable through the real generator pipeline is (a) the
    /// three <see cref="HandlebarsDotNet.HandlebarsException"/> guard clauses - every call site
    /// passes exactly one <see cref="IProperty"/> - and (b) <c>WritePropertyName</c>'s
    /// derived-property lower-casing branch, since both templates skip derived properties
    /// (<c>{{#unless this.IsDerived}}</c>) before ever calling the helper. This fixture invokes the
    /// helpers directly, bypassing those template guards, to reach the branches the real pipeline
    /// cannot - the same rationale already applied to <c>DeSerializer.DeserializeArray</c>'s
    /// unreachable guard in <c>DeSerializerTestFixture</c>.
    /// </remarks>
    [TestFixture]
    public class PropertyNameHelperTestFixture
    {
        private IHandlebars handlebars;

        [SetUp]
        public void SetUp()
        {
            this.handlebars = Handlebars.CreateSharedEnvironment();

            // Mirrors HandleBarsGenerator's own setup: without registering the community
            // Handlebars.Net.Helpers package, Handlebars.Net 2.1.6 fails to compile a parenthesised
            // subexpression used as a block helper's argument (e.g. {{#if (Property.IsThingId
            // property)}}), and a helper invoked without a leading '#' silently renders nothing
            // instead of throwing/returning its value.
            HandlebarsHelpers.Register(this.handlebars);

            this.handlebars.RegisterPropertyNameHelper();
        }

        /// <summary>
        /// Mirrors <c>UmlHandleBarsGenerator.QueryAllClasses</c>'s own class lookup (including its
        /// <c>.Single(x =&gt; x.Name == className)</c> resolution, already relied on elsewhere in this
        /// codebase, e.g. <c>UmlCoreJsonDtoSerializerGenerator.GenerateDtoSerializerClassAsync</c>) so
        /// this fixture can reach a real, model-backed <see cref="IProperty"/> without duplicating a
        /// new query pattern.
        /// </summary>
        private static IProperty GetOwnedProperty(string className, string propertyName)
        {
            var umlClass = GeneratorSetupFixture.XmiReaderResult.Packages
                .SelectMany(package => package.QueryPackages())
                .SelectMany(package => package.PackagedElement.OfType<IClass>())
                .Single(x => x.Name == className);

            return umlClass.OwnedAttribute.Single(x => x.Name == propertyName);
        }

        [Test]
        public void Verify_that_WritePropertyName_capitalizes_a_regular_property_name()
        {
            var property = GetOwnedProperty("Organization", "LogoBlobReference");

            var template = this.handlebars.Compile("{{Property.WritePropertyName property}}");

            Assert.That(template(new { property }), Is.EqualTo("LogoBlobReference"));
        }

        [Test]
        public void Verify_that_WritePropertyName_lower_cases_a_derived_property_name()
        {
            var property = GetOwnedProperty("Package", "downloadCount");

            var template = this.handlebars.Compile("{{Property.WritePropertyName property}}");

            Assert.That(template(new { property }), Is.EqualTo("downloadCount"));
        }

        [Test]
        public void Verify_that_WritePropertyName_throws_when_given_no_arguments()
        {
            var template = this.handlebars.Compile("{{Property.WritePropertyName}}");

            Assert.That(() => template(new { }), Throws.TypeOf<HandlebarsException>());
        }

        [Test]
        public void Verify_that_WritePropertyName_throws_when_given_a_non_IProperty_argument()
        {
            var template = this.handlebars.Compile("{{Property.WritePropertyName property}}");

            Assert.That(() => template(new { property = "not-a-property" }), Throws.TypeOf<HandlebarsException>());
        }

        [Test]
        public void Verify_that_IsThingId_returns_true_for_the_Id_property()
        {
            var property = GetOwnedProperty("Thing", "id");

            var template = this.handlebars.Compile("{{#if (Property.IsThingId property)}}yes{{else}}no{{/if}}");

            Assert.That(template(new { property }), Is.EqualTo("yes"));
        }

        [Test]
        public void Verify_that_IsThingId_returns_false_for_a_non_id_property()
        {
            var property = GetOwnedProperty("Thing", "createdAt");

            var template = this.handlebars.Compile("{{#if (Property.IsThingId property)}}yes{{else}}no{{/if}}");

            Assert.That(template(new { property }), Is.EqualTo("no"));
        }

        [Test]
        public void Verify_that_IsThingId_throws_when_given_no_arguments()
        {
            var template = this.handlebars.Compile("{{#if (Property.IsThingId)}}yes{{else}}no{{/if}}");

            Assert.That(() => template(new { }), Throws.TypeOf<HandlebarsException>());
        }

        [Test]
        public void Verify_that_IsThingId_throws_when_given_a_non_IProperty_argument()
        {
            var template = this.handlebars.Compile("{{#if (Property.IsThingId property)}}yes{{else}}no{{/if}}");

            Assert.That(() => template(new { property = "not-a-property" }), Throws.TypeOf<HandlebarsException>());
        }

        [Test]
        public void Verify_that_QueryIsCSharpString_returns_true_for_a_string_property()
        {
            var property = GetOwnedProperty("Scope", "billingEmail");

            var template = this.handlebars.Compile("{{#if (Property.QueryIsCSharpString property)}}yes{{else}}no{{/if}}");

            Assert.That(template(new { property }), Is.EqualTo("yes"));
        }

        [Test]
        public void Verify_that_QueryIsCSharpString_returns_false_for_a_non_string_property()
        {
            var property = GetOwnedProperty("Package", "packageType");

            var template = this.handlebars.Compile("{{#if (Property.QueryIsCSharpString property)}}yes{{else}}no{{/if}}");

            Assert.That(template(new { property }), Is.EqualTo("no"));
        }

        [Test]
        public void Verify_that_QueryIsCSharpString_throws_when_given_no_arguments()
        {
            var template = this.handlebars.Compile("{{#if (Property.QueryIsCSharpString)}}yes{{else}}no{{/if}}");

            Assert.That(() => template(new { }), Throws.TypeOf<HandlebarsException>());
        }

        [Test]
        public void Verify_that_QueryIsCSharpString_throws_when_given_a_non_IProperty_argument()
        {
            var template = this.handlebars.Compile("{{#if (Property.QueryIsCSharpString property)}}yes{{else}}no{{/if}}");

            Assert.That(() => template(new { property = "not-a-property" }), Throws.TypeOf<HandlebarsException>());
        }
    }
}
