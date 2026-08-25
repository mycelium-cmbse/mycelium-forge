// ------------------------------------------------------------------------------------------------
// <copyright file="VerifyEmailTestFixture.cs" company="Starion Group S.A.">
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

    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Components.Pages;
    using Mycelium.Forge.ViewModels.VerifyEmail;

    [TestFixture]
    public class VerifyEmailTestFixture
    {
        private BunitContext context;
        private Mock<IVerifyEmailViewModel> viewModelMock;
        private ToastService toastService;
        private NavigationManager navigationManager;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IVerifyEmailViewModel>();

            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.toastService = this.context.Services.GetRequiredService<ToastService>();
            this.navigationManager = this.context.Services.GetRequiredService<NavigationManager>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyOnInitialized()
        {
            this.navigationManager.NavigateTo("verify-email?email=user@example.com");
            var verifyEmailPage = this.context.Render<VerifyEmail>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(verifyEmailPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel(), Times.Once);
                this.viewModelMock.VerifySet(x => x.Email = "user@example.com", Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnSendEmail()
        {
            var verifyEmailPage = this.context.Render<VerifyEmail>();
            var resendButton = verifyEmailPage.Find("#verify-email-resend-button");

            this.viewModelMock.Setup(x => x.SendEmail()).Returns(Result.Ok());
            await verifyEmailPage.InvokeAsync(() => resendButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.SendEmail(), Times.Once);
                Assert.That(this.toastService.Toasts, Has.Count.EqualTo(1));
            }

            this.viewModelMock.Setup(x => x.SendEmail()).Returns(Result.Fail("Rate limit exceeded"));
            await verifyEmailPage.InvokeAsync(() => resendButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.SendEmail(), Times.Exactly(2));
                Assert.That(this.toastService.Toasts, Has.Count.EqualTo(2));
            }
        }
    }
}
