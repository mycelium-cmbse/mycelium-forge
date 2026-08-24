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
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages;
    using Mycelium.Forge.Models.Package;
    using Mycelium.Forge.ViewModels.MyPackages;

    [TestFixture]
    public class MyPackagesTestFixture
    {
        private BunitContext context;
        private Mock<IMyPackagesViewModel> viewModelMock;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IMyPackagesViewModel>();

            var packages = new List<PackageModel>
            {
                new(new Package { Name = "ECSS-MM-PWR", ShortName = "ecss-mm-pwr", Visibility = VisibilityKind.PUBLIC }, "Starion Group"),
                new(new Package { Name = "Internal-Core", ShortName = "internal-core", Visibility = VisibilityKind.PRIVATE }, "ESA")
            };

            this.viewModelMock.SetupGet(x => x.Packages).Returns(packages);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyFilteredPackages()
        {
            var myPackagesPage = this.context.Render<MyPackages>();

            var allPackages = myPackagesPage.Instance.FilteredPackages();
            myPackagesPage.Instance.SelectedPublisher = "Starion Group";
            var starionPackages = myPackagesPage.Instance.FilteredPackages();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(allPackages, Has.Count.EqualTo(2));
                Assert.That(starionPackages, Has.Count.EqualTo(1));
            }
        }

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

        [Test]
        public void VerifyGetVisibilityBadgeVariant()
        {
            var myPackagesPage = this.context.Render<MyPackages>();
            var publicPkg = new PackageModel(new Package { Visibility = VisibilityKind.PUBLIC });
            var privatePkg = new PackageModel(new Package { Visibility = VisibilityKind.PRIVATE });

            var publicVariant = myPackagesPage.Instance.GetVisibilityBadgeVariant(publicPkg);
            var privateVariant = myPackagesPage.Instance.GetVisibilityBadgeVariant(privatePkg);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(publicVariant, Is.EqualTo(BadgeVariant.Secondary));
                Assert.That(privateVariant, Is.EqualTo(BadgeVariant.Default));
            }
        }

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
