// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.Documentation
{
    using Mycelium.Forge.ViewModels.Documentation;

    [TestFixture]
    public class DocumentationViewModelTestFixture
    {
        private DocumentationViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new DocumentationViewModel();
        }

        [Test]
        public void VerifyInitializeOverview()
        {
            this.viewModel.InitializeOverview();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.NavGroups, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.KeySections, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.TableOfContents, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.LastUpdated, Is.Not.Empty);
                Assert.That(this.viewModel.FeedbackGiven, Is.False);
                Assert.That(this.viewModel.IsHelpful, Is.Null);
            }
        }

        [Test]
        public void VerifyInitializePackagesAndKpar()
        {
            this.viewModel.InitializePackagesAndKpar();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.NavGroups, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.KeySections, Has.Count.EqualTo(0));
                Assert.That(this.viewModel.TableOfContents, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.FeedbackGiven, Is.False);
            }
        }

        [Test]
        public void VerifyInitializePage()
        {
            this.viewModel.InitializePage("What is Forge", [], []);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.NavGroups, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.KeySections, Has.Count.EqualTo(0));
                Assert.That(this.viewModel.TableOfContents, Has.Count.EqualTo(0));
                Assert.That(this.viewModel.LastUpdated, Is.EqualTo("29 July 2026"));
                Assert.That(this.viewModel.FeedbackGiven, Is.False);
                Assert.That(this.viewModel.IsHelpful, Is.Null);
            }
        }

        [Test]
        public void VerifyRecordFeedback()
        {
            this.viewModel.RecordFeedback(true);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.FeedbackGiven, Is.True);
                Assert.That(this.viewModel.IsHelpful, Is.True);
            }

            this.viewModel.RecordFeedback(false);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.FeedbackGiven, Is.True);
                Assert.That(this.viewModel.IsHelpful, Is.False);
            }
        }
    }
}
