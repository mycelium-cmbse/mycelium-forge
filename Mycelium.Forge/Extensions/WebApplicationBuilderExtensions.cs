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
            builder.Services.AddTransient<IOrganizationDetailsViewModel, OrganizationDetailsViewModel>();
            builder.Services.AddTransient<IPackageDetailsViewModel, PackageDetailsViewModel>();
            builder.Services.AddTransient<IPublishViewModel, PublishViewModel>();
            builder.Services.AddTransient<IMyPackagesViewModel, MyPackagesViewModel>();
            builder.Services.AddTransient<IApiKeysViewModel, ApiKeysViewModel>();
            builder.Services.AddTransient<IPackageSettingsViewModel, PackageSettingsViewModel>();
            builder.Services.AddTransient<IAccountSettingsViewModel, AccountSettingsViewModel>();
            builder.Services.AddTransient<IOrganizationSettingsViewModel, OrganizationSettingsViewModel>();
            builder.Services.AddTransient<IAdminAccountsViewModel, AdminAccountsViewModel>();
            builder.Services.AddTransient<ISignUpViewModel, SignUpViewModel>();
            builder.Services.AddTransient<ILoginViewModel, LoginViewModel>();
            builder.Services.AddTransient<IVerifyEmailViewModel, VerifyEmailViewModel>();

            return builder;
        }
    }
}
