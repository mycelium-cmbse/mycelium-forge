// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakUserSeederTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Data
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;
    using Moq.Protected;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Data;

    [TestFixture]
    public class KeycloakUserSeederTestFixture
    {
        private const string SendAsync = "SendAsync";

        private Mock<HttpMessageHandler> httpMessageHandlerMock;
        private Mock<ILogger<KeycloakUserSeeder>> loggerMock;
        private IOptions<KeycloakOptions> options;
        private KeycloakUserSeeder seeder;

        [SetUp]
        public void SetUp()
        {
            this.httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            this.loggerMock = new Mock<ILogger<KeycloakUserSeeder>>();

            this.options = Options.Create(new KeycloakOptions
            {
                BaseUrl = "http://localhost:8080/",
                Realm = "forge",
                AdminUsername = "admin",
                AdminPassword = "adminpassword",
                SeedUserPassword = "userpassword"
            });

            this.httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.RequestUri.ToString().Contains("protocol/openid-connect/token")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"access_token\": \"mock_token\"}", Encoding.UTF8, "application/json")
                });

            this.httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().Contains("/users")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{\"id\": \"user-1\"}]", Encoding.UTF8, "application/json")
                });

            this.httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri != null && req.RequestUri.ToString().Contains("/users/user-1")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent });

            this.httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.RequestUri.ToString().Contains("/partialImport")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            this.httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().Contains("/roles/")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"id\": \"role-id\", \"name\": \"Role\"}", Encoding.UTF8, "application/json")
                });

            this.httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.RequestUri.ToString().Contains("/role-mappings/realm")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent });

            var httpClient = new HttpClient(this.httpMessageHandlerMock.Object) { BaseAddress = new Uri(this.options.Value.BaseUrl) };
            this.seeder = new KeycloakUserSeeder(httpClient, this.options, this.loggerMock.Object);
        }

        [Test]
        public async Task VerifySeedAsync()
        {
            // Case 1: Keycloak successfully seeds admin token, cleans users, imports realm, and maps roles (success)
            var seedSuccess = await this.seeder.SeedAsync();
            Assert.That(seedSuccess, Is.True);

            // Case 2: Network failure during token acquisition (fail)
            this.httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.RequestUri.ToString().Contains("protocol/openid-connect/token")), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Connection refused"));

            var failure = await this.seeder.SeedAsync();
            Assert.That(failure, Is.False);
        }
    }
}
