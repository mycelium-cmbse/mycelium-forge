// ------------------------------------------------------------------------------------------------
// <copyright file="CreateApiKeyDialogTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.ApiKeys.Dialogs
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;

    using Moq;

    using Mycelium.Forge.Components.Pages.ApiKeys.Dialogs;
    using Mycelium.Forge.Models.DialogResults;

    [TestFixture]
    public class CreateApiKeyDialogTestFixture
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

            var dialog = this.context.Render<CreateApiKeyDialog>(parameters =>
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

            var dialogNoRef = this.context.Render<CreateApiKeyDialog>(parameters => { parameters.AddCascadingValue(this.dialogReferenceMock.Object); });

            dialogNoRef.Instance.DialogReference = null;
            Assert.DoesNotThrowAsync(() => dialogNoRef.InvokeAsync(() => dialogNoRef.Instance.OnCancelClicked()));
        }

        [Test]
        public async Task VerifyOnCreateKeyClicked()
        {
            CreateApiKeyResult? capturedResult = null;
            var onResult = new EventCallbackFactory().Create(this, (CreateApiKeyResult r) => { capturedResult = r; });

            var dialog = this.context.Render<CreateApiKeyDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.OnResult, onResult);
            });

            // Empty key name -> validation fails
            dialog.Instance.OnKeyNameChanged(string.Empty);
            await dialog.InvokeAsync(() => dialog.Instance.OnCreateKeyClicked());

            Assert.That(capturedResult, Is.Null);

            // Valid key name -> success
            dialog.Instance.OnKeyNameChanged("ci-publish");
            dialog.Instance.OnScopeChanged("@starion");
            await dialog.InvokeAsync(() => dialog.Instance.OnCreateKeyClicked());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedResult, Is.Not.Null);
                Assert.That(capturedResult.KeyName, Is.EqualTo("ci-publish"));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }

            // Null DialogReference should not throw
            var dialogNoRef = this.context.Render<CreateApiKeyDialog>(parameters => { parameters.AddCascadingValue(this.dialogReferenceMock.Object); });

            dialogNoRef.Instance.DialogReference = null;
            dialogNoRef.Instance.OnKeyNameChanged("test");
            dialogNoRef.Instance.OnScopeChanged("@starion");
            Assert.DoesNotThrowAsync(() => dialogNoRef.InvokeAsync(() => dialogNoRef.Instance.OnCreateKeyClicked()));
        }

        [Test]
        public void VerifyOnKeyNameChanged()
        {
            var dialog = this.context.Render<CreateApiKeyDialog>(parameters => { parameters.AddCascadingValue(this.dialogReferenceMock.Object); });

            dialog.Instance.OnKeyNameChanged("my-api-key");
            var withValue = dialog.Instance.KeyName;

            dialog.Instance.OnKeyNameChanged(null);
            var withNull = dialog.Instance.KeyName;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(withValue, Is.EqualTo("my-api-key"));
                Assert.That(withNull, Is.EqualTo(string.Empty));
            }
        }

        [Test]
        public void VerifyOnScopeChanged()
        {
            var dialog = this.context.Render<CreateApiKeyDialog>(parameters => { parameters.AddCascadingValue(this.dialogReferenceMock.Object); });

            dialog.Instance.OnScopeChanged("@esa");
            var withValue = dialog.Instance.SelectedScope;

            dialog.Instance.OnScopeChanged(null);
            var withNull = dialog.Instance.SelectedScope;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(withValue, Is.EqualTo("@esa"));
                Assert.That(withNull, Is.EqualTo(string.Empty));
            }
        }
    }
}
