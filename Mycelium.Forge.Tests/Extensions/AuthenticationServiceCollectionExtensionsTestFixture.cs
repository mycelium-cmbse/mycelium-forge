// ------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationServiceCollectionExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Extensions
{
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    using ErrorOr;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using Microsoft.IdentityModel.Tokens;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Services;

    using AuthenticationServiceCollectionExtensions = Mycelium.Forge.Extensions.AuthenticationServiceCollectionExtensions;

    [TestFixture]
    public class AuthenticationServiceCollectionExtensionsTestFixture
    {
        private Mock<IAccountProvisioner> provisionerMock;

        [SetUp]
        public void SetUp()
        {
            this.provisionerMock = new Mock<IAccountProvisioner>();
            this.provisionerMock.Setup(x => x.ProvisionAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
        }

        [Test]
        public async Task VerifyOnTokenValidated()
        {
            var services = new ServiceCollection();
            services.AddSingleton(this.provisionerMock.Object);
            var serviceProvider = services.BuildServiceProvider();

            var scheme = new AuthenticationScheme(OpenIdConnectDefaults.AuthenticationScheme, null, typeof(OpenIdConnectHandler));
            var oidcOptions = new OpenIdConnectOptions();

            // Case 1: Invalid Principal (no ClaimsIdentity)
            var invalidPrincipalContext = new TokenValidatedContext(new DefaultHttpContext { RequestServices = serviceProvider }, scheme, oidcOptions, new ClaimsPrincipal(), new AuthenticationProperties());
            await AuthenticationServiceCollectionExtensions.OnTokenValidated(invalidPrincipalContext);
            Assert.That(invalidPrincipalContext.Result.Failure, Is.Not.Null);

            // Case 2: Success with valid realm roles
            var identity = new ClaimsIdentity("TestAuth");
            var validPrincipal = new ClaimsPrincipal(identity);
            var tokenContext = new TokenValidatedContext(new DefaultHttpContext { RequestServices = serviceProvider }, scheme, oidcOptions, validPrincipal, new AuthenticationProperties());

            const string payload = "{\"realm_access\":{\"roles\":[\"InstallationAdministrator\",\"Account\",\"UnknownRole\"]}}";
            tokenContext.TokenEndpointResponse = new OpenIdConnectMessage { AccessToken = CreateTestJwt(payload) };

            await AuthenticationServiceCollectionExtensions.OnTokenValidated(tokenContext);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(identity.HasClaim(ClaimTypes.Role, nameof(RoleKind.InstallationAdministrator)), Is.True);
                Assert.That(identity.HasClaim(ClaimTypes.Role, nameof(RoleKind.Account)), Is.True);
                Assert.That(identity.HasClaim(ClaimTypes.Role, "UnknownRole"), Is.False);
            }

            // Case 3: Empty access token
            var emptyTokenIdentity = new ClaimsIdentity("TestAuth");

            var emptyTokenContext = new TokenValidatedContext(new DefaultHttpContext { RequestServices = serviceProvider }, scheme, oidcOptions, new ClaimsPrincipal(emptyTokenIdentity), new AuthenticationProperties())
            {
                TokenEndpointResponse = new OpenIdConnectMessage { AccessToken = string.Empty }
            };

            await AuthenticationServiceCollectionExtensions.OnTokenValidated(emptyTokenContext);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyTokenIdentity.Claims, Is.Empty);
                Assert.That(emptyTokenContext.Result, Is.Null);
            }

            // Case 4: Missing realm_access in access token
            var noRolesIdentity = new ClaimsIdentity("TestAuth");

            var noRolesContext = new TokenValidatedContext(new DefaultHttpContext { RequestServices = serviceProvider }, scheme, oidcOptions, new ClaimsPrincipal(noRolesIdentity), new AuthenticationProperties())
            {
                TokenEndpointResponse = new OpenIdConnectMessage { AccessToken = CreateTestJwt("{\"sub\":\"123\"}") }
            };

            await AuthenticationServiceCollectionExtensions.OnTokenValidated(noRolesContext);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(noRolesIdentity.Claims, Is.Empty);
                Assert.That(noRolesContext.Result, Is.Null);
            }

            // Case 5: Non-array roles in access token
            var nonArrayRolesIdentity = new ClaimsIdentity("TestAuth");

            var nonArrayRolesContext = new TokenValidatedContext(new DefaultHttpContext { RequestServices = serviceProvider }, scheme, oidcOptions, new ClaimsPrincipal(nonArrayRolesIdentity), new AuthenticationProperties())
            {
                TokenEndpointResponse = new OpenIdConnectMessage { AccessToken = CreateTestJwt("{\"realm_access\":{\"roles\":\"not-an-array\"}}") }
            };

            await AuthenticationServiceCollectionExtensions.OnTokenValidated(nonArrayRolesContext);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nonArrayRolesIdentity.Claims, Is.Empty);
                Assert.That(nonArrayRolesContext.Result, Is.Null);
            }

            // Case 6: Provisioning Error
            var failingIdentity = new ClaimsIdentity("TestAuth");
            var failingTokenContext = new TokenValidatedContext(new DefaultHttpContext { RequestServices = serviceProvider }, scheme, oidcOptions, new ClaimsPrincipal(failingIdentity), new AuthenticationProperties());

            this.provisionerMock
                .Setup(x => x.ProvisionAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Error.Failure("provision.error", "Failed to provision"));

            await AuthenticationServiceCollectionExtensions.OnTokenValidated(failingTokenContext);
            Assert.That(failingTokenContext.Result.Failure?.Message, Does.Contain("Failed to provision"));
        }

        private static string CreateTestJwt(string payloadJson)
        {
            var header = Base64UrlEncoder.Encode("{\"alg\":\"none\",\"typ\":\"JWT\"}");
            var payload = Base64UrlEncoder.Encode(payloadJson);
            return $"{header}.{payload}.";
        }
    }
}
