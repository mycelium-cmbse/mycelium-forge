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
    using Mycelium.Forge.ViewModels.Login;

    [TestFixture]
    public class LoginViewModelTestFixture
    {
        private LoginViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new LoginViewModel();
        }

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

        [Test]
        public void VerifyLogin()
        {
            var result = this.viewModel.Login();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(this.viewModel.IsSubmitting, Is.False);
            }
        }
    }
}
