// ------------------------------------------------------------------------------------------------
// <copyright file="ILoginViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Login
{
    using ErrorOr;

    /// <summary>
    /// Defines the view model contract for the user authentication and sign-in page.
    /// </summary>
    public interface ILoginViewModel
    {
        /// <summary>
        /// Gets or sets the primary email address for the account.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the account password.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a login authentication submission is currently in progress.
        /// </summary>
        bool IsSubmitting { get; set; }

        /// <summary>
        /// Initializes the view model state and populates default sign-in form values.
        /// </summary>
        void InitializeViewModel();

        /// <summary>
        /// Submits the credentials to authenticate the user.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>This is only a placeholder and will be removed once OIDC is configured</remarks>
        /// <returns>
        /// A <see cref="Task{TResult}" /> containing an <see cref="ErrorOr{TValue}" /> indicating success or failure of
        /// the login operation.
        /// </returns>
        Task<ErrorOr<Success>> Login(CancellationToken cancellationToken = default);
    }
}
