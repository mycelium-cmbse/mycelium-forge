// ------------------------------------------------------------------------------------------------
// <copyright file="SignUpTestFixture.cs" company="Starion Group S.A.">
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
    using Mycelium.Forge.ViewModels.SignUp;

    [TestFixture]
    public class SignUpTestFixture
    {
        private BunitContext context;
        private Mock<ISignUpViewModel> viewModelMock;
        private ToastService toastService;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<ISignUpViewModel>();

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
            var signUpPage = this.context.Render<SignUp>();
            var ssoButton = signUpPage.Find("#signup-sso-button");

            await signUpPage.InvokeAsync(() => ssoButton.ClickAsync());

            Assert.That(this.toastService.Toasts, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task VerifyOnCreateAccount()
        {
            var signUpPage = this.context.Render<SignUp>();
            var createAccountButton = signUpPage.Find("#signup-create-account-button");

            this.viewModelMock.Setup(x => x.Username).Returns(string.Empty);
            this.viewModelMock.Setup(x => x.Email).Returns(string.Empty);
            this.viewModelMock.Setup(x => x.Password).Returns(string.Empty);
            await signUpPage.InvokeAsync(() => createAccountButton.ClickAsync());
            this.viewModelMock.Verify(x => x.SignUp(), Times.Never);

            this.viewModelMock.Setup(x => x.Username).Returns("j.doe");
            this.viewModelMock.Setup(x => x.Email).Returns("j.doe@example.com");
            this.viewModelMock.Setup(x => x.Password).Returns("Secret123!");
            this.viewModelMock.Setup(x => x.SignUp()).Returns(Result.Ok());

            await signUpPage.InvokeAsync(() => createAccountButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.SignUp(), Times.Once);
                Assert.That(this.toastService.Toasts, Has.Count.EqualTo(1));
            }

            this.viewModelMock.Setup(x => x.SignUp()).Returns(Result.Fail("Registration failed"));
            await signUpPage.InvokeAsync(() => createAccountButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.SignUp(), Times.Exactly(2));
                Assert.That(this.toastService.Toasts, Has.Count.EqualTo(2));
            }
        }

        [Test]
        public void VerifyOnInitialized()
        {
            var signUpPage = this.context.Render<SignUp>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(signUpPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel(), Times.Once);
            }
        }

        [Test]
        public void VerifyOnInputChanged()
        {
            var signUpPage = this.context.Render<SignUp>();

            signUpPage.Instance.OnUsernameChanged("j.doe");
            signUpPage.Instance.OnEmailChanged("j.doe@example.com");
            signUpPage.Instance.OnPasswordChanged("password123");

            this.viewModelMock.VerifySet(x => x.Username = "j.doe", Times.Once);
            this.viewModelMock.VerifySet(x => x.Email = "j.doe@example.com", Times.Once);
            this.viewModelMock.VerifySet(x => x.Password = "password123", Times.Once);

            signUpPage.Instance.OnUsernameChanged(null);
            signUpPage.Instance.OnEmailChanged(null);
            signUpPage.Instance.OnPasswordChanged(null);

            this.viewModelMock.VerifySet(x => x.Username = string.Empty, Times.Once);
            this.viewModelMock.VerifySet(x => x.Email = string.Empty, Times.Once);
            this.viewModelMock.VerifySet(x => x.Password = string.Empty, Times.Once);
        }
    }
}
