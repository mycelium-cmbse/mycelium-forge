// ------------------------------------------------------------------------------------------------
// <copyright file="HealthEndpointTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Services;

    [TestFixture]
    public class HealthEndpointTestFixture
    {
        private Mock<IForgeContext> forgeContextMock;
        private WebApplicationFactory<Program> factory;

        [SetUp]
        public void SetUp()
        {
            this.forgeContextMock = new Mock<IForgeContext>();
            this.forgeContextMock.Setup(x => x.ForgeId).Returns(Guid.NewGuid());
            this.forgeContextMock.Setup(x => x.IsInitialized).Returns(true);

            this.factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => { builder.ConfigureTestServices(services => { services.AddSingleton(this.forgeContextMock.Object); }); });
        }

        [TearDown]
        public void TearDown()
        {
            this.factory.Dispose();
        }

        [Test]
        public async Task Verify_that_the_liveness_probe_reports_healthy()
        {
            var client = this.factory.CreateClient();

            this.forgeContextMock.Setup(x => x.IsInitialized).Returns(false);
            var uninitializedResponse = await client.GetAsync("/healthz");
            Assert.That(uninitializedResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            this.forgeContextMock.Setup(x => x.IsInitialized).Returns(true);
            var initializedResponse = await client.GetAsync("/healthz");
            Assert.That(initializedResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Verify_that_the_readiness_probe_reports_ready()
        {
            var client = this.factory.CreateClient();

            this.forgeContextMock.Setup(x => x.IsInitialized).Returns(false);
            var notReadyResponse = await client.GetAsync("/ready");
            Assert.That(notReadyResponse.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));

            this.forgeContextMock.Setup(x => x.IsInitialized).Returns(true);
            var readyResponse = await client.GetAsync("/ready");
            Assert.That(readyResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
