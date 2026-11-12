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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using ErrorOr;

    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageSettings.Dialogs;
    using Mycelium.Forge.ViewModels.PackageSettings;

    [TestFixture]
    public class TransferPackageOwnershipDialogTestFixture
    {
        private BunitContext context;
        private Mock<IDialogReference> dialogReferenceMock;
        private Mock<ITransferPackageOwnershipDialogViewModel> viewModelMock;
        private Package package;

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

            this.package = new Package { Id = Guid.NewGuid(), Name = "ECSS-MM-PWR", ShortName = "ecss-mm-pwr" };
            this.viewModelMock = new Mock<ITransferPackageOwnershipDialogViewModel>();

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyOnCancelClicked()
        {
            var onCancelCalled = false;
            var onCancel = new EventCallbackFactory().Create(this, () => { onCancelCalled = true; });

            var dialog = this.context.Render<TransferPackageOwnershipDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Package, this.package);
                parameters.Add(x => x.OnCancel, onCancel);
            });

            var cancelButton = dialog.Find("#transfer-package-cancel-button");
            await dialog.InvokeAsync(() => cancelButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(onCancelCalled, Is.True);
                this.dialogReferenceMock.Verify(x => x.CancelAsync(), Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnInputsChanged()
        {
            var dialog = this.context.Render<TransferPackageOwnershipDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Package, this.package);
            });

            var inputs = dialog.FindComponents<BbInput>();
            var targetInput = inputs.First(i => i.Instance.Id == "transfer-target-scope-input");
            var confirmInput = inputs.First(i => i.Instance.Id == "transfer-confirm-package-name-input");

            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync("new-owner"));
            var targetWithValue = dialog.Instance.TargetScope;

            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync(null!));
            var targetWithNull = dialog.Instance.TargetScope;

            await dialog.InvokeAsync(() => confirmInput.Instance.ValueChanged.InvokeAsync("ECSS-MM-PWR"));
            var confirmWithValue = dialog.Instance.ConfirmationPackageName;

            await dialog.InvokeAsync(() => confirmInput.Instance.ValueChanged.InvokeAsync(null!));
            var confirmWithNull = dialog.Instance.ConfirmationPackageName;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(targetWithValue, Is.EqualTo("new-owner"));
                Assert.That(targetWithNull, Is.EqualTo(string.Empty));
                Assert.That(confirmWithValue, Is.EqualTo("ECSS-MM-PWR"));
                Assert.That(confirmWithNull, Is.EqualTo(string.Empty));
            }
        }

        [Test]
        public async Task VerifyOnTransferClicked()
        {
            IScope receivedScope = null!;
            var onResult = new EventCallbackFactory().Create(this, (IScope scope) => { receivedScope = scope; });

            var dialog = this.context.Render<TransferPackageOwnershipDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Package, this.package);
                parameters.Add(x => x.CurrentOwner, "starion");
                parameters.Add(x => x.OnResult, onResult);
            });

            var inputs = dialog.FindComponents<BbInput>();
            var targetInput = inputs.First(i => i.Instance.Id == "transfer-target-scope-input");
            var confirmInput = inputs.First(i => i.Instance.Id == "transfer-confirm-package-name-input");
            var submitButton = dialog.Find("#transfer-package-submit-button");

            // Case 1: Empty inputs -> validation fails
            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync(string.Empty));
            await dialog.InvokeAsync(() => confirmInput.Instance.ValueChanged.InvokeAsync(string.Empty));
            await dialog.InvokeAsync(() => submitButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedScope, Is.Null);
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(dialog.Instance.TargetScope)), Is.True);
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(dialog.Instance.ConfirmationPackageName)), Is.True);
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Never);
            }

            // Case 2: Invalid target scope shortname format
            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync("@invalid-name"));
            await dialog.InvokeAsync(() => confirmInput.Instance.ValueChanged.InvokeAsync("ECSS-MM-PWR"));
            await dialog.InvokeAsync(() => submitButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedScope, Is.Null);
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(dialog.Instance.TargetScope)), Is.True);
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(dialog.Instance.ConfirmationPackageName)), Is.False);
            }

            // Case 3: Confirmation mismatch
            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync("newowner"));
            await dialog.InvokeAsync(() => confirmInput.Instance.ValueChanged.InvokeAsync("Wrong Name"));
            await dialog.InvokeAsync(() => submitButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedScope, Is.Null);
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(dialog.Instance.TargetScope)), Is.False);
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(dialog.Instance.ConfirmationPackageName)), Is.True);
            }

            // Case 4: View model validation error is bound to the target scope validation error
            this.viewModelMock.Setup(x => x.ValidateTargetAsync(this.package, "starion", "unknown", It.IsAny<CancellationToken>())).ReturnsAsync(Error.NotFound(description: "Scope 'unknown' not found."));
            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync("unknown"));
            await dialog.InvokeAsync(() => confirmInput.Instance.ValueChanged.InvokeAsync("ECSS-MM-PWR"));
            await dialog.InvokeAsync(() => submitButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedScope, Is.Null);
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(dialog.Instance.TargetScope)), Is.True);
                Assert.That(dialog.Instance.ValidationManager[nameof(dialog.Instance.TargetScope)].First(), Is.EqualTo("Scope 'unknown' not found."));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Never);
            }

            // Case 5: Only the first error description is displayed
            this.viewModelMock.Setup(x => x.ValidateTargetAsync(this.package, "starion", "starion", It.IsAny<CancellationToken>())).ReturnsAsync([Error.Validation(description: "You cannot transfer ownership to the current owner scope."), Error.Validation(description: "Second error.")]);
            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync("starion"));
            await dialog.InvokeAsync(() => submitButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedScope, Is.Null);
                Assert.That(dialog.Instance.ValidationManager[nameof(dialog.Instance.TargetScope)].ToList(), Is.EqualTo(["You cannot transfer ownership to the current owner scope."]));
            }

            // Case 6: Valid target -> OnResult invoked with the resolved scope and dialog closed
            var validOrg = new Organization { Id = Guid.NewGuid(), ShortName = "validorg" };
            this.viewModelMock.Setup(x => x.ValidateTargetAsync(this.package, "starion", "validorg", It.IsAny<CancellationToken>())).ReturnsAsync(ErrorOrFactory.From<IScope>(validOrg));
            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync("validorg"));
            await dialog.InvokeAsync(() => submitButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedScope, Is.EqualTo(validOrg));
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(dialog.Instance.TargetScope)), Is.False);
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }
        }
    }
}
