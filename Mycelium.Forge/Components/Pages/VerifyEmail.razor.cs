// ------------------------------------------------------------------------------------------------
// <copyright file="VerifyEmail.razor.cs" company="Starion Group S.A.">
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
    using Mycelium.Forge.ViewModels;
    using Mycelium.Forge.ViewModels.VerifyEmail;

    /// <summary>
    /// Represents the user email verification page of the Mycelium Forge registry.
    /// </summary>
    public partial class VerifyEmail : ComponentBase
    {
        /// <summary>
        /// Gets or sets the view model for the email verification page.
        /// </summary>
        [Inject]
        public IVerifyEmailViewModel ViewModel { get; set; }

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
        /// Gets or sets the target email address supplied via URL query parameter.
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery(Name = UrlParameterNames.Email)]
        public string Email { get; set; }

        /// <summary>
        /// Executes the send verification email workflow.
        /// </summary>
        public void OnSendEmail()
        {
            var result = this.ViewModel.SendEmail();

            if (result.IsSuccess)
            {
                this.ToastService.Success("Verification link sent. Please check your inbox.", "Email Sent");
            }
            else
            {
                var errorMessage = result.Reasons.Count > 0 ? result.Reasons[0].Message : "Failed to send verification email.";
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

            if (!string.IsNullOrWhiteSpace(this.Email))
            {
                this.ViewModel.Email = this.Email;
            }
        }
    }
}
