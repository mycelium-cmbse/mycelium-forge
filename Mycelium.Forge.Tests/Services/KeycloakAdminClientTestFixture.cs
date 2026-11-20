// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakAdminClientTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Services
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;
    using Moq.Protected;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Services;

    [TestFixture]
    public class KeycloakAdminClientTestFixture
    {
        private const string SendAsync = "SendAsync";

        private Mock<HttpMessageHandler> httpMessageHandlerMock;
        private Mock<ILogger<KeycloakAdminClient>> loggerMock;
        private IOptions<KeycloakOptions> options;
        private KeycloakAdminClient client;

        [SetUp]
        public void SetUp()
        {
            this.httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            this.loggerMock = new Mock<ILogger<KeycloakAdminClient>>();

            this.options = Options.Create(new KeycloakOptions
            {
                BaseUrl = "http://localhost:8080/",
                Realm = "forge",
                AdminUsername = "admin",
                AdminPassword = "adminpassword"
            });

            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.RequestUri.ToString().Contains("protocol/openid-connect/token")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"access_token\": \"mock_token\"}", Encoding.UTF8, "application/json")
            });

            var httpClient = new HttpClient(this.httpMessageHandlerMock.Object);
            this.client = new KeycloakAdminClient(httpClient, this.options, this.loggerMock.Object);
        }

        [Test]
        public async Task VerifyAssignRealmRolesAsync()
        {
            var userId = Guid.NewGuid();

            // Case 1: Fetches realm roles and posts role mappings
            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().Contains("/roles/")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"id\": \"role-id\", \"name\": \"Account\"}", Encoding.UTF8, "application/json")
            });

            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri != null && req.RequestUri.ToString().Contains($"/users/{userId}/role-mappings/realm")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent });

            var successResult = await this.client.AssignRealmRolesAsync(userId, ["Account"], CancellationToken.None);
            Assert.That(successResult.IsError, Is.False);

            // Case 2: Role fetch failure returns Keycloak.SyncFailed error
            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().Contains("/roles/")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound });

            var errorResult = await this.client.AssignRealmRolesAsync(userId, ["MissingRole"], CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(errorResult.IsError, Is.True);
                Assert.That(errorResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
            }

            // Case 3: Role mapping POST failure returns Keycloak.SyncFailed error
            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().Contains("/roles/")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"id\": \"role-id\", \"name\": \"Account\"}", Encoding.UTF8, "application/json")
            });

            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri != null && req.RequestUri.ToString().Contains($"/users/{userId}/role-mappings/realm")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest });

            var postErrorResult = await this.client.AssignRealmRolesAsync(userId, ["Account"], CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(postErrorResult.IsError, Is.True);
                Assert.That(postErrorResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
            }
        }

        [Test]
        public async Task VerifyDeleteAllUsersAsync()
        {
            // Case 1: Empty user list succeeds without DELETE calls
            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().EndsWith("/users")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[]", Encoding.UTF8, "application/json")
            });

            var emptyResult = await this.client.DeleteAllUsersAsync(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyResult.IsError, Is.False);
                this.httpMessageHandlerMock.Protected().Verify(SendAsync, Times.Never(), ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete), ItExpr.IsAny<CancellationToken>());
            }

            // Case 2: Deletes each non-empty user id
            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().EndsWith("/users")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{\"id\": \"user-1\"}, {\"id\": \" \"}]", Encoding.UTF8, "application/json")
            });

            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri != null && req.RequestUri.ToString().EndsWith("/users/user-1")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent });

            var deleteResult = await this.client.DeleteAllUsersAsync(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deleteResult.IsError, Is.False);
                this.httpMessageHandlerMock.Protected().Verify(SendAsync, Times.Once(), ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete), ItExpr.IsAny<CancellationToken>());
            }

            // Case 3: Failure reading users returns Keycloak.SyncFailed error
            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().EndsWith("/users")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError });

            var errorResult = await this.client.DeleteAllUsersAsync(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(errorResult.IsError, Is.True);
                Assert.That(errorResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
            }
        }

        [Test]
        public async Task VerifyPartialImportUsersAsync()
        {
            // Case 1: Sends POST partialImport with OVERWRITE and users
            string? capturedBody = null;

            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri != null && req.RequestUri.ToString().EndsWith("/partialImport")), ItExpr.IsAny<CancellationToken>()).Returns(async (HttpRequestMessage req, CancellationToken ct) =>
            {
                capturedBody = req.Content != null ? await req.Content.ReadAsStringAsync(ct) : null;
                return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            });

            var importResult = await this.client.PartialImportUsersAsync([new { username = "alex" }], CancellationToken.None);

            using var jsonDocument = JsonDocument.Parse(capturedBody!);
            var root = jsonDocument.RootElement;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(importResult.IsError, Is.False);
                Assert.That(root.GetProperty("ifResourceExists").GetString(), Is.EqualTo("OVERWRITE"));
                Assert.That(root.GetProperty("users").GetArrayLength(), Is.EqualTo(1));
            }

            // Case 2: Unconfigured BaseUrl returns Keycloak.SyncFailed error without HTTP calls
            var unconfiguredHandlerMock = new Mock<HttpMessageHandler>();
            var unconfiguredClient = new KeycloakAdminClient(new HttpClient(unconfiguredHandlerMock.Object), Options.Create(new KeycloakOptions { BaseUrl = string.Empty, Realm = "forge" }), this.loggerMock.Object);

            var unconfiguredResult = await unconfiguredClient.PartialImportUsersAsync([new { username = "alex" }], CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unconfiguredResult.IsError, Is.True);
                Assert.That(unconfiguredResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
                unconfiguredHandlerMock.Protected().Verify(SendAsync, Times.Never(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            }

            // Case 3: Non-success status returns Keycloak.SyncFailed error
            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri != null && req.RequestUri.ToString().EndsWith("/partialImport")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest });

            var errorResult = await this.client.PartialImportUsersAsync([new { username = "alex" }], CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(errorResult.IsError, Is.True);
                Assert.That(errorResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
            }
        }

        [Test]
        public async Task VerifyUpdateUserAsync()
        {
            var userId = Guid.NewGuid();

            // Case 1: Empty BaseUrl returns Keycloak.SyncFailed error without HTTP calls
            var emptyBaseUrlOptions = Options.Create(new KeycloakOptions { BaseUrl = "   ", Realm = "forge", AdminUsername = "admin", AdminPassword = "adminpassword" });
            var unconfiguredHandlerMock = new Mock<HttpMessageHandler>();
            var unconfiguredClient = new KeycloakAdminClient(new HttpClient(unconfiguredHandlerMock.Object), emptyBaseUrlOptions, this.loggerMock.Object);

            var emptyUrlResult = await unconfiguredClient.UpdateUserAsync(userId, "alex@example.com", "Alex", "Rivera", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyUrlResult.IsError, Is.True);
                Assert.That(emptyUrlResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
                unconfiguredHandlerMock.Protected().Verify(SendAsync, Times.Never(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
                this.loggerMock.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true), It.IsAny<Exception?>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
            }

            // Case 2: Missing admin credentials returns Keycloak.SyncFailed error without HTTP calls
            var missingCredentialsOptions = Options.Create(new KeycloakOptions { BaseUrl = "http://localhost:8080/", Realm = "forge", AdminUsername = "admin", AdminPassword = " " });
            var missingCredentialsClient = new KeycloakAdminClient(new HttpClient(unconfiguredHandlerMock.Object), missingCredentialsOptions, this.loggerMock.Object);

            var missingCredentialsResult = await missingCredentialsClient.DeleteAllUsersAsync(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(missingCredentialsResult.IsError, Is.True);
                Assert.That(missingCredentialsResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
                unconfiguredHandlerMock.Protected().Verify(SendAsync, Times.Never(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            }

            // Case 3: Fetches existing user representation and sends PUT preserving createdTimestamp and username while updating { email, firstName, lastName }
            string? capturedUri = null;
            string? capturedAuth = null;
            string? capturedBody = null;

            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().Contains($"/users/{userId}")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent($"{{\"id\":\"{userId}\",\"username\":\"alex-rivera\",\"createdTimestamp\":1700000000000,\"enabled\":true,\"email\":\"old@example.com\",\"firstName\":\"Old\",\"lastName\":\"Name\"}}", Encoding.UTF8, "application/json")
            });

            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put && req.RequestUri != null && req.RequestUri.ToString().Contains($"/users/{userId}")), ItExpr.IsAny<CancellationToken>()).Returns(async (HttpRequestMessage req, CancellationToken ct) =>
            {
                capturedUri = req.RequestUri?.ToString();
                capturedAuth = req.Headers.Authorization?.ToString();
                capturedBody = req.Content != null ? await req.Content.ReadAsStringAsync(ct) : null;
                return new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent };
            });

            var successResult = await this.client.UpdateUserAsync(userId, "alex@example.com", "Alex", "Rivera", CancellationToken.None);

            using var jsonDocument = JsonDocument.Parse(capturedBody!);
            var root = jsonDocument.RootElement;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(successResult.IsError, Is.False);
                Assert.That(capturedUri, Is.EqualTo($"http://localhost:8080/admin/realms/forge/users/{userId}"));
                Assert.That(capturedAuth, Is.EqualTo("Bearer mock_token"));
                Assert.That(root.GetProperty("email").GetString(), Is.EqualTo("alex@example.com"));
                Assert.That(root.GetProperty("firstName").GetString(), Is.EqualTo("Alex"));
                Assert.That(root.GetProperty("lastName").GetString(), Is.EqualTo("Rivera"));
                Assert.That(root.GetProperty("username").GetString(), Is.EqualTo("alex-rivera"));
                Assert.That(root.GetProperty("createdTimestamp").GetInt64(), Is.EqualTo(1700000000000L));
            }

            // Case 4: Non-success HTTP status on GET returns Keycloak.SyncFailed error without PUT
            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().Contains($"/users/{userId}")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound });

            var getErrorResult = await this.client.UpdateUserAsync(userId, "alex@example.com", "Alex", "Rivera", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(getErrorResult.IsError, Is.True);
                Assert.That(getErrorResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
            }

            // Case 5: Non-success HTTP status on PUT returns Keycloak.SyncFailed error
            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().Contains($"/users/{userId}")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent($"{{\"id\":\"{userId}\",\"username\":\"alex-rivera\",\"createdTimestamp\":1700000000000}}", Encoding.UTF8, "application/json")
            });

            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest });

            var httpErrorResult = await this.client.UpdateUserAsync(userId, "alex@example.com", "Alex", "Rivera", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(httpErrorResult.IsError, Is.True);
                Assert.That(httpErrorResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
            }

            // Case 6: HttpRequestException returns Keycloak.SyncFailed error
            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put), ItExpr.IsAny<CancellationToken>()).ThrowsAsync(new HttpRequestException("Connection refused"));

            var exceptionResult = await this.client.UpdateUserAsync(userId, "alex@example.com", "Alex", "Rivera", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(exceptionResult.IsError, Is.True);
                Assert.That(exceptionResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
                Assert.That(exceptionResult.FirstError.Description, Does.Contain("Connection refused"));
            }

            // Case 7: Token endpoint failure returns Keycloak.SyncFailed error
            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.RequestUri.ToString().Contains("protocol/openid-connect/token")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized });

            var tokenErrorResult = await this.client.UpdateUserAsync(userId, "alex@example.com", "Alex", "Rivera", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(tokenErrorResult.IsError, Is.True);
                Assert.That(tokenErrorResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
            }

            // Case 8: Caller cancellation propagates OperationCanceledException without swallowing it
            using var cancellationTokenSource = new CancellationTokenSource();
            await cancellationTokenSource.CancelAsync();

            this.httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ThrowsAsync(new OperationCanceledException(cancellationTokenSource.Token));

            Assert.That(async () => await this.client.UpdateUserAsync(userId, "alex@example.com", "Alex", "Rivera", cancellationTokenSource.Token), Throws.InstanceOf<OperationCanceledException>());
        }
    }
}
