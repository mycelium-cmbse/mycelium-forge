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

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.AccountSettings.Dialogs;
    using Mycelium.Forge.Models.Common;
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
        /// Determines whether the current user is an administrator of the specified organization.
        /// </summary>
        /// <param name="organization">The organization to evaluate.</param>
        /// <returns><c>true</c> if the user is an administrator; otherwise, <c>false</c>.</returns>
        public bool IsAdmin(IOrganization organization)
        {
            return organization.Administrator.Contains(this.ViewModel.Profile.Id);
        }

        /// <summary>
        /// Gets the role label for the current user in the organization.
        /// </summary>
        /// <param name="organization">The organization to evaluate.</param>
        /// <returns>A string representing the role.</returns>
        public string GetRole(IOrganization organization)
        {
            return this.IsAdmin(organization) ? "Administrator" : "Member";
        }

        /// <summary>
        /// Handles the action to change the user's username.
        /// </summary>
        public void OnChangeUsername()
        {
            // Implementation pending future backend support.
        }

        /// <summary>
        /// Handles the action to change the user's primary email address.
        /// </summary>
        public void OnChangeEmail()
        {
            // Implementation pending future backend support.
        }

        /// <summary>
        /// Handles the action to edit the user's display name.
        /// </summary>
        public void OnEditDisplayName()
        {
            // Implementation pending future backend support.
        }

        /// <summary>
        /// Handles the action to edit the user's location.
        /// </summary>
        public void OnEditLocation()
        {
            // Implementation pending future backend support.
        }

        /// <summary>
        /// Handles the action to edit the user's website URL.
        /// </summary>
        public void OnEditWebsite()
        {
            // Implementation pending future backend support.
        }

        /// <summary>
        /// Handles the action to create or transfer organization memberships.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnCreateOrganization()
        {
            var onResult = new EventCallbackFactory().Create(this, async (CreateOrganizationResult result) => await this.HandleCreateOrganization(result));

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
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task HandleCreateOrganization(CreateOrganizationResult result)
        {
            await this.ViewModel.CreateOrganization(result);
        }

        /// <summary>
        /// Handles the action to deactivate the user account.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnDeactivateAccount()
        {
            await this.ViewModel.DeactivateAccount();
        }

        /// <summary>
        /// Handles the action to delete the user account.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnDeleteAccount()
        {
            var options = new ConfirmDialogOptions
            {
                Destructive = true,
                ConfirmText = "Delete",
                CancelText = "Cancel"
            };

            var result = await this.DialogService.ConfirmAsync("Delete account", "Are you sure you want to delete your account? This action cannot be undone.", options);

            if (result.Confirmed)
            {
                await this.ViewModel.DeleteAccount();
            }
        }

        /// <summary>
        /// Initializes the component and view model state asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await this.ViewModel.InitializeViewModel();
        }

        /// <summary>
        /// Gets the breadcrumb navigation items for the account settings page.
        /// </summary>
        /// <returns>A collection of <see cref="BreadcrumbItem" /> entries representing the trail.</returns>
        private static IEnumerable<BreadcrumbItem> GetBreadcrumbItems()
        {
            return
            [
                new BreadcrumbItem
                {
                    Name = "Account"
                },
                new BreadcrumbItem
                {
                    Name = "Settings"
                }
            ];
        }
    }
}
