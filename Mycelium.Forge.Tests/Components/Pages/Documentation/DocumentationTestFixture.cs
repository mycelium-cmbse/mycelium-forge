// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.Documentation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Components.Pages.Documentation;
    using Mycelium.Forge.Models.Documentation;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.Documentation;

    [TestFixture]
    public class DocumentationTestFixture
    {
        private BunitContext context;
        private Mock<IDocumentationViewModel> viewModelMock;
        private Mock<IJsInterop> jsInteropMock;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.jsInteropMock = new Mock<IJsInterop>();
            this.context.Services.AddSingleton(this.jsInteropMock.Object);

            this.viewModelMock = new Mock<IDocumentationViewModel>();

            var keySections = new List<DocumentationSectionCardModel>
            {
                new()
                {
                    Title = "Getting started",
                    Description = "Install the CLI and import your first package.",
                    Href = "/docs",
                    IconName = "book-open"
                }
            };

            var tableOfContents = new List<DocumentationTocItemModel>
            {
                new()
                {
                    Title = "What is Mycelium Forge?",
                    TargetId = "what-is-mycelium-forge",
                    Href = "#what-is-mycelium-forge",
                    IsActive = true
                }
            };

            this.viewModelMock.Setup(x => x.NavGroups).Returns([]);
            this.viewModelMock.Setup(x => x.KeySections).Returns(keySections);
            this.viewModelMock.Setup(x => x.TableOfContents).Returns(tableOfContents);
            this.viewModelMock.Setup(x => x.LastUpdated).Returns("29 July 2026");
            this.viewModelMock.Setup(x => x.FeedbackGiven).Returns(false);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyOnInitialized()
        {
            var documentationPage = this.context.Render<Documentation>();
            var markup = documentationPage.Markup;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(documentationPage.Instance, Is.Not.Null);
                Assert.That(markup, Does.Contain("Welcome to Forge Docs"));
                Assert.That(markup, Does.Contain("Getting started"));
                this.viewModelMock.Verify(x => x.InitializeOverview(), Times.Once);
            }
        }

        [Test]
        public void VerifySubmitFeedback()
        {
            var documentationPage = this.context.Render<Documentation>();

            documentationPage.Instance.SubmitFeedback(true);
            documentationPage.Instance.SubmitFeedback(false);

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.RecordFeedback(true), Times.Once);
                this.viewModelMock.Verify(x => x.RecordFeedback(false), Times.Once);
            }
        }
    }
}
