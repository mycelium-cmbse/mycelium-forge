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
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
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
        /// Handles changing the assigned role of an organization member asynchronously.
        /// </summary>
        /// <param name="member">The member whose role is changing.</param>
        /// <param name="role">The selected new role for the member.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnChangeMemberRole(IAccount member, OrganizationInvitationKind role)
        {
            await this.ViewModel.ChangeMemberRole(member, role);
        }

        /// <summary>
        /// Handles the action to remove a member from the organization asynchronously.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnRemoveMember(IAccount member)
        {
            await this.ViewModel.RemoveMember(member);
        }

        /// <summary>
        /// Handles the action to resend a pending membership invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The invitation to resend.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnResendInvitation(IOrganizationInvitation invitation)
        {
            await this.ViewModel.ResendInvitation(invitation);
        }

        /// <summary>
        /// Handles the action to revoke a pending membership invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The invitation to revoke.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnRevokeInvitation(IOrganizationInvitation invitation)
        {
            await this.ViewModel.RevokeInvitation(invitation);
        }

        /// <summary>
        /// Handles the action to initiate an organization transfer asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnTransferOrganization()
        {
            await this.ViewModel.TransferOrganization();
        }

        /// <summary>
        /// Resolves the role of the specified member in the organization.
        /// </summary>
        /// <param name="member">The member account.</param>
        /// <returns>The organization invitation kind representing the role.</returns>
        public OrganizationInvitationKind GetMemberRole(IAccount member)
        {
            return this.ViewModel.Organization.Administrator.Contains(member.Id)
                ? OrganizationInvitationKind.ADMINISTRATOR
                : OrganizationInvitationKind.MEMBER;
        }

        /// <summary>
        /// Gets the breadcrumb navigation items for the organization settings page.
        /// </summary>
        /// <returns>A collection of <see cref="BreadcrumbItem" /> entries representing the trail.</returns>
        public IEnumerable<BreadcrumbItem> GetBreadcrumbItems()
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
        /// Handles component parameter updates and initializes the view model with the organization identifier asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (!string.IsNullOrWhiteSpace(this.Id))
            {
                await this.ViewModel.InitializeViewModel(this.Id);
            }
        }
    }
}
