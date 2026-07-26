// ------------------------------------------------------------------------------------------------
// <copyright file="WebInterfaceTestFixture.cs" company="Starion Group S.A.">
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
    /// Exercises the public Forge web interface through a real browser.
    /// </summary>
    /// <remarks>
    /// SSS-FG-REG-W9J requires the web interface to be reachable by unauthenticated users. These
    /// suites therefore run without any credential and assert that content renders, which also
    /// verifies that the pages are served as static server-side rendered markup.
    /// </remarks>
    [TestFixture]
    [Category("EndToEnd")]
    public class WebInterfaceTestFixture : PageTest
    {
        /// <summary>
        /// Points the browser context at the host under test.
        /// </summary>
        /// <returns>
        /// The browser context options used for every page in this fixture.
        /// </returns>
        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions
            {
                BaseURL = TestConfiguration.BaseUrl,
                IgnoreHTTPSErrors = true
            };
        }

        [Test]
        public async Task Verify_that_the_home_page_renders_for_an_anonymous_visitor()
        {
            await this.Page.GotoAsync("/");

            await Expect(this.Page.Locator("h1")).ToHaveTextAsync("Mycelium Forge");
        }

        [Test]
        public async Task Verify_that_an_unknown_address_renders_the_not_found_page()
        {
            await this.Page.GotoAsync("/no-such-address");

            await Expect(this.Page.Locator("h1")).ToHaveTextAsync("404");
        }

        [Test]
        public async Task Verify_that_the_search_form_is_available_without_authentication()
        {
            await this.Page.GotoAsync("/");

            await Expect(this.Page.Locator("input[name='q']")).ToBeVisibleAsync();
        }
    }
}
