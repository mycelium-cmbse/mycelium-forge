// ------------------------------------------------------------------------------------------------
// <copyright file="JsInteropTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Services
{
    using System.Threading.Tasks;

    using Microsoft.JSInterop;
    using Microsoft.JSInterop.Infrastructure;

    using Moq;

    using Mycelium.Forge.Services;

    [TestFixture]
    public class JsInteropTestFixture
    {
        private Mock<IJSRuntime> jsRuntimeMock;
        private JsInterop jsInterop;

        [SetUp]
        public void SetUp()
        {
            this.jsRuntimeMock = new Mock<IJSRuntime>();
            this.jsInterop = new JsInterop(this.jsRuntimeMock.Object);
        }

        [Test]
        public async Task VerifyCopyToClipboard()
        {
            var emptyResult = await this.jsInterop.CopyToClipboard(string.Empty);
            var nullResult = await this.jsInterop.CopyToClipboard(null);

            this.jsRuntimeMock
                .Setup(x => x.InvokeAsync<bool>("forgeInterop.copyToClipboard", It.Is<object[]>(args => args.Length == 1 && (string)args[0] == "test text")))
                .ReturnsAsync(true);

            var successResult = await this.jsInterop.CopyToClipboard("test text");

            this.jsRuntimeMock
                .Setup(x => x.InvokeAsync<bool>("forgeInterop.copyToClipboard", It.Is<object[]>(args => args.Length == 1 && (string)args[0] == "error text")))
                .ThrowsAsync(new JSException("Clipboard error"));

            var exceptionResult = await this.jsInterop.CopyToClipboard("error text");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyResult, Is.False);
                Assert.That(nullResult, Is.False);
                Assert.That(successResult, Is.True);
                Assert.That(exceptionResult, Is.False);
            }
        }

        [Test]
        public async Task VerifyGetDarkMode()
        {
            this.jsRuntimeMock
                .Setup(x => x.InvokeAsync<bool>("forgeInterop.getDarkMode", It.IsAny<object[]>()))
                .ReturnsAsync(true);

            var isDark = await this.jsInterop.GetDarkMode();

            Assert.That(isDark, Is.True);
        }

        [Test]
        public async Task VerifySetDarkMode()
        {
            this.jsRuntimeMock
                .Setup(x => x.InvokeAsync<IJSVoidResult>("forgeInterop.setDarkMode", It.Is<object[]>(args => args.Length == 1 && (bool)args[0] == true)))
                .Returns(new ValueTask<IJSVoidResult>(default(IJSVoidResult)!));

            await this.jsInterop.SetDarkMode(true);

            this.jsRuntimeMock.Verify(x => x.InvokeAsync<IJSVoidResult>("forgeInterop.setDarkMode", It.Is<object[]>(args => args.Length == 1 && (bool)args[0] == true)), Times.Once);
        }
    }
}
