// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDetailsActionsViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.PackageDetails
{
    using System;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Model;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.PackageDetails;

    /// <summary>
    /// Test fixture for <see cref="PackageDetailsActionsViewModel" />.
    /// </summary>
    [TestFixture]
    public class PackageDetailsActionsViewModelTestFixture
    {
        private Mock<INotificationService> notificationServiceMock;
        private PackageDetailsActionsViewModel viewModel;

        /// <summary>
        /// Sets up the test context and mocks before each test execution.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.notificationServiceMock = new Mock<INotificationService>();
            this.viewModel = new PackageDetailsActionsViewModel(this.notificationServiceMock.Object);
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetailsActionsViewModel.Initialize" /> populates properties and commands.
        /// </summary>
        [Test]
        public void VerifyInitialize()
        {
            var org = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Starion Group",
                ShortName = "starion"
            };

            var package = new Package
            {
                Id = Guid.NewGuid(),
                Name = "ECSS-MM-PWR",
                ShortName = "ecss-mm-pwr",
                Owner = org.Id
            };

            var version = new PackageVersion
            {
                Id = Guid.NewGuid(),
                Owner = package.Id,
                Version = "1.3.0"
            };

            this.viewModel.Initialize(package, org, version, true);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Package, Is.EqualTo(package));
                Assert.That(this.viewModel.Organization, Is.EqualTo(org));
                Assert.That(this.viewModel.SelectedVersion, Is.EqualTo(version));
                Assert.That(this.viewModel.IsUserAdmin, Is.True);
                Assert.That(this.viewModel.InstallCommands, Has.Count.GreaterThan(0));
            }
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetailsActionsViewModel.MigrateInBloom" /> sends a notification.
        /// </summary>
        [Test]
        public void VerifyMigrateInBloom()
        {
            var package = new Package
            {
                Id = Guid.NewGuid(),
                Name = "ECSS-MM-PWR",
                ShortName = "ecss-mm-pwr"
            };

            this.viewModel.Package = package;

            var result = new MigrateInBloomResult
            {
                ProjectName = "Spacecraft Mission",
                VersionConstraint = "^1.3.0"
            };

            this.viewModel.MigrateInBloom(result);

            this.notificationServiceMock.Verify(
                x => x.AddNotification(
                    It.Is<string>(msg => msg.Contains("ECSS-MM-PWR") && msg.Contains("Spacecraft Mission")),
                    "Migration Initiated",
                    NotificationType.Success),
                Times.Once);
        }
    }
}
