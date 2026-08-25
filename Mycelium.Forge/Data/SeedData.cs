// ------------------------------------------------------------------------------------------------
// <copyright file="SeedData.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Data
{
    using System.Diagnostics.CodeAnalysis;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models.Admin;
    using Mycelium.Forge.Models.Organization;
    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Provides centralized seed data and mock models for registry entities.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SeedData
    {
        /// <summary>
        /// Constant label representing verified account status.
        /// </summary>
        private const string VerifiedStatus = "Verified";

        /// <summary>
        /// Constant representing the Starion publisher scope prefix.
        /// </summary>
        private const string StarionScope = "@starion";

        /// <summary>
        /// Constant representing the SysML v2 format name.
        /// </summary>
        private const string SysmlV2Format = "SysML v2";

        /// <summary>
        /// Constant representing version 1.0.0.
        /// </summary>
        private const string Version100 = "v1.0.0";

        static SeedData()
        {
            RegisAccount = CreateAccount(
                "a1111111-1111-1111-1111-111111111111",
                "R. André",
                "r.andre",
                "regis.andre@starion.eu",
                new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                "stariongroup.eu",
                "Toulouse, France");

            StefanAccount = CreateAccount(
                "a2222222-2222-2222-2222-222222222222",
                "S. Kramer",
                "s.kramer",
                "stefan.kramer@starion.eu",
                new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc));

            KleinAccount = CreateAccount(
                "a3333333-3333-3333-3333-333333333333",
                "J. Klein",
                "j.klein",
                "j.klein@esa.int",
                new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc));

            BlancAccount = CreateAccount(
                "a4444444-4444-4444-4444-444444444444",
                "M. Blanc",
                "m.blanc",
                "m.blanc@starion.eu",
                new DateTime(2025, 4, 5, 0, 0, 0, DateTimeKind.Utc));

            NovakAccount = CreateAccount(
                "a5555555-5555-5555-5555-555555555555",
                "A. Novak",
                "a.novak",
                "a.novak@esa.int",
                new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc));

            StarionOrganization = CreateOrganization(
                "b1111111-1111-1111-1111-111111111111",
                "Starion Group",
                "starion",
                "Systems engineering models and ECSS mission libraries for early-phase spacecraft design.",
                new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            EsaOrganization = CreateOrganization(
                "b2222222-2222-2222-2222-222222222222",
                "European Space Agency",
                "esa",
                "European Space Agency engineering libraries and flight dynamics models.",
                new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            OmgOrganization = CreateOrganization(
                "b3333333-3333-3333-3333-333333333333",
                "Object Management Group",
                "omg",
                "Official SysML v2 and KerML specification standard libraries.",
                new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            StarionOrganizationModel = new OrganizationModel(
                StarionOrganization,
                "SG",
                StarionOrganization.Origin,
                true,
                6,
                14,
                390);

            EsaOrganizationModel = new OrganizationModel(
                EsaOrganization,
                "ES",
                EsaOrganization.Origin,
                true,
                4,
                8,
                210);

            StarionMembers =
            [
                new OrganizationMemberModel(RegisAccount, OrganizationInvitationKind.ADMINISTRATOR),
                new OrganizationMemberModel(StefanAccount, OrganizationInvitationKind.ADMINISTRATOR),
                new OrganizationMemberModel(KleinAccount),
                new OrganizationMemberModel(BlancAccount)
            ];

            RegisOrganizationMemberships =
            [
                new AccountOrganizationMembershipModel(StarionOrganization, OrganizationInvitationKind.ADMINISTRATOR),
                new AccountOrganizationMembershipModel(EsaOrganization, OrganizationInvitationKind.ADMINISTRATOR)
            ];

            AdminAccounts =
            [
                new AdminAccountModel(RegisAccount, true, VerifiedStatus, "@starion (admin), @esa (admin)"),
                new AdminAccountModel(StefanAccount, false, VerifiedStatus, "@starion (admin)"),
                new AdminAccountModel(KleinAccount, false, VerifiedStatus, "@starion (publisher)"),
                new AdminAccountModel(BlancAccount, false, "Pending", "@starion (member)"),
                new AdminAccountModel(NovakAccount, false, VerifiedStatus, "@esa (member)")
            ];

            ApiKeys =
            [
                CreateApiKey("c1111111-1111-1111-1111-111111111111", "ci-publish", new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc), 3),
                CreateApiKey("c2222222-2222-2222-2222-222222222222", "release-bot", new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc), 14),
                CreateApiKey("c3333333-3333-3333-3333-333333333333", "local-dev", new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc), 30)
            ];

            StandardLibraryPackages =
            [
                SysmlIsqQuantitiesPackageModel,
                SysmlKernelLibraryPackageModel,
                EcssEnvironmentPackageModel
            ];

            RecentlyUpdatedPackages =
            [
                EcssPowerSubsystemPackageModel,
                SmallSatPlatformPackageModel,
                EcssRfCommsPackageModel
            ];

            MostUsedPackages =
            [
                SysmlKernelLibraryPackageModel,
                SysmlIsqQuantitiesPackageModel,
                EcssEnvironmentPackageModel
            ];

            ModelsFromOtherMbseTools =
            [
                Cdp4CometCorePackageModel,
                CapellaSystemTemplatePackageModel,
                EcssMechanicalPackageModel
            ];

            MyPackages =
            [
                CreateMyPackage(EcssPowerSubsystemPackageModel.Package, StarionScope, "v1.2.0", SysmlV2Format, "ECSS mission model: Power subsystem.", "210"),
                CreateMyPackage(SmallSatPlatformPackageModel.Package, StarionScope, "v0.8.2", SysmlV2Format, "Parametric smallsat platform model.", "145"),
                CreateMyPackage(EcssRfCommsPackageModel.Package, StarionScope, "v0.3.0", SysmlV2Format, "RF telecommunication link budget.", "190"),
                CreateMyPackage(EcssMechanicalPackageModel.Package, StarionScope, Version100, SysmlV2Format, "Structural and mechanical engineering domain metamodels.", "165"),
                CreateMyPackage(Cdp4CometCorePackageModel.Package, StarionScope, "v10.25.1", "CDP4-COMET", "Core concurrent engineering data definitions.", "320", PackageInvitationKind.MAINTAINER),
                CreateMyPackage(EcssEnvironmentPackageModel.Package, "@esa", Version100, SysmlV2Format, "Space environment definitions and planetary constants.", "860", PackageInvitationKind.MAINTAINER)
            ];

            CatalogPackages =
            [
                EcssPowerSubsystemPackageModel,
                SysmlIsqQuantitiesPackageModel,
                SysmlKernelLibraryPackageModel,
                EcssEnvironmentPackageModel,
                SmallSatPlatformPackageModel,
                Cdp4CometCorePackageModel
            ];
        }

        /// <summary>
        /// Gets the primary user account for Regis André.
        /// </summary>
        public static Account RegisAccount { get; }

        /// <summary>
        /// Gets the user account for Stefan Kramer.
        /// </summary>
        public static Account StefanAccount { get; }

        /// <summary>
        /// Gets the user account for J. Klein.
        /// </summary>
        public static Account KleinAccount { get; }

        /// <summary>
        /// Gets the user account for M. Blanc.
        /// </summary>
        public static Account BlancAccount { get; }

        /// <summary>
        /// Gets the user account for A. Novak.
        /// </summary>
        public static Account NovakAccount { get; }

        /// <summary>
        /// Gets the Starion Group organization.
        /// </summary>
        public static Organization StarionOrganization { get; }

        /// <summary>
        /// Gets the European Space Agency organization.
        /// </summary>
        public static Organization EsaOrganization { get; }

        /// <summary>
        /// Gets the Object Management Group organization.
        /// </summary>
        public static Organization OmgOrganization { get; }

        /// <summary>
        /// Gets the presentation model for Starion Group.
        /// </summary>
        public static OrganizationModel StarionOrganizationModel { get; }

        /// <summary>
        /// Gets the presentation model for European Space Agency.
        /// </summary>
        public static OrganizationModel EsaOrganizationModel { get; }

        /// <summary>
        /// Gets the list of members for Starion Group.
        /// </summary>
        public static IReadOnlyList<OrganizationMemberModel> StarionMembers { get; }

        /// <summary>
        /// Gets the organization memberships for the current user.
        /// </summary>
        public static IReadOnlyList<AccountOrganizationMembershipModel> RegisOrganizationMemberships { get; }

        /// <summary>
        /// Gets the list of administrator accounts for installation management.
        /// </summary>
        public static IReadOnlyList<AdminAccountModel> AdminAccounts { get; }

        /// <summary>
        /// Gets the master list of API keys.
        /// </summary>
        public static IReadOnlyList<APIKey> ApiKeys { get; }

        /// <summary>
        /// Gets the list of standard library packages for the home catalog.
        /// </summary>
        public static IReadOnlyList<PackageModel> StandardLibraryPackages { get; }

        /// <summary>
        /// Gets the list of recently updated packages for the home catalog.
        /// </summary>
        public static IReadOnlyList<PackageModel> RecentlyUpdatedPackages { get; }

        /// <summary>
        /// Gets the list of most used packages for the home catalog.
        /// </summary>
        public static IReadOnlyList<PackageModel> MostUsedPackages { get; }

        /// <summary>
        /// Gets the list of packages from other MBSE tools for the home catalog.
        /// </summary>
        public static IReadOnlyList<PackageModel> ModelsFromOtherMbseTools { get; }

        /// <summary>
        /// Gets the list of packages owned or maintained by the current user.
        /// </summary>
        public static IReadOnlyList<PackageModel> MyPackages { get; }

        /// <summary>
        /// Gets the list of package discovery and catalog search result items.
        /// </summary>
        public static IReadOnlyList<PackageModel> CatalogPackages { get; }

        /// <summary>
        /// Gets the mock model for the SysMLv2-ISQ-Quantities package.
        /// </summary>
        public static PackageModel SysmlIsqQuantitiesPackageModel { get; } = CreateVerifiedPackage(
            CreatePackage("SysMLv2-ISQ-Quantities", "sysmlv2-isq-quantities", 30),
            "@omg",
            "v2025.2",
            SysmlV2Format,
            "Standard quantities and units definition package for SysML v2 models based on ISO/IEC 80000.",
            "standard-library · units · quantities · isq",
            "1.4k");

        /// <summary>
        /// Gets the mock model for the SysMLv2-Kernel-Library package.
        /// </summary>
        public static PackageModel SysmlKernelLibraryPackageModel { get; } = CreateVerifiedPackage(
            CreatePackage("SysMLv2-Kernel-Library", "sysmlv2-kernel-library", 30),
            "@omg",
            "v2025.2",
            SysmlV2Format,
            "Fundamental KerML metamodel library containing base types, collections, and control functions.",
            "standard-library · kerml · kernel",
            "2.1k");

        /// <summary>
        /// Gets the mock model for the ECSS-E-ST-10-04C package.
        /// </summary>
        public static PackageModel EcssEnvironmentPackageModel { get; } = CreateVerifiedPackage(
            CreatePackage("ECSS-E-ST-10-04C", "ecss-e-st-10-04c", 60),
            "@esa",
            Version100,
            SysmlV2Format,
            "Space environment definitions and planetary constants for mission analysis and spacecraft design.",
            "standard-library · space-environment · ecss",
            "860");

        /// <summary>
        /// Gets the mock model for the ECSS-MM-PWR package.
        /// </summary>
        public static PackageModel EcssPowerSubsystemPackageModel { get; } = CreateVerifiedPackage(
            CreatePackage("ECSS-MM-PWR", "ecss-mm-pwr", 14),
            StarionScope,
            "v1.2.0",
            SysmlV2Format,
            "ECSS mission model: Power subsystem. Part definitions for power bus, battery, solar array, and PCU.",
            "mission-model · power · ecss",
            "210");

        /// <summary>
        /// Gets the mock model for the SmallSat-Platform-Model package.
        /// </summary>
        public static PackageModel SmallSatPlatformPackageModel { get; } = CreateVerifiedPackage(
            CreatePackage("SmallSat-Platform-Model", "smallsat-platform-model", 21),
            StarionScope,
            "v0.8.2",
            SysmlV2Format,
            "Parametric smallsat platform model including propulsion and telemetry budget templates.",
            "mission-model · smallsat · platform",
            "145");

        /// <summary>
        /// Gets the mock model for the ecss-e-st-32-10c package.
        /// </summary>
        public static PackageModel EcssRfCommsPackageModel { get; } = CreateVerifiedPackage(
            CreatePackage("ecss-e-st-32-10c", "ecss-e-st-32-10c", 60),
            StarionScope,
            "v0.3.0",
            SysmlV2Format,
            "RF telecommunication link budget and space communication interfaces.",
            "comms · rf · telemetry · ecss",
            "190");

        /// <summary>
        /// Gets the mock model for the CDP4-COMET-Core package.
        /// </summary>
        public static PackageModel Cdp4CometCorePackageModel { get; } = CreateVerifiedPackage(
            CreatePackage("CDP4-COMET-Core", "cdp4-comet-core", 30),
            StarionScope,
            "v10.25.1",
            "CDP4-COMET",
            "Core concurrent engineering data definitions and iteration exchange schemas for ECSS-E-TM-10-25.",
            "concurrent-design · cdp4 · ecss-10-25",
            "320");

        /// <summary>
        /// Gets the mock model for the Capella-System-Template package.
        /// </summary>
        public static PackageModel CapellaSystemTemplatePackageModel { get; } = CreateVerifiedPackage(
            CreatePackage("Capella-System-Template", "capella-system-template", 90),
            "@esa",
            "v6.1.0",
            "Capella",
            "Arcadia methodology operational analysis and system architecture template for space instruments.",
            "arcadia · capella · operational-analysis",
            "185");

        /// <summary>
        /// Gets the mock model for the ecss-e-st-31-01c package.
        /// </summary>
        public static PackageModel EcssMechanicalPackageModel { get; } = CreateVerifiedPackage(
            CreatePackage("ecss-e-st-31-01c", "ecss-e-st-31-01c", 90),
            StarionScope,
            Version100,
            SysmlV2Format,
            "Structural and mechanical engineering domain metamodels and loads analysis.",
            "mechanical · structures · loads · ecss",
            "165");

        /// <summary>
        /// Creates an active account entity.
        /// </summary>
        /// <param name="guid">The unique identifier string.</param>
        /// <param name="name">The display name.</param>
        /// <param name="shortName">The short name.</param>
        /// <param name="email">The email address.</param>
        /// <param name="createdAt">The creation date.</param>
        /// <param name="website">The optional website.</param>
        /// <param name="origin">The optional origin.</param>
        /// <returns>A new <see cref="Account" />.</returns>
        private static Account CreateAccount(
            string guid,
            string name,
            string shortName,
            string email,
            DateTime createdAt,
            string website = "",
            string origin = "")
        {
            return new Account
            {
                Id = Guid.Parse(guid),
                Name = name,
                ShortName = shortName,
                Email = email,
                Website = website,
                Origin = origin,
                Status = ScopeStatusKind.ACTIVE,
                CreatedAt = createdAt
            };
        }

        /// <summary>
        /// Creates an active organization entity.
        /// </summary>
        /// <param name="guid">The unique identifier string.</param>
        /// <param name="name">The organization name.</param>
        /// <param name="shortName">The short name.</param>
        /// <param name="origin">The origin or mission description.</param>
        /// <param name="createdAt">The creation date.</param>
        /// <returns>A new <see cref="Organization" />.</returns>
        private static Organization CreateOrganization(string guid, string name, string shortName, string origin, DateTime createdAt)
        {
            return new Organization
            {
                Id = Guid.Parse(guid),
                Name = name,
                ShortName = shortName,
                Origin = origin,
                Status = ScopeStatusKind.ACTIVE,
                CreatedAt = createdAt
            };
        }

        /// <summary>
        /// Creates an API key entity with specified properties.
        /// </summary>
        /// <param name="guid">The unique identifier string.</param>
        /// <param name="name">The key name.</param>
        /// <param name="createdAt">The creation date.</param>
        /// <param name="expiresAt">The expiration date.</param>
        /// <param name="lastUsedDaysAgo">The number of days since last use.</param>
        /// <returns>A new <see cref="APIKey" />.</returns>
        private static APIKey CreateApiKey(string guid, string name, DateTime createdAt, DateTime expiresAt, int lastUsedDaysAgo)
        {
            return new APIKey
            {
                Id = Guid.Parse(guid),
                Name = name,
                CreatedAt = createdAt,
                ExpiresAt = expiresAt,
                LastUsedAt = DateTime.UtcNow.AddDays(-lastUsedDaysAgo)
            };
        }

        /// <summary>
        /// Creates a public package entity with standard metadata.
        /// </summary>
        /// <param name="name">The name of the package.</param>
        /// <param name="shortName">The short URL-friendly name.</param>
        /// <param name="daysAgo">The creation age in days.</param>
        /// <returns>A new <see cref="Package" />.</returns>
        private static Package CreatePackage(string name, string shortName, int daysAgo)
        {
            return new Package
            {
                Name = name,
                ShortName = shortName,
                Visibility = VisibilityKind.PUBLIC,
                CreatedAt = DateTime.UtcNow.AddDays(-daysAgo)
            };
        }

        /// <summary>
        /// Creates a verified package model with standard metadata.
        /// </summary>
        /// <param name="package">The underlying package entity.</param>
        /// <param name="scope">The publisher scope prefix.</param>
        /// <param name="version">The package version string.</param>
        /// <param name="format">The format descriptor.</param>
        /// <param name="description">The summary description.</param>
        /// <param name="tags">The associated tags.</param>
        /// <param name="importCount">The import count display value.</param>
        /// <returns>A new verified <see cref="PackageModel" />.</returns>
        private static PackageModel CreateVerifiedPackage(
            IPackage package,
            string scope,
            string version,
            string format,
            string description,
            string tags,
            string importCount)
        {
            return new PackageModel(
                package,
                scope,
                version,
                format,
                description,
                tags,
                importCount)
            {
                IsVerified = true
            };
        }

        /// <summary>
        /// Creates a user package model with the specified role and import count.
        /// </summary>
        /// <param name="package">The underlying package entity.</param>
        /// <param name="scope">The publisher scope prefix.</param>
        /// <param name="version">The package version string.</param>
        /// <param name="format">The format descriptor.</param>
        /// <param name="description">The summary description.</param>
        /// <param name="importCount">The import count display value.</param>
        /// <param name="role">The user role for the package.</param>
        /// <returns>A new <see cref="PackageModel" />.</returns>
        private static PackageModel CreateMyPackage(
            IPackage package,
            string scope,
            string version,
            string format,
            string description,
            string importCount,
            PackageInvitationKind role = PackageInvitationKind.OWNER)
        {
            return new PackageModel(package, scope, version, format, description, importCount: importCount)
            {
                IsVerified = true,
                Role = role
            };
        }
    }
}
