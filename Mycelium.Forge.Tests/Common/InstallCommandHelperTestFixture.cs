// ------------------------------------------------------------------------------------------------
// <copyright file="InstallCommandHelperTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Common
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Test fixture for <see cref="InstallCommandHelper" />.
    /// </summary>
    [TestFixture]
    public class InstallCommandHelperTestFixture
    {
        /// <summary>
        /// Verifies that <see cref="InstallCommandHelper.GenerateInstallCommands(string, string, string)" /> generates commands
        /// for all methods.
        /// </summary>
        [Test]
        public void VerifyGenerateInstallCommands()
        {
            var commands = InstallCommandHelper.GenerateInstallCommands("starion", "ecss-mm-pwr", "1.3.0");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(commands, Has.Count.EqualTo(4));
                Assert.That(commands[InstallCommandConstants.ForgeCli], Is.EqualTo("forge add @starion/ecss-mm-pwr@^1.3.0"));
                Assert.That(commands[InstallCommandConstants.SysMlV2Import], Is.EqualTo("import ecss_mm_pwr::*;"));
                Assert.That(commands[InstallCommandConstants.Manifest], Is.EqualTo("@starion/ecss-mm-pwr = \"^1.3.0\""));
                Assert.That(commands[InstallCommandConstants.Purl], Is.EqualTo("pkg:forge/starion/ecss-mm-pwr@1.3.0"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="InstallCommandHelper.GeneratePurl(string, string, string)" /> generates valid purl syntax.
        /// </summary>
        [Test]
        public void VerifyGeneratePurl()
        {
            var purl = InstallCommandHelper.GeneratePurl("starion", "ecss-mm-pwr", "1.3.0");

            Assert.That(purl, Is.EqualTo("pkg:forge/starion/ecss-mm-pwr@1.3.0"));
        }

        /// <summary>
        /// Verifies that <see cref="InstallCommandHelper.GenerateSysMlV2Import(string)" /> generates valid SysML v2 import syntax.
        /// </summary>
        [Test]
        public void VerifyGenerateSysMlV2Import()
        {
            var import = InstallCommandHelper.GenerateSysMlV2Import("ecss-mm-pwr");

            Assert.That(import, Is.EqualTo("import ecss_mm_pwr::*;"));
        }
    }
}
