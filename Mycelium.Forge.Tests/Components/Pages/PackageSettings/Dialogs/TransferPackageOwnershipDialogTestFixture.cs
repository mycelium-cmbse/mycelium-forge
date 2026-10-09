// ------------------------------------------------------------------------------------------------
// <copyright file="TransferPackageOwnershipDialogTestFixture.cs" company="Starion Group S.A.">
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
    public class TransferPackageOwnershipDialogTestFixture
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
        public void VerifyIsTransferDisabled()
        {
            var dialog = this.context.Render<TransferPackageOwnershipDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.PackageName, "ECSS-MM-PWR");
            });

            // Empty target scope and empty confirmation
            var disabledInitially = dialog.Instance.IsTransferDisabled();

            // Target scope filled, confirmation wrong
            dialog.Instance.TargetScope = "new-owner";
            dialog.Instance.ConfirmationPackageName = "wrong-name";
            var disabledWithWrongName = dialog.Instance.IsTransferDisabled();

            // Target scope empty, confirmation match
            dialog.Instance.TargetScope = string.Empty;
            dialog.Instance.ConfirmationPackageName = "ECSS-MM-PWR";
            var disabledWithEmptyTarget = dialog.Instance.IsTransferDisabled();

            // Both valid
            dialog.Instance.TargetScope = "new-owner";
            dialog.Instance.ConfirmationPackageName = "ECSS-MM-PWR";
            var enabledWhenValid = dialog.Instance.IsTransferDisabled();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(disabledInitially, Is.True);
                Assert.That(disabledWithWrongName, Is.True);
                Assert.That(disabledWithEmptyTarget, Is.True);
                Assert.That(enabledWhenValid, Is.False);
            }
        }

        [Test]
        public async Task VerifyOnCancelClicked()
        {
            var onCancelCalled = false;
            var onCancel = new EventCallbackFactory().Create(this, () => { onCancelCalled = true; });

            var dialog = this.context.Render<TransferPackageOwnershipDialog>(parameters =>
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
        public async Task VerifyOnTransferClicked()
        {
            string? receivedTarget = null;
            var onResult = new EventCallbackFactory().Create(this, (string target) => { receivedTarget = target; });

            var dialog = this.context.Render<TransferPackageOwnershipDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.PackageName, "ECSS-MM-PWR");
                parameters.Add(x => x.CurrentOwner, "starion");
                parameters.Add(x => x.OnResult, onResult);
            });

            // Scenario 1: Disabled transfer does not execute
            dialog.Instance.TargetScope = string.Empty;
            dialog.Instance.ConfirmationPackageName = "ECSS-MM-PWR";
            await dialog.InvokeAsync(() => dialog.Instance.OnTransferClicked());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedTarget, Is.Null);
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Never);
            }

            // Scenario 2: Valid inputs clean target scope and close dialog
            dialog.Instance.TargetScope = "@new-owner ";
            dialog.Instance.ConfirmationPackageName = "ECSS-MM-PWR";
            await dialog.InvokeAsync(() => dialog.Instance.OnTransferClicked());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedTarget, Is.EqualTo("new-owner"));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }
        }
    }
}
