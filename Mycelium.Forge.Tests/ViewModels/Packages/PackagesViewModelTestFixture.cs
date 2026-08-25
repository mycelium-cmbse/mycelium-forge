// ------------------------------------------------------------------------------------------------
// <copyright file="PackagesViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.Packages
{
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Package;
    using Mycelium.Forge.ViewModels.Packages;

    [TestFixture]
    public class PackagesViewModelTestFixture
    {
        private PackagesViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new PackagesViewModel();
        }

        [Test]
        public void VerifyApplyFacetFilters()
        {
            this.viewModel.InitializeViewModel(string.Empty, PackageSortOption.Relevance, false);

            var formatFacet = this.viewModel.Facets.Find(f => f.Property == nameof(PackageModel.Format));

            if (formatFacet != null)
            {
                formatFacet.IsChecked = true;
            }

            var kindFacet = this.viewModel.Facets.Find(f => f.Property == "Kind");

            if (kindFacet != null)
            {
                kindFacet.IsChecked = true;
            }

            var publisherFacet = this.viewModel.Facets.Find(f => f.Property == nameof(PackageModel.Publisher));

            if (publisherFacet != null)
            {
                publisherFacet.IsChecked = true;
            }

            var categoryFacet = this.viewModel.Facets.Find(f => f.Property == "Category");

            if (categoryFacet != null)
            {
                categoryFacet.IsChecked = true;
            }

            var tagsFacet = this.viewModel.Facets.Find(f => f.Property == nameof(PackageModel.Tags));

            if (tagsFacet != null)
            {
                tagsFacet.IsChecked = true;
            }

            var metamodelFacet = this.viewModel.Facets.Find(f => f.Property == "Metamodel");

            if (metamodelFacet != null)
            {
                metamodelFacet.IsChecked = true;
            }

            var licenseFacet = this.viewModel.Facets.Find(f => f.Property == nameof(PackageModel.License));

            if (licenseFacet != null)
            {
                licenseFacet.IsChecked = true;
            }

            this.viewModel.Search();

            Assert.That(this.viewModel.PackageResults, Is.Not.Null);
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.InitializeViewModel(string.Empty, PackageSortOption.Relevance, false);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Facets, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.PackageResults, Has.Count.GreaterThan(0));
            }
        }

        [Test]
        public void VerifySearch()
        {
            this.viewModel.InitializeViewModel(string.Empty, PackageSortOption.Relevance, false);

            this.viewModel.Search("ECSS");
            var queryCount = this.viewModel.PackageResults.Count;

            this.viewModel.Search(string.Empty, PackageSortOption.Downloads);
            var downloadsSorted = this.viewModel.PackageResults;

            this.viewModel.Search(string.Empty, PackageSortOption.Alphabetical);
            var alphaSorted = this.viewModel.PackageResults;

            this.viewModel.Search(string.Empty, PackageSortOption.RecentlyUpdated);
            var recentSorted = this.viewModel.PackageResults;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(queryCount, Is.GreaterThan(0));
                Assert.That(downloadsSorted, Has.Count.GreaterThan(0));
                Assert.That(alphaSorted, Has.Count.GreaterThan(0));
                Assert.That(recentSorted, Has.Count.GreaterThan(0));
            }
        }
    }
}
