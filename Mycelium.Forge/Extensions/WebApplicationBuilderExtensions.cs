// ------------------------------------------------------------------------------------------------
// <copyright file="WebApplicationBuilderExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using Mycelium.Forge.ViewModels;

    /// <summary>
    /// Provides extension methods for <see cref="WebApplicationBuilder" /> to configure application dependencies.
    /// </summary>
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// Registers page view model dependencies in the application service collection.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder" /> to configure.</param>
        /// <returns>The configured <see cref="WebApplicationBuilder" /> instance.</returns>
        public static WebApplicationBuilder RegisterViewModels(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IHomeViewModel, HomeViewModel>();
            builder.Services.AddTransient<IPackagesViewModel, PackagesViewModel>();
            builder.Services.AddTransient<IOrganizationViewModel, OrganizationViewModel>();

            return builder;
        }
    }
}
