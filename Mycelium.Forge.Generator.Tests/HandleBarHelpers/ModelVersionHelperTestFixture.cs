// ------------------------------------------------------------------------------------------------
// <copyright file="ModelVersionHelperTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests.HandleBarHelpers
{
    using System;

    using HandlebarsDotNet;
    using HandlebarsDotNet.Helpers;

    using Mycelium.Forge.Generator.HandleBarHelpers;

    /// <summary>
    /// Suite of tests for the <see cref="ModelVersionHelper" /> class.
    /// </summary>
    [TestFixture]
    public class ModelVersionHelperTestFixture
    {
        private IHandlebars handlebars;

        /// <summary>
        /// Sets up the test environment before each test execution.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.handlebars = Handlebars.CreateSharedEnvironment();
            HandlebarsHelpers.Register(this.handlebars);
        }

        /// <summary>
        /// Verifies that <see cref="ModelVersionHelper.RegisterModelVersionHelper" /> guards against null arguments.
        /// </summary>
        [Test]
        public void VerifyRegisterModelVersionHelper()
        {
            Assert.That(() => ModelVersionHelper.RegisterModelVersionHelper(null!), Throws.TypeOf<ArgumentNullException>());
        }

        /// <summary>
        /// Verifies that <see cref="ModelVersionHelper.WriteModelVersion" /> correctly writes the model version.
        /// </summary>
        [Test]
        public void VerifyWriteModelVersion()
        {
            this.handlebars.RegisterModelVersionHelper("1.0.0-fallback");

            var templateArg = this.handlebars.Compile("{{Forge.ModelVersion \"3.0.0\"}}");
            var templateNoArg = this.handlebars.Compile("{{Forge.ModelVersion}}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(templateArg(new { }), Is.EqualTo("3.0.0"));
                Assert.That(templateNoArg(new { }), Is.EqualTo("1.0.0-fallback"));
            }
        }
    }
}
