// ------------------------------------------------------------------------------------------------
// <copyright file="ISignUpViewModel.cs" company="Starion Group S.A.">
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
    /// Defines operations and state management for the user account registration and sign-up page.
    /// </summary>
    public interface ISignUpViewModel
    {
        /// <summary>
        /// Gets or sets the installation-unique username handle.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Gets or sets the primary email address for the account.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the account password.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an account registration submission is currently in progress.
        /// </summary>
        bool IsSubmitting { get; set; }

        /// <summary>
        /// Initializes the view model state and populates default registration form values.
        /// </summary>
        void InitializeViewModel();

        /// <summary>
        /// Submits the registration details to create a new user account.
        /// </summary>
        /// <returns>A <see cref="Result" /> indicating success or failure of the registration operation.</returns>
        Result SignUp();
    }
}
