// ------------------------------------------------------------------------------------------------
// <copyright file="HomeTestFixture.cs" company="Starion Group S.A.">
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
    using Mycelium.Forge.ViewModels.Home;

    [TestFixture]
    public class HomeTestFixture
    {
        private BunitContext context;
        private Mock<IHomeViewModel> viewModelMock;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IHomeViewModel>();

            var packages = new List<PackageModel>
            {
                new(
                    new Package { Name = "ECSS-MM-PWR", ShortName = "ecss-mm-pwr", Visibility = VisibilityKind.PUBLIC },
                    "Starion Group",
                    "1.3.0",
                    description: "Power subsystem model.")
            };

            this.viewModelMock.Setup(x => x.PackageCount).Returns("120");
            this.viewModelMock.Setup(x => x.VersionCount).Returns("450");
            this.viewModelMock.Setup(x => x.PublisherCount).Returns("35");
            this.viewModelMock.Setup(x => x.ImportCount).Returns("15.2k");
            this.viewModelMock.Setup(x => x.StandardLibraries).Returns(packages);
            this.viewModelMock.Setup(x => x.RecentlyUpdated).Returns(packages);
            this.viewModelMock.Setup(x => x.MostUsed).Returns(packages);
            this.viewModelMock.Setup(x => x.ModelsFromOtherMbseTools).Returns(packages);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyHomeRenders()
        {
            var homePage = this.context.Render<Home>();
            var markup = homePage.Markup;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(markup, Does.Contain("packages"));
                Assert.That(markup, Does.Contain("versions"));
                Assert.That(markup, Does.Contain("publishers"));
                Assert.That(markup, Does.Contain("imports"));
            }
        }

        [Test]
        public void VerifyOnInitialized()
        {
            var homePage = this.context.Render<Home>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(homePage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel(), Times.Once);
            }
        }
    }
}
