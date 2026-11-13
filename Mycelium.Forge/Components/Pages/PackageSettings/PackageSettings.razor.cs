// ------------------------------------------------------------------------------------------------
// <copyright file="PackageSettings.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageSettings
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageSettings.Dialogs;
    using Mycelium.Forge.Models.Common;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.ViewModels.PackageSettings;

    /// <summary>
    /// Represents the package settings and governance management view of the Mycelium Forge registry.
    /// </summary>
    public partial class PackageSettings : ComponentBase
    {
        /// <summary>
        /// Gets or sets the scope segment supplied from the URL route.
        /// </summary>
        [Parameter]
        public string Scope { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the package short name supplied from the URL route.
        /// </summary>
        [Parameter]
        public string PackageShortname { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the view model for the package settings page.
        /// </summary>
        [Inject]
        public IPackageSettingsViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the dialog service used to display modal dialogs.
        /// </summary>
        [Inject]
        public DialogService DialogService { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager instance.
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Handles component parameter updates and initializes the view model with the route parameters asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (string.IsNullOrWhiteSpace(this.PackageShortname) || string.IsNullOrWhiteSpace(this.Scope))
            {
                return;
            }

            await this.ViewModel.InitializeViewModel(this.PackageShortname, this.Scope);
        }

        /// <summary>
        /// Determines whether transferring package ownership is enabled and visible for the current user.
        /// </summary>
        /// <returns><c>true</c> if transferring package ownership is enabled; otherwise, <c>false</c>.</returns>
        private bool IsTransferOwnershipEnabled()
        {
            if (PermissionGuard.HasRole(this.ViewModel.UserContext, RoleKind.InstallationAdministrator))
            {
                return true;
            }

            var accountId = this.ViewModel.UserContext.AccountId;

            return accountId.HasValue && this.ViewModel.Package.PackageOwner.Contains(accountId.Value);
        }

        /// <summary>
        /// Gets the breadcrumb navigation items for the package settings page.
        /// </summary>
        /// <returns>A collection of <see cref="BreadcrumbItem" /> entries representing the trail.</returns>
        private IEnumerable<BreadcrumbItem> GetBreadcrumbItems()
        {
            var publisher = this.ViewModel.Owner.ShortName;
            var packageShortname = this.ViewModel.Package.ShortName;

            return
            [
                new BreadcrumbItem
                {
                    Name = "Search",
                    Link = PageRoutes.Packages
                },
                new BreadcrumbItem
                {
                    Name = $"@{publisher}",
                    Link = PageRoutes.GetOrganizationRoute(publisher)
                },
                new BreadcrumbItem
                {
                    Name = packageShortname,
                    Link = PageRoutes.GetPackageRoute(publisher, packageShortname)
                },
                new BreadcrumbItem
                {
                    Name = "Settings"
                }
            ];
        }

        /// <summary>
        /// Handles opening the modal dialog to invite a new maintainer or owner to the package asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnAddMaintainer()
        {
            var onResult = new EventCallbackFactory().Create<InvitePackageCollaboratorResult>(this, async result =>
            {
                await this.ViewModel.InviteCollaborator(result.TargetAccount, result.Role);
                await this.InvokeAsync(this.StateHasChanged);
            });

            var parameters = new Dictionary<string, object>
            {
                { nameof(AddMaintainerDialog.Package), this.ViewModel.Package },
                { nameof(AddMaintainerDialog.PendingInvitations), this.ViewModel.PendingInvitations },
                { nameof(AddMaintainerDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Add maintainer"
            };

            await this.DialogService.OpenAsync<AddMaintainerDialog>(parameters, options);
        }

        /// <summary>
        /// Handles opening the modal dialog to change the role of the specified collaborator asynchronously.
        /// </summary>
        /// <param name="collaborator">The target collaborator account.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnChangeCollaboratorRole(IAccount collaborator)
        {
            var currentRole = this.GetCollaboratorRole(collaborator);

            var onResult = new EventCallbackFactory().Create<PackageInvitationKind>(this, async newRole =>
            {
                if (newRole != currentRole)
                {
                    await this.ViewModel.ChangeCollaboratorRole(collaborator, newRole);
                    await this.InvokeAsync(this.StateHasChanged);
                }
            });

            var parameters = new Dictionary<string, object>
            {
                { nameof(ChangeCollaboratorRoleDialog.Collaborator), collaborator },
                { nameof(ChangeCollaboratorRoleDialog.CurrentRole), currentRole },
                { nameof(ChangeCollaboratorRoleDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Change role"
            };

            await this.DialogService.OpenAsync<ChangeCollaboratorRoleDialog>(parameters, options);
        }

        /// <summary>
        /// Navigates to the profile page of the specified collaborator account.
        /// </summary>
        /// <param name="collaborator">The target collaborator account.</param>
        private void OnViewCollaboratorProfile(IAccount collaborator)
        {
            this.NavigationManager.NavigateTo(PageRoutes.GetAccountRoute(collaborator.ShortName));
        }

        /// <summary>
        /// Prompts confirmation to remove the specified collaborator from the package asynchronously.
        /// </summary>
        /// <param name="collaborator">The target collaborator account.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnRemoveCollaborator(IAccount collaborator)
        {
            var dialogOptions = new ConfirmDialogOptions
            {
                Destructive = true
            };

            var dialogResult = await this.DialogService.ConfirmAsync("Collaborator Removal", "Are you sure you want to remove this collaborator from the package?", dialogOptions);

            if (dialogResult.Confirmed)
            {
                await this.ViewModel.RemoveCollaborator(collaborator);
            }
        }

        /// <summary>
        /// Handles resending a pending package invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The invitation to resend.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnResendInvitation(PackageInvitation invitation)
        {
            await this.ViewModel.ResendInvitation(invitation);
        }

        /// <summary>
        /// Handles revoking a pending package invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The invitation to revoke.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnRevokeInvitation(PackageInvitation invitation)
        {
            await this.ViewModel.RevokeInvitation(invitation);
        }

        /// <summary>
        /// Resolves the role of the specified collaborator within the package.
        /// </summary>
        /// <param name="collaborator">The collaborator account.</param>
        /// <returns>The <see cref="PackageInvitationKind" /> representing the collaborator's role.</returns>
        private PackageInvitationKind GetCollaboratorRole(IAccount collaborator)
        {
            return this.ViewModel.Package.PackageOwner.Contains(collaborator.Id)
                ? PackageInvitationKind.OWNER
                : PackageInvitationKind.MAINTAINER;
        }

        /// <summary>
        /// Determines whether role change or removal actions are disabled for the specified collaborator.
        /// </summary>
        /// <param name="collaborator">The collaborator account.</param>
        /// <returns><c>true</c> if the actions are disabled; otherwise, <c>false</c>.</returns>
        private bool IsCollaboratorActionDisabled(IAccount collaborator)
        {
            return !this.ViewModel.CanManagePackageTeam || collaborator.Id == this.ViewModel.CurrentUserId || this.IsSolePackageOwner(collaborator);
        }

        /// <summary>
        /// Determines whether the specified collaborator is the sole owner of the package.
        /// </summary>
        /// <param name="collaborator">The collaborator account.</param>
        /// <returns><c>true</c> if the collaborator is the sole package owner; otherwise, <c>false</c>.</returns>
        private bool IsSolePackageOwner(IAccount collaborator)
        {
            return this.ViewModel.Package.PackageOwner.Count == 1 && this.ViewModel.Package.PackageOwner.Contains(collaborator.Id);
        }

        /// <summary>
        /// Gets the display short name of the target account associated with a given package invitation.
        /// </summary>
        /// <param name="invitation">The package invitation.</param>
        /// <returns>The short name of the target account.</returns>
        private string GetInvitationTargetName(IPackageInvitation invitation)
        {
            var target = this.ViewModel.InvitedAccounts.FirstOrDefault(a => a.Id == invitation.Target);
            return target?.ShortName ?? "Unknown";
        }

        /// <summary>
        /// Selects the specified visibility option for the package and saves the update asynchronously.
        /// </summary>
        /// <param name="visibility">The visibility kind to set.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnSelectVisibility(VisibilityKind visibility)
        {
            if (this.ViewModel.Package.Visibility != visibility)
            {
                await this.ViewModel.SetVisibility(visibility);
            }
        }

        /// <summary>
        /// Navigates to the publish page to publish a new version of the current package.
        /// </summary>
        private void OnPublishNewVersion()
        {
            this.NavigationManager.NavigateTo(PageRoutes.GetPublishVersionRoute(this.Scope, this.PackageShortname));
        }

        /// <summary>
        /// Prompts confirmation to unlist the specified package version.
        /// </summary>
        /// <param name="version">The package version to unlist.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnUnlistVersion(PackageVersion version)
        {
            var onResult = new EventCallbackFactory().Create(this, async () => { await this.ViewModel.UnlistVersion(version); });

            var parameters = new Dictionary<string, object>
            {
                { nameof(ConfirmPackageActionDialog.PackageName), this.ViewModel.Package.Name },
                { nameof(ConfirmPackageActionDialog.Title), $"Unlist version {version.Version}" },
                { nameof(ConfirmPackageActionDialog.Description), $"Unlisting version {version.Version} hides it from search and discovery while keeping existing installations working." },
                { nameof(ConfirmPackageActionDialog.ActionButtonText), "Unlist version" },
                { nameof(ConfirmPackageActionDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = $"Unlist version {version.Version}"
            };

            await this.DialogService.OpenAsync<ConfirmPackageActionDialog>(parameters, options);
        }

        /// <summary>
        /// Relists the specified unlisted package version asynchronously.
        /// </summary>
        /// <param name="version">The package version to relist.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnRelistVersion(PackageVersion version)
        {
            await this.ViewModel.RelistVersion(version);
        }

        /// <summary>
        /// Deprecates the specified package version asynchronously.
        /// </summary>
        /// <param name="version">The package version to deprecate.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnDeprecateVersion(PackageVersion version)
        {
            await this.ViewModel.DeprecateVersion(version);
        }

        /// <summary>
        /// Opens the confirmation dialog to transfer package ownership.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnTransferOwnership()
        {
            var onResult = new EventCallbackFactory().Create<IScope>(this, async targetScope =>
            {
                var result = await this.ViewModel.TransferOwnership(targetScope);

                if (!result.IsError)
                {
                    this.NavigationManager.NavigateTo(PageRoutes.GetPackageRoute(targetScope.ShortName, this.ViewModel.Package.ShortName), replace: true);
                }
            });

            var parameters = new Dictionary<string, object>
            {
                { nameof(TransferPackageOwnershipDialog.Package), this.ViewModel.Package },
                { nameof(TransferPackageOwnershipDialog.CurrentOwner), this.ViewModel.Owner.ShortName },
                { nameof(TransferPackageOwnershipDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Transfer package ownership"
            };

            await this.DialogService.OpenAsync<TransferPackageOwnershipDialog>(parameters, options);
        }

        /// <summary>
        /// Opens the confirmation dialog to permanently delete the package.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnDeletePackage()
        {
            var onResult = new EventCallbackFactory().Create(this, async () =>
            {
                var result = await this.ViewModel.DeletePackage();

                if (!result.IsError)
                {
                    this.NavigationManager.NavigateTo(PageRoutes.Packages, replace: true);
                }
            });

            var parameters = new Dictionary<string, object>
            {
                { nameof(ConfirmPackageActionDialog.PackageName), this.ViewModel.Package.Name },
                { nameof(ConfirmPackageActionDialog.Title), "Delete package" },
                { nameof(ConfirmPackageActionDialog.Description), "This will permanently delete the package and all its versions. Existing consumers will break." },
                { nameof(ConfirmPackageActionDialog.ActionButtonText), "Delete package" },
                { nameof(ConfirmPackageActionDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Delete package"
            };

            await this.DialogService.OpenAsync<ConfirmPackageActionDialog>(parameters, options);
        }

        /// <summary>
        /// Determines whether the specified version is the latest published release.
        /// </summary>
        /// <param name="version">The package version to evaluate.</param>
        /// <returns><c>true</c> if the version is the newest release; otherwise, <c>false</c>.</returns>
        private bool IsLatestVersion(IPackageVersion version)
        {
            return this.ViewModel.Versions.Count > 0 && this.ViewModel.Versions[0].Id == version.Id;
        }
    }
}
