// ------------------------------------------------------------------------------------------------
// <copyright file="DistributedApplicationBuilderExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.AppHost.Extensions
{
    using System.Diagnostics.CodeAnalysis;

    using Aspire.Hosting;
    using Aspire.Hosting.ApplicationModel;

    /// <summary>
    /// Provides extension methods for <see cref="IDistributedApplicationBuilder" /> to register parameters.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DistributedApplicationBuilderExtensions
    {
        /// <param name="builder">The <see cref="IDistributedApplicationBuilder" /> to configure.</param>
        extension(IDistributedApplicationBuilder builder)
        {
            /// <summary>
            /// Registers a secret parameter that uses <paramref name="developmentDefault" /> in run mode and has no default value in
            /// publish mode.
            /// </summary>
            /// <param name="name">The parameter name.</param>
            /// <param name="developmentDefault">The default value used when running locally.</param>
            /// <returns>The configured <see cref="IResourceBuilder{ParameterResource}" /> instance.</returns>
            public IResourceBuilder<ParameterResource> AddSecretParameter(string name, string developmentDefault)
            {
                return builder.ExecutionContext.IsPublishMode 
                    ? builder.AddParameter(name, true) 
                    : builder.AddParameter(name, developmentDefault, secret: true);
            }
        }
    }
}
