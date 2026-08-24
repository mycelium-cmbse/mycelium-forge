// ------------------------------------------------------------------------------------------------
// <copyright file="VerifyEmailViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.VerifyEmail
{
    using Mycelium.Forge.ViewModels.VerifyEmail;

    [TestFixture]
    public class VerifyEmailViewModelTestFixture
    {
        private VerifyEmailViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new VerifyEmailViewModel();
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.IsSending = true;
            this.viewModel.InitializeViewModel();

            Assert.That(this.viewModel.IsSending, Is.False);
        }

        [Test]
        public void VerifySendEmail()
        {
            this.viewModel.Email = "regis.andre@starion.eu";
            var successResult = this.viewModel.SendEmail();

            this.viewModel.Email = string.Empty;
            var failureResult = this.viewModel.SendEmail();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(successResult.IsSuccess, Is.True);
                Assert.That(failureResult.IsFailed, Is.True);
                Assert.That(this.viewModel.IsSending, Is.False);
            }
        }
    }
}
