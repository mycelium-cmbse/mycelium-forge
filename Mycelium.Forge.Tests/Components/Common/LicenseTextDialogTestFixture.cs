// ------------------------------------------------------------------------------------------------
// <copyright file="LicenseTextDialogTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Common
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Tests.TestHelpers;

    [TestFixture]
    public class LicenseTextDialogTestFixture
    {
        private BunitContext context;
        private Mock<IDialogReference> dialogReferenceMock;
        private License license;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();
            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.dialogReferenceMock = new Mock<IDialogReference>();
            this.dialogReferenceMock.Setup(x => x.CloseAsync(It.IsAny<DialogResult>())).Returns(Task.CompletedTask);

            this.license = new License
            {
                Id = Guid.NewGuid(),
                Name = "MIT License",
                SpdxIdentifier = "MIT",
                LicenseText = "Permission is hereby granted, free of charge..."
            };
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyRendering()
        {
            var emptyDialog = this.context.Render<LicenseTextDialog>(parameters => parameters.AddCascadingValue(this.dialogReferenceMock.Object));
            var populatedDialog = this.context.Render<LicenseTextDialog>(parameters => parameters.AddCascadingValue(this.dialogReferenceMock.Object).Add(x => x.License, this.license));

            var closeButton = populatedDialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("license-text-dialog-close-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyDialog.Markup.Trim(), Has.Length.LessThan(5));
                Assert.That(populatedDialog.Markup, Does.Contain("Permission is hereby granted, free of charge..."));
                Assert.That(populatedDialog.Markup, Does.Not.Contain("MIT License"));
                Assert.That(populatedDialog.FindComponents<BbBadge>(), Is.Empty);
                Assert.That(closeButton, Is.Not.Null);
            }
        }

        [Test]
        public async Task VerifyOnCloseClicked()
        {
            var populatedDialog = this.context.Render<LicenseTextDialog>(parameters => parameters.AddCascadingValue(this.dialogReferenceMock.Object).Add(x => x.License, this.license));
            var closeButton = populatedDialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("license-text-dialog-close-button"));
            Assert.That(closeButton, Is.Not.Null);

            await populatedDialog.InvokeAsync(() => closeButton!.Find("button").ClickAsync());
            this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);

            var detachedDialog = this.context.Render<LicenseTextDialog>(parameters => parameters.Add(x => x.License, this.license));
            var detachedCloseButton = detachedDialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("license-text-dialog-close-button"));
            Assert.That(detachedCloseButton, Is.Not.Null);

            await detachedDialog.InvokeAsync(() => detachedCloseButton!.Find("button").ClickAsync());
            this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
        }
    }
}
