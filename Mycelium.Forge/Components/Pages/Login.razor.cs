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
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Model;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.Login;

    using ReactiveUI;

    /// <summary>
    /// Represents the user authentication and sign-in page of the Mycelium Forge registry.
    /// </summary>
    public partial class Login : DisposableComponent
    {
        /// <summary>
        /// Gets or sets the view model for the sign-in page.
        /// </summary>
        [Inject]
        public ILoginViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the notification service.
        /// </summary>
        [Inject]
        public INotificationService NotificationService { get; set; }

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
        /// Gets or sets the return URL parameter to navigate to after successful sign in.
        /// </summary>
        [SupplyParameterFromQuery(Name = UrlParameterNames.ReturnUrl)]
        public string ReturnUrl { get; set; }

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
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnLogin()
        {
            var isValid = this.ValidationManager
                .Check(nameof(this.ViewModel.Email), !string.IsNullOrWhiteSpace(this.ViewModel.Email), "Email address is required.")
                .Check(nameof(this.ViewModel.Password), !string.IsNullOrWhiteSpace(this.ViewModel.Password), "Password is required.")
                .IsValid;

            if (!isValid)
            {
                return;
            }

            var result = await this.ViewModel.Login();

            if (!result.IsError)
            {
                this.NavigationManager.NavigateTo(PageRoutes.AuthLogin, forceLoad: true);
            }
        }

        /// <summary>
        /// Initiates single sign-on redirect with the organization identity provider.
        /// </summary>
        public void OnContinueWithSso()
        {
            this.NotificationService.AddNotification("Redirecting to identity provider...", "Single Sign-On", NotificationType.Info);
        }

        /// <summary>
        /// Initializes the component lifecycle, populates the view model state, and subscribes to property changes.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.Disposables.Add(this.ViewModel.WhenAnyValue(x => x.IsSubmitting).Subscribe(_ => this.InvokeAsync(this.StateHasChanged)));
            this.ViewModel.InitializeViewModel();
        }
    }
}
