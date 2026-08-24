// ------------------------------------------------------------------------------------------------
// <copyright file="LoginTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using FluentResults;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Components.Pages;
    using Mycelium.Forge.ViewModels.Login;

    [TestFixture]
    public class LoginTestFixture
    {
        private BunitContext context;
        private Mock<ILoginViewModel> viewModelMock;
        private ToastService toastService;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<ILoginViewModel>();

            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.toastService = this.context.Services.GetRequiredService<ToastService>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyOnContinueWithSso()
        {
            var loginPage = this.context.Render<Login>();
            var ssoButton = loginPage.Find("#login-sso-button");

            await loginPage.InvokeAsync(() => ssoButton.ClickAsync());

            Assert.That(this.toastService.Toasts, Has.Count.EqualTo(1));
        }

        [Test]
        public void VerifyOnEmailAndPasswordChanged()
        {
            var loginPage = this.context.Render<Login>();

            loginPage.Instance.OnEmailChanged("user@example.com");
            loginPage.Instance.OnPasswordChanged("password123");

            this.viewModelMock.VerifySet(x => x.Email = "user@example.com", Times.Once);
            this.viewModelMock.VerifySet(x => x.Password = "password123", Times.Once);

            loginPage.Instance.OnEmailChanged(null);
            loginPage.Instance.OnPasswordChanged(null);

            this.viewModelMock.VerifySet(x => x.Email = string.Empty, Times.Once);
            this.viewModelMock.VerifySet(x => x.Password = string.Empty, Times.Once);
        }

        [Test]
        public void VerifyOnInitialized()
        {
            var loginPage = this.context.Render<Login>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(loginPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel(), Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnLogin()
        {
            var loginPage = this.context.Render<Login>();
            var loginButton = loginPage.Find("#login-submit-button");

            this.viewModelMock.SetupGet(x => x.Email).Returns(string.Empty);
            this.viewModelMock.SetupGet(x => x.Password).Returns(string.Empty);
            await loginPage.InvokeAsync(() => loginButton.ClickAsync());
            this.viewModelMock.Verify(x => x.Login(), Times.Never);

            this.viewModelMock.SetupGet(x => x.Email).Returns("user@example.com");
            this.viewModelMock.SetupGet(x => x.Password).Returns("Secret123!");
            this.viewModelMock.Setup(x => x.Login()).Returns(Result.Ok());

            await loginPage.InvokeAsync(() => loginButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.Login(), Times.Once);
                Assert.That(this.toastService.Toasts, Has.Count.EqualTo(1));
            }

            this.viewModelMock.Setup(x => x.Login()).Returns(Result.Fail("Invalid credentials"));
            await loginPage.InvokeAsync(() => loginButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.Login(), Times.Exactly(2));
                Assert.That(this.toastService.Toasts, Has.Count.EqualTo(2));
            }
        }
    }
}
