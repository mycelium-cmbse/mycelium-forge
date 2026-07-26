// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeApiTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.EndToEndTests
{
    using System.Threading.Tasks;

    using Microsoft.Playwright;
    using Microsoft.Playwright.NUnit;

    /// <summary>
    /// Exercises the Forge HTTP API over the network.
    /// </summary>
    /// <remarks>
    /// Playwright's request context is used rather than a browser page so that the API is driven as
    /// a programmatic client would drive it. SSS-FG-REG-Y2L permits read access to public packages
    /// without authentication, so these suites send no credential.
    /// </remarks>
    [TestFixture]
    [Category("EndToEnd")]
    public class ForgeApiTestFixture : PlaywrightTest
    {
        private IAPIRequestContext request = null!;

        [SetUp]
        public async Task SetUp()
        {
            this.request = await this.Playwright.APIRequest.NewContextAsync(
                new APIRequestNewContextOptions
                {
                    BaseURL = TestConfiguration.BaseUrl,
                    IgnoreHTTPSErrors = true
                });
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.request.DisposeAsync();
        }

        [Test]
        public async Task Verify_that_the_liveness_probe_reports_healthy()
        {
            var response = await this.request.GetAsync("/healthz");

            Assert.That(response.Status, Is.EqualTo(200));
        }

        [Test]
        public async Task Verify_that_the_readiness_probe_reports_ready()
        {
            var response = await this.request.GetAsync("/ready");

            Assert.That(response.Status, Is.EqualTo(200));
        }

        [Test]
        public async Task Verify_that_the_search_endpoint_is_routed()
        {
            var response = await this.request.GetAsync("/api/v1/packages");

            // The handler is not implemented yet; this asserts the route reaches Carter rather
            // than falling through to the Blazor router and rendering the not-found page.
            Assert.That(response.Status, Is.EqualTo(501));
        }

        [Test]
        public async Task Verify_that_the_package_metadata_endpoint_is_routed()
        {
            var response = await this.request.GetAsync("/api/v1/packages/acme/thermal");

            Assert.That(response.Status, Is.EqualTo(501));
        }
    }
}
