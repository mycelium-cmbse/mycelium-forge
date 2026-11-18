// ------------------------------------------------------------------------------------------------
// <copyright file="IKeycloakAdminClient.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using ErrorOr;

    /// <summary>
    /// Defines administration operations against the Keycloak Admin REST API.
    /// </summary>
    public interface IKeycloakAdminClient
    {
        /// <summary>
        /// Asynchronously updates the profile attributes of an existing Keycloak user by fetching the current representation
        /// and merging the updated fields so existing metadata such as <c>createdTimestamp</c> is preserved.
        /// </summary>
        /// <remarks>
        /// The <c>username</c> field is intentionally left untouched because <c>editUsernameAllowed</c> is <c>false</c> in
        /// <c>forge-realm.json</c> and scope rename (<c>ScopeAlias</c>, design section 8.5) is not yet implemented.
        /// </remarks>
        /// <param name="userId">The unique identifier of the Keycloak user.</param>
        /// <param name="email">The updated email address.</param>
        /// <param name="firstName">The updated first name.</param>
        /// <param name="lastName">The updated last name.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the update succeeded.</returns>
        Task<ErrorOr<Success>> UpdateUserAsync(Guid userId, string email, string firstName, string lastName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously deletes all existing users in the configured Keycloak realm.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the deletion succeeded.</returns>
        Task<ErrorOr<Success>> DeleteAllUsersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously imports a collection of user representations into the configured Keycloak realm using partial import.
        /// </summary>
        /// <param name="users">The user representation payloads to import.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the import succeeded.</returns>
        Task<ErrorOr<Success>> PartialImportUsersAsync(IEnumerable<object> users, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously assigns the specified realm roles to a Keycloak user.
        /// </summary>
        /// <param name="userId">The unique identifier of the Keycloak user.</param>
        /// <param name="roleNames">The names of the realm roles to assign.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the role assignment succeeded.</returns>
        Task<ErrorOr<Success>> AssignRealmRolesAsync(Guid userId, IEnumerable<string> roleNames, CancellationToken cancellationToken = default);
    }
}
