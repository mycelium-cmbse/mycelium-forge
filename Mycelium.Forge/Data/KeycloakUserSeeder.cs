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
    using System.Net.Http.Headers;
    using System.Text.Json;

    using Microsoft.Extensions.Options;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Seeds the development users into Forge's Keycloak through its admin REST API.
    /// </summary>
    public class KeycloakUserSeeder : IKeycloakUserSeeder
    {
        /// <summary>
        /// The injected <see cref="HttpClient" /> used to call Keycloak.
        /// </summary>
        private readonly HttpClient httpClient;

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
        /// <param name="httpClient">The injected <see cref="HttpClient" />.</param>
        /// <param name="options">The injected <see cref="KeycloakOptions" />.</param>
        /// <param name="logger">The injected <see cref="ILogger{KeycloakUserSeeder}" />.</param>
        public KeycloakUserSeeder(HttpClient httpClient, IOptions<KeycloakOptions> options, ILogger<KeycloakUserSeeder> logger)
        {
            this.httpClient = httpClient;
            this.options = options.Value;
            this.logger = logger;
            this.httpClient.BaseAddress = new Uri(this.options.BaseUrl);
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
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await this.GetAdminTokenAsync(cancellationToken));

                await this.DeleteExistingUsersAsync(cancellationToken);

                var users = SeedData.Accounts.Select(account =>
                {
                    var nameParts = account.Name.Split(' ');
                    var firstName = nameParts.FirstOrDefault();
                    var lastName = nameParts.Length > 1 ? nameParts.LastOrDefault() : null;

                    return new
                    {
                        id = account.Id,
                        username = account.ShortName,
                        email = account.Email,
                        firstName,
                        lastName,
                        enabled = true,
                        emailVerified = true,
                        requiredActions = Array.Empty<string>(),
                        credentials = new[] { new { type = "password", value = this.options.SeedUserPassword, temporary = false } },
                        realmRoles = SeedData.AccountRoles[account.Id].Select(role => role.ToString())
                    };
                });

                using var response = await this.httpClient.PostAsJsonAsync($"admin/realms/{this.options.Realm}/partialImport", new { ifResourceExists = "OVERWRITE", users }, cancellationToken);
                response.EnsureSuccessStatusCode();

                await Task.WhenAll(SeedData.Accounts.Select(account => this.AssignRolesAsync(account, cancellationToken)));

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

        /// <summary>
        /// Deletes all existing users in the realm to ensure a clean seeding state.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task DeleteExistingUsersAsync(CancellationToken cancellationToken)
        {
            var existingUsers = await this.httpClient.GetFromJsonAsync<List<JsonElement>>($"admin/realms/{this.options.Realm}/users", cancellationToken);

            if (existingUsers is not { Count: > 0 })
            {
                return;
            }

            var deleteTasks = existingUsers
                .Select(user => user.GetProperty("id").GetString())
                .Select(id => this.DeleteUserAsync(id, cancellationToken));

            await Task.WhenAll(deleteTasks);
        }

        /// <summary>
        /// Deletes an individual user by their identifier from the realm.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task DeleteUserAsync(string userId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            using var response = await this.httpClient.DeleteAsync($"admin/realms/{this.options.Realm}/users/{userId}", cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Maps the realm roles of a seeded account onto its Keycloak user.
        /// </summary>
        /// <param name="account">The seeded account.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task AssignRolesAsync(Account account, CancellationToken cancellationToken)
        {
            var roleTasks = SeedData.AccountRoles[account.Id].Select(role => this.httpClient.GetFromJsonAsync<JsonElement>($"admin/realms/{this.options.Realm}/roles/{role}", cancellationToken));
            var roles = await Task.WhenAll(roleTasks);

            using var response = await this.httpClient.PostAsJsonAsync($"admin/realms/{this.options.Realm}/users/{account.Id}/role-mappings/realm", roles, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Requests an access token of the Keycloak administrator from the master realm.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The access token.</returns>
        private async Task<string> GetAdminTokenAsync(CancellationToken cancellationToken)
        {
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", "admin-cli" },
                { "username", this.options.AdminUsername },
                { "password", this.options.AdminPassword }
            });

            using var response = await this.httpClient.PostAsync("realms/master/protocol/openid-connect/token", form, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync(cancellationToken), cancellationToken: cancellationToken);
            return document.RootElement.GetProperty("access_token").GetString();
        }
    }
}
