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
    using ErrorOr;

    using Mycelium.Forge.Data;
    using Mycelium.Forge.Model;
    using Mycelium.Forge.Services;

    using ReactiveUI;

    /// <summary>
    /// Provides view model state and operations for the user authentication and sign-in page.
    /// </summary>
    public class LoginViewModel : DisposableViewModel, ILoginViewModel
    {
        /// <summary>
        /// The (injected) <see cref="ILogger{LoginViewModel}" /> used to log diagnostic and error messages.
        /// </summary>
        private readonly ILogger<LoginViewModel> logger;

        /// <summary>
        /// The (injected) <see cref="INotificationService" /> used to dispatch notifications.
        /// </summary>
        private readonly INotificationService notificationService;

        /// <summary>
        /// The (injected) <see cref="IUserService" /> used to manage the active session user identity.
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel" /> class.
        /// </summary>
        /// <param name="userService">The (injected) <see cref="IUserService" />.</param>
        /// <param name="notificationService">The (injected) <see cref="INotificationService" />.</param>
        /// <param name="logger">The (injected) <see cref="ILogger{LoginViewModel}" />.</param>
        public LoginViewModel(IUserService userService, INotificationService notificationService, ILogger<LoginViewModel> logger)
        {
            this.userService = userService;
            this.notificationService = notificationService;
            this.logger = logger;
        }

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
        public bool IsSubmitting
        {
            get;
            set => this.RaiseAndSetIfChanged(ref field, value);
        }

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
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>This is only a placeholder and will be removed once OIDC is configured</remarks>
        /// <returns>
        /// A <see cref="Task{TResult}" /> containing an <see cref="ErrorOr{TValue}" /> indicating success or failure of
        /// the login operation.
        /// </returns>
        public async Task<ErrorOr<Success>> Login(CancellationToken cancellationToken = default)
        {
            this.IsSubmitting = true;

            try
            {
                await this.userService.SetCurrentUser(SeedData.RegisAccount.Id, cancellationToken);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "An error occurred while authenticating user.");
                this.notificationService.AddNotification("An unexpected error occurred during authentication.", "Error", NotificationType.Error);
                return Error.Unexpected(description: "An unexpected error occurred during authentication.");
            }
            finally
            {
                this.IsSubmitting = false;
            }

            return Result.Success;
        }
    }
}
