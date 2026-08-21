// ------------------------------------------------------------------------------------------------
// <copyright file="Login.razor.cs" company="Starion Group S.A.">
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
    using Mycelium.Forge.ViewModels;
    using Mycelium.Forge.ViewModels.Login;

    /// <summary>
    /// Represents the user authentication and sign-in page of the Mycelium Forge registry.
    /// </summary>
    public partial class Login : ComponentBase
    {
        /// <summary>
        /// Gets or sets the view model for the sign-in page.
        /// </summary>
        [Inject]
        public ILoginViewModel ViewModel { get; set; }

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
        public ValidationManager ValidationManager { get; } = new ValidationManager();

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
        /// Executes the login authentication workflow.
        /// </summary>
        public void OnLogin()
        {
            var isValid = this.ValidationManager
                .Check(nameof(this.ViewModel.Email), !string.IsNullOrWhiteSpace(this.ViewModel.Email), "Email address is required.")
                .Check(nameof(this.ViewModel.Password), !string.IsNullOrWhiteSpace(this.ViewModel.Password), "Password is required.")
                .IsValid;

            if (!isValid)
            {
                return;
            }

            var result = this.ViewModel.Login();

            if (result.IsSuccess)
            {
                this.ToastService.Success("Signed in successfully.", "Welcome");
                this.NavigationManager.NavigateTo(PageRoutes.Home);
            }
            else
            {
                var errorMessage = result.Reasons.Count > 0 ? result.Reasons[0].Message : "Failed to sign in.";
                this.ToastService.Error(errorMessage, "Error");
            }
        }

        /// <summary>
        /// Initiates single sign-on redirect with the organization identity provider.
        /// </summary>
        public void OnContinueWithSso()
        {
            var result = this.ViewModel.ContinueWithSso();

            if (result.IsSuccess)
            {
                this.ToastService.Info("Redirecting to identity provider...", "Single Sign-On");
            }
            else
            {
                var errorMessage = result.Reasons.Count > 0 ? result.Reasons[0].Message : "Failed to initiate SSO.";
                this.ToastService.Error(errorMessage, "Error");
            }
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
