// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeFileUploadTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    using Mycelium.Forge.Components.Common;

    [TestFixture]
    public class ForgeFileUploadTestFixture
    {
        private BunitContext context;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyGetContainerClass()
        {
            var defaultUpload = this.context.Render<ForgeFileUpload>();

            var disabledUpload = this.context.Render<ForgeFileUpload>(parameters => { parameters.Add(x => x.Disabled, true); });

            var draggingUpload = this.context.Render<ForgeFileUpload>();
            draggingUpload.Find("div").DragEnter();

            var customClassUpload = this.context.Render<ForgeFileUpload>(parameters => { parameters.Add(x => x.Class, "my-custom-dropzone"); });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(defaultUpload.Instance.GetContainerClass(), Does.Contain("border-input hover:border-primary"));
                Assert.That(disabledUpload.Instance.GetContainerClass(), Does.Contain("border-input opacity-60 cursor-not-allowed"));
                Assert.That(draggingUpload.Instance.GetContainerClass(), Does.Contain("border-primary bg-accent/30"));
                Assert.That(customClassUpload.Instance.GetContainerClass(), Does.Contain("my-custom-dropzone"));
            }
        }

        [Test]
        public void VerifyHandleDragEvents()
        {
            var upload = this.context.Render<ForgeFileUpload>();
            var container = upload.Find("div");

            container.DragEnter();
            var enterClass = upload.Instance.GetContainerClass();

            container.DragLeave();
            var leaveClass = upload.Instance.GetContainerClass();

            container.DragEnter();
            container.Drop();
            var dropClass = upload.Instance.GetContainerClass();

            var disabledUpload = this.context.Render<ForgeFileUpload>(parameters => { parameters.Add(x => x.Disabled, true); });
            disabledUpload.Find("div").DragEnter();
            var disabledClass = disabledUpload.Instance.GetContainerClass();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(enterClass, Does.Contain("border-primary bg-accent/30"));
                Assert.That(leaveClass, Does.Contain("border-input hover:border-primary"));
                Assert.That(dropClass, Does.Contain("border-input hover:border-primary"));
                Assert.That(disabledClass, Does.Contain("border-input opacity-60 cursor-not-allowed"));
            }
        }

        [Test]
        public void VerifyHandleFileChange()
        {
            var onChangeInvoked = false;
            IReadOnlyList<IBrowserFile> selectedResult = [];

            var onFilesSubmitted = new EventCallbackFactory().Create(this, (InputFileChangeEventArgs _) => { onChangeInvoked = true; });
            var onSelectedFilesChanged = new EventCallbackFactory().Create(this, (IReadOnlyList<IBrowserFile> files) => { selectedResult = files; });

            var singleUpload = this.context.Render<ForgeFileUpload>(parameters =>
            {
                parameters.Add(x => x.OnFilesSubmitted, onFilesSubmitted);
                parameters.Add(x => x.OnSelectedFilesChanged, onSelectedFilesChanged);
            });

            singleUpload.FindComponent<InputFile>().UploadFiles(InputFileContent.CreateFromText("sample content", "model.kpar"));

            var multipleResult = new List<IBrowserFile>();

            var multipleUpload = this.context.Render<ForgeFileUpload>(parameters =>
            {
                parameters.Add(x => x.MaxAllowedFiles, 3);
                parameters.Add(x => x.OnSelectedFilesChanged, new EventCallbackFactory().Create(this, (IReadOnlyList<IBrowserFile> files) => { multipleResult = [.. files]; }));
            });

            multipleUpload.FindComponent<InputFile>().UploadFiles(
                InputFileContent.CreateFromText("part1", "file1.kpar"),
                InputFileContent.CreateFromText("part2", "file2.kpar")
            );

            var disabledInvoked = false;

            var disabledUpload = this.context.Render<ForgeFileUpload>(parameters =>
            {
                parameters.Add(x => x.Disabled, true);
                parameters.Add(x => x.OnFilesSubmitted, new EventCallbackFactory().Create(this, (InputFileChangeEventArgs _) => { disabledInvoked = true; }));
            });

            disabledUpload.FindComponent<InputFile>().UploadFiles(InputFileContent.CreateFromText("disabled content", "disabled.kpar"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(onChangeInvoked, Is.True);
                Assert.That(selectedResult, Has.Count.EqualTo(1));
                Assert.That(multipleResult, Has.Count.EqualTo(2));
                Assert.That(disabledInvoked, Is.False);
            }
        }

        [Test]
        public void VerifyRenderLayout()
        {
            var upload = this.context.Render<ForgeFileUpload>(parameters =>
            {
                parameters.Add(x => x.Title, "Upload Title");
                parameters.Add(x => x.Subtitle, "Upload Subtitle");
                parameters.Add(x => x.ButtonText, "Browse files");
                parameters.Add(x => x.Accept, ".kpar,.zip");
                parameters.Add(x => x.MaxAllowedFiles, 5);
                parameters.Add(x => x.AdditionalAttributes, new Dictionary<string, object> { { "data-testid", "forge-upload-input" } });
            });

            var inputElement = upload.Find("input[type='file']");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(inputElement.GetAttribute("accept"), Is.EqualTo(".kpar,.zip"));
                Assert.That(inputElement.HasAttribute("multiple"), Is.True);
                Assert.That(inputElement.GetAttribute("data-testid"), Is.EqualTo("forge-upload-input"));
                Assert.That(upload.Markup, Does.Contain("Upload Title"));
                Assert.That(upload.Markup, Does.Contain("Upload Subtitle"));
                Assert.That(upload.Markup, Does.Contain("Browse files"));
            }
        }
    }
}
