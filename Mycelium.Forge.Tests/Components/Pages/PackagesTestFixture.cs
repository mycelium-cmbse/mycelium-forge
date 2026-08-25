// ------------------------------------------------------------------------------------------------
// <copyright file="PackagesTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Package;
    using Mycelium.Forge.ViewModels.Packages;

    [TestFixture]
    public class PackagesTestFixture
    {
        private BunitContext context;
        private Mock<IPackagesViewModel> viewModelMock;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IPackagesViewModel>();

            var packageModel = new PackageModel(
                new Package { Name = "ECSS-MM-PWR", ShortName = "ecss-mm-pwr", Visibility = VisibilityKind.PUBLIC },
                "Starion Group");

            this.viewModelMock.Setup(x => x.Facets).Returns(
            [
                new OptionModel(nameof(PackageModel.Format), "SysML v2", 3, true)
            ]);

            this.viewModelMock.Setup(x => x.PackageResults).Returns([packageModel]);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyFilteringActions()
        {
            var packagesPage = this.context.Render<Packages>();

            var facet = this.viewModelMock.Object.Facets[0];
            Packages.ToggleFacet(facet);

            var applyBtn = packagesPage.Find("#packages-apply-filters-button");
            await packagesPage.InvokeAsync(() => applyBtn.ClickAsync());

            var resetBtn = packagesPage.Find("#packages-reset-filters-button");
            await packagesPage.InvokeAsync(() => resetBtn.ClickAsync());

            this.viewModelMock.Setup(x => x.PackageResults).Returns([]);
            var emptyPackagesPage = this.context.Render<Packages>();
            var browseAllBtn = emptyPackagesPage.Find("#packages-browse-all-button");
            await emptyPackagesPage.InvokeAsync(() => browseAllBtn.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.Search("ecss", PackageSortOption.Relevance, false), Times.Once);
                this.viewModelMock.Verify(x => x.Search(string.Empty, PackageSortOption.Relevance, false), Times.Exactly(2));
            }
        }

        [Test]
        public void VerifyGetResultsCountText()
        {
            var packagesPage = this.context.Render<Packages>();

            var countTextSingle = packagesPage.Instance.GetResultsCountText();
            this.viewModelMock.Setup(x => x.PackageResults).Returns([]);
            var countTextEmpty = packagesPage.Instance.GetResultsCountText();

            packagesPage.Instance.SearchQuery = string.Empty;
            this.viewModelMock.Setup(x => x.PackageResults).Returns([new PackageModel(new Package { Name = "P1" }, "@starion")]);
            var countTextNoQuerySingle = packagesPage.Instance.GetResultsCountText();
            this.viewModelMock.Setup(x => x.PackageResults).Returns([]);
            var countTextNoQueryEmpty = packagesPage.Instance.GetResultsCountText();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(countTextSingle, Is.EqualTo("1 package matches “ecss”"));
                Assert.That(countTextEmpty, Is.EqualTo("0 packages match “ecss”"));
                Assert.That(countTextNoQuerySingle, Is.EqualTo("1 package"));
                Assert.That(countTextNoQueryEmpty, Is.EqualTo("0 packages"));
            }
        }

        [Test]
        public void VerifyOnIncludePrereleasesChanged()
        {
            var packagesPage = this.context.Render<Packages>();
            packagesPage.Instance.OnIncludePrereleasesChanged(true);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(packagesPage.Instance.IncludePrereleases, Is.True);
                this.viewModelMock.Verify(x => x.Search("ecss", PackageSortOption.Relevance, true), Times.Once);
            }
        }

        [Test]
        public void VerifyOnInitialized()
        {
            var packagesPage = this.context.Render<Packages>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(packagesPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel("ecss", PackageSortOption.Relevance, false), Times.Once);
            }
        }

        [Test]
        public void VerifyOnSearchValueChanged()
        {
            var packagesPage = this.context.Render<Packages>();
            packagesPage.Instance.OnSearchValueChanged("sysml");
            packagesPage.Instance.OnSearchValueChanged(null);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(packagesPage.Instance.SearchQuery, Is.EqualTo(string.Empty));
                this.viewModelMock.Verify(x => x.Search("sysml", PackageSortOption.Relevance, false), Times.Once);
                this.viewModelMock.Verify(x => x.Search(string.Empty, PackageSortOption.Relevance, false), Times.AtLeastOnce);
            }
        }

        [Test]
        public void VerifyOnSortOptionChanged()
        {
            var packagesPage = this.context.Render<Packages>();

            packagesPage.Instance.OnSortOptionChanged(PackageSortOption.Downloads);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(packagesPage.Instance.SelectedSortOption, Is.EqualTo(PackageSortOption.Downloads));
                this.viewModelMock.Verify(x => x.Search("ecss", PackageSortOption.Downloads, false), Times.Once);
            }
        }

        [Test]
        public void VerifyStaticHelpers()
        {
            var label = Packages.GetSortOptionLabel(PackageSortOption.Alphabetical);
            var activeClass = Packages.GetFacetLabelClass(new OptionModel("Prop", "Label", 1, true));
            var inactiveClass = Packages.GetFacetLabelClass(new OptionModel("Prop", "Label", 1));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(label, Is.EqualTo("Alphabetical"));
                Assert.That(activeClass, Does.Contain("font-medium text-foreground"));
                Assert.That(inactiveClass, Does.Contain("font-normal text-secondary-text"));
            }
        }
    }
}
