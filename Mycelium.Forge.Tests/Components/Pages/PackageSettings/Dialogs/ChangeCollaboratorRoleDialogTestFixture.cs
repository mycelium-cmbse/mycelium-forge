// ------------------------------------------------------------------------------------------------
// <copyright file="ChangeCollaboratorRoleDialogTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageSettings.Dialogs
{
    using System;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Components.Pages.PackageSettings.Dialogs;

    [TestFixture]
    public class ChangeCollaboratorRoleDialogTestFixture
    {
        private BunitContext context;
        private Mock<IDialogReference> dialogReferenceMock;
        private Account testCollaborator;

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

            this.testCollaborator = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Sam Developer",
                ShortName = "sam.dev"
            };
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

            var dialog = this.context.Render<ChangeCollaboratorRoleDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Collaborator, this.testCollaborator);
                parameters.Add(x => x.CurrentRole, PackageInvitationKind.MAINTAINER);
                parameters.Add(x => x.OnCancel, onCancel);
            });

            var cancelButton = dialog.Find("#change-collaborator-role-cancel-button");
            await dialog.InvokeAsync(() => cancelButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(onCancelCalled, Is.True);
                this.dialogReferenceMock.Verify(x => x.CancelAsync(), Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnRoleChanged()
        {
            var dialog = this.context.Render<ChangeCollaboratorRoleDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Collaborator, this.testCollaborator);
                parameters.Add(x => x.CurrentRole, PackageInvitationKind.MAINTAINER);
            });

            var initialSelectedRole = dialog.Instance.SelectedRole;

            var roleSelect = dialog.FindComponent<ForgeSelect<PackageInvitationKind>>();
            await dialog.InvokeAsync(() => roleSelect.Instance.ValueChanged.InvokeAsync(PackageInvitationKind.OWNER));

            dialog.Render(parameters => { parameters.Add(x => x.CurrentRole, PackageInvitationKind.MAINTAINER); });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(initialSelectedRole, Is.EqualTo(PackageInvitationKind.MAINTAINER));
                Assert.That(dialog.Instance.SelectedRole, Is.EqualTo(PackageInvitationKind.OWNER));
            }
        }

        [Test]
        public async Task VerifyOnSaveClicked()
        {
            PackageInvitationKind? receivedRole = null;
            var onResult = new EventCallbackFactory().Create(this, (PackageInvitationKind role) => { receivedRole = role; });

            var dialog = this.context.Render<ChangeCollaboratorRoleDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Collaborator, this.testCollaborator);
                parameters.Add(x => x.CurrentRole, PackageInvitationKind.MAINTAINER);
                parameters.Add(x => x.OnResult, onResult);
            });

            var roleSelect = dialog.FindComponent<ForgeSelect<PackageInvitationKind>>();
            await dialog.InvokeAsync(() => roleSelect.Instance.ValueChanged.InvokeAsync(PackageInvitationKind.OWNER));

            var saveButton = dialog.Find("#change-collaborator-role-submit-button");
            await dialog.InvokeAsync(() => saveButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(dialog.Markup, Does.Contain("Sam Developer"));
                Assert.That(receivedRole, Is.EqualTo(PackageInvitationKind.OWNER));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }
        }
    }
}
