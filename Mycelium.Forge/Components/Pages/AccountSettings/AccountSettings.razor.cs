// ------------------------------------------------------------------------------------------------
// <copyright file="AccountSettings.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.AccountSettings
{
    using BlazorBlueprint.Components;

    using FluentResults;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Components.Pages.AccountSettings.Dialogs;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.ViewModels.AccountSettings;

    /// <summary>
    /// Represents the user account settings and profile configuration view of the Mycelium Forge registry.
    /// </summary>
    public partial class AccountSettings : ComponentBase
    {
        /// <summary>
        /// Gets or sets the dialog service used to display modal dialogs.
        /// </summary>
        [Inject]
        public DialogService DialogService { get; set; }

        /// <summary>
        /// Gets or sets the view model for the account settings page.
        /// </summary>
        [Inject]
        public IAccountSettingsViewModel ViewModel { get; set; }

        /// <summary>
        /// Handles the action to change the user's username.
        /// </summary>
        public void OnChangeUsername()
        {
        }

        /// <summary>
        /// Handles the action to change the user's primary email address.
        /// </summary>
        public void OnChangeEmail()
        {
        }

        /// <summary>
        /// Handles the action to edit the user's display name.
        /// </summary>
        public void OnEditDisplayName()
        {
        }

        /// <summary>
        /// Handles the action to edit the user's company affiliation.
        /// </summary>
        public void OnEditCompany()
        {
        }

        /// <summary>
        /// Handles the action to edit the user's location.
        /// </summary>
        public void OnEditLocation()
        {
        }

        /// <summary>
        /// Handles the action to edit the user's website URL.
        /// </summary>
        public void OnEditWebsite()
        {
        }

        /// <summary>
        /// Handles the action to edit the user's biography.
        /// </summary>
        public void OnEditBiography()
        {
        }

        /// <summary>
        /// Handles the action to create or transfer organization memberships.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnCreateOrganization()
        {
            var onResult = new EventCallbackFactory().Create(this, (CreateOrganizationResult result) => this.HandleCreateOrganization(result));

            var parameters = new Dictionary<string, object>
            {
                { nameof(CreateOrganizationDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Create an organization",
                Description = "An Organization owns a package scope and its members. You become its Organization Administrator."
            };

            await this.DialogService.OpenAsync<CreateOrganizationDialog>(parameters, options);
        }

        /// <summary>
        /// Handles the result when an organization is created.
        /// </summary>
        /// <param name="result">The create organization result details.</param>
        /// <returns>A <see cref="Result" /> indicating the outcome of the create organization operation.</returns>
        public Result HandleCreateOrganization(CreateOrganizationResult result)
        {
            return this.ViewModel.CreateOrganization(result);
        }

        /// <summary>
        /// Handles the action to deactivate the user account.
        /// </summary>
        public void OnDeactivateAccount()
        {
            this.ViewModel.DeactivateAccount();
        }

        /// <summary>
        /// Handles the action to delete the user account.
        /// </summary>
        public void OnDeleteAccount()
        {
            this.ViewModel.DeleteAccount();
        }

        /// <summary>
        /// Initializes the component and view model state.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.ViewModel.InitializeViewModel();
        }
    }
}
