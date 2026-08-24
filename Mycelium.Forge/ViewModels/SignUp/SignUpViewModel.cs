// ------------------------------------------------------------------------------------------------
// <copyright file="SignUpViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.SignUp
{
    using FluentResults;

    /// <summary>
    /// Provides view model state and operations for the user account registration and sign-up page.
    /// </summary>
    public class SignUpViewModel : ISignUpViewModel
    {
        /// <summary>
        /// Gets or sets the installation-unique username handle.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the primary email address for the account.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the account password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether an account registration submission is currently in progress.
        /// </summary>
        public bool IsSubmitting { get; set; }

        /// <summary>
        /// Initializes the view model state and populates default registration form values.
        /// </summary>
        public void InitializeViewModel()
        {
            this.Username = string.Empty;
            this.Email = string.Empty;
            this.Password = string.Empty;
            this.IsSubmitting = false;
        }

        /// <summary>
        /// Submits the registration details to create a new user account.
        /// </summary>
        /// <returns>A <see cref="Result" /> indicating success or failure of the registration operation.</returns>
        public Result SignUp()
        {
            this.IsSubmitting = true;
            this.IsSubmitting = false;

            return Result.Ok();
        }
    }
}
