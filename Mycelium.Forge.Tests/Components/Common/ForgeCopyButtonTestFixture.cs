// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeCopyButtonTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Common
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Services;

    [TestFixture]
    public class ForgeCopyButtonTestFixture
    {
        private BunitContext context;
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
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyCopyTextToClipboard()
        {
            this.jsInteropMock
                .Setup(x => x.CopyToClipboard(It.IsAny<string>()))
                .ReturnsAsync(true);

            var copiedResult = false;
            var onCopied = new EventCallbackFactory().Create(this, (bool result) => { copiedResult = result; });

            var copyButton = this.context.Render<ForgeCopyButton>(parameters =>
            {
                parameters.Add(x => x.Text, "sample text to copy");
                parameters.Add(x => x.Label, "Copy");
                parameters.Add(x => x.OnCopied, onCopied);
            });

            await copyButton.Find("button").ClickAsync(new MouseEventArgs());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(copyButton.Instance.IsCopied, Is.True);
                Assert.That(copiedResult, Is.True);
                Assert.That(copyButton.Markup, Does.Contain("Copied!"));
                this.jsInteropMock.Verify(x => x.CopyToClipboard("sample text to copy"), Times.Once);
            }

            // Test when CopyToClipboard returns false
            this.jsInteropMock
                .Setup(x => x.CopyToClipboard("failing text"))
                .ReturnsAsync(false);

            var failingButton = this.context.Render<ForgeCopyButton>(parameters =>
            {
                parameters.Add(x => x.Text, "failing text");
                parameters.Add(x => x.OnCopied, onCopied);
            });

            await failingButton.InvokeAsync(() => failingButton.Instance.CopyTextToClipboard());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(failingButton.Instance.IsCopied, Is.False);
                Assert.That(copiedResult, Is.False);
            }

            // Test when Text is null or whitespace
            var emptyButton = this.context.Render<ForgeCopyButton>(parameters => { parameters.Add(x => x.Text, string.Empty); });

            await emptyButton.InvokeAsync(() => emptyButton.Instance.CopyTextToClipboard());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyButton.Instance.IsCopied, Is.False);
                this.jsInteropMock.Verify(x => x.CopyToClipboard(string.Empty), Times.Never);
            }
        }

        [Test]
        public void VerifyGetButtonClass()
        {
            var iconOnlyButton = this.context.Render<ForgeCopyButton>(parameters =>
            {
                parameters.Add(x => x.Label, string.Empty);
                parameters.Add(x => x.Class, string.Empty);
            });

            var labeledButton = this.context.Render<ForgeCopyButton>(parameters =>
            {
                parameters.Add(x => x.Label, "Copy Text");
                parameters.Add(x => x.Class, string.Empty);
                parameters.Add(x => x.ShowDropdownChevron, true);
            });

            var customClassButton = this.context.Render<ForgeCopyButton>(parameters => { parameters.Add(x => x.Class, "custom-button-class"); });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(iconOnlyButton.Find("button").GetAttribute("class"), Does.Contain("p-1 rounded text-muted-foreground"));
                Assert.That(labeledButton.Find("button").GetAttribute("class"), Does.Contain("flex items-center gap-1.5"));
                Assert.That(labeledButton.Markup, Does.Contain("Copy Text"));
                Assert.That(customClassButton.Find("button").GetAttribute("class"), Does.Contain("custom-button-class"));
            }
        }
    }
}
