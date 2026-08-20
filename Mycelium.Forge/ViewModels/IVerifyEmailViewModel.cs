// ------------------------------------------------------------------------------------------------
// <copyright file="IVerifyEmailViewModel.cs" company="Starion Group S.A.">
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
    /// Defines operations and state management for the user email verification page.
    /// </summary>
    public interface IVerifyEmailViewModel
    {
        /// <summary>
        /// Gets or sets the email address to which the verification message was sent.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an email sending operation is currently in progress.
        /// </summary>
        bool IsSending { get; set; }

        /// <summary>
        /// Initializes the view model state and populates default values.
        /// </summary>
        void InitializeViewModel();

        /// <summary>
        /// Sends the verification email to the configured email address.
        /// </summary>
        /// <returns>A <see cref="Result" /> indicating the success or failure of the send operation.</returns>
        Result SendEmail();
    }
}
