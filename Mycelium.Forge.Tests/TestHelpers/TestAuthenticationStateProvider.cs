// ------------------------------------------------------------------------------------------------
// <copyright file="TestAuthenticationStateProvider.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.TestHelpers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Components.Authorization;

    /// <summary>
    /// Test double for <see cref="AuthenticationStateProvider" /> providing synchronous in-memory state manipulation
    /// and event notification support.
    /// </summary>
    public class TestAuthenticationStateProvider : AuthenticationStateProvider
    {
        /// <summary>
        /// The current authentication state.
        /// </summary>
        private AuthenticationState currentAuthenticationState = new(new ClaimsPrincipal(new ClaimsIdentity()));

        /// <summary>
        /// Asynchronously provides the current <see cref="AuthenticationState" />.
        /// </summary>
        /// <returns>A task that completes with the current <see cref="AuthenticationState" />.</returns>
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(this.currentAuthenticationState);
        }

        /// <summary>
        /// Sets the current authentication state and notifies subscribers of the change.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal" /> representing the authenticated or unauthenticated user.</param>
        public void SetAuthenticationState(ClaimsPrincipal principal)
        {
            this.currentAuthenticationState = new AuthenticationState(principal);
            this.NotifyAuthenticationStateChanged(Task.FromResult(this.currentAuthenticationState));
        }
    }
}
