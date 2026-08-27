// ------------------------------------------------------------------------------------------------
// <copyright file="WebApplicationBuilderExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Extensions
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.JSInterop;

    using Moq;

    using Mycelium.Forge.Config;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.Home;

    /// <summary>
    /// Suite of tests for the <see cref="WebApplicationBuilderExtensions" /> class.
    /// </summary>
    [TestFixture]
    public class WebApplicationBuilderExtensionsTestFixture
    {
        private WebApplicationBuilder builder;

        /// <summary>
        /// Sets up the test context before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var options = new WebApplicationOptions
            {
                EnvironmentName = "Testing"
            };

            this.builder = WebApplication.CreateBuilder(options);
        }

        /// <summary>
        /// Verifies that <see cref="WebApplicationBuilderExtensions.RegisterDatabase" /> registers the database configuration.
        /// </summary>
        [Test]
        public void VerifyRegisterDatabase()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "DatabaseConnection:Host", "db.test.local" },
                { "DatabaseConnection:Port", "5432" },
                { "DatabaseConnection:Database", "test_forge" },
                { "DatabaseConnection:Username", "test_user" },
                { "DatabaseConnection:Password", "test_pass" }
            };

            this.builder.Configuration.AddInMemoryCollection(inMemorySettings!);

            this.builder.RegisterDatabase();

            using var app = this.builder.Build();
            var databaseConfig = app.Services.GetService<DatabaseConfig>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(databaseConfig, Is.Not.Null);
                Assert.That(databaseConfig!.Host, Is.EqualTo("db.test.local"));
                Assert.That(databaseConfig.Database, Is.EqualTo("test_forge"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="WebApplicationBuilderExtensions.RegisterViewModels" /> registers view models and services.
        /// </summary>
        [Test]
        public void VerifyRegisterViewModels()
        {
            this.builder.Services.AddSingleton(Mock.Of<IJSRuntime>());
            this.builder.RegisterViewModels();

            using var app = this.builder.Build();
            using var scope = app.Services.CreateScope();
            var homeViewModel = scope.ServiceProvider.GetService<IHomeViewModel>();
            var themeService = scope.ServiceProvider.GetService<IThemeService>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(homeViewModel, Is.Not.Null);
                Assert.That(themeService, Is.Not.Null);
            }
        }
    }
}
