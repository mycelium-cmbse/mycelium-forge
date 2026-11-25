// ------------------------------------------------------------------------------------------------
// <copyright file="ChangeEmailDialogTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.AccountSettings.Dialogs
{
    using System.Linq;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;

    using Moq;

    using Mycelium.Forge.Components.Pages.AccountSettings.Dialogs;
    using Mycelium.Forge.Tests.TestHelpers;

    [TestFixture]
    public class ChangeEmailDialogTestFixture
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

            var dialog = this.context.Render<ChangeEmailDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.CurrentEmail, "current@example.com");
                parameters.Add(x => x.OnCancel, onCancel);
            });

            var cancelButton = dialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("change-email-cancel-button"));
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
            string? capturedEmail = null;
            var onResult = new EventCallbackFactory().Create(this, (string email) => { capturedEmail = email; });

            var dialog = this.context.Render<ChangeEmailDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.CurrentEmail, "current@example.com");
                parameters.Add(x => x.OnResult, onResult);
            });

            var input = dialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("change-email-input"));
            var confirmButton = dialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("change-email-submit-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(input, Is.Not.Null);
                Assert.That(confirmButton, Is.Not.Null);
            }

            // Case 1: Empty or invalid format
            await dialog.InvokeAsync(() => confirmButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedEmail, Is.Null);
                Assert.That(dialog.Instance.ValidationManager[nameof(ChangeEmailDialog.NewEmail)], Does.Contain("Please enter a valid email address."));
            }

            await dialog.InvokeAsync(() => input!.Instance.ValueChanged.InvokeAsync("not-an-email"));
            await dialog.InvokeAsync(() => confirmButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedEmail, Is.Null);
                Assert.That(dialog.Instance.ValidationManager[nameof(ChangeEmailDialog.NewEmail)], Does.Contain("Please enter a valid email address."));
            }

            // Case 2: Same as current email (case-insensitive with whitespace)
            await dialog.InvokeAsync(() => input!.Instance.ValueChanged.InvokeAsync("  CURRENT@EXAMPLE.COM  "));
            await dialog.InvokeAsync(() => confirmButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedEmail, Is.Null);
                Assert.That(dialog.Instance.ValidationManager[nameof(ChangeEmailDialog.NewEmail)], Does.Contain("New email address must be different from your current email."));
            }

            // Case 3: Valid email is trimmed and lowercased before invoking OnResult and closing
            await dialog.InvokeAsync(() => input!.Instance.ValueChanged.InvokeAsync("  New.Email@Example.COM  "));

            await dialog.InvokeAsync(() => confirmButton!.Find("button").ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedEmail, Is.EqualTo("new.email@example.com"));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnNewEmailChanged()
        {
            var dialog = this.context.Render<ChangeEmailDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.CurrentEmail, "current@example.com");
            });

            var input = dialog.FindComponents<BbInput>().FirstOrDefault(x => x.Instance.HasId("change-email-input"));
            var confirmButton = dialog.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("change-email-submit-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(input, Is.Not.Null);
                Assert.That(confirmButton, Is.Not.Null);
            }

            await dialog.InvokeAsync(() => confirmButton!.Find("button").ClickAsync());
            Assert.That(dialog.Instance.ValidationManager.HasError(nameof(ChangeEmailDialog.NewEmail)), Is.True);

            await dialog.InvokeAsync(() => input!.Instance.ValueChanged.InvokeAsync("new@example.com"));
            var withValue = dialog.Instance.NewEmail;
            var hasErrorAfterChange = dialog.Instance.ValidationManager.HasError(nameof(ChangeEmailDialog.NewEmail));

            await dialog.InvokeAsync(() => input!.Instance.ValueChanged.InvokeAsync(null!));
            var withNull = dialog.Instance.NewEmail;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(withValue, Is.EqualTo("new@example.com"));
                Assert.That(hasErrorAfterChange, Is.False);
                Assert.That(withNull, Is.EqualTo(string.Empty));
            }
        }
    }
}
