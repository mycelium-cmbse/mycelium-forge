// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationDetailsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages;
    using Mycelium.Forge.ViewModels.OrganizationDetails;

    /// <summary>
    /// Test fixture for <see cref="OrganizationDetails" /> component.
    /// </summary>
    [TestFixture]
    public class OrganizationDetailsTestFixture
    {
        private BunitContext context;
        private Mock<IOrganizationDetailsViewModel> viewModelMock;
        private Organization organization;

        /// <summary>
        /// Sets up the test context and mocked dependencies.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IOrganizationDetailsViewModel>();

            this.organization = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Starion Group",
                ShortName = "starion",
                Origin = "https://example.com",
                CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            this.viewModelMock.Setup(x => x.Organization).Returns(this.organization);
            this.viewModelMock.Setup(x => x.Packages).Returns([]);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        /// <summary>
        /// Disposes the test context.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        /// <summary>
        /// Verifies that OnParametersSet initializes the view model with the short name.
        /// </summary>
        [Test]
        public void VerifyOnParametersSet()
        {
            var orgDetailsPage = this.context.Render<OrganizationDetails>(parameters => parameters
                .Add(p => p.ShortName, "starion"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(orgDetailsPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel("starion", It.IsAny<CancellationToken>()), Times.Once);
            }
        }
    }
}
