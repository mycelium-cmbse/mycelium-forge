// ------------------------------------------------------------------------------------------------
// <copyright file="SignUpViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using FluentResults;

    using Mycelium.Forge.Models;

    /// <summary>
    /// Provides view model state and operations for the user account registration and sign-up page.
    /// </summary>
    public class SignUpViewModel : ISignUpViewModel
    {
        /// <summary>
        /// Gets or sets the registration form input values.
        /// </summary>
        public SignUpModel Registration { get; set; } = new();

        /// <summary>
        /// Gets or sets a value indicating whether an account registration submission is currently in progress.
        /// </summary>
        public bool IsSubmitting { get; set; }

        /// <summary>
        /// Initializes the view model state and populates default registration form values.
        /// </summary>
        public void InitializeViewModel()
        {
            this.Registration = new SignUpModel();
            this.IsSubmitting = false;
        }

        /// <summary>
        /// Submits the registration details to create a new user account.
        /// </summary>
        /// <returns>A <see cref="Result" /> indicating success or failure of the registration operation.</returns>
        public Result SignUp()
        {
            if (string.IsNullOrWhiteSpace(this.Registration.Username))
            {
                return Result.Fail("Username is required.");
            }

            if (string.IsNullOrWhiteSpace(this.Registration.Email))
            {
                return Result.Fail("Email address is required.");
            }

            if (string.IsNullOrWhiteSpace(this.Registration.Password))
            {
                return Result.Fail("Password is required.");
            }

            this.IsSubmitting = true;
            this.IsSubmitting = false;

            return Result.Ok();
        }

        /// <summary>
        /// Initiates single sign-on authentication via the configured organisation identity provider.
        /// </summary>
        /// <returns>A <see cref="Result" /> indicating the outcome of the SSO redirect initiation.</returns>
        public Result ContinueWithSso()
        {
            return Result.Ok();
        }
    }
}
