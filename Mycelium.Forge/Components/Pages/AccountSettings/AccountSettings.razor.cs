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

    using ErrorOr;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Common.Extensions;
    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Components.Pages.AccountSettings.Dialogs;
    using Mycelium.Forge.Dal.AutoGenDtoValidator;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Models.Common;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.ViewModels.AccountSettings;

    /// <summary>
    /// Represents the user account settings and profile configuration view of the Mycelium Forge registry.
    /// </summary>
    public partial class AccountSettings : DisposableComponent
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
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// Override this method if you will perform an asynchronous operation and
        /// want the component to refresh when that operation is completed.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            this.Disposables.Add(this.ViewModel.Organizations.Connect().SubscribeAsync(_ => this.InvokeAsync(this.StateHasChanged)));

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

        /// <summary>
        /// Validates that the entered display name includes both a first and last name.
        /// </summary>
        /// <param name="displayName">The trimmed display name entered by the user.</param>
        /// <returns>A <see cref="Task{TResult}" /> resolving to <see cref="Result.Success" /> or a validation error.</returns>
        private static Task<ErrorOr<Success>> ValidateDisplayNameAsync(string displayName)
        {
            var (firstName, lastName) = displayName.SplitFullName();

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                return Task.FromResult<ErrorOr<Success>>(Error.Validation(description: "Please enter both your first and last name (at least two words)."));
            }

            return Task.FromResult<ErrorOr<Success>>(Result.Success);
        }

        /// <summary>
        /// Validates that the entered website URL is either empty or a valid absolute http or https URL.
        /// </summary>
        /// <param name="website">The trimmed website URL entered by the user.</param>
        /// <returns>A <see cref="Task{TResult}" /> resolving to <see cref="Result.Success" /> or a validation error.</returns>
        private static Task<ErrorOr<Success>> ValidateWebsiteAsync(string website)
        {
            if (string.IsNullOrWhiteSpace(website) || AccountValidator.IsValidWebsite(website))
            {
                return Task.FromResult<ErrorOr<Success>>(Result.Success);
            }

            return Task.FromResult<ErrorOr<Success>>(Error.Validation(description: "Please enter a valid absolute http or https URL."));
        }

        /// <summary>
        /// Handles the action to delete the user account.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnDeleteAccount()
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
        /// Determines whether the current user is an administrator of the specified organization.
        /// </summary>
        /// <param name="organization">The organization to evaluate.</param>
        /// <returns><c>true</c> if the user is an administrator; otherwise, <c>false</c>.</returns>
        private bool IsAdmin(IOrganization organization)
        {
            return organization.Administrator.Contains(this.ViewModel.Profile.Id);
        }

        /// <summary>
        /// Gets the role label for the current user in the organization.
        /// </summary>
        /// <param name="organization">The organization to evaluate.</param>
        /// <returns>A string representing the role.</returns>
        private string GetRole(IOrganization organization)
        {
            return this.IsAdmin(organization) ? "Administrator" : "Member";
        }

        /// <summary>
        /// Handles the action to change the user's primary email address.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnChangeEmail()
        {
            var onResult = new EventCallbackFactory().Create(this, async (string email) =>
            {
                var updated = this.ViewModel.Profile.Clone();
                updated.Email = email;
                updated.IsVerified = false;

                await this.ViewModel.UpdateProfile(updated);
            });

            var parameters = new Dictionary<string, object>
            {
                { nameof(ChangeEmailDialog.CurrentEmail), this.ViewModel.Profile.Email },
                { nameof(ChangeEmailDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Change email"
            };

            await this.DialogService.OpenAsync<ChangeEmailDialog>(parameters, options);
        }

        /// <summary>
        /// Handles the action to edit the user's display name.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnEditDisplayName()
        {
            var onResult = new EventCallbackFactory().Create(this, async (string displayName) =>
            {
                var updated = this.ViewModel.Profile.Clone();
                updated.Name = displayName;
                await this.ViewModel.UpdateProfile(updated);
            });

            var validate = ValidateDisplayNameAsync;

            var parameters = new Dictionary<string, object>
            {
                { nameof(InputPromptDialog.Description), "Enter your new display name." },
                { nameof(InputPromptDialog.Placeholder), "Display name" },
                { nameof(InputPromptDialog.InitialValue), this.ViewModel.Profile.Name },
                { nameof(InputPromptDialog.Required), true },
                { nameof(InputPromptDialog.ValidateAsync), validate },
                { nameof(InputPromptDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Edit display name"
            };

            await this.DialogService.OpenAsync<InputPromptDialog>(parameters, options);
        }

        /// <summary>
        /// Handles the action to edit the user's website URL.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnEditWebsite()
        {
            var onResult = new EventCallbackFactory().Create(this, async (string website) =>
            {
                var updated = this.ViewModel.Profile.Clone();
                updated.Website = string.IsNullOrWhiteSpace(website) ? null : website;
                await this.ViewModel.UpdateProfile(updated);
            });

            var validate = ValidateWebsiteAsync;

            var parameters = new Dictionary<string, object>
            {
                { nameof(InputPromptDialog.Description), "Enter your website URL." },
                { nameof(InputPromptDialog.Placeholder), "https://example.com" },
                { nameof(InputPromptDialog.InitialValue), this.ViewModel.Profile.Website ?? string.Empty },
                { nameof(InputPromptDialog.ValidateAsync), validate },
                { nameof(InputPromptDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Edit website"
            };

            await this.DialogService.OpenAsync<InputPromptDialog>(parameters, options);
        }

        /// <summary>
        /// Handles the action to create or transfer organization memberships.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnCreateOrganization()
        {
            var onResult = new EventCallbackFactory().Create(this, async (CreateOrganizationResult result) => await this.ViewModel.CreateOrganization(result));

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
        /// Handles the action to deactivate the user account.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnDeactivateAccount()
        {
            var dialogOptions = new ConfirmDialogOptions
            {
                Destructive = true
            };

            var dialogResult = await this.DialogService.ConfirmAsync("Deactivate account", "Are you sure you want to deactivate your account? This action can be reversed at any time.", dialogOptions);

            if (dialogResult.Confirmed)
            {
                await this.ViewModel.DeactivateAccount();
            }
        }
    }
}
