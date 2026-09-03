// ------------------------------------------------------------------------------------------------
// <copyright file="ExpectedOutputTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Mycelium.Forge.Generator.Extensions;
    using Mycelium.Forge.Generator.Generators;

    using uml4net.Reporting.Generators;

    /// <summary>
    /// Follows the uml4net code generation tutorial's own verification pattern
    /// (https://github.com/STARIONGROUP/uml4net/wiki/Code-Generation-Tutorial): a small,
    /// hand-authored, human-reviewed sample of "interesting" classes lives under
    /// <c>Expected/</c> and freshly generated output is compared against it directly. This is an
    /// automated check that catches template regressions on every test run; regenerating the *full*
    /// set of DTOs/enums for review is instead a deliberate, manual step - see
    /// <see cref="AutoGenDtoRegenerationTests" />/<see cref="AutoGenEnumRegenerationTests" />.
    /// </summary>
    /// <remarks>
    /// The class names below are exactly the "interesting classes" the tutorial's step 2 calls for
    /// determining with <c>ModelInspector</c> - not a hand-picked sample. That determination isn't
    /// just a one-off, manually reproduced CLI run; it's enforced on every test run by
    /// <see cref="Verify_that_the_interesting_class_selection_still_matches_ModelInspector" />, which
    /// calls the same <see cref="ModelInspector.QueryInterestingClasses" /> the <c>uml4nettools inspect</c>
    /// CLI is built on, directly against the live model, and fails - printing the current model's
    /// actual class names - the moment its result diverges from <see cref="ConcreteInterestingClasses" />/
    /// <see cref="AbstractInterestingClasses" /> below. That divergence means the Forge model changed;
    /// update both arrays and the corresponding <c>Expected/</c> golden files to match.
    /// </remarks>
    [TestFixture]
    public class ExpectedOutputTestFixture
    {
        private static readonly string[] ConcreteInterestingClasses =
        [
            "APIKey", "Forge", "Organization", "Package", "PackageInvitation", "PackageVersion", "ProfileLink"
        ];

        private static readonly string[] AbstractInterestingClasses = ["Invitation", "Scope", "Thing"];

        [Test]
        public void Verify_that_the_interesting_class_selection_still_matches_ModelInspector()
        {
            var modelInspector = new ModelInspector();

            var interestingClasses = GeneratorSetupFixture.XmiReaderResult.Packages
                .SelectMany(package => modelInspector.QueryInterestingClasses(package))
                .Distinct()
                .ToList();

            var actualConcrete = interestingClasses
                .Where(x => !x.IsAbstract).Select(x => x.Name).OrderBy(x => x).ToArray();

            var actualAbstract = interestingClasses
                .Where(x => x.IsAbstract).Select(x => x.Name).OrderBy(x => x).ToArray();

            Assert.That(actualConcrete, Is.EqualTo([.. ConcreteInterestingClasses.OrderBy(x => x)]),
                $"ModelInspector.QueryInterestingClasses() no longer reports the same concrete classes as " +
                $"{nameof(ConcreteInterestingClasses)} in this fixture (see 'actual'/'expected' above). The " +
                $"Forge model most likely changed - update {nameof(ConcreteInterestingClasses)} and the " +
                "corresponding Expected/AutoGenDto golden files to match.");

            Assert.That(actualAbstract, Is.EqualTo([.. AbstractInterestingClasses.OrderBy(x => x)]),
                $"ModelInspector.QueryInterestingClasses() no longer reports the same abstract classes as " +
                $"{nameof(AbstractInterestingClasses)} in this fixture (see 'actual'/'expected' above). The " +
                $"Forge model most likely changed - update {nameof(AbstractInterestingClasses)} and the " +
                "corresponding Expected/AutoGenDto golden files to match.");
        }

        [TestCaseSource(nameof(ConcreteInterestingClasses))]
        public async Task Verify_that_the_expected_dto_class_is_generated(string className)
        {
            var generator = new UmlCoreDtoGenerator();

            var generated = await generator.GenerateDataTransferObjectClassAsync(GeneratorSetupFixture.XmiReaderResult, className);

            var expected = await File.ReadAllTextAsync(
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Expected", "AutoGenDto", $"{className}.cs"));

            Assert.That(generated.NormalizeLineEndings(), Is.EqualTo(expected.NormalizeLineEndings()));
        }

        [TestCaseSource(nameof(ConcreteInterestingClasses))]
        [TestCaseSource(nameof(AbstractInterestingClasses))]
        public async Task Verify_that_the_expected_dto_interface_is_generated(string className)
        {
            var generator = new UmlCoreDtoGenerator();

            var generated = await generator.GenerateDataTransferObjectInterfaceAsync(GeneratorSetupFixture.XmiReaderResult, className);

            var expected = await File.ReadAllTextAsync(
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Expected", "AutoGenDto", $"I{className}.cs"));

            Assert.That(generated.NormalizeLineEndings(), Is.EqualTo(expected.NormalizeLineEndings()));
        }

        [TestCase("VisibilityKind")]
        public async Task Verify_that_the_expected_enumeration_is_generated(string enumerationName)
        {
            var generator = new UmlCoreEnumGenerator();

            var generated = await generator.GenerateEnumerationAsync(GeneratorSetupFixture.XmiReaderResult, enumerationName);

            var expected = await File.ReadAllTextAsync(
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Expected", "AutoGenEnum", $"{enumerationName}.cs"));

            Assert.That(generated.NormalizeLineEndings(), Is.EqualTo(expected.NormalizeLineEndings()));
        }

        [TestCase("VisibilityKind")]
        public async Task Verify_that_the_expected_enumeration_provider_is_generated(string enumerationName)
        {
            var generator = new UmlCoreEnumProviderGenerator();

            var outputDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "_Forge.Common.AutoGenEnumProvider"));
            outputDirectory.Create();

            var generated = await generator.GenerateEnumerationProviderAsync(GeneratorSetupFixture.XmiReaderResult, outputDirectory, enumerationName);

            var expected = await File.ReadAllTextAsync(
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Expected", "AutoGenEnumProvider", $"{enumerationName}Provider.cs"));

            Assert.That(generated.NormalizeLineEndings(), Is.EqualTo(expected.NormalizeLineEndings()));
        }

        [TestCaseSource(nameof(ConcreteInterestingClasses))]
        public async Task Verify_that_the_expected_serializer_is_generated(string className)
        {
            var generator = new UmlCoreJsonDtoSerializerGenerator();

            var generated = await generator.GenerateDtoSerializerClassAsync(GeneratorSetupFixture.XmiReaderResult, className);

            var expected = await File.ReadAllTextAsync(
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Expected", "AutoGenSerializer", $"{className}Serializer.cs"));

            Assert.That(generated.NormalizeLineEndings(), Is.EqualTo(expected.NormalizeLineEndings()));
        }

        [TestCaseSource(nameof(ConcreteInterestingClasses))]
        public async Task Verify_that_the_expected_deserializer_is_generated(string className)
        {
            var generator = new UmlCoreJsonDtoDeSerializerGenerator();

            var generated = await generator.GenerateDtoDeSerializerClassAsync(GeneratorSetupFixture.XmiReaderResult, className);

            var expected = await File.ReadAllTextAsync(
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Expected", "AutoGenDeSerializer", $"{className}DeSerializer.cs"));

            Assert.That(generated.NormalizeLineEndings(), Is.EqualTo(expected.NormalizeLineEndings()));
        }

        /// <summary>
        /// Verifies that the generated DTO comparer class for each concrete interesting class matches the expected golden file.
        /// </summary>
        /// <param name="className">The name of the class being tested.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [TestCaseSource(nameof(ConcreteInterestingClasses))]
        public async Task Verify_that_the_expected_dto_comparer_is_generated(string className)
        {
            var generator = new UmlCoreDtoComparerGenerator();

            var generated = await generator.GenerateDtoComparerClassAsync(GeneratorSetupFixture.XmiReaderResult, className);

            var expected = await File.ReadAllTextAsync(
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Expected", "AutoGenDtoComparer", $"{className}Comparer.cs"));

            Assert.That(generated.NormalizeLineEndings(), Is.EqualTo(expected.NormalizeLineEndings()));
        }

        /// <summary>
        /// Verifies that the expected DAO class is generated.
        /// </summary>
        /// <param name="className">The name of the class being tested.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [TestCase("Package")]
        public async Task Verify_that_the_expected_dao_is_generated(string className)
        {
            var generator = new UmlCoreDaoGenerator();

            var generated = await generator.GenerateDaoClassAsync(GeneratorSetupFixture.XmiReaderResult, className);

            var expected = await File.ReadAllTextAsync(
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Expected", "AutoGenDao", $"{className}Dao.cs"));

            Assert.That(generated.NormalizeLineEndings(), Is.EqualTo(expected.NormalizeLineEndings()));
        }

        /// <summary>
        /// Verifies that the expected DTO validator class for each concrete interesting class matches the expected golden file.
        /// </summary>
        /// <param name="className">The name of the class being tested.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [TestCaseSource(nameof(ConcreteInterestingClasses))]
        public async Task Verify_that_the_expected_dto_validator_is_generated(string className)
        {
            var generator = new UmlCoreDtoValidatorGenerator();

            var generated = await generator.GenerateDtoValidatorClassAsync(GeneratorSetupFixture.XmiReaderResult, className);

            var expected = await File.ReadAllTextAsync(
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Expected", "AutoGenDtoValidator", $"{className}Validator.cs"));

            Assert.That(generated.NormalizeLineEndings(), Is.EqualTo(expected.NormalizeLineEndings()));
        }

        /// <summary>
        /// Verifies that the expected service class is generated.
        /// </summary>
        /// <param name="className">The name of the class being tested.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [TestCase("Package")]
        public async Task Verify_that_the_expected_service_is_generated(string className)
        {
            var generator = new UmlCoreServiceGenerator();

            var generated = await generator.GenerateServiceClassAsync(GeneratorSetupFixture.XmiReaderResult, className);

            var expected = await File.ReadAllTextAsync(
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Expected", "AutoGenService", $"{className}Service.cs"));

            Assert.That(generated.NormalizeLineEndings(), Is.EqualTo(expected.NormalizeLineEndings()));
        }

        /// <summary>
        /// Verifies that the expected read filter class is generated.
        /// </summary>
        /// <param name="className">The name of the class being tested.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [TestCase("Package")]
        public async Task Verify_that_the_expected_read_filter_is_generated(string className)
        {
            var generator = new UmlCoreReadFilterGenerator();

            var generated = await generator.GenerateReadFilterClassAsync(GeneratorSetupFixture.XmiReaderResult, className);

            var expected = await File.ReadAllTextAsync(
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Expected", "AutoGenReadFilter", $"{className}ReadFilter.cs"));

            Assert.That(generated.NormalizeLineEndings(), Is.EqualTo(expected.NormalizeLineEndings()));
        }
    }
}
