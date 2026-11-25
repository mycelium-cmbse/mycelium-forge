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
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Text.Json.Nodes;

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
        public Task<ErrorOr<Success>> UpdateUserAsync(Guid userId, string email, string firstName, string lastName, CancellationToken cancellationToken = default)
        {
            return this.ExecuteAdminOperationAsync(async accessToken =>
            {
                var requestUri = $"admin/realms/{this.options.Realm}/users/{userId}";
                using var getResponse = await this.SendAuthenticatedAsync(HttpMethod.Get, requestUri, accessToken, null, cancellationToken);
                var getStatusResult = this.EnsureSuccessStatus(getResponse, HttpMethod.Get, "users/{id}");

                if (getStatusResult.IsError)
                {
                    return getStatusResult.Errors;
                }

                var user = await getResponse.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);

                if (user == null)
                {
                    return Error.Failure(SyncFailedErrorCode, "Keycloak returned an empty user representation.");
                }

                user["email"] = email;
                user["firstName"] = firstName;
                user["lastName"] = lastName;

                using var content = JsonContent.Create(user);
                using var putResponse = await this.SendAuthenticatedAsync(HttpMethod.Put, requestUri, accessToken, content, cancellationToken);

                return this.EnsureSuccessStatus(putResponse, HttpMethod.Put, "users/{id}");
            }, "update Keycloak user", cancellationToken);
        }

        /// <summary>
        /// Asynchronously deletes all existing users in the configured Keycloak realm.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the deletion succeeded.</returns>
        public Task<ErrorOr<Success>> DeleteAllUsersAsync(CancellationToken cancellationToken = default)
        {
            return this.ExecuteAdminOperationAsync(async accessToken =>
            {
                var requestUri = $"admin/realms/{this.options.Realm}/users";
                using var response = await this.SendAuthenticatedAsync(HttpMethod.Get, requestUri, accessToken, null, cancellationToken);
                var statusResult = this.EnsureSuccessStatus(response, HttpMethod.Get, "users");

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
            }, "delete existing Keycloak users", cancellationToken);
        }

        /// <summary>
        /// Asynchronously imports a collection of user representations into the configured Keycloak realm using partial import.
        /// </summary>
        /// <param name="users">The user representation payloads to import.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the import succeeded.</returns>
        public Task<ErrorOr<Success>> PartialImportUsersAsync(IEnumerable<object> users, CancellationToken cancellationToken = default)
        {
            return this.ExecuteAdminOperationAsync(async accessToken =>
            {
                var payload = new
                {
                    ifResourceExists = "OVERWRITE",
                    users
                };

                using var content = JsonContent.Create(payload);
                var requestUri = $"admin/realms/{this.options.Realm}/partialImport";
                using var response = await this.SendAuthenticatedAsync(HttpMethod.Post, requestUri, accessToken, content, cancellationToken);

                return this.EnsureSuccessStatus(response, HttpMethod.Post, "partialImport");
            }, "import Keycloak users", cancellationToken);
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
            return this.ExecuteAdminOperationAsync(async accessToken =>
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

                return this.EnsureSuccessStatus(response, HttpMethod.Post, "users/{id}/role-mappings/realm");
            }, "assign Keycloak roles", cancellationToken);
        }

        /// <summary>
        /// Acquires an administrator access token, runs the supplied Keycloak operation, and translates failures into
        /// <see cref="ErrorOr{TValue}" /> errors.
        /// </summary>
        /// <param name="operation">The operation to execute with the acquired bearer token.</param>
        /// <param name="operationDescription">A short description of the operation for error logging.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> containing the operation result or an error.</returns>
        private async Task<ErrorOr<Success>> ExecuteAdminOperationAsync(Func<string, Task<ErrorOr<Success>>> operation, string operationDescription, CancellationToken cancellationToken)
        {
            var sanitizedDescription = operationDescription.SanitizeForLog();

            if (string.IsNullOrWhiteSpace(this.options.BaseUrl) || string.IsNullOrWhiteSpace(this.options.AdminUsername) || string.IsNullOrWhiteSpace(this.options.AdminPassword))
            {
                this.logger.LogError("Keycloak admin client is not configured; cannot {Operation}", sanitizedDescription);
                return Error.Validation(SyncFailedErrorCode, "Keycloak admin client is not configured.");
            }

            try
            {
                var tokenResult = await this.GetAdminTokenAsync(cancellationToken);

                if (tokenResult.IsError)
                {
                    this.logger.LogError("Failed to {Operation}: {Errors}", sanitizedDescription, tokenResult.Errors.ToJoinedDescriptions());
                    return tokenResult.Errors;
                }

                return await operation.Invoke(tokenResult.Value);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to {Operation}", sanitizedDescription);
                return Error.Failure(SyncFailedErrorCode, exception.Message);
            }
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

            return this.EnsureSuccessStatus(response, HttpMethod.Delete, "users/{id}");
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
            var statusResult = this.EnsureSuccessStatus(response, HttpMethod.Get, "roles/{roleName}");

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
            var statusResult = this.EnsureSuccessStatus(response, HttpMethod.Post, "protocol/openid-connect/token");

            if (statusResult.IsError)
            {
                return response.StatusCode == HttpStatusCode.BadRequest
                    ? Error.Unauthorized(SyncFailedErrorCode, statusResult.FirstError.Description)
                    : statusResult.Errors;
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
        private async Task<HttpResponseMessage> SendAuthenticatedAsync(HttpMethod method, string requestUri, string accessToken, HttpContent content, CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = content;

            return await this.httpClient.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Validates that the Keycloak HTTP response has a success status code, logging and returning a failure error otherwise.
        /// </summary>
        /// <param name="response">The HTTP response received from Keycloak.</param>
        /// <param name="method">The HTTP method that was sent.</param>
        /// <param name="endpoint">The static endpoint name that was called.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> representing the HTTP response status.</returns>
        private ErrorOr<Success> EnsureSuccessStatus(HttpResponseMessage response, HttpMethod method, string endpoint)
        {
            if (response.IsSuccessStatusCode)
            {
                return Result.Success;
            }

            var statusCode = (int)response.StatusCode;
            this.logger.LogError("Keycloak admin request {Method} {Endpoint} failed with status code {StatusCode}", method, endpoint, statusCode);
            var description = $"Keycloak request {method} {endpoint} failed with status code {statusCode}.";

            return response.StatusCode switch
            {
                HttpStatusCode.BadRequest or HttpStatusCode.UnprocessableEntity => Error.Validation(SyncFailedErrorCode, description),
                HttpStatusCode.Unauthorized => Error.Unauthorized(SyncFailedErrorCode, description),
                HttpStatusCode.Forbidden => Error.Forbidden(SyncFailedErrorCode, description),
                HttpStatusCode.NotFound => Error.NotFound(SyncFailedErrorCode, description),
                HttpStatusCode.Conflict => Error.Conflict(SyncFailedErrorCode, description),
                _ => Error.Failure(SyncFailedErrorCode, description)
            };
        }
    }
}
