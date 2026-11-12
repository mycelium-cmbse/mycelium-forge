// ------------------------------------------------------------------------------------------------
// <copyright file="PublishedToForgeDialogTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.Publish.Dialogs
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.Publish.Dialogs;

    [TestFixture]
    public class PublishedToForgeDialogTestFixture
    {
        private BunitContext context;
        private Mock<IDialogReference> dialogReferenceMock;
        private NavigationManager navigationManager;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();
            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.dialogReferenceMock = new Mock<IDialogReference>();
            this.dialogReferenceMock.Setup(x => x.CloseAsync(It.IsAny<DialogResult>())).Returns(Task.CompletedTask);

            this.navigationManager = this.context.Services.GetRequiredService<NavigationManager>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyOnCloseClicked()
        {
            var dialog = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Scope, "starion");
                parameters.Add(x => x.PackageShortname, "test-pkg");
                parameters.Add(x => x.Version, "1.0.0");
            });

            var closeButton = dialog.Find("#published-to-forge-close-button");
            await closeButton.ClickAsync(new MouseEventArgs());

            using (Assert.EnterMultipleScope())
            {
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
                Assert.That(this.navigationManager.Uri, Does.EndWith(PageRoutes.MyPackages));
            }
        }

        [Test]
        public async Task VerifyOnViewPackageClicked()
        {
            var dialog = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Scope, "starion");
                parameters.Add(x => x.PackageShortname, "test-pkg");
                parameters.Add(x => x.Version, "1.0.0");
            });

            var viewButton = dialog.Find("#published-to-forge-view-button");
            await viewButton.ClickAsync(new MouseEventArgs());

            using (Assert.EnterMultipleScope())
            {
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
                Assert.That(this.navigationManager.Uri, Does.EndWith(PageRoutes.GetPackageRoute("starion", "test-pkg")));
            }
        }
    }
}
