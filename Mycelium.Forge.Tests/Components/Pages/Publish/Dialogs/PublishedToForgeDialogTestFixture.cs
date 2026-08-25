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
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.Publish.Dialogs;
    using Mycelium.Forge.Models.Package;

    [TestFixture]
    public class PublishedToForgeDialogTestFixture
    {
        private BunitContext context;
        private Mock<IDialogReference> dialogReferenceMock;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.dialogReferenceMock = new Mock<IDialogReference>();
            this.dialogReferenceMock.Setup(x => x.CloseAsync(It.IsAny<DialogResult>())).Returns(Task.CompletedTask);
            this.dialogReferenceMock.Setup(x => x.CancelAsync()).Returns(Task.CompletedTask);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyGetDescriptionText()
        {
            var dialog1 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Description, "Custom description.");
            });

            var customDescription = dialog1.Instance.GetDescriptionText();

            var dialog2 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Description, string.Empty);
                parameters.Add(x => x.Scope, "@starion");
                parameters.Add(x => x.PackageName, "ECSS-MM-PWR");
                parameters.Add(x => x.Version, "v1.3.0");
            });

            var builtDescription = dialog2.Instance.GetDescriptionText();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(customDescription, Is.EqualTo("Custom description."));
                Assert.That(builtDescription, Does.Contain("@starion/ECSS-MM-PWR"));
                Assert.That(builtDescription, Does.Contain("v1.3.0"));
                Assert.That(builtDescription, Does.Contain("is live"));
            }
        }

        [Test]
        public void VerifyGetFormattedVersion()
        {
            var dialog1 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Package, new PackageModel(new Package { Name = "P" }, "@org", "2.0.0"));
            });

            var fromPackageNoPrefix = dialog1.Instance.GetFormattedVersion();

            var dialog2 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Package, new PackageModel(new Package { Name = "P" }, "@org", "v2.0.0"));
            });

            var fromPackageWithPrefix = dialog2.Instance.GetFormattedVersion();

            var dialog3 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Version, "3.0.0");
            });

            var fromVersionField = dialog3.Instance.GetFormattedVersion();

            var dialog4 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Version, "v3.0.0");
            });

            var fromVersionFieldWithPrefix = dialog4.Instance.GetFormattedVersion();

            var dialog5 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Version, string.Empty);
            });

            var defaultVersion = dialog5.Instance.GetFormattedVersion();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(fromPackageNoPrefix, Is.EqualTo("v2.0.0"));
                Assert.That(fromPackageWithPrefix, Is.EqualTo("v2.0.0"));
                Assert.That(fromVersionField, Is.EqualTo("v3.0.0"));
                Assert.That(fromVersionFieldWithPrefix, Is.EqualTo("v3.0.0"));
                Assert.That(defaultVersion, Is.EqualTo("v1.3.0"));
            }
        }

        [Test]
        public void VerifyGetPackageFullName()
        {
            var dialog1 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Package, new PackageModel(new Package { Name = "MyPkg" }, "@org", "1.0.0"));
            });

            var withPackageFullName = dialog1.Instance.GetPackageFullName();

            var dialog2 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Scope, "starion");
                parameters.Add(x => x.PackageName, "ECSS-MM-PWR");
            });

            var withScopeNoAt = dialog2.Instance.GetPackageFullName();

            var dialog3 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Scope, "@starion");
                parameters.Add(x => x.PackageName, "ECSS-MM-PWR");
            });

            var withScopeWithAt = dialog3.Instance.GetPackageFullName();

            var dialog4 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Scope, string.Empty);
                parameters.Add(x => x.PackageName, "MyPackage");
            });

            var withPackageNameOnly = dialog4.Instance.GetPackageFullName();

            var dialog5 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Scope, string.Empty);
                parameters.Add(x => x.PackageName, string.Empty);
            });

            var defaultResult = dialog5.Instance.GetPackageFullName();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(withPackageFullName, Is.EqualTo("@org/MyPkg"));
                Assert.That(withScopeNoAt, Is.EqualTo("@starion/ECSS-MM-PWR"));
                Assert.That(withScopeWithAt, Is.EqualTo("@starion/ECSS-MM-PWR"));
                Assert.That(withPackageNameOnly, Is.EqualTo("MyPackage"));
                Assert.That(defaultResult, Is.EqualTo("@starion/ECSS-MM-PWR"));
            }
        }

        [Test]
        public void VerifyGetPackageHref()
        {
            var dialog1 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.PackageHref, "/explicit/href");
            });

            var explicitHref = dialog1.Instance.GetPackageHref();

            var dialog2 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.PackageHref, string.Empty);
                parameters.Add(x => x.Package, new PackageModel(new Package { Name = "P" }, "@org", "1.0.0") { Href = "/packages/@org/P" });
            });

            var packageHref = dialog2.Instance.GetPackageHref();

            var dialog3 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.PackageHref, string.Empty);
                parameters.Add(x => x.Scope, "@starion");
                parameters.Add(x => x.PackageName, "ECSS-MM-PWR");
            });

            var builtHref = dialog3.Instance.GetPackageHref();

            var dialog4 = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.PackageHref, string.Empty);
                parameters.Add(x => x.Scope, string.Empty);
                parameters.Add(x => x.PackageName, string.Empty);
            });

            var fallbackHref = dialog4.Instance.GetPackageHref();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(explicitHref, Is.EqualTo("/explicit/href"));
                Assert.That(packageHref, Is.EqualTo("/packages/@org/P"));
                Assert.That(builtHref, Does.Contain("starion").And.Contain("ECSS-MM-PWR"));
                Assert.That(fallbackHref, Is.EqualTo(PageRoutes.Packages));
            }
        }

        [Test]
        public async Task VerifyOnCloseClicked()
        {
            var onCloseCalled = false;
            var onClose = new EventCallbackFactory().Create(this, () => { onCloseCalled = true; });

            var dialog = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.OnClose, onClose);
            });

            await dialog.InvokeAsync(() => dialog.Instance.OnCloseClicked());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(onCloseCalled, Is.True);
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }

            var dialogNoRef = this.context.Render<PublishedToForgeDialog>(parameters => { parameters.AddCascadingValue(new Mock<IDialogReference>().Object); });

            dialogNoRef.Instance.DialogReference = null;
            Assert.DoesNotThrowAsync(() => dialogNoRef.InvokeAsync(() => dialogNoRef.Instance.OnCloseClicked()));
        }

        [Test]
        public async Task VerifyOnViewPackageClicked()
        {
            var onViewPackageCalled = false;
            var onViewPackage = new EventCallbackFactory().Create(this, () => { onViewPackageCalled = true; });

            var dialog = this.context.Render<PublishedToForgeDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.OnViewPackage, onViewPackage);
                parameters.Add(x => x.Scope, "@starion");
                parameters.Add(x => x.PackageName, "ECSS-MM-PWR");
            });

            await dialog.InvokeAsync(() => dialog.Instance.OnViewPackageClicked());

            var navigationManager = this.context.Services.GetRequiredService<NavigationManager>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(onViewPackageCalled, Is.True);
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
                Assert.That(navigationManager.Uri, Is.Not.EqualTo("http://localhost/"));
            }

            var dialogNoRef = this.context.Render<PublishedToForgeDialog>(parameters => { parameters.AddCascadingValue(new Mock<IDialogReference>().Object); });

            dialogNoRef.Instance.DialogReference = null;
            Assert.DoesNotThrowAsync(() => dialogNoRef.InvokeAsync(() => dialogNoRef.Instance.OnViewPackageClicked()));
        }
    }
}
