// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationSettings.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.OrganizationSettings
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Common;
    using Mycelium.Forge.ViewModels.OrganizationSettings;

    /// <summary>
    /// Represents the organization settings, membership administration, and scope management view of the Mycelium Forge
    /// registry.
    /// </summary>
    public partial class OrganizationSettings : ComponentBase
    {
        /// <summary>
        /// Gets or sets the organization short name supplied from the URL route.
        /// </summary>
        [Parameter]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the view model for the organization settings page.
        /// </summary>
        [Inject]
        public IOrganizationSettingsViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the dialog service used to display modal dialogs.
        /// </summary>
        [Inject]
        public DialogService DialogService { get; set; }

        /// <summary>
        /// Handles component parameter updates and initializes the view model with the organization identifier asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (string.IsNullOrWhiteSpace(this.Id))
            {
                return;
            }

            await this.ViewModel.InitializeViewModel(this.Id);
        }

        /// <summary>
        /// Gets the breadcrumb navigation items for the organization settings page.
        /// </summary>
        /// <returns>A collection of <see cref="BreadcrumbItem" /> entries representing the trail.</returns>
        private IEnumerable<BreadcrumbItem> GetBreadcrumbItems()
        {
            var scope = this.ViewModel.Organization.ShortName;

            return
            [
                new BreadcrumbItem
                {
                    Name = "Search",
                    Link = PageRoutes.Packages
                },
                new BreadcrumbItem
                {
                    Name = $"@{scope}",
                    Link = PageRoutes.GetOrganizationRoute(scope)
                },
                new BreadcrumbItem
                {
                    Name = "Settings"
                }
            ];
        }

        /// <summary>
        /// Handles changing the assigned role of an organization member asynchronously.
        /// </summary>
        /// <param name="member">The member whose role is changing.</param>
        /// <param name="role">The selected new role for the member.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnChangeMemberRole(IAccount member, OrganizationRole role)
        {
            var currentRole = this.GetMemberRole(member);

            if (role != currentRole)
            {
                await this.ViewModel.ChangeMemberRole(member, role);
            }
        }

        /// <summary>
        /// Handles the action to remove a member from the organization asynchronously.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnRemoveMember(IAccount member)
        {
            var dialogOptions = new ConfirmDialogOptions
            {
                Destructive = true
            };

            var dialogResult = await this.DialogService.ConfirmAsync("Member Removal", "Are you sure you want to remove this member from the organization?", dialogOptions);

            if (dialogResult.Confirmed)
            {
                await this.ViewModel.RemoveMember(member);
            }
        }

        /// <summary>
        /// Handles the action to resend a pending membership invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The invitation to resend.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnResendInvitation(IOrganizationInvitation invitation)
        {
            await this.ViewModel.ResendInvitation(invitation);
        }

        /// <summary>
        /// Handles the action to revoke a pending membership invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The invitation to revoke.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnRevokeInvitation(IOrganizationInvitation invitation)
        {
            await this.ViewModel.RevokeInvitation(invitation);
        }

        /// <summary>
        /// Handles the action to initiate an organization transfer asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnTransferOrganization()
        {
            await this.ViewModel.TransferOrganization();
        }

        /// <summary>
        /// Resolves the role of the specified member in the organization.
        /// </summary>
        /// <param name="member">The member account.</param>
        /// <returns>The organization role representing the role.</returns>
        private OrganizationRole GetMemberRole(IAccount member)
        {
            return this.ViewModel.Organization.Administrator.Contains(member.Id)
                ? OrganizationRole.Administrator
                : OrganizationRole.Member;
        }

        /// <summary>
        /// Determines whether the specified member account corresponds to the currently logged in user.
        /// </summary>
        /// <param name="member">The member account to check.</param>
        /// <returns><c>true</c> if the member is the current user; otherwise, <c>false</c>.</returns>
        private bool IsCurrentUser(IAccount member)
        {
            return member.Id == this.ViewModel.CurrentUserId;
        }

        /// <summary>
        /// Gets the display name of the target account associated with a given organization invitation.
        /// </summary>
        /// <param name="invitation">The organization invitation.</param>
        /// <returns>The display name of the target account.</returns>
        private string GetInvitationTargetName(IOrganizationInvitation invitation)
        {
            var target = this.ViewModel.InvitedAccounts.FirstOrDefault(a => a.Id == invitation.Target);
            return target?.ShortName ?? "Unknown";
        }
    }
}
