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
    using System.Threading;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using ErrorOr;

    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.Login;

    using Error = ErrorOr.Error;

    /// <summary>
    /// Test fixture for <see cref="Login" />.
    /// </summary>
    [TestFixture]
    public class LoginTestFixture
    {
        private BunitContext context;
        private Mock<ILoginViewModel> viewModelMock;
        private NotificationService notificationService;

        /// <summary>
        /// Sets up mock dependencies and test context before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<ILoginViewModel>();

            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.notificationService = new NotificationService();
            this.context.Services.AddSingleton<INotificationService>(this.notificationService);
        }

        /// <summary>
        /// Tears down the BUnit context after each test.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        /// <summary>
        /// Verifies clicking the continue with SSO button triggers a notification.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        [Test]
        public async Task VerifyOnContinueWithSso()
        {
            var loginPage = this.context.Render<Login>();
            var ssoButton = loginPage.Find("#login-sso-button");

            await loginPage.InvokeAsync(() => ssoButton.ClickAsync());

            Assert.That(this.notificationService.Results.Items, Has.Count.EqualTo(1));
        }

        /// <summary>
        /// Verifies updating email and password fields on the login page.
        /// </summary>
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

        /// <summary>
        /// Verifies that the login page initializes the view model.
        /// </summary>
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

        /// <summary>
        /// Verifies login form submission scenarios.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        [Test]
        public async Task VerifyOnLogin()
        {
            var loginPage = this.context.Render<Login>();
            var loginButton = loginPage.Find("#login-submit-button");

            this.viewModelMock.Setup(x => x.Email).Returns(string.Empty);
            this.viewModelMock.Setup(x => x.Password).Returns(string.Empty);
            await loginPage.InvokeAsync(() => loginButton.ClickAsync());
            this.viewModelMock.Verify(x => x.Login(It.IsAny<CancellationToken>()), Times.Never);

            this.viewModelMock.Setup(x => x.Email).Returns("user@example.com");
            this.viewModelMock.Setup(x => x.Password).Returns("Secret123!");
            this.viewModelMock.Setup(x => x.Login(It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

            await loginPage.InvokeAsync(() => loginButton.ClickAsync());

            var nav = this.context.Services.GetRequiredService<NavigationManager>();

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.Login(It.IsAny<CancellationToken>()), Times.Once);
                Assert.That(nav.Uri, Does.Contain(PageRoutes.AuthLogin));
            }

            this.viewModelMock.Setup(x => x.Login(It.IsAny<CancellationToken>())).ReturnsAsync(Error.Failure(description: "Invalid credentials"));
            await loginPage.InvokeAsync(() => loginButton.ClickAsync());

            this.viewModelMock.Verify(x => x.Login(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}
