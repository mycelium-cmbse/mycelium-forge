// ------------------------------------------------------------------------------------------------
// <copyright file="IKeycloakUserSeeder.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Data
{
    /// <summary>
    /// Defines the seeding of the development users into Forge's Keycloak.
    /// </summary>
    public interface IKeycloakUserSeeder
    {
        /// <summary>
        /// Asynchronously creates one Keycloak user per seeded account, with the account identifier as user identifier and
        /// the account roles as realm roles. Existing users are replaced.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to abort the seeding operation.</param>
        /// <returns>
        /// A task representing the asynchronous operation, resolving to <see langword="true" /> if seeding completed
        /// successfully; otherwise <see langword="false" />.
        /// </returns>
        Task<bool> SeedAsync(CancellationToken cancellationToken = default);
    }
}
