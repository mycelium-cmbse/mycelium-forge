// ------------------------------------------------------------------------------------------------
// <copyright file="IAccountProvisioner.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using System.Security.Claims;

    using ErrorOr;

    /// <summary>
    /// Provisions the Forge account of an identity provider user on first sign-in (SSS-FG-AUTH-A2F).
    /// </summary>
    public interface IAccountProvisioner
    {
        /// <summary>
        /// Ensures a Forge account exists for the authenticated principal, creating it from the identity provider claims
        /// when it does not exist yet.
        /// </summary>
        /// <param name="principal">The principal authenticated by the identity provider.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{TValue}" /> indicating whether the account exists after the call.</returns>
        Task<ErrorOr<Success>> ProvisionAsync(ClaimsPrincipal principal, CancellationToken cancellationToken);
    }
}
