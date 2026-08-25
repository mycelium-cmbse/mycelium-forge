// ------------------------------------------------------------------------------------------------
// <copyright file="CreateOrganizationDialogTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.AccountSettings.Dialogs
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;

    using Moq;

    using Mycelium.Forge.Components.Pages.AccountSettings.Dialogs;
    using Mycelium.Forge.Models.DialogResults;

    [TestFixture]
    public class CreateOrganizationDialogTestFixture
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
        public void VerifyOnBillingEmailChanged()
        {
            var dialog = this.context.Render<CreateOrganizationDialog>(parameters => { parameters.AddCascadingValue(this.dialogReferenceMock.Object); });

            dialog.Instance.OnBillingEmailChanged("billing@starion.eu");
            var withValue = dialog.Instance.BillingEmail;

            dialog.Instance.OnBillingEmailChanged(null);
            var withNull = dialog.Instance.BillingEmail;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(withValue, Is.EqualTo("billing@starion.eu"));
                Assert.That(withNull, Is.EqualTo(string.Empty));
            }
        }

        [Test]
        public async Task VerifyOnCancelClicked()
        {
            var onCancelCalled = false;
            var onCancel = new EventCallbackFactory().Create(this, () => { onCancelCalled = true; });

            var dialog = this.context.Render<CreateOrganizationDialog>(parameters =>
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
        public async Task VerifyOnCreateOrganizationClicked()
        {
            CreateOrganizationResult? capturedResult = null;
            var onResult = new EventCallbackFactory().Create(this, (CreateOrganizationResult r) => { capturedResult = r; });

            var dialog = this.context.Render<CreateOrganizationDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.OnResult, onResult);
            });

            // Empty fields -> validation fails
            await dialog.InvokeAsync(() => dialog.Instance.OnCreateOrganizationClicked());

            Assert.That(capturedResult, Is.Null);

            // All valid -> success
            dialog.Instance.OnOrganizationNameChanged("Starion Group");
            dialog.Instance.OnScopeChanged("@starion");
            dialog.Instance.OnBillingEmailChanged("billing@starion.eu");
            await dialog.InvokeAsync(() => dialog.Instance.OnCreateOrganizationClicked());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedResult, Is.Not.Null);
                Assert.That(capturedResult.OrganizationName, Is.EqualTo("Starion Group"));
                Assert.That(capturedResult.Scope, Is.EqualTo("@starion"));
                Assert.That(capturedResult.BillingEmail, Is.EqualTo("billing@starion.eu"));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }
        }

        [Test]
        public void VerifyOnOrganizationNameChanged()
        {
            var dialog = this.context.Render<CreateOrganizationDialog>(parameters => { parameters.AddCascadingValue(this.dialogReferenceMock.Object); });

            dialog.Instance.OnOrganizationNameChanged("Starion Group");
            var withValue = dialog.Instance.OrganizationName;

            dialog.Instance.OnOrganizationNameChanged(null);
            var withNull = dialog.Instance.OrganizationName;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(withValue, Is.EqualTo("Starion Group"));
                Assert.That(withNull, Is.EqualTo(string.Empty));
            }
        }

        [Test]
        public void VerifyOnScopeChanged()
        {
            var dialog = this.context.Render<CreateOrganizationDialog>(parameters => { parameters.AddCascadingValue(this.dialogReferenceMock.Object); });

            dialog.Instance.OnScopeChanged("@starion");
            var withValue = dialog.Instance.Scope;

            dialog.Instance.OnScopeChanged(null);
            var withNull = dialog.Instance.Scope;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(withValue, Is.EqualTo("@starion"));
                Assert.That(withNull, Is.EqualTo(string.Empty));
            }
        }
    }
}
