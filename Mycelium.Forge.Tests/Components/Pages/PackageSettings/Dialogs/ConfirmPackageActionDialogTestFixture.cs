// ------------------------------------------------------------------------------------------------
// <copyright file="ConfirmPackageActionDialogTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageSettings.Dialogs
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;

    using Moq;

    using Mycelium.Forge.Components.Pages.PackageSettings.Dialogs;

    [TestFixture]
    public class ConfirmPackageActionDialogTestFixture
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
        public void VerifyIsConfirmDisabled()
        {
            var dialog = this.context.Render<ConfirmPackageActionDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.PackageName, "ECSS-MM-PWR");
            });

            dialog.Instance.ConfirmationInput = "WrongName";
            var disabledWhenMismatch = dialog.Instance.IsConfirmDisabled();

            dialog.Instance.ConfirmationInput = "ECSS-MM-PWR";
            var enabledWhenMatch = dialog.Instance.IsConfirmDisabled();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(disabledWhenMismatch, Is.True);
                Assert.That(enabledWhenMatch, Is.False);
            }
        }

        [Test]
        public async Task VerifyOnCancelClicked()
        {
            var onCancelCalled = false;
            var onCancel = new EventCallbackFactory().Create(this, () => { onCancelCalled = true; });

            var dialog = this.context.Render<ConfirmPackageActionDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.OnCancel, onCancel);
            });

            await dialog.InvokeAsync(() => dialog.Instance.OnCancelClicked());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(onCancelCalled, Is.True);
                this.dialogReferenceMock.Verify(x => x.CancelAsync(), Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnConfirmClicked()
        {
            var onResultCalled = false;
            var onResult = new EventCallbackFactory().Create(this, () => { onResultCalled = true; });

            var dialog = this.context.Render<ConfirmPackageActionDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.PackageName, "ECSS-MM-PWR");
                parameters.Add(x => x.ActionButtonText, "Delete");
                parameters.Add(x => x.Description, "Permanently delete.");
                parameters.Add(x => x.OnResult, onResult);
            });

            // Scenario 1: Disabled confirmation does nothing
            dialog.Instance.ConfirmationInput = "Mismatch";
            await dialog.InvokeAsync(() => dialog.Instance.OnConfirmClicked());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(onResultCalled, Is.False);
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Never);
            }

            // Scenario 2: Matching confirmation closes dialog
            dialog.Instance.ConfirmationInput = "ECSS-MM-PWR";
            await dialog.InvokeAsync(() => dialog.Instance.OnConfirmClicked());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(onResultCalled, Is.True);
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }
        }
    }
}
