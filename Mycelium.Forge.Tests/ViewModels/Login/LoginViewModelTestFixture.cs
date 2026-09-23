// ------------------------------------------------------------------------------------------------
// <copyright file="LoginViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.Login
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Mycelium.Forge.Data;
    using Mycelium.Forge.Model;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.Login;

    /// <summary>
    /// Test fixture for <see cref="LoginViewModel" />.
    /// </summary>
    [TestFixture]
    public class LoginViewModelTestFixture
    {
        private Mock<IUserService> userServiceMock;

        private Mock<INotificationService> notificationServiceMock;

        private Mock<ILogger<LoginViewModel>> loggerMock;

        private LoginViewModel viewModel;

        /// <summary>
        /// Sets up mock dependencies and creates the view model before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.userServiceMock = new Mock<IUserService>();
            this.notificationServiceMock = new Mock<INotificationService>();
            this.loggerMock = new Mock<ILogger<LoginViewModel>>();

            this.viewModel = new LoginViewModel(
                this.userServiceMock.Object,
                this.notificationServiceMock.Object,
                this.loggerMock.Object);
        }

        /// <summary>
        /// Tears down the view model after each test execution.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.viewModel.Dispose();
        }

        /// <summary>
        /// Verifies that <see cref="LoginViewModel.InitializeViewModel" /> resets form properties.
        /// </summary>
        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.Email = "custom@example.com";
            this.viewModel.Password = "password";
            this.viewModel.IsSubmitting = true;

            this.viewModel.InitializeViewModel();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Email, Is.Empty);
                Assert.That(this.viewModel.Password, Is.Empty);
                Assert.That(this.viewModel.IsSubmitting, Is.False);
            }
        }

        /// <summary>
        /// Verifies login execution for success and exception failure scenarios.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        [Test]
        public async Task VerifyLogin()
        {
            var successResult = await this.viewModel.Login(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(successResult.IsError, Is.False);
                Assert.That(this.viewModel.IsSubmitting, Is.False);
                this.userServiceMock.Verify(service => service.SetCurrentUser(SeedData.RegisAccount.Id, It.IsAny<CancellationToken>()), Times.Once);
            }

            // Exception case
            this.userServiceMock
                .Setup(service => service.SetCurrentUser(It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Connection broken"));

            var exceptionResult = await this.viewModel.Login(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(exceptionResult.IsError, Is.True);
                Assert.That(this.viewModel.IsSubmitting, Is.False);
                this.notificationServiceMock.Verify(service => service.AddNotification("An unexpected error occurred during authentication.", "Error", NotificationType.Error), Times.Once);
            }
        }
    }
}
