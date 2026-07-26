// ------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge
{
    using Autofac.Extensions.DependencyInjection;

    using BlazorBlueprint.Components;

    using Carter;

    using Mycelium.Forge.Components;

    using OpenTelemetry.Resources;
    using OpenTelemetry.Trace;

    using Serilog;

    /// <summary>
    /// Provides the entry point for the Mycelium Forge package registry.
    /// </summary>
    /// <remarks>
    /// Forge exposes three surfaces over one backing store, per SSS 5.2.3.1: a public web interface,
    /// the Forge HTTP API, and a first-party client library that wraps that API. This host serves the
    /// first two. The web interface renders as static server-side rendering by default; components
    /// that genuinely require interactivity opt in individually with an explicit render mode.
    /// </remarks>
    /// <remarks>
    /// Declared non-static so that <c>WebApplicationFactory&lt;Program&gt;</c> can use it as the entry
    /// point marker for integration tests; a static class cannot be used as a type argument.
    /// </remarks>
    public class Program
    {
        /// <summary>
        /// The service name reported to the observability pipeline.
        /// </summary>
        private const string ServiceName = "Mycelium.Forge";

        /// <summary>
        /// Configures and starts the Mycelium Forge host.
        /// </summary>
        /// <param name="args">
        /// The command-line arguments provided when starting the application.
        /// </param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            // SSS-FB-OBS-S1A: every server log line is emitted as a structured JSON record.
            builder.Host.UseSerilog((context, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(new Serilog.Formatting.Compact.CompactJsonFormatter()));

            // SSS-FB-OBS-D2B: OpenTelemetry traces covering inbound HTTP requests.
            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(ServiceName))
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter());

            // SSS-FB-OBS-H4D: liveness and readiness probes for the orchestrator.
            builder.Services.AddHealthChecks();

            // The Forge HTTP API (SSS-FG-REG-A5E, D6F, Q7G, M8H) is routed through Carter modules.
            builder.Services.AddCarter();

            // Every component is statically server-rendered. No interactive render mode is
            // registered: InteractiveServer is ruled out by horizontal scaling, and no screen
            // requires a component runtime (docs/design.md DD-02, section 7.4).
            builder.Services.AddRazorComponents();

            builder.Services.AddBlazorBlueprintComponents();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error", true);
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

            // SSS-CC-EXT-FG1: the Forge HTTP API is served over HTTPS.
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();

            app.MapHealthChecks("/healthz");
            app.MapHealthChecks("/ready");

            app.MapCarter();

            app.MapRazorComponents<App>();

            app.Run();
        }
    }
}
