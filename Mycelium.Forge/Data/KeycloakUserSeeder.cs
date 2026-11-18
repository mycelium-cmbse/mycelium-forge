// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakUserSeeder.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Data
{
    using Microsoft.Extensions.Options;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Common.Extensions;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Seeds the development users into Forge's Keycloak through its admin REST API.
    /// </summary>
    public class KeycloakUserSeeder : IKeycloakUserSeeder
    {
        /// <summary>
        /// The injected <see cref="IKeycloakAdminClient" /> used to call Keycloak.
        /// </summary>
        private readonly IKeycloakAdminClient keycloakAdminClient;

        /// <summary>
        /// The injected <see cref="ILogger{KeycloakUserSeeder}" /> used for logging.
        /// </summary>
        private readonly ILogger<KeycloakUserSeeder> logger;

        /// <summary>
        /// The injected <see cref="KeycloakOptions" /> holding the Keycloak administration settings.
        /// </summary>
        private readonly KeycloakOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeycloakUserSeeder" /> class.
        /// </summary>
        /// <param name="keycloakAdminClient">The injected <see cref="IKeycloakAdminClient" />.</param>
        /// <param name="options">The injected <see cref="KeycloakOptions" />.</param>
        /// <param name="logger">The injected <see cref="ILogger{KeycloakUserSeeder}" />.</param>
        public KeycloakUserSeeder(IKeycloakAdminClient keycloakAdminClient, IOptions<KeycloakOptions> options, ILogger<KeycloakUserSeeder> logger)
        {
            this.keycloakAdminClient = keycloakAdminClient;
            this.options = options.Value;
            this.logger = logger;
        }

        /// <summary>
        /// Asynchronously creates one Keycloak user per seeded account, with the account identifier as user identifier and
        /// the account roles as realm roles. Existing users are deleted prior to import.
        /// </summary>
        /// <remarks>
        /// The partial import endpoint is used because it, unlike user creation, preserves the supplied user identifiers.
        /// </remarks>
        /// <param name="cancellationToken">The cancellation token to abort the seeding operation.</param>
        /// <returns>
        /// A task representing the asynchronous operation, resolving to <see langword="true" /> if seeding completed
        /// successfully; otherwise <see langword="false" />.
        /// </returns>
        public async Task<bool> SeedAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var deleteResult = await this.keycloakAdminClient.DeleteAllUsersAsync(cancellationToken);

                if (deleteResult.IsError)
                {
                    this.logger.LogError("Failed to delete existing Keycloak users: {Errors}", deleteResult.Errors.ToJoinedDescriptions());
                    return false;
                }

                var users = SeedData.Accounts.Select(account =>
                {
                    var (firstName, lastName) = account.Name.SplitFullName();
                    string[] requiredActions = [];
                    object[] credentials = [new { type = "password", value = this.options.SeedUserPassword, temporary = false }];

                    return new
                    {
                        id = account.Id,
                        username = account.ShortName,
                        email = account.Email,
                        firstName,
                        lastName,
                        enabled = true,
                        emailVerified = true,
                        requiredActions,
                        credentials,
                        realmRoles = SeedData.AccountRoles[account.Id].Select(role => role.ToString())
                    };
                });

                var importResult = await this.keycloakAdminClient.PartialImportUsersAsync(users, cancellationToken);

                if (importResult.IsError)
                {
                    this.logger.LogError("Failed to import Keycloak users: {Errors}", importResult.Errors.ToJoinedDescriptions());
                    return false;
                }

                var roleResults = await Task.WhenAll(SeedData.Accounts.Select(account => this.keycloakAdminClient.AssignRealmRolesAsync(account.Id, SeedData.AccountRoles[account.Id].Select(role => role.ToString()), cancellationToken)));

                if (Array.Exists(roleResults, result => result.IsError))
                {
                    this.logger.LogError("Failed to assign realm roles to seeded Keycloak users");
                    return false;
                }

                if (this.logger.IsEnabled(LogLevel.Information))
                {
                    this.logger.LogInformation("Seeded {Count} Keycloak users", SeedData.Accounts.Count);
                }

                return true;
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to seed the Keycloak users");
                return false;
            }
        }
    }
}
