// ------------------------------------------------------------------------------------------------
// <copyright file="ThemeServiceTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Services
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.JSInterop;

    using Moq;

    using Mycelium.Forge.Services;

    [TestFixture]
    public class ThemeServiceTestFixture
    {
        private Mock<IJsInterop> jsInteropMock;
        private Mock<ILogger<ThemeService>> loggerMock;
        private ThemeService themeService;

        [SetUp]
        public void SetUp()
        {
            this.jsInteropMock = new Mock<IJsInterop>();
            this.loggerMock = new Mock<ILogger<ThemeService>>();
            this.themeService = new ThemeService(this.jsInteropMock.Object, this.loggerMock.Object);
        }

        [Test]
        public async Task VerifyInitializeThemeAsync()
        {
            var changeFired = false;
            this.themeService.OnChange += () => changeFired = true;

            this.jsInteropMock.Setup(x => x.GetDarkMode()).ReturnsAsync(true);
            await this.themeService.InitializeThemeAsync();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.themeService.IsDarkMode, Is.True);
                Assert.That(changeFired, Is.True);
            }

            await this.themeService.InitializeThemeAsync();
            this.jsInteropMock.Verify(x => x.GetDarkMode(), Times.Once);

            var failingService = new ThemeService(this.jsInteropMock.Object, this.loggerMock.Object);
            this.jsInteropMock.Setup(x => x.GetDarkMode()).ThrowsAsync(new JSException("JS Error"));
            Assert.DoesNotThrowAsync(async () => await failingService.InitializeThemeAsync());

            var invalidOpService = new ThemeService(this.jsInteropMock.Object, this.loggerMock.Object);
            this.jsInteropMock.Setup(x => x.GetDarkMode()).ThrowsAsync(new InvalidOperationException("Circuit error"));
            Assert.DoesNotThrowAsync(async () => await invalidOpService.InitializeThemeAsync());

            var generalExService = new ThemeService(this.jsInteropMock.Object, this.loggerMock.Object);
            this.jsInteropMock.Setup(x => x.GetDarkMode()).ThrowsAsync(new Exception("General error"));
            Assert.DoesNotThrowAsync(async () => await generalExService.InitializeThemeAsync());
        }

        [Test]
        public async Task VerifySetDarkMode()
        {
            var changeCount = 0;
            this.themeService.OnChange += () => changeCount++;

            await this.themeService.SetDarkMode(true);
            await this.themeService.SetDarkMode(true);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.themeService.IsDarkMode, Is.True);
                Assert.That(changeCount, Is.EqualTo(1));
                this.jsInteropMock.Verify(x => x.SetDarkMode(true), Times.Once);
            }

            this.jsInteropMock.Setup(x => x.SetDarkMode(false)).ThrowsAsync(new JSException("error"));
            Assert.DoesNotThrowAsync(async () => await this.themeService.SetDarkMode(false));

            var failingService2 = new ThemeService(this.jsInteropMock.Object, this.loggerMock.Object);
            this.jsInteropMock.Setup(x => x.SetDarkMode(true)).ThrowsAsync(new InvalidOperationException("error"));
            Assert.DoesNotThrowAsync(async () => await failingService2.SetDarkMode(true));

            var failingService3 = new ThemeService(this.jsInteropMock.Object, this.loggerMock.Object);
            this.jsInteropMock.Setup(x => x.SetDarkMode(true)).ThrowsAsync(new Exception("error"));
            Assert.DoesNotThrowAsync(async () => await failingService3.SetDarkMode(true));
        }

        [Test]
        public async Task VerifyToggleDarkMode()
        {
            var changeCount = 0;
            this.themeService.OnChange += () => changeCount++;

            await this.themeService.ToggleDarkMode();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.themeService.IsDarkMode, Is.True);
                Assert.That(changeCount, Is.EqualTo(1));
                this.jsInteropMock.Verify(x => x.SetDarkMode(true), Times.Once);
            }

            await this.themeService.ToggleDarkMode();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.themeService.IsDarkMode, Is.False);
                Assert.That(changeCount, Is.EqualTo(2));
                this.jsInteropMock.Verify(x => x.SetDarkMode(false), Times.Once);
            }
        }
    }
}
