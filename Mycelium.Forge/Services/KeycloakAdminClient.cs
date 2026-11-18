// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakAdminClient.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using System.Net.Http.Headers;
    using System.Text.Json;

    using ErrorOr;

    using Microsoft.Extensions.Options;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Common.Extensions;

    /// <summary>
    /// Executes administration operations against the Keycloak Admin REST API.
    /// </summary>
    public class KeycloakAdminClient : IKeycloakAdminClient
    {
        /// <summary>
        /// The error code returned when a Keycloak administration request fails.
        /// </summary>
        public const string SyncFailedErrorCode = "Keycloak.SyncFailed";

        /// <summary>
        /// The injected <see cref="HttpClient" /> used to communicate with Keycloak.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// The injected <see cref="ILogger{KeycloakAdminClient}" /> used for logging.
        /// </summary>
        private readonly ILogger<KeycloakAdminClient> logger;

        /// <summary>
        /// The injected <see cref="KeycloakOptions" /> holding the Keycloak administration settings.
        /// </summary>
        private readonly KeycloakOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeycloakAdminClient" /> class.
        /// </summary>
        /// <param name="httpClient">The injected <see cref="HttpClient" />.</param>
        /// <param name="options">The injected <see cref="KeycloakOptions" />.</param>
        /// <param name="logger">The injected <see cref="ILogger{KeycloakAdminClient}" />.</param>
        public KeycloakAdminClient(HttpClient httpClient, IOptions<KeycloakOptions> options, ILogger<KeycloakAdminClient> logger)
        {
            this.httpClient = httpClient;
            this.options = options.Value;
            this.logger = logger;

            if (!string.IsNullOrWhiteSpace(this.options.BaseUrl))
            {
                this.httpClient.BaseAddress = new Uri(this.options.BaseUrl);
            }
        }

        /// <summary>
        /// Asynchronously updates the profile attributes of an existing Keycloak user.
        /// </summary>
        /// <remarks>
        /// The <c>username</c> field is intentionally excluded because <c>editUsernameAllowed</c> is <c>false</c> in
        /// <c>forge-realm.json</c> and scope rename (<c>ScopeAlias</c>, design section 8.5) is not yet implemented.
        /// </remarks>
        /// <param name="userId">The unique identifier of the Keycloak user.</param>
        /// <param name="email">The updated email address.</param>
        /// <param name="firstName">The updated first name.</param>
        /// <param name="lastName">The updated last name.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the update succeeded.</returns>
        public Task<ErrorOr<Success>> UpdateUserAsync(Guid userId, string email, string firstName, string lastName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(this.options.BaseUrl))
            {
                this.logger.LogWarning("Keycloak BaseUrl is not configured; skipping Keycloak user update for {UserId}", userId.SanitizeForLog());
                return Task.FromResult<ErrorOr<Success>>(Result.Success);
            }

            return this.ExecuteAdminOperationAsync(accessToken => this.UpdateUserCoreAsync(userId, email, firstName, lastName, accessToken, cancellationToken), $"update Keycloak user {userId}", cancellationToken);
        }

        /// <summary>
        /// Asynchronously deletes all existing users in the configured Keycloak realm.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the deletion succeeded.</returns>
        public Task<ErrorOr<Success>> DeleteAllUsersAsync(CancellationToken cancellationToken = default)
        {
            return this.ExecuteAdminOperationAsync(accessToken => this.DeleteAllUsersCoreAsync(accessToken, cancellationToken), "delete existing Keycloak users", cancellationToken);
        }

        /// <summary>
        /// Asynchronously imports a collection of user representations into the configured Keycloak realm using partial import.
        /// </summary>
        /// <param name="users">The user representation payloads to import.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the import succeeded.</returns>
        public Task<ErrorOr<Success>> PartialImportUsersAsync(IEnumerable<object> users, CancellationToken cancellationToken = default)
        {
            return this.ExecuteAdminOperationAsync(accessToken => this.PartialImportUsersCoreAsync(users, accessToken, cancellationToken), "import Keycloak users", cancellationToken);
        }

        /// <summary>
        /// Asynchronously assigns the specified realm roles to a Keycloak user.
        /// </summary>
        /// <param name="userId">The unique identifier of the Keycloak user.</param>
        /// <param name="roleNames">The names of the realm roles to assign.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the role assignment succeeded.</returns>
        public Task<ErrorOr<Success>> AssignRealmRolesAsync(Guid userId, IEnumerable<string> roleNames, CancellationToken cancellationToken = default)
        {
            return this.ExecuteAdminOperationAsync(accessToken => this.AssignRealmRolesCoreAsync(userId, roleNames, accessToken, cancellationToken), $"assign Keycloak roles to user {userId}", cancellationToken);
        }

        /// <summary>
        /// Acquires an administrator access token, runs the supplied Keycloak operation, and translates failures into
        /// <see cref="ErrorOr{TValue}" /> errors.
        /// </summary>
        /// <typeparam name="T">The operation result type.</typeparam>
        /// <param name="operation">The operation to execute with the acquired bearer token.</param>
        /// <param name="operationDescription">A short description of the operation for error logging.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{TValue}" /> containing the operation result or an error.</returns>
        private async Task<ErrorOr<T>> ExecuteAdminOperationAsync<T>(Func<string, Task<ErrorOr<T>>> operation, string operationDescription, CancellationToken cancellationToken)
        {
            try
            {
                var tokenResult = await this.GetAdminTokenAsync(cancellationToken);

                if (tokenResult.IsError)
                {
                    this.logger.LogError("Failed to {Operation}: {Errors}", operationDescription, tokenResult.Errors.ToJoinedDescriptions());
                    return tokenResult.Errors;
                }

                return await operation(tokenResult.Value);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to {Operation}", operationDescription);
                return Error.Failure(SyncFailedErrorCode, exception.Message);
            }
        }

        /// <summary>
        /// Updates the profile attributes of a Keycloak user using the supplied administrator access token.
        /// </summary>
        /// <param name="userId">The unique identifier of the Keycloak user.</param>
        /// <param name="email">The updated email address.</param>
        /// <param name="firstName">The updated first name.</param>
        /// <param name="lastName">The updated last name.</param>
        /// <param name="accessToken">The Keycloak administrator access token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the update succeeded.</returns>
        private async Task<ErrorOr<Success>> UpdateUserCoreAsync(Guid userId, string email, string firstName, string lastName, string accessToken, CancellationToken cancellationToken)
        {
            var payload = new
            {
                email,
                firstName,
                lastName
            };

            using var content = JsonContent.Create(payload);
            var requestUri = $"admin/realms/{this.options.Realm}/users/{userId}";
            using var response = await this.SendAuthenticatedAsync(HttpMethod.Put, requestUri, accessToken, content, cancellationToken);

            return this.EnsureSuccessStatus(response, HttpMethod.Put, requestUri);
        }

        /// <summary>
        /// Deletes all existing users in the configured Keycloak realm using the supplied administrator access token.
        /// </summary>
        /// <param name="accessToken">The Keycloak administrator access token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the deletion succeeded.</returns>
        private async Task<ErrorOr<Success>> DeleteAllUsersCoreAsync(string accessToken, CancellationToken cancellationToken)
        {
            var requestUri = $"admin/realms/{this.options.Realm}/users";
            using var response = await this.SendAuthenticatedAsync(HttpMethod.Get, requestUri, accessToken, null, cancellationToken);
            var statusResult = this.EnsureSuccessStatus(response, HttpMethod.Get, requestUri);

            if (statusResult.IsError)
            {
                return statusResult.Errors;
            }

            var existingUsers = await response.Content.ReadFromJsonAsync<List<JsonElement>>(cancellationToken);

            if (existingUsers is not { Count: > 0 })
            {
                return Result.Success;
            }

            var deleteResults = await Task.WhenAll(existingUsers.Select(user => this.DeleteUserAsync(user.GetProperty("id").GetString(), accessToken, cancellationToken)));
            var failed = Array.Find(deleteResults, result => result.IsError);

            return failed.IsError ? failed.Errors : Result.Success;
        }

        /// <summary>
        /// Deletes an individual user by identifier using the supplied administrator access token.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <param name="accessToken">The Keycloak administrator access token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the deletion succeeded.</returns>
        private async Task<ErrorOr<Success>> DeleteUserAsync(string userId, string accessToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result.Success;
            }

            var requestUri = $"admin/realms/{this.options.Realm}/users/{userId}";
            using var response = await this.SendAuthenticatedAsync(HttpMethod.Delete, requestUri, accessToken, null, cancellationToken);

            return this.EnsureSuccessStatus(response, HttpMethod.Delete, requestUri);
        }

        /// <summary>
        /// Imports user representations into the configured Keycloak realm using the supplied administrator access token.
        /// </summary>
        /// <param name="users">The user representation payloads to import.</param>
        /// <param name="accessToken">The Keycloak administrator access token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the import succeeded.</returns>
        private async Task<ErrorOr<Success>> PartialImportUsersCoreAsync(IEnumerable<object> users, string accessToken, CancellationToken cancellationToken)
        {
            var payload = new
            {
                ifResourceExists = "OVERWRITE",
                users
            };

            using var content = JsonContent.Create(payload);
            var requestUri = $"admin/realms/{this.options.Realm}/partialImport";
            using var response = await this.SendAuthenticatedAsync(HttpMethod.Post, requestUri, accessToken, content, cancellationToken);

            return this.EnsureSuccessStatus(response, HttpMethod.Post, requestUri);
        }

        /// <summary>
        /// Assigns realm roles to a Keycloak user using the supplied administrator access token.
        /// </summary>
        /// <param name="userId">The unique identifier of the Keycloak user.</param>
        /// <param name="roleNames">The names of the realm roles to assign.</param>
        /// <param name="accessToken">The Keycloak administrator access token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the role assignment succeeded.</returns>
        private async Task<ErrorOr<Success>> AssignRealmRolesCoreAsync(Guid userId, IEnumerable<string> roleNames, string accessToken, CancellationToken cancellationToken)
        {
            var roleResults = await Task.WhenAll(roleNames.Select(roleName => this.GetRealmRoleAsync(roleName, accessToken, cancellationToken)));
            var failedRole = Array.Find(roleResults, result => result.IsError);

            if (failedRole.IsError)
            {
                return failedRole.Errors;
            }

            var roles = roleResults.Select(result => result.Value).ToArray();
            using var content = JsonContent.Create(roles);
            var requestUri = $"admin/realms/{this.options.Realm}/users/{userId}/role-mappings/realm";
            using var response = await this.SendAuthenticatedAsync(HttpMethod.Post, requestUri, accessToken, content, cancellationToken);

            return this.EnsureSuccessStatus(response, HttpMethod.Post, requestUri);
        }

        /// <summary>
        /// Retrieves a realm role JSON representation from Keycloak.
        /// </summary>
        /// <param name="roleName">The name of the realm role.</param>
        /// <param name="accessToken">The Keycloak administrator access token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{JsonElement}" /> containing the role representation or an error.</returns>
        private async Task<ErrorOr<JsonElement>> GetRealmRoleAsync(string roleName, string accessToken, CancellationToken cancellationToken)
        {
            var requestUri = $"admin/realms/{this.options.Realm}/roles/{roleName}";
            using var response = await this.SendAuthenticatedAsync(HttpMethod.Get, requestUri, accessToken, null, cancellationToken);
            var statusResult = this.EnsureSuccessStatus(response, HttpMethod.Get, requestUri);

            if (statusResult.IsError)
            {
                return statusResult.Errors;
            }

            return await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        }

        /// <summary>
        /// Requests an access token of the Keycloak administrator from the master realm.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{TValue}" /> containing the access token or an error.</returns>
        private async Task<ErrorOr<string>> GetAdminTokenAsync(CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", "admin-cli" },
                { "username", this.options.AdminUsername },
                { "password", this.options.AdminPassword }
            };

            using var form = new FormUrlEncodedContent(parameters);
            using var response = await this.httpClient.PostAsync("realms/master/protocol/openid-connect/token", form, cancellationToken);
            var statusResult = this.EnsureSuccessStatus(response, HttpMethod.Post, "realms/master/protocol/openid-connect/token");

            if (statusResult.IsError)
            {
                return statusResult.Errors;
            }

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
            var token = document.RootElement.GetProperty("access_token").GetString();

            if (string.IsNullOrWhiteSpace(token))
            {
                return Error.Failure(SyncFailedErrorCode, "Keycloak admin token response did not contain an access token.");
            }

            return token;
        }

        /// <summary>
        /// Sends an HTTP request with a per-request bearer authorization header.
        /// </summary>
        /// <param name="method">The HTTP method to send.</param>
        /// <param name="requestUri">The relative request URI.</param>
        /// <param name="accessToken">The bearer access token.</param>
        /// <param name="content">The optional HTTP request content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="HttpResponseMessage" /> returned by Keycloak.</returns>
        private Task<HttpResponseMessage> SendAuthenticatedAsync(HttpMethod method, string requestUri, string accessToken, HttpContent content, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = content;

            return this.SendAndDisposeRequestAsync(request, cancellationToken);
        }

        /// <summary>
        /// Sends and disposes an <see cref="HttpRequestMessage" />.
        /// </summary>
        /// <param name="request">The HTTP request message to send.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="HttpResponseMessage" /> returned by the server.</returns>
        private async Task<HttpResponseMessage> SendAndDisposeRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (request)
            {
                return await this.httpClient.SendAsync(request, cancellationToken);
            }
        }

        /// <summary>
        /// Validates that the Keycloak HTTP response has a success status code, logging and returning a failure error otherwise.
        /// </summary>
        /// <param name="response">The HTTP response received from Keycloak.</param>
        /// <param name="method">The HTTP method that was sent.</param>
        /// <param name="requestUri">The request URI that was called.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> representing the HTTP response status.</returns>
        private ErrorOr<Success> EnsureSuccessStatus(HttpResponseMessage response, HttpMethod method, string requestUri)
        {
            if (response.IsSuccessStatusCode)
            {
                return Result.Success;
            }

            this.logger.LogError("Keycloak admin request {Method} {Uri} failed with status code {StatusCode}", method, requestUri, (int)response.StatusCode);
            return Error.Failure(SyncFailedErrorCode, $"Keycloak request {method} {requestUri} failed with status code {(int)response.StatusCode}.");
        }
    }
}
