// ------------------------------------------------------------------------------------------------
// <copyright file="ApiKeyCreatedDialogTestFixture.cs" company="Starion Group S.A.">
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
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.ApiKeys.Dialogs;
    using Mycelium.Forge.Models.ApiKey;
    using Mycelium.Forge.Services;

    [TestFixture]
    public class ApiKeyCreatedDialogTestFixture
    {
        private BunitContext context;
        private Mock<IDialogReference> dialogReferenceMock;
        private Mock<IJsInterop> jsInteropMock;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.jsInteropMock = new Mock<IJsInterop>();
            this.context.Services.AddSingleton(this.jsInteropMock.Object);

            this.dialogReferenceMock = new Mock<IDialogReference>();
            this.dialogReferenceMock.Setup(x => x.CloseAsync(It.IsAny<DialogResult>())).Returns(Task.CompletedTask);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyOnDoneClicked()
        {
            var onDoneCalled = false;
            var onDone = new EventCallbackFactory().Create(this, () => { onDoneCalled = true; });

            var keyModel = new ApiKeyModel(
                new APIKey { Name = "ci-bot-token" },
                "@starion",
                "Read, Write, Publish",
                "mfg_secret_token_123456")
            {
                ExpirationText = "Expires in 30 days"
            };

            var dialog = this.context.Render<ApiKeyCreatedDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.CreatedKey, keyModel);
                parameters.Add(x => x.OnDone, onDone);
            });

            await dialog.InvokeAsync(() => dialog.Instance.OnDoneClicked());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(onDoneCalled, Is.True);
                Assert.That(dialog.Markup, Does.Contain("ci-bot-token"));
                Assert.That(dialog.Markup, Does.Contain("@starion"));
                Assert.That(dialog.Markup, Does.Contain("mfg_secret_token_123456"));
                Assert.That(dialog.Markup, Does.Contain("Read, Write, Publish"));
                Assert.That(dialog.Markup, Does.Contain("Expires in 30 days"));
                this.dialogReferenceMock.Verify(x => x.CloseAsync(It.IsAny<DialogResult>()), Times.Once);
            }

            var dialogNoRef = this.context.Render<ApiKeyCreatedDialog>(parameters =>
            {
                parameters.AddCascadingValue(this.dialogReferenceMock.Object);
                parameters.Add(x => x.CreatedKey, keyModel);
            });

            dialogNoRef.Instance.DialogReference = null;
            Assert.DoesNotThrowAsync(() => dialogNoRef.InvokeAsync(() => dialogNoRef.Instance.OnDoneClicked()));
        }
    }
}
