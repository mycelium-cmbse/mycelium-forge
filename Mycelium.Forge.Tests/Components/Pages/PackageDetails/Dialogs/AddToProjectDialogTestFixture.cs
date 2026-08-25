// ------------------------------------------------------------------------------------------------
// <copyright file="AddToProjectDialogTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageDetails.Dialogs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageDetails.Dialogs;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Models.Package;

    [TestFixture]
    public class AddToProjectDialogTestFixture
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
        public async Task VerifyOnAddDependencyClicked()
        {
            AddToProjectResult? capturedResult = null;
            var onResult = new EventCallbackFactory().Create(this, (AddToProjectResult r) => { capturedResult = r; });

            var dialog = this.context.Render<AddToProjectDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.OnResult, onResult);
            });

            // Empty SelectedProject -> validation fails
            dialog.Instance.OnProjectChanged(string.Empty);
            await dialog.InvokeAsync(() => dialog.Instance.OnAddDependencyClicked());

            Assert.That(capturedResult, Is.Null);

            // Valid inputs -> success
            dialog.Instance.OnProjectChanged("Spacecraft Mission");
            dialog.Instance.OnVersionConstraintChanged("^1.3.0");
            await dialog.InvokeAsync(() => dialog.Instance.OnAddDependencyClicked());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedResult, Is.Not.Null);
                Assert.That(capturedResult.ProjectName, Is.EqualTo("Spacecraft Mission"));
                Assert.That(capturedResult.VersionConstraint, Is.EqualTo("^1.3.0"));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnCancelClicked()
        {
            var onCancelCalled = false;
            var onCancel = new EventCallbackFactory().Create(this, () => { onCancelCalled = true; });

            var dialog = this.context.Render<AddToProjectDialog>(parameters =>
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
        public void VerifyOnInitialized()
        {
            var projects = new List<string> { "Spacecraft Mission", "CubeSat Constellation" };
            var package = new PackageModel(new Package { Name = "ECSS-MM-PWR" }, "@starion", "1.3.0");

            var dialog = this.context.Render<AddToProjectDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.Projects, projects);
                parameters.Add(x => x.Package, package);
            });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(dialog.Instance.SelectedProject, Is.EqualTo("Spacecraft Mission"));
                Assert.That(dialog.Instance.VersionConstraint, Is.EqualTo(package.GetDefaultVersionConstraint()));
            }
        }

        [Test]
        public void VerifyOnProjectChanged()
        {
            var dialog = this.context.Render<AddToProjectDialog>(parameters => { parameters.AddCascadingValue(this.dialogReferenceMock.Object); });

            dialog.Instance.OnProjectChanged("CubeSat Constellation");
            var withValue = dialog.Instance.SelectedProject;

            dialog.Instance.OnProjectChanged(null);
            var withNull = dialog.Instance.SelectedProject;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(withValue, Is.EqualTo("CubeSat Constellation"));
                Assert.That(withNull, Is.EqualTo(string.Empty));
            }
        }

        [Test]
        public void VerifyOnVersionConstraintChanged()
        {
            var dialog = this.context.Render<AddToProjectDialog>(parameters => { parameters.AddCascadingValue(this.dialogReferenceMock.Object); });

            dialog.Instance.OnVersionConstraintChanged("^2.0.0");
            var withValue = dialog.Instance.VersionConstraint;

            dialog.Instance.OnVersionConstraintChanged(null);
            var withNull = dialog.Instance.VersionConstraint;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(withValue, Is.EqualTo("^2.0.0"));
                Assert.That(withNull, Is.EqualTo(string.Empty));
            }
        }
    }
}
