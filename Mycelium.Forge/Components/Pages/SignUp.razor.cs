// ------------------------------------------------------------------------------------------------
// <copyright file="SignUp.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.SignUp;

    /// <summary>
    /// Represents the user registration and sign-up page of the Mycelium Forge registry.
    /// </summary>
    public partial class SignUp : ComponentBase
    {
        /// <summary>
        /// Gets or sets the view model for the sign-up page.
        /// </summary>
        [Inject]
        public ISignUpViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the toast notification service.
        /// </summary>
        [Inject]
        public ToastService ToastService { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager instance.
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets the validation manager instance handling field validation states.
        /// </summary>
        public ValidationManager ValidationManager { get; } = new();

        /// <summary>
        /// Handles changes to the username input value.
        /// </summary>
        /// <param name="username">The new username value.</param>
        public void OnUsernameChanged(string username)
        {
            this.ViewModel.Username = username ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.ViewModel.Username));
        }

        /// <summary>
        /// Handles changes to the email input value.
        /// </summary>
        /// <param name="email">The new email address value.</param>
        public void OnEmailChanged(string email)
        {
            this.ViewModel.Email = email ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.ViewModel.Email));
        }

        /// <summary>
        /// Handles changes to the password input value.
        /// </summary>
        /// <param name="password">The new password value.</param>
        public void OnPasswordChanged(string password)
        {
            this.ViewModel.Password = password ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.ViewModel.Password));
        }

        /// <summary>
        /// Executes the account creation workflow.
        /// </summary>
        public void OnCreateAccount()
        {
            var isValid = this.ValidationManager
                .Check(nameof(this.ViewModel.Username), !string.IsNullOrWhiteSpace(this.ViewModel.Username), "Username is required.")
                .Check(nameof(this.ViewModel.Email), !string.IsNullOrWhiteSpace(this.ViewModel.Email), "Email address is required.")
                .Check(nameof(this.ViewModel.Password), !string.IsNullOrWhiteSpace(this.ViewModel.Password), "Password is required.")
                .IsValid;

            if (!isValid)
            {
                return;
            }

            var result = this.ViewModel.SignUp();

            if (result.IsSuccess)
            {
                this.ToastService.Success("Account created successfully. Please check your email for verification.", "Account Created");
                this.NavigationManager.NavigateTo(PageRoutes.Login);
            }
            else
            {
                var errorMessage = result.Reasons.Count > 0 ? result.Reasons[0].Message : "Failed to create account.";
                this.ToastService.Error(errorMessage, "Error");
            }
        }

        /// <summary>
        /// Initiates single sign-on redirect with the organization identity provider.
        /// </summary>
        public void OnContinueWithSso()
        {
            this.ToastService.Info("Redirecting to identity provider...", "Single Sign-On");
        }

        /// <summary>
        /// Initializes the component lifecycle and populates the view model state.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ViewModel.InitializeViewModel();
        }
    }
}
