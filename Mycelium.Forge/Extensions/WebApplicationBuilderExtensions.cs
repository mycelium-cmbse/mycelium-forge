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
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.AccountSettings;
    using Mycelium.Forge.ViewModels.AdminAccounts;
    using Mycelium.Forge.ViewModels.ApiKeys;
    using Mycelium.Forge.ViewModels.Documentation;
    using Mycelium.Forge.ViewModels.Home;
    using Mycelium.Forge.ViewModels.Login;
    using Mycelium.Forge.ViewModels.MyPackages;
    using Mycelium.Forge.ViewModels.OrganizationDetails;
    using Mycelium.Forge.ViewModels.OrganizationSettings;
    using Mycelium.Forge.ViewModels.PackageDetails;
    using Mycelium.Forge.ViewModels.Packages;
    using Mycelium.Forge.ViewModels.PackageSettings;
    using Mycelium.Forge.ViewModels.Publish;
    using Mycelium.Forge.ViewModels.SignUp;
    using Mycelium.Forge.ViewModels.VerifyEmail;

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
            builder.Services.AddTransient<IDocumentationViewModel, DocumentationViewModel>();
            builder.Services.AddScoped<IThemeService, ThemeService>();
            builder.Services.AddScoped<IJsInterop, JsInterop>();

            return builder;
        }
    }
}
