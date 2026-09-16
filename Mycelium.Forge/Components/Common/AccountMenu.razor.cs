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
        /// Gets or sets the injected user service.
        /// </summary>
        [Inject]
        public IUserService UserService { get; set; }

        /// <summary>
        /// Gets or sets the primary organization identifier for navigation.
        /// </summary>
        public string Organization { get; set; } = "starion";

        /// <summary>
        /// Gets a value indicating whether the current user has the installation administrator role.
        /// </summary>
        public bool IsInstallationAdministrator => this.UserService.CurrentRoles.Contains(RoleKind.InstallationAdministrator);

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

            this.currentUser = this.UserService.GetCurrentUser();
        }
    }
}
