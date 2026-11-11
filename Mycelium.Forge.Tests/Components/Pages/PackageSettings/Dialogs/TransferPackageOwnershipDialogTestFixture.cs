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
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageSettings.Dialogs;
    using Mycelium.Forge.Dal.Services;
    using Mycelium.Forge.Services;

    [TestFixture]
    public class TransferPackageOwnershipDialogTestFixture
    {
        private BunitContext context;
        private Mock<IDialogReference> dialogReferenceMock;
        private Mock<IScopeService> scopeServiceMock;
        private Mock<IUserService> userServiceMock;
        private UserContext userContext;
        private Guid currentUserId;

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

            this.currentUserId = Guid.NewGuid();
            this.userContext = new UserContext { AccountId = this.currentUserId };

            this.scopeServiceMock = new Mock<IScopeService>();
            this.userServiceMock = new Mock<IUserService>();
            this.userServiceMock.Setup(x => x.GetUserContext(false)).ReturnsAsync(this.userContext);

            this.context.Services.AddSingleton(this.scopeServiceMock.Object);
            this.context.Services.AddSingleton(this.userServiceMock.Object);
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
            var dialog = this.context.Render<TransferPackageOwnershipDialog>(parameters => { parameters.AddCascadingValue(this.dialogReferenceMock.Object); });

            var inputs = dialog.FindComponents<BbInput>();
            var targetInput = inputs.First(i => i.Instance.Id == "transfer-target-scope-input");
            var confirmInput = inputs.First(i => i.Instance.Id == "transfer-confirm-package-name-input");

            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync("new-owner"));
            var targetWithValue = dialog.Instance.TargetScope;

            dialog.Instance.OnTargetScopeChanged(null!);
            var targetWithNull = dialog.Instance.TargetScope;

            await dialog.InvokeAsync(() => confirmInput.Instance.ValueChanged.InvokeAsync("ECSS-MM-PWR"));
            var confirmWithValue = dialog.Instance.ConfirmationPackageName;

            dialog.Instance.OnConfirmationPackageNameChanged(null!);
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
                parameters.Add(x => x.PackageName, "ECSS-MM-PWR");
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

            // Case 4: Scope not found
            this.scopeServiceMock.Setup(x => x.ReadByShortNameAsync(this.userContext, It.IsAny<CancellationToken>(), It.Is<string[]>(a => ((IEnumerable<string>)a).Contains("unknown")))).ReturnsAsync(ImmutableList<IScope>.Empty);
            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync("unknown"));
            await dialog.InvokeAsync(() => confirmInput.Instance.ValueChanged.InvokeAsync("ECSS-MM-PWR"));
            await dialog.InvokeAsync(() => submitButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedScope, Is.Null);
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(dialog.Instance.TargetScope)), Is.True);
            }

            // Case 5: Current owner scope transfer attempt fails
            var currentScope = new Organization { Id = Guid.NewGuid(), ShortName = "starion" };
            this.scopeServiceMock.Setup(x => x.ReadByShortNameAsync(this.userContext, It.IsAny<CancellationToken>(), It.Is<string[]>(a => ((IEnumerable<string>)a).Contains("starion")))).ReturnsAsync(ImmutableList.Create<IScope>(currentScope));
            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync("starion"));
            await dialog.InvokeAsync(() => confirmInput.Instance.ValueChanged.InvokeAsync("ECSS-MM-PWR"));
            await dialog.InvokeAsync(() => submitButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedScope, Is.Null);
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(dialog.Instance.TargetScope)), Is.True);
            }

            // Case 6: Target organization without administrative rights -> validation fails
            var unownedOrg = new Organization { Id = Guid.NewGuid(), ShortName = "unownedorg", Administrator = [Guid.NewGuid()] };
            this.scopeServiceMock.Setup(x => x.ReadByShortNameAsync(this.userContext, It.IsAny<CancellationToken>(), It.Is<string[]>(a => ((IEnumerable<string>)a).Contains("unownedorg")))).ReturnsAsync(ImmutableList.Create<IScope>(unownedOrg));
            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync("unownedorg"));
            await dialog.InvokeAsync(() => confirmInput.Instance.ValueChanged.InvokeAsync("ECSS-MM-PWR"));
            await dialog.InvokeAsync(() => submitButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedScope, Is.Null);
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(dialog.Instance.TargetScope)), Is.True);
            }

            // Case 7: Valid transfer to target organization where user is an administrator -> success
            var validOrg = new Organization { Id = Guid.NewGuid(), ShortName = "validorg", Administrator = [this.currentUserId] };
            this.scopeServiceMock.Setup(x => x.ReadByShortNameAsync(this.userContext, It.IsAny<CancellationToken>(), It.Is<string[]>(a => ((IEnumerable<string>)a).Contains("validorg")))).ReturnsAsync(ImmutableList.Create<IScope>(validOrg));
            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync("validorg"));
            await dialog.InvokeAsync(() => confirmInput.Instance.ValueChanged.InvokeAsync("ECSS-MM-PWR"));
            await dialog.InvokeAsync(() => submitButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedScope, Is.EqualTo(validOrg));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }

            // Case 8: Valid transfer to target account -> success
            var recipientAccount = new Account { Id = Guid.NewGuid(), ShortName = "recipientuser" };
            this.scopeServiceMock.Setup(x => x.ReadByShortNameAsync(this.userContext, It.IsAny<CancellationToken>(), It.Is<string[]>(a => ((IEnumerable<string>)a).Contains("recipientuser")))).ReturnsAsync(ImmutableList.Create<IScope>(recipientAccount));
            await dialog.InvokeAsync(() => targetInput.Instance.ValueChanged.InvokeAsync("recipientuser"));
            await dialog.InvokeAsync(() => confirmInput.Instance.ValueChanged.InvokeAsync("ECSS-MM-PWR"));
            await dialog.InvokeAsync(() => submitButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(receivedScope, Is.EqualTo(recipientAccount));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Exactly(2));
            }
        }
    }
}
