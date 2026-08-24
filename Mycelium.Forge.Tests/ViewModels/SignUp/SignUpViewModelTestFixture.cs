// ------------------------------------------------------------------------------------------------
// <copyright file="SignUpViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.SignUp
{
    using Mycelium.Forge.ViewModels.SignUp;

    [TestFixture]
    public class SignUpViewModelTestFixture
    {
        private SignUpViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new SignUpViewModel();
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.Username = "user";
            this.viewModel.Email = "custom@example.com";
            this.viewModel.Password = "password";
            this.viewModel.IsSubmitting = true;

            this.viewModel.InitializeViewModel();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Username, Is.Empty);
                Assert.That(this.viewModel.Email, Is.Empty);
                Assert.That(this.viewModel.Password, Is.Empty);
                Assert.That(this.viewModel.IsSubmitting, Is.False);
            }
        }

        [Test]
        public void VerifySignUp()
        {
            var result = this.viewModel.SignUp();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(this.viewModel.IsSubmitting, Is.False);
            }
        }
    }
}
