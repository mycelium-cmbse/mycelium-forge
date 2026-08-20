// ------------------------------------------------------------------------------------------------
// <copyright file="VerifyEmailViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using FluentResults;

    /// <summary>
    /// Provides view model state and operations for the user email verification page.
    /// </summary>
    public class VerifyEmailViewModel : IVerifyEmailViewModel
    {
        /// <summary>
        /// Gets or sets the email address to which the verification message was sent.
        /// </summary>
        public string Email { get; set; } = "regis.andre@starion.eu";

        /// <summary>
        /// Gets or sets a value indicating whether an email sending operation is currently in progress.
        /// </summary>
        public bool IsSending { get; set; }

        /// <summary>
        /// Initializes the view model state and populates default values.
        /// </summary>
        public void InitializeViewModel()
        {
            this.IsSending = false;
        }

        /// <summary>
        /// Sends the verification email to the configured email address.
        /// </summary>
        /// <returns>A <see cref="Result" /> indicating the success or failure of the send operation.</returns>
        public Result SendEmail()
        {
            if (string.IsNullOrWhiteSpace(this.Email))
            {
                return Result.Fail("Email address is required.");
            }

            this.IsSending = true;
            this.IsSending = false;

            return Result.Ok();
        }
    }
}
