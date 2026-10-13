// ------------------------------------------------------------------------------------------------
// <copyright file="PublishUploadStepTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.Publish
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Mycelium.Forge.Components.Pages.Publish;

    [TestFixture]
    public class PublishUploadStepTestFixture
    {
        private BunitContext context;
        private IConfiguration configuration;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();
            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            var configurationManager = new ConfigurationManager();
            configurationManager["MaximumUploadFileSizeInMB"] = "100";
            this.configuration = configurationManager;

            this.context.Services.AddSingleton(this.configuration);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyHandleContinue()
        {
            var continuedFiles = new List<IBrowserFile>();
            var onContinue = new EventCallbackFactory().Create(this, (IBrowserFile file) => { continuedFiles.Add(file); });

            var uploadStepComponent = this.context.Render<PublishUploadStep>(parameters => { parameters.Add(x => x.OnContinue, onContinue); });

            var inputFileComponent = uploadStepComponent.FindComponent<InputFile>();
            inputFileComponent.UploadFiles(InputFileContent.CreateFromText("sample data", "library.kpar"));

            var continueButton = uploadStepComponent.Find("#publish-continue-button");
            await uploadStepComponent.InvokeAsync(() => continueButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(continuedFiles, Has.Count.EqualTo(1));
                Assert.That(continuedFiles[0].Name, Is.EqualTo("library.kpar"));
            }
        }

        [Test]
        public void VerifyHandleFilesSelected()
        {
            var uploadStepComponent = this.context.Render<PublishUploadStep>();

            var inputFileComponent = uploadStepComponent.FindComponent<InputFile>();
            inputFileComponent.UploadFiles(InputFileContent.CreateFromText("kpar content", "model.kpar"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(uploadStepComponent.Markup, Does.Contain("model.kpar"));
                Assert.That(uploadStepComponent.Markup, Does.Contain("SysML v2 (kpar)"));
                Assert.That(uploadStepComponent.Find("#publish-continue-button").HasAttribute("disabled"), Is.False);
            }

            inputFileComponent.UploadFiles(InputFileContent.CreateFromText("zip content", "archive.zip"));
            Assert.That(uploadStepComponent.Markup, Does.Contain("ECSS-E-TM-10-25"));

            inputFileComponent.UploadFiles(InputFileContent.CreateFromText("tar content", "model.tar.gz"));
            Assert.That(uploadStepComponent.Markup, Does.Contain("Capella"));

            inputFileComponent.UploadFiles(InputFileContent.CreateFromText("xyz content", "archive.xyz"));
            Assert.That(uploadStepComponent.Markup, Does.Contain(".xyz"));

            inputFileComponent.UploadFiles(InputFileContent.CreateFromText("no ext content", "archive"));
            Assert.That(uploadStepComponent.Markup, Does.Contain("unknown"));
        }

        [Test]
        public void VerifyRendering()
        {
            var uploadStepComponent = this.context.Render<PublishUploadStep>();

            var badges = uploadStepComponent.FindComponents<BbBadge>();
            var continueButton = uploadStepComponent.Find("#publish-continue-button");
            var publishingGuideLink = uploadStepComponent.FindComponent<BbButton>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(uploadStepComponent.Instance, Is.Not.Null);
                Assert.That(badges, Has.Count.EqualTo(3));
                Assert.That(uploadStepComponent.Markup, Does.Contain("SysML v2 (kpar)"));
                Assert.That(uploadStepComponent.Markup, Does.Contain("ECSS-E-TM-10-25"));
                Assert.That(uploadStepComponent.Markup, Does.Contain("Capella"));
                Assert.That(uploadStepComponent.Markup, Does.Contain("100 MB"));
                Assert.That(uploadStepComponent.Markup, Does.Contain("BEFORE YOU UPLOAD"));
                Assert.That(continueButton.HasAttribute("disabled"), Is.True);
                Assert.That(publishingGuideLink, Is.Not.Null);
            }
        }
    }
}
