// ------------------------------------------------------------------------------------------------
// <copyright file="ForwardedHeadersTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;

    /// <summary>
    /// Verifies that a request forwarded from a TLS-terminating reverse proxy (GH111) is honoured
    /// instead of being redirect-looped back to itself.
    /// </summary>
    [TestFixture]
    public class ForwardedHeadersTestFixture
    {
        private WebApplicationFactory<Program> factory;

        [SetUp]
        public void SetUp()
        {
            // UseHttpsRedirection/UseHsts only run outside Development (Program.cs), so the
            // redirect behaviour this fixture is guarding against only exists in Production.
            // https_port is set explicitly because the in-memory test server has no HTTPS endpoint
            // for UseHttpsRedirection to infer one from - without it, it can't redirect at all.
            this.factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Production");
                    builder.UseSetting("https_port", "443");
                });
        }

        [TearDown]
        public void TearDown()
        {
            this.factory.Dispose();
        }

        [Test]
        public async Task Verify_that_a_request_with_X_Forwarded_Proto_https_is_not_redirected()
        {
            var client = this.factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            var request = new HttpRequestMessage(HttpMethod.Get, "/healthz");
            request.Headers.Add("X-Forwarded-Proto", "https");

            var response = await client.SendAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Verify_that_a_request_without_X_Forwarded_Proto_is_still_redirected_to_https()
        {
            var client = this.factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            var response = await client.GetAsync("/healthz");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.TemporaryRedirect));
                Assert.That(response.Headers.Location?.Scheme, Is.EqualTo("https"));
            }
        }
    }
}
