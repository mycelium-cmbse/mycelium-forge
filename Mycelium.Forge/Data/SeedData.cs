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
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models;
    using Mycelium.Forge.ViewModels;

    /// <summary>
    /// Provides centralized seed data and mock models for registry entities.
    /// </summary>
    public static class SeedData
    {
        static SeedData()
        {
            RegisAccount = new Account
            {
                Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                Name = "R. André",
                ShortName = "r.andre",
                Email = "regis.andre@starion.eu",
                Website = "stariongroup.eu",
                Origin = "Toulouse, France",
                Status = ScopeStatusKind.ACTIVE,
                CreatedAt = new DateTime(2025, 1, 15)
            };

            StefanAccount = new Account
            {
                Id = Guid.Parse("a2222222-2222-2222-2222-222222222222"),
                Name = "S. Kramer",
                ShortName = "s.kramer",
                Email = "stefan.kramer@starion.eu",
                Status = ScopeStatusKind.ACTIVE,
                CreatedAt = new DateTime(2025, 2, 1)
            };

            KleinAccount = new Account
            {
                Id = Guid.Parse("a3333333-3333-3333-3333-333333333333"),
                Name = "J. Klein",
                ShortName = "j.klein",
                Email = "j.klein@esa.int",
                Status = ScopeStatusKind.ACTIVE,
                CreatedAt = new DateTime(2025, 3, 10)
            };

            BlancAccount = new Account
            {
                Id = Guid.Parse("a4444444-4444-4444-4444-444444444444"),
                Name = "M. Blanc",
                ShortName = "m.blanc",
                Email = "m.blanc@starion.eu",
                Status = ScopeStatusKind.ACTIVE,
                CreatedAt = new DateTime(2025, 4, 5)
            };

            NovakAccount = new Account
            {
                Id = Guid.Parse("a5555555-5555-5555-5555-555555555555"),
                Name = "A. Novak",
                ShortName = "a.novak",
                Email = "a.novak@esa.int",
                Status = ScopeStatusKind.ACTIVE,
                CreatedAt = new DateTime(2025, 5, 20)
            };

            StarionOrganization = new Organization
            {
                Id = Guid.Parse("b1111111-1111-1111-1111-111111111111"),
                Name = "Starion Group",
                ShortName = "starion",
                Origin = "Systems engineering models and ECSS mission libraries for early-phase spacecraft design.",
                Status = ScopeStatusKind.ACTIVE,
                CreatedAt = new DateTime(2025, 1, 1)
            };

            EsaOrganization = new Organization
            {
                Id = Guid.Parse("b2222222-2222-2222-2222-222222222222"),
                Name = "European Space Agency",
                ShortName = "esa",
                Origin = "European Space Agency engineering libraries and flight dynamics models.",
                Status = ScopeStatusKind.ACTIVE,
                CreatedAt = new DateTime(2025, 1, 1)
            };

            OmgOrganization = new Organization
            {
                Id = Guid.Parse("b3333333-3333-3333-3333-333333333333"),
                Name = "Object Management Group",
                ShortName = "omg",
                Origin = "Official SysML v2 and KerML specification standard libraries.",
                Status = ScopeStatusKind.ACTIVE,
                CreatedAt = new DateTime(2025, 1, 1)
            };

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
                new OrganizationMemberModel(KleinAccount, OrganizationInvitationKind.MEMBER),
                new OrganizationMemberModel(BlancAccount, OrganizationInvitationKind.MEMBER)
            ];

            RegisOrganizationMemberships =
            [
                new AccountOrganizationMembershipModel(StarionOrganization, OrganizationInvitationKind.ADMINISTRATOR),
                new AccountOrganizationMembershipModel(EsaOrganization, OrganizationInvitationKind.ADMINISTRATOR)
            ];

            AdminAccounts =
            [
                new AdminAccountModel(RegisAccount, true, "Verified", "@starion (admin), @esa (admin)", ScopeStatusKind.ACTIVE),
                new AdminAccountModel(StefanAccount, false, "Verified", "@starion (admin)", ScopeStatusKind.ACTIVE),
                new AdminAccountModel(KleinAccount, false, "Verified", "@starion (publisher)", ScopeStatusKind.ACTIVE),
                new AdminAccountModel(BlancAccount, false, "Pending", "@starion (member)", ScopeStatusKind.ACTIVE),
                new AdminAccountModel(NovakAccount, false, "Verified", "@esa (member)", ScopeStatusKind.ACTIVE)
            ];

            ApiKeys =
            [
                new APIKey
                {
                    Id = Guid.Parse("c1111111-1111-1111-1111-111111111111"),
                    Name = "ci-publish",
                    CreatedAt = new DateTime(2026, 1, 1),
                    ExpiresAt = new DateTime(2026, 7, 1),
                    LastUsedAt = DateTime.UtcNow.AddDays(-3)
                },
                new APIKey
                {
                    Id = Guid.Parse("c2222222-2222-2222-2222-222222222222"),
                    Name = "release-bot",
                    CreatedAt = new DateTime(2025, 12, 1),
                    ExpiresAt = new DateTime(2026, 6, 1),
                    LastUsedAt = DateTime.UtcNow.AddDays(-14)
                },
                new APIKey
                {
                    Id = Guid.Parse("c3333333-3333-3333-3333-333333333333"),
                    Name = "local-dev",
                    CreatedAt = new DateTime(2025, 11, 1),
                    ExpiresAt = new DateTime(2026, 4, 1),
                    LastUsedAt = DateTime.UtcNow.AddDays(-30)
                }
            ];

            StandardLibraryPackages =
            [
                new PackageModel(
                    new Package { Name = "SysMLv2-ISQ-Quantities", ShortName = "sysmlv2-isq-quantities", Visibility = VisibilityKind.PUBLIC },
                    "@omg",
                    "v2025.2",
                    "SysML v2",
                    "Standard quantities and units definition package for SysML v2 models based on ISO/IEC 80000.",
                    "standard-library · units · quantities · isq",
                    "1.4k",
                    true,
                    "1 month ago"),
                new PackageModel(
                    new Package { Name = "SysMLv2-Kernel-Library", ShortName = "sysmlv2-kernel-library", Visibility = VisibilityKind.PUBLIC },
                    "@omg",
                    "v2025.2",
                    "SysML v2",
                    "Fundamental KerML metamodel library containing base types, collections, and control functions.",
                    "standard-library · kerml · kernel",
                    "2.1k",
                    true,
                    "1 month ago"),
                new PackageModel(
                    new Package { Name = "ECSS-E-ST-10-04C", ShortName = "ecss-e-st-10-04c", Visibility = VisibilityKind.PUBLIC },
                    "@esa",
                    "v1.0.0",
                    "SysML v2",
                    "Space environment definitions and planetary constants for mission analysis and spacecraft design.",
                    "standard-library · space-environment · ecss",
                    "860",
                    true,
                    "2 months ago")
            ];

            RecentlyUpdatedPackages =
            [
                new PackageModel(
                    new Package { Name = "ECSS-MM-PWR", ShortName = "ecss-mm-pwr", Visibility = VisibilityKind.PUBLIC },
                    "@starion",
                    "v1.2.0",
                    "SysML v2",
                    "ECSS mission model: Power subsystem. Part definitions for power bus, battery, solar array, and PCU.",
                    "mission-model · power · ecss",
                    "210",
                    true,
                    "2 weeks ago"),
                new PackageModel(
                    new Package { Name = "SmallSat-Platform-Model", ShortName = "smallsat-platform-model", Visibility = VisibilityKind.PUBLIC },
                    "@starion",
                    "v0.8.2",
                    "SysML v2",
                    "Parametric smallsat platform model including propulsion and telemetry budget templates.",
                    "mission-model · smallsat · platform",
                    "145",
                    true,
                    "3 weeks ago"),
                new PackageModel(
                    new Package { Name = "ecss-e-st-32-10c", ShortName = "ecss-e-st-32-10c", Visibility = VisibilityKind.PUBLIC },
                    "@starion",
                    "v0.3.0",
                    "SysML v2",
                    "RF telecommunication link budget and space communication interfaces.",
                    "comms · rf · telemetry · ecss",
                    "190",
                    true,
                    "2 months ago")
            ];

            MostUsedPackages =
            [
                new PackageModel(
                    new Package { Name = "SysMLv2-Kernel-Library", ShortName = "sysmlv2-kernel-library", Visibility = VisibilityKind.PUBLIC },
                    "@omg",
                    "v2025.2",
                    "SysML v2",
                    "Fundamental KerML metamodel library containing base types, collections, and control functions.",
                    "standard-library · kerml · kernel",
                    "2.1k",
                    true,
                    "1 month ago"),
                new PackageModel(
                    new Package { Name = "SysMLv2-ISQ-Quantities", ShortName = "sysmlv2-isq-quantities", Visibility = VisibilityKind.PUBLIC },
                    "@omg",
                    "v2025.2",
                    "SysML v2",
                    "Standard quantities and units definition package for SysML v2 models based on ISO/IEC 80000.",
                    "standard-library · units · quantities · isq",
                    "1.4k",
                    true,
                    "1 month ago"),
                new PackageModel(
                    new Package { Name = "ECSS-E-ST-10-04C", ShortName = "ecss-e-st-10-04c", Visibility = VisibilityKind.PUBLIC },
                    "@esa",
                    "v1.0.0",
                    "SysML v2",
                    "Space environment definitions and planetary constants for mission analysis and spacecraft design.",
                    "standard-library · space-environment · ecss",
                    "860",
                    true,
                    "2 months ago")
            ];

            ModelsFromOtherMbseTools =
            [
                new PackageModel(
                    new Package { Name = "CDP4-COMET-Core", ShortName = "cdp4-comet-core", Visibility = VisibilityKind.PUBLIC },
                    "@starion",
                    "v10.25.1",
                    "CDP4-COMET",
                    "Core concurrent engineering data definitions and iteration exchange schemas for ECSS-E-TM-10-25.",
                    "concurrent-design · cdp4 · ecss-10-25",
                    "320",
                    true,
                    "1 month ago"),
                new PackageModel(
                    new Package { Name = "Capella-System-Template", ShortName = "capella-system-template", Visibility = VisibilityKind.PUBLIC },
                    "@esa",
                    "v6.1.0",
                    "Capella",
                    "Arcadia methodology operational analysis and system architecture template for space instruments.",
                    "arcadia · capella · operational-analysis",
                    "185",
                    true,
                    "3 months ago"),
                new PackageModel(
                    new Package { Name = "ecss-e-st-31-01c", ShortName = "ecss-e-st-31-01c", Visibility = VisibilityKind.PUBLIC },
                    "@starion",
                    "v1.0.0",
                    "SysML v2",
                    "Structural and mechanical engineering domain metamodels and loads analysis.",
                    "mechanical · structures · loads · ecss",
                    "165",
                    true,
                    "3 months ago")
            ];

            MyPackages =
            [
                new PackageModel(
                    new Package { Name = "ECSS-MM-PWR", ShortName = "ecss-mm-pwr", Visibility = VisibilityKind.PUBLIC },
                    "@starion",
                    "v1.2.0",
                    "SysML v2",
                    "ECSS mission model: Power subsystem.",
                    importCount: "210",
                    isVerified: true,
                    lastPublished: "2 weeks ago",
                    role: PackageInvitationKind.OWNER),
                new PackageModel(
                    new Package { Name = "SmallSat-Platform-Model", ShortName = "smallsat-platform-model", Visibility = VisibilityKind.PUBLIC },
                    "@starion",
                    "v0.8.2",
                    "SysML v2",
                    "Parametric smallsat platform model.",
                    importCount: "145",
                    isVerified: true,
                    lastPublished: "3 weeks ago",
                    role: PackageInvitationKind.OWNER),
                new PackageModel(
                    new Package { Name = "ecss-e-st-32-10c", ShortName = "ecss-e-st-32-10c", Visibility = VisibilityKind.PUBLIC },
                    "@starion",
                    "v0.3.0",
                    "SysML v2",
                    "RF telecommunication link budget.",
                    importCount: "190",
                    isVerified: true,
                    lastPublished: "2 months ago",
                    role: PackageInvitationKind.OWNER),
                new PackageModel(
                    new Package { Name = "ecss-e-st-31-01c", ShortName = "ecss-e-st-31-01c", Visibility = VisibilityKind.PUBLIC },
                    "@starion",
                    "v1.0.0",
                    "SysML v2",
                    "Structural and mechanical engineering domain metamodels.",
                    importCount: "165",
                    isVerified: true,
                    lastPublished: "3 months ago",
                    role: PackageInvitationKind.OWNER),
                new PackageModel(
                    new Package { Name = "CDP4-COMET-Core", ShortName = "cdp4-comet-core", Visibility = VisibilityKind.INTERNAL },
                    "@starion",
                    "v10.25.1",
                    "CDP4-COMET",
                    "Core concurrent engineering data definitions.",
                    importCount: "320",
                    isVerified: true,
                    lastPublished: "1 month ago",
                    role: PackageInvitationKind.MAINTAINER),
                new PackageModel(
                    new Package { Name = "ECSS-E-ST-10-04C", ShortName = "ecss-e-st-10-04c", Visibility = VisibilityKind.PUBLIC },
                    "@esa",
                    "v1.0.0",
                    "SysML v2",
                    "Space environment definitions and planetary constants.",
                    importCount: "860",
                    isVerified: true,
                    lastPublished: "2 months ago",
                    role: PackageInvitationKind.MAINTAINER)
            ];

            CatalogPackages =
            [
                new PackageModel(
                    "ECSS-MM-PWR",
                    "ECSS mission model: Power subsystem. Part definitions for power bus, battery, solar array, and power conditioning unit, typed by ISQ quantity kinds.",
                    "SysML v2 (kpar)",
                    "@starion",
                    "v1.2.0",
                    "mission-model · power · ecss",
                    "2 weeks ago",
                    "210",
                    true),
                new PackageModel(
                    "SysMLv2-ISQ-Quantities",
                    "Standard quantities and units definition package for SysML v2 models based on ISO/IEC 80000. Quantities of kind, measurement units, and dimension vectors.",
                    "SysML v2 (kpar)",
                    "@omg",
                    "v2025.2",
                    "standard-library · quantities-units · isq · sysml2",
                    "1 month ago",
                    "1.4k",
                    true),
                new PackageModel(
                    "SysMLv2-Kernel-Library",
                    "Fundamental KerML metamodel library containing base types, collections, control functions, and measurement scales used by all SysML v2 packages.",
                    "SysML v2 (kpar)",
                    "@omg",
                    "v2025.2",
                    "standard-library · kerml · kernel · sysml2",
                    "1 month ago",
                    "2.1k",
                    true),
                new PackageModel(
                    "ECSS-E-ST-10-04C",
                    "Space environment definitions following ECSS-E-ST-10-04C. Earth atmosphere models, solar radiation, geomagnetic field, and planetary constants.",
                    "SysML v2 (kpar)",
                    "@esa",
                    "v1.0.0",
                    "mission-model · space-environment · ecss · esa",
                    "2 months ago",
                    "860",
                    true),
                new PackageModel(
                    "SmallSat-Platform-Model",
                    "Parametric smallsat platform model including bus geometry, mass properties, power budget, and propulsion subsystem interfaces.",
                    "SysML v2 (kpar)",
                    "@starion",
                    "v0.8.2",
                    "mission-model · smallsat · platform · starion",
                    "3 weeks ago",
                    "145",
                    true),
                new PackageModel(
                    "CDP4-COMET-Core",
                    "Core concurrent engineering data definitions and iteration exchange schemas for ECSS-E-TM-10-25 concurrent design platform.",
                    "CDP4-COMET (10-25)",
                    "@starion",
                    "v10.25.1",
                    "concurrent-engineering · cdp4 · comet · ecss-10-25",
                    "1 month ago",
                    "320",
                    true,
                    license: "MIT")
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
    }
}
