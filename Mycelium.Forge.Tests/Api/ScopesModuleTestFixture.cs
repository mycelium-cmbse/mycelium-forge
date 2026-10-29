// ------------------------------------------------------------------------------------------------
// <copyright file="ScopesModuleTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Api
{
    using System;
    using System.Collections.Immutable;
    using System.Threading;
    using System.Threading.Tasks;

    using ErrorOr;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    using Moq;

    using Mycelium.Forge.Api;
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.Services;

    [TestFixture]
    public class ScopesModuleTestFixture
    {
        private const string ConfiguredSecret = "test-secret-key-123";

        private Mock<IScopeService> scopeServiceMock;

        [SetUp]
        public void SetUp()
        {
            this.scopeServiceMock = new Mock<IScopeService>();
        }

        [Test]
        public async Task VerifyScopeShortNameExistsAsync()
        {
            var validOptions = Options.Create(new KeycloakSpiOptions { Secret = ConfiguredSecret });
            var unconfiguredOptions = Options.Create(new KeycloakSpiOptions { Secret = string.Empty });
            var httpContext = new DefaultHttpContext();

            // Case 1: Unconfigured server secret returns 401
            var unconfiguredResult = await ScopesModule.ScopeShortNameExistsAsync("any-scope", httpContext.Request, unconfiguredOptions, this.scopeServiceMock.Object, CancellationToken.None);
            Assert.That((unconfiguredResult as IStatusCodeHttpResult)?.StatusCode, Is.EqualTo(StatusCodes.Status401Unauthorized));

            // Case 2: Missing secret header returns 401
            httpContext.Request.Headers.Clear();
            var missingKeyResult = await ScopesModule.ScopeShortNameExistsAsync("any-scope", httpContext.Request, validOptions, this.scopeServiceMock.Object, CancellationToken.None);
            Assert.That((missingKeyResult as IStatusCodeHttpResult)?.StatusCode, Is.EqualTo(StatusCodes.Status401Unauthorized));

            // Case 3: Invalid secret header returns 401
            httpContext.Request.Headers[KeycloakSpiOptions.SecretHeaderName] = "wrong-secret";
            var invalidKeyResult = await ScopesModule.ScopeShortNameExistsAsync("any-scope", httpContext.Request, validOptions, this.scopeServiceMock.Object, CancellationToken.None);
            Assert.That((invalidKeyResult as IStatusCodeHttpResult)?.StatusCode, Is.EqualTo(StatusCodes.Status401Unauthorized));

            // Case 4: Scope exists returns 200
            httpContext.Request.Headers[KeycloakSpiOptions.SecretHeaderName] = ConfiguredSecret;
            var existingScope = new Account { Id = Guid.NewGuid(), ShortName = "existing-scope" };
            this.scopeServiceMock.Setup(x => x.ReadByShortNameAsync(It.IsAny<IUserContext>(), It.IsAny<CancellationToken>(), It.IsAny<string[]>())).ReturnsAsync(ImmutableList.Create<IScope>(existingScope));
            var existsResult = await ScopesModule.ScopeShortNameExistsAsync("existing-scope", httpContext.Request, validOptions, this.scopeServiceMock.Object, CancellationToken.None);
            Assert.That((existsResult as IStatusCodeHttpResult)?.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

            // Case 5: Scope absent (empty list) returns 404
            this.scopeServiceMock.Setup(x => x.ReadByShortNameAsync(It.IsAny<IUserContext>(), It.IsAny<CancellationToken>(), It.IsAny<string[]>())).ReturnsAsync(ImmutableList<IScope>.Empty);
            var emptyResult = await ScopesModule.ScopeShortNameExistsAsync("missing-scope", httpContext.Request, validOptions, this.scopeServiceMock.Object, CancellationToken.None);
            Assert.That((emptyResult as IStatusCodeHttpResult)?.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));

            // Case 6: Scope service returns NotFound error returns 404
            this.scopeServiceMock.Setup(x => x.ReadByShortNameAsync(It.IsAny<IUserContext>(), It.IsAny<CancellationToken>(), It.IsAny<string[]>())).ReturnsAsync(Error.NotFound());
            var notFoundErrorResult = await ScopesModule.ScopeShortNameExistsAsync("missing-scope", httpContext.Request, validOptions, this.scopeServiceMock.Object, CancellationToken.None);
            Assert.That((notFoundErrorResult as IStatusCodeHttpResult)?.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));

            // Case 7: Scope service returns general error returns 500
            this.scopeServiceMock.Setup(x => x.ReadByShortNameAsync(It.IsAny<IUserContext>(), It.IsAny<CancellationToken>(), It.IsAny<string[]>())).ReturnsAsync(Error.Failure());
            var serverErrorResult = await ScopesModule.ScopeShortNameExistsAsync("error-scope", httpContext.Request, validOptions, this.scopeServiceMock.Object, CancellationToken.None);
            Assert.That((serverErrorResult as IStatusCodeHttpResult)?.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }
    }
}
