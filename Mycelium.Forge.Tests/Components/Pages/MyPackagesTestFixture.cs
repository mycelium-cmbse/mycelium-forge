// ------------------------------------------------------------------------------------------------
// <copyright file="MyPackagesTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages;
    using Mycelium.Forge.ViewModels.MyPackages;

    /// <summary>
    /// Suite of tests for the <see cref="MyPackages" /> component.
    /// </summary>
    [TestFixture]
    public class MyPackagesTestFixture
    {
        private BunitContext context;
        private Mock<IMyPackagesViewModel> viewModelMock;
        private Package starionPackage;
        private Package esaPackage;

        /// <summary>
        /// Sets up mock dependencies and test context before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IMyPackagesViewModel>();

            this.starionPackage = new Package
            {
                Id = Guid.NewGuid(),
                Name = "ECSS-MM-PWR",
                ShortName = "ecss-mm-pwr",
                Visibility = VisibilityKind.PUBLIC
            };

            this.esaPackage = new Package
            {
                Id = Guid.NewGuid(),
                Name = "Internal-Core",
                ShortName = "internal-core",
                Visibility = VisibilityKind.PRIVATE
            };

            List<IPackage> packages = [this.starionPackage, this.esaPackage];

            this.viewModelMock.Setup(x => x.Packages).Returns(packages);
            this.viewModelMock.Setup(x => x.GetPublisher(this.starionPackage)).Returns("@starion");
            this.viewModelMock.Setup(x => x.GetPublisher(this.esaPackage)).Returns("@esa");
            this.viewModelMock.Setup(x => x.GetRole(It.IsAny<IPackage>())).Returns(PackageInvitationKind.OWNER);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        /// <summary>
        /// Tears down the bUnit context after each test.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        /// <summary>
        /// Verifies that packages are filtered correctly based on the selected publisher.
        /// </summary>
        [Test]
        public void VerifyFilteredPackages()
        {
            var myPackagesPage = this.context.Render<MyPackages>();

            var allPackages = myPackagesPage.Instance.FilteredPackages();
            myPackagesPage.Instance.SelectedPublisher = "@starion";
            var starionPackages = myPackagesPage.Instance.FilteredPackages();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(allPackages, Has.Count.EqualTo(2));
                Assert.That(starionPackages, Has.Count.EqualTo(1));
            }
        }

        /// <summary>
        /// Verifies the CSS classes applied to publisher filter chips.
        /// </summary>
        [Test]
        public void VerifyGetPublisherChipClass()
        {
            var myPackagesPage = this.context.Render<MyPackages>();
            myPackagesPage.Instance.SelectedPublisher = "all";

            var allClass = myPackagesPage.Instance.GetPublisherChipClass("all");
            var otherClass = myPackagesPage.Instance.GetPublisherChipClass("esa");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(allClass, Does.Contain("text-primary font-semibold"));
                Assert.That(otherClass, Does.Contain("text-secondary-foreground font-medium"));
            }
        }

        /// <summary>
        /// Verifies that the publisher filter options list is correctly generated with counts.
        /// </summary>
        [Test]
        public void VerifyGetPublisherFilterOptions()
        {
            var myPackagesPage = this.context.Render<MyPackages>();
            var options = myPackagesPage.Instance.GetPublisherFilterOptions();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(options, Has.Count.EqualTo(3));
                Assert.That(options[0].Key, Is.EqualTo("all"));
            }
        }

        /// <summary>
        /// Verifies that the correct badge variant is returned for public and private visibility.
        /// </summary>
        [Test]
        public void VerifyGetVisibilityBadgeVariant()
        {
            var publicPkg = new Package { Visibility = VisibilityKind.PUBLIC };
            var privatePkg = new Package { Visibility = VisibilityKind.PRIVATE };

            var publicVariant = MyPackages.GetVisibilityBadgeVariant(publicPkg);
            var privateVariant = MyPackages.GetVisibilityBadgeVariant(privatePkg);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(publicVariant, Is.EqualTo(BadgeVariant.Secondary));
                Assert.That(privateVariant, Is.EqualTo(BadgeVariant.Default));
            }
        }

        /// <summary>
        /// Verifies that the component initializes the view model on render.
        /// </summary>
        [Test]
        public void VerifyOnInitialized()
        {
            var myPackagesPage = this.context.Render<MyPackages>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(myPackagesPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel(), Times.Once);
            }
        }
    }
}
