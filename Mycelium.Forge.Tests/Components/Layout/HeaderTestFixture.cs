// ------------------------------------------------------------------------------------------------
// <copyright file="HeaderTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Layout
{
    using System;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Components.Layout;
    using Mycelium.Forge.Services;

    [TestFixture]
    public class HeaderTestFixture
    {
        private BunitContext context;
        private Mock<IThemeService> themeServiceMock;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.themeServiceMock = new Mock<IThemeService>();
            this.themeServiceMock.SetupAdd(x => x.OnChange += It.IsAny<Action>());
            this.themeServiceMock.SetupRemove(x => x.OnChange -= It.IsAny<Action>());
            this.themeServiceMock.Setup(x => x.ToggleDarkMode()).Returns(Task.CompletedTask);
            this.themeServiceMock.Setup(x => x.InitializeThemeAsync()).Returns(Task.CompletedTask);

            this.context.Services.AddSingleton(this.themeServiceMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyDispose()
        {
            var header = this.context.Render<Header>();
            Assert.DoesNotThrow(() => header.Instance.Dispose());
        }

        [Test]
        public void VerifyGetNavLinkClass()
        {
            var nav = this.context.Services.GetRequiredService<NavigationManager>();
            nav.NavigateTo("/packages");

            var header = this.context.Render<Header>();

            var activeClass = header.Instance.GetNavLinkClass("packages");
            var inactiveClass = header.Instance.GetNavLinkClass("documentation");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(activeClass, Does.Not.Contain("text-muted-foreground"));
                Assert.That(inactiveClass, Does.Contain("text-muted-foreground"));
            }
        }

        [Test]
        public void VerifyIsDocumentationPage()
        {
            var header = this.context.Render<Header>();
            var isDocsAtRoot = header.Instance.IsDocumentationPage();
            Assert.That(isDocsAtRoot, Is.False);
        }

        [Test]
        public void VerifyIsHomePage()
        {
            var header = this.context.Render<Header>();
            var isHome = header.Instance.IsHomePage();
            Assert.That(isHome, Is.True);
        }

        [Test]
        public void VerifyIsRouteActive()
        {
            var header = this.context.Render<Header>();

            var emptyResult = header.Instance.IsRouteActive(string.Empty);
            var rootResult = header.Instance.IsRouteActive("/");
            var nonMatchResult = header.Instance.IsRouteActive("documentation/overview");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyResult, Is.False);
                Assert.That(rootResult, Is.True);
                Assert.That(nonMatchResult, Is.False);
            }
        }

        [Test]
        public void VerifyOnSearchInputChanged()
        {
            var header = this.context.Render<Header>();
            header.Instance.OnSearchInputChanged("sysml");
            Assert.That(header.Instance.SearchQuery, Is.EqualTo("sysml"));
        }

        [Test]
        public void VerifySelectLanguage()
        {
            var header = this.context.Render<Header>();
            header.Instance.SelectLanguage("PT");
            Assert.That(header.Instance.SelectedLanguage, Is.EqualTo("PT"));
        }

        [Test]
        public async Task VerifyToggleDarkMode()
        {
            var header = this.context.Render<Header>();
            await header.InvokeAsync(() => header.Instance.ToggleDarkMode());
            this.themeServiceMock.Verify(x => x.ToggleDarkMode(), Times.Once);
        }
    }
}
