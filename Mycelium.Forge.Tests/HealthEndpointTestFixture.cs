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
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;

    /// <summary>
    /// Verifies that the host boots and serves the orchestrator probes required by SSS-FB-OBS-H4D.
    /// </summary>
    [TestFixture]
    public class HealthEndpointTestFixture
    {
        private WebApplicationFactory<Program> factory;

        [SetUp]
        public void SetUp()
        {
            this.factory = new WebApplicationFactory<Program>();
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

            var response = await client.GetAsync("/healthz");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Verify_that_the_readiness_probe_reports_ready()
        {
            var client = this.factory.CreateClient();

            var response = await client.GetAsync("/ready");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
