// ------------------------------------------------------------------------------------------------
// <copyright file="AccountMenu.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Represents the user account dropdown menu triggered from the header avatar.
    /// </summary>
    public partial class AccountMenu : ComponentBase
    {
        /// <summary>
        /// Gets or sets the current user account retrieved from the user service.
        /// </summary>
        private IAccount currentUser;

        /// <summary>
        /// Gets a value indicating whether the current user has the installation administrator role.
        /// </summary>
        private bool isInstallationAdministrator;

        /// <summary>
        /// Gets or sets the injected user service.
        /// </summary>
        [Inject]
        public IUserService UserService { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager instance.
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the primary organization identifier for navigation.
        /// </summary>
        public string Organization { get; set; } = "starion";

        /// <summary>
        /// Handles the user sign-out action by clearing current authentication and navigating to login.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnSignOut()
        {
            await this.UserService.SetCurrentUser(null);
            this.NavigationManager.NavigateTo(PageRoutes.Logout, forceLoad: true);
        }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// Override this method if you will perform an asynchronous operation and
        /// want the component to refresh when that operation is completed.
        /// </summary>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            this.currentUser = await this.UserService.GetCurrentUser();

            var userContext = await this.UserService.GetUserContext();
            this.isInstallationAdministrator = userContext.CurrentRoles.Contains(RoleKind.InstallationAdministrator);
        }
    }
}
