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
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages;
    using Mycelium.Forge.Models.Organization;
    using Mycelium.Forge.ViewModels.OrganizationDetails;

    [TestFixture]
    public class OrganizationDetailsTestFixture
    {
        private BunitContext context;
        private Mock<IOrganizationDetailsViewModel> viewModelMock;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IOrganizationDetailsViewModel>();

            var organization = new OrganizationModel(
                new Organization { Name = "Starion Group", ShortName = "starion" },
                description: "Engineering systems.",
                isVerified: true,
                packageCount: 12,
                versionCount: 48,
                importCount: 3500)
            {
                MemberSinceYear = 2023
            };

            this.viewModelMock.SetupGet(x => x.Organization).Returns(organization);
            this.viewModelMock.SetupGet(x => x.Packages).Returns([]);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyGetOrganizationMetaText()
        {
            var orgDetailsPage = this.context.Render<OrganizationDetails>();
            var metaText = orgDetailsPage.Instance.GetOrganizationMetaText();

            Assert.That(metaText, Does.Contain("Verified publisher · 12 packages · 48 versions · 3500 imports · member since 2023"));
        }

        [Test]
        public void VerifyOnParametersSet()
        {
            var guid = Guid.NewGuid();

            var orgDetailsPage = this.context.Render<OrganizationDetails>(parameters => parameters
                .Add(p => p.Id, guid.ToString()));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(orgDetailsPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel(guid), Times.Once);
            }
        }
    }
}
