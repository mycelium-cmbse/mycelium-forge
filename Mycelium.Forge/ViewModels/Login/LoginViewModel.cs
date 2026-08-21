// ------------------------------------------------------------------------------------------------
// <copyright file="LoginViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Login
{
    using FluentResults;

    /// <summary>
    /// Provides view model state and operations for the user authentication and sign-in page.
    /// </summary>
    public class LoginViewModel : ILoginViewModel
    {
        /// <summary>
        /// Gets or sets the primary email address for the account.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the account password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether a login authentication submission is currently in progress.
        /// </summary>
        public bool IsSubmitting { get; set; }

        /// <summary>
        /// Initializes the view model state and populates default sign-in form values.
        /// </summary>
        public void InitializeViewModel()
        {
            this.Email = string.Empty;
            this.Password = string.Empty;
            this.IsSubmitting = false;
        }

        /// <summary>
        /// Submits the credentials to authenticate the user.
        /// </summary>
        /// <returns>A <see cref="Result" /> indicating success or failure of the login operation.</returns>
        public Result Login()
        {
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
