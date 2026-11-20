// ------------------------------------------------------------------------------------------------
// <copyright file="InputPromptDialogTestFixture.cs" company="Starion Group S.A.">
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

    using ErrorOr;

    using Microsoft.AspNetCore.Components;

    using Moq;

    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Tests.TestHelpers;

    [TestFixture]
    public class InputPromptDialogTestFixture
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
        public async Task VerifyOnCancelClicked()
        {
            var onCancelCalled = false;
            var onCancel = new EventCallbackFactory().Create(this, () => { onCancelCalled = true; });

            var dialog = this.context.Render<InputPromptDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.OnCancel, onCancel);
            });

            var cancelButton = dialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-cancel-button"));
            Assert.That(cancelButton, Is.Not.Null);

            await dialog.InvokeAsync(() => cancelButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(onCancelCalled, Is.True);
                this.dialogReferenceMock.Verify(x => x.CancelAsync(), Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnConfirmClicked()
        {
            string? capturedResult = null;
            var onResult = new EventCallbackFactory().Create(this, (string value) => { capturedResult = value; });

            var validator = new Func<string, Task<ErrorOr<Success>>>(value => value.Contains(' ', StringComparison.Ordinal)
                ? Task.FromResult<ErrorOr<Success>>(Result.Success)
                : Task.FromResult<ErrorOr<Success>>(Error.Validation("Invalid", "Please enter two words.")));

            var dialog = this.context.Render<InputPromptDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Required, true);
                parameters.Add(x => x.ValidateAsync, validator);
                parameters.Add(x => x.OnResult, onResult);
            });

            var input = dialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-input"));
            var confirmButton = dialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("input-prompt-dialog-confirm-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(input, Is.Not.Null);
                Assert.That(confirmButton, Is.Not.Null);
            }

            // Case 1: Empty input fails Required check without closing
            await dialog.InvokeAsync(() => confirmButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedResult, Is.Null);
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(InputPromptDialog.InputValue)), Is.True);
                Assert.That(dialog.Instance.ValidationManager[nameof(InputPromptDialog.InputValue)], Does.Contain("This field is required."));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Never);
            }

            // Case 2: Custom ValidateAsync failure keeps dialog open and clears error on input change
            await dialog.InvokeAsync(() => input!.Instance.ValueChanged.InvokeAsync("Single"));

            Assert.That(dialog.Instance.ValidationManager.HasError(nameof(InputPromptDialog.InputValue)), Is.False);

            await dialog.InvokeAsync(() => confirmButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedResult, Is.Null);
                Assert.That(dialog.Instance.ValidationManager.HasError(nameof(InputPromptDialog.InputValue)), Is.True);
                Assert.That(dialog.Instance.ValidationManager[nameof(InputPromptDialog.InputValue)], Does.Contain("Please enter two words."));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Never);
            }

            // Case 3: Valid input trims value, invokes OnResult, and closes dialog
            await dialog.InvokeAsync(() => input!.Instance.ValueChanged.InvokeAsync("  Alex Updated  "));
            await dialog.InvokeAsync(() => confirmButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedResult, Is.EqualTo("Alex Updated"));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }
        }

        [Test]
        public void VerifyOnInitialized()
        {
            var dialog = this.context.Render<InputPromptDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.InitialValue, "Alex Rivera");
                parameters.Add(x => x.Description, "Enter your display name.");
                parameters.Add(x => x.Label, "Display name");
            });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(dialog.Instance.InputValue, Is.EqualTo("Alex Rivera"));
                Assert.That(dialog.Instance.HasDescription, Is.True);
                Assert.That(dialog.Instance.HasLabel, Is.True);
                Assert.That(dialog.Find("p").TextContent.Trim(), Is.EqualTo("Enter your display name."));
                Assert.That(dialog.Find("label").TextContent.Trim(), Is.EqualTo("Display name"));
            }
        }

        [Test]
        public void VerifyOnInputValueChanged()
        {
            var dialog = this.context.Render<InputPromptDialog>(parameters => { parameters.AddCascadingValue(this.dialogReferenceMock.Object); });

            dialog.Instance.OnInputValueChanged("Alex Updated");
            var withValue = dialog.Instance.InputValue;

            dialog.Instance.OnInputValueChanged(null);
            var withNull = dialog.Instance.InputValue;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(withValue, Is.EqualTo("Alex Updated"));
                Assert.That(withNull, Is.EqualTo(string.Empty));
            }
        }
    }
}
