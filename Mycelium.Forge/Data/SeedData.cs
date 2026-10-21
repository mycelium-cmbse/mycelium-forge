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

    /// <summary>
    /// Provides centralized development seed data entities for database persistence.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SeedData
    {
        static SeedData()
        {
            var forgeId = Guid.NewGuid();

            RegisAccount = new Account
            {
                Id = Guid.Parse("c7e6d9d6-f1c4-4b95-b66f-8e182c331601"),
                Owner = forgeId,
                Name = "R. André",
                ShortName = "r-andre",
                Email = "regis.andre@starion.eu",
                Website = "stariongroup.eu",
                Origin = "Toulouse, France",
                Status = ScopeStatusKind.ACTIVE
            };

            StefanAccount = new Account
            {
                Id = Guid.NewGuid(),
                Owner = forgeId,
                Name = "S. Kramer",
                ShortName = "s-kramer",
                Email = "stefan.kramer@starion.eu",
                Origin = "local",
                Status = ScopeStatusKind.ACTIVE
            };

            KleinAccount = new Account
            {
                Id = Guid.NewGuid(),
                Owner = forgeId,
                Name = "J. Klein",
                ShortName = "j-klein",
                Email = "j.klein@esa.int",
                Origin = "local",
                Status = ScopeStatusKind.ACTIVE
            };

            BlancAccount = new Account
            {
                Id = Guid.NewGuid(),
                Owner = forgeId,
                Name = "M. Blanc",
                ShortName = "m-blanc",
                Email = "m.blanc@starion.eu",
                Origin = "local",
                Status = ScopeStatusKind.ACTIVE
            };

            NovakAccount = new Account
            {
                Id = Guid.NewGuid(),
                Owner = forgeId,
                Name = "A. Novak",
                ShortName = "a-novak",
                Email = "a.novak@esa.int",
                Origin = "local",
                Status = ScopeStatusKind.ACTIVE
            };

            Accounts =
            [
                RegisAccount,
                StefanAccount,
                KleinAccount,
                BlancAccount,
                NovakAccount
            ];

            Forge = new Forge
            {
                Id = forgeId,
                Name = "Mycelium Forge",
                ShortName = "forge",
                Description = "The central package registry and distribution platform for MBSE model libraries.",
                Administrator = [RegisAccount.Id],
                Account =
                [
                    RegisAccount.Id,
                    StefanAccount.Id,
                    KleinAccount.Id,
                    BlancAccount.Id,
                    NovakAccount.Id
                ]
            };

            Countries =
            [
                new Country
                {
                    Id = Guid.NewGuid(),
                    Owner = forgeId,
                    Alpha2Code = "FR",
                    Alpha3Code = "FRA",
                    NumericCode = "250",
                    Name = "France"
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Owner = forgeId,
                    Alpha2Code = "NL",
                    Alpha3Code = "NLD",
                    NumericCode = "528",
                    Name = "Netherlands"
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Owner = forgeId,
                    Alpha2Code = "DE",
                    Alpha3Code = "DEU",
                    NumericCode = "276",
                    Name = "Germany"
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Owner = forgeId,
                    Alpha2Code = "US",
                    Alpha3Code = "USA",
                    NumericCode = "840",
                    Name = "United States"
                }
            ];

            SysmlV2PackageType = new PackageType
            {
                Id = Guid.NewGuid(),
                Owner = forgeId,
                Name = PackageFormatConstants.SysMlV2,
                Description = "Systems Modeling Language v2 package format."
            };

            Cdp4CometPackageType = new PackageType
            {
                Id = Guid.NewGuid(),
                Owner = forgeId,
                Name = "Cdp4Comet",
                Description = "Concurrent Design Platform COMET data definitions."
            };

            CapellaPackageType = new PackageType
            {
                Id = Guid.NewGuid(),
                Owner = forgeId,
                Name = "Capella",
                Description = "Capella system architecture and Arcadia methodology models."
            };

            PackageTypes =
            [
                SysmlV2PackageType,
                Cdp4CometPackageType,
                CapellaPackageType
            ];

            StarionOrganization = new Organization
            {
                Id = Guid.NewGuid(),
                Owner = forgeId,
                Name = "Starion Group",
                ShortName = "starion",
                Email = "contact@stariongroup.eu",
                Origin = "Systems engineering models and ECSS mission libraries for early-phase spacecraft design.",
                Status = ScopeStatusKind.ACTIVE,
                Administrator = [RegisAccount.Id, StefanAccount.Id],
                Member = [RegisAccount.Id, StefanAccount.Id, KleinAccount.Id, BlancAccount.Id]
            };

            EsaOrganization = new Organization
            {
                Id = Guid.NewGuid(),
                Owner = forgeId,
                Name = "European Space Agency",
                ShortName = "esa",
                Email = "contact@esa.int",
                Origin = "European Space Agency engineering libraries and flight dynamics models.",
                Status = ScopeStatusKind.ACTIVE,
                Administrator = [RegisAccount.Id],
                Member = [RegisAccount.Id, KleinAccount.Id, NovakAccount.Id]
            };

            OmgOrganization = new Organization
            {
                Id = Guid.NewGuid(),
                Owner = forgeId,
                Name = "Object Management Group",
                ShortName = "omg",
                Email = "info@omg.org",
                Origin = "Official SysML v2 and KerML specification standard libraries.",
                Status = ScopeStatusKind.ACTIVE,
                Administrator = [RegisAccount.Id],
                Member = [RegisAccount.Id]
            };

            Organizations =
            [
                StarionOrganization,
                EsaOrganization,
                OmgOrganization
            ];

            SysmlIsqQuantitiesPackage = new Package
            {
                Id = Guid.NewGuid(),
                Owner = OmgOrganization.Id,
                PackageType = SysmlV2PackageType.Id,
                Name = "SysMLv2-ISQ-Quantities",
                ShortName = "sysmlv2-isq-quantities",
                Description = "Standard quantities and units definition package for SysML v2 models based on ISO/IEC 80000.",
                Visibility = VisibilityKind.PUBLIC,
                Listed = true,
                PackageOwner = [RegisAccount.Id],
                PackageMaintainer = [StefanAccount.Id]
            };

            SysmlKernelLibraryPackage = new Package
            {
                Id = Guid.NewGuid(),
                Owner = OmgOrganization.Id,
                PackageType = SysmlV2PackageType.Id,
                Name = "SysMLv2-Kernel-Library",
                ShortName = "sysmlv2-kernel-library",
                Description = "Fundamental KerML metamodel library containing base types, collections, and control functions.",
                Visibility = VisibilityKind.PUBLIC,
                Listed = true,
                PackageOwner = [RegisAccount.Id],
                PackageMaintainer = [StefanAccount.Id]
            };

            EcssEnvironmentPackage = new Package
            {
                Id = Guid.NewGuid(),
                Owner = EsaOrganization.Id,
                PackageType = SysmlV2PackageType.Id,
                Name = "ECSS-E-ST-10-04C",
                ShortName = "ecss-e-st-10-04c",
                Description = "Space environment definitions and planetary constants for mission analysis and spacecraft design.",
                Visibility = VisibilityKind.PUBLIC,
                Listed = true,
                PackageOwner = [NovakAccount.Id],
                PackageMaintainer = [RegisAccount.Id]
            };

            EcssPowerSubsystemPackage = new Package
            {
                Id = Guid.NewGuid(),
                Owner = StarionOrganization.Id,
                PackageType = SysmlV2PackageType.Id,
                Name = "ECSS-MM-PWR",
                ShortName = "ecss-mm-pwr",
                Description = "ECSS mission model: Power subsystem. Part definitions for power bus, battery, solar array, and PCU.",
                Visibility = VisibilityKind.PUBLIC,
                Listed = true,
                PackageOwner = [RegisAccount.Id],
                PackageMaintainer = [StefanAccount.Id]
            };

            SmallSatPlatformPackage = new Package
            {
                Id = Guid.NewGuid(),
                Owner = StarionOrganization.Id,
                PackageType = SysmlV2PackageType.Id,
                Name = "SmallSat-Platform-Model",
                ShortName = "smallsat-platform-model",
                Description = "Parametric smallsat platform model including propulsion and telemetry budget templates.",
                Visibility = VisibilityKind.PUBLIC,
                Listed = true,
                PackageOwner = [RegisAccount.Id],
                PackageMaintainer = [StefanAccount.Id]
            };

            EcssRfCommsPackage = new Package
            {
                Id = Guid.NewGuid(),
                Owner = StarionOrganization.Id,
                PackageType = SysmlV2PackageType.Id,
                Name = "ecss-e-st-32-10c",
                ShortName = "ecss-e-st-32-10c",
                Description = "RF telecommunication link budget and space communication interfaces.",
                Visibility = VisibilityKind.PUBLIC,
                Listed = true,
                PackageOwner = [RegisAccount.Id],
                PackageMaintainer = [StefanAccount.Id]
            };

            Cdp4CometCorePackage = new Package
            {
                Id = Guid.NewGuid(),
                Owner = StarionOrganization.Id,
                PackageType = Cdp4CometPackageType.Id,
                Name = "CDP4-COMET-Core",
                ShortName = "cdp4-comet-core",
                Description = "Core concurrent engineering data definitions and iteration exchange schemas for ECSS-E-TM-10-25.",
                Visibility = VisibilityKind.PUBLIC,
                Listed = true,
                PackageOwner = [StefanAccount.Id],
                PackageMaintainer = [RegisAccount.Id]
            };

            CapellaSystemTemplatePackage = new Package
            {
                Id = Guid.NewGuid(),
                Owner = EsaOrganization.Id,
                PackageType = CapellaPackageType.Id,
                Name = "Capella-System-Template",
                ShortName = "capella-system-template",
                Description = "Arcadia methodology operational analysis and system architecture template for space instruments.",
                Visibility = VisibilityKind.PUBLIC,
                Listed = true,
                PackageOwner = [KleinAccount.Id],
                PackageMaintainer = [StefanAccount.Id, RegisAccount.Id]
            };

            EcssMechanicalPackage = new Package
            {
                Id = Guid.NewGuid(),
                Owner = StarionOrganization.Id,
                PackageType = SysmlV2PackageType.Id,
                Name = "ecss-e-st-31-01c",
                ShortName = "ecss-e-st-31-01c",
                Description = "Structural and mechanical engineering domain metamodels and loads analysis.",
                Visibility = VisibilityKind.PUBLIC,
                Listed = true,
                PackageOwner = [RegisAccount.Id],
                PackageMaintainer = [StefanAccount.Id]
            };

            var baseDate = DateTime.UtcNow;

            PackageVersions =
            [
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = SysmlIsqQuantitiesPackage.Id,
                    Version = "2025.2",
                    DownloadCount = 1400,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-30)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = SysmlKernelLibraryPackage.Id,
                    Version = "2025.2",
                    DownloadCount = 2100,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-30)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = EcssEnvironmentPackage.Id,
                    Version = "1.0.0",
                    DownloadCount = 860,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-60)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = EcssPowerSubsystemPackage.Id,
                    Version = "1.2.0",
                    DownloadCount = 210,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-14)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = SmallSatPlatformPackage.Id,
                    Version = "0.8.2",
                    DownloadCount = 145,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-21)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = EcssRfCommsPackage.Id,
                    Version = "0.3.0",
                    DownloadCount = 190,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-60)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = Cdp4CometCorePackage.Id,
                    Version = "10.25.1",
                    DownloadCount = 320,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-30)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = CapellaSystemTemplatePackage.Id,
                    Version = "6.1.0",
                    DownloadCount = 185,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-90)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = EcssMechanicalPackage.Id,
                    Version = "1.0.0",
                    DownloadCount = 165,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-90)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = SysmlIsqQuantitiesPackage.Id,
                    Version = "2025.1",
                    DownloadCount = 980,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-120)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = SysmlKernelLibraryPackage.Id,
                    Version = "2025.1",
                    DownloadCount = 1450,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-120)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = EcssEnvironmentPackage.Id,
                    Version = "0.9.0",
                    DownloadCount = 320,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-150)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = EcssPowerSubsystemPackage.Id,
                    Version = "1.1.0",
                    DownloadCount = 180,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-104)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = EcssPowerSubsystemPackage.Id,
                    Version = "1.0.0",
                    DownloadCount = 95,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-194)
                },
                new PackageVersion
                {
                    Id = Guid.NewGuid(),
                    Owner = Cdp4CometCorePackage.Id,
                    Version = "10.25.0",
                    DownloadCount = 185,
                    Listed = true,
                    PublicationDate = baseDate.AddDays(-120)
                }
            ];

            PackageMetaDatas = PackageVersions.Select(CreatePackageMetaData).ToList();

            Packages =
            [
                SysmlIsqQuantitiesPackage,
                SysmlKernelLibraryPackage,
                EcssEnvironmentPackage,
                EcssPowerSubsystemPackage,
                SmallSatPlatformPackage,
                EcssRfCommsPackage,
                Cdp4CometCorePackage,
                CapellaSystemTemplatePackage,
                EcssMechanicalPackage
            ];

            foreach (var package in Packages)
            {
                package.Version = PackageVersions.Where(x => x.Owner == package.Id).Select(x => x.Id).ToList();
            }

            PackageInvitations = Accounts.Select(CreatePackageInvitation).ToList();

            OrganizationInvitations =
            [
                new OrganizationInvitation
                {
                    Id = Guid.NewGuid(),
                    Owner = RegisAccount.Id,
                    Organization = StarionOrganization.Id,
                    Target = NovakAccount.Id,
                    OrganizationInvitationKind = OrganizationInvitationKind.MEMBER,
                    Status = InvitationStatusKind.PENDING,
                    ExperiesAt = DateTime.UtcNow.AddDays(7)
                },
                new OrganizationInvitation
                {
                    Id = Guid.NewGuid(),
                    Owner = RegisAccount.Id,
                    Organization = StarionOrganization.Id,
                    Target = StefanAccount.Id,
                    OrganizationInvitationKind = OrganizationInvitationKind.ADMINISTRATOR,
                    Status = InvitationStatusKind.PENDING,
                    ExperiesAt = DateTime.UtcNow.AddDays(7)
                }
            ];

            ApiKeys =
            [
                new APIKey
                {
                    Id = Guid.NewGuid(),
                    Owner = RegisAccount.Id,
                    Name = "ci-publish",
                    ExpiresAt = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                    LastUsedAt = DateTime.UtcNow.AddDays(-3),
                    RevokedAt = new DateTime(2099, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    SecretHash = [1, 2, 3],
                    Permissions = ["read", "write"]
                },
                new APIKey
                {
                    Id = Guid.NewGuid(),
                    Owner = RegisAccount.Id,
                    Name = "release-bot",
                    ExpiresAt = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    LastUsedAt = DateTime.UtcNow.AddDays(-14),
                    RevokedAt = new DateTime(2099, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    SecretHash = [1, 2, 3],
                    Permissions = ["read", "write"]
                },
                new APIKey
                {
                    Id = Guid.NewGuid(),
                    Owner = RegisAccount.Id,
                    Name = "local-dev",
                    ExpiresAt = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    LastUsedAt = DateTime.UtcNow.AddDays(-30),
                    RevokedAt = new DateTime(2099, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    SecretHash = [1, 2, 3],
                    Permissions = ["read", "write"]
                }
            ];
        }

        /// <summary>
        /// Gets the root Forge platform entity.
        /// </summary>
        public static Forge Forge { get; }

        /// <summary>
        /// Gets the list of initial countries.
        /// </summary>
        public static IReadOnlyList<Country> Countries { get; }

        /// <summary>
        /// Gets the SysML v2 package type entity.
        /// </summary>
        public static PackageType SysmlV2PackageType { get; }

        /// <summary>
        /// Gets the CDP4-COMET package type entity.
        /// </summary>
        public static PackageType Cdp4CometPackageType { get; }

        /// <summary>
        /// Gets the Capella package type entity.
        /// </summary>
        public static PackageType CapellaPackageType { get; }

        /// <summary>
        /// Gets the list of initial package types.
        /// </summary>
        public static IReadOnlyList<PackageType> PackageTypes { get; }

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
        /// Gets the list of initial accounts.
        /// </summary>
        public static IReadOnlyList<Account> Accounts { get; }

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
        /// Gets the list of initial organizations.
        /// </summary>
        public static IReadOnlyList<Organization> Organizations { get; }

        /// <summary>
        /// Gets the package entity for SysMLv2-ISQ-Quantities.
        /// </summary>
        public static Package SysmlIsqQuantitiesPackage { get; }

        /// <summary>
        /// Gets the package entity for SysMLv2-Kernel-Library.
        /// </summary>
        public static Package SysmlKernelLibraryPackage { get; }

        /// <summary>
        /// Gets the package entity for ECSS-E-ST-10-04C.
        /// </summary>
        public static Package EcssEnvironmentPackage { get; }

        /// <summary>
        /// Gets the package entity for ECSS-MM-PWR.
        /// </summary>
        public static Package EcssPowerSubsystemPackage { get; }

        /// <summary>
        /// Gets the package entity for SmallSat-Platform-Model.
        /// </summary>
        public static Package SmallSatPlatformPackage { get; }

        /// <summary>
        /// Gets the package entity for ecss-e-st-32-10c.
        /// </summary>
        public static Package EcssRfCommsPackage { get; }

        /// <summary>
        /// Gets the package entity for CDP4-COMET-Core.
        /// </summary>
        public static Package Cdp4CometCorePackage { get; }

        /// <summary>
        /// Gets the package entity for Capella-System-Template.
        /// </summary>
        public static Package CapellaSystemTemplatePackage { get; }

        /// <summary>
        /// Gets the package entity for ecss-e-st-31-01c.
        /// </summary>
        public static Package EcssMechanicalPackage { get; }

        /// <summary>
        /// Gets the master list of seeded package entities.
        /// </summary>
        public static IReadOnlyList<Package> Packages { get; }

        /// <summary>
        /// Gets the master list of seeded package version entities.
        /// </summary>
        public static IReadOnlyList<PackageVersion> PackageVersions { get; }

        /// <summary>
        /// Gets the master list of seeded package metadata entities.
        /// </summary>
        public static IReadOnlyList<PackageMetaData> PackageMetaDatas { get; }

        /// <summary>
        /// Gets the master list of seeded package invitations.
        /// </summary>
        public static IReadOnlyList<PackageInvitation> PackageInvitations { get; }

        /// <summary>
        /// Gets the master list of seeded organization invitations.
        /// </summary>
        public static IReadOnlyList<OrganizationInvitation> OrganizationInvitations { get; }

        /// <summary>
        /// Gets the master list of API keys.
        /// </summary>
        public static IReadOnlyList<APIKey> ApiKeys { get; }

        /// <summary>
        /// Creates a <see cref="PackageMetaData" /> instance linked to the specified <see cref="PackageVersion" /> using only
        /// codegenerated properties.
        /// </summary>
        /// <param name="packageVersion">The owning package version.</param>
        /// <returns>A configured <see cref="PackageMetaData" /> entity.</returns>
        private static PackageMetaData CreatePackageMetaData(PackageVersion packageVersion)
        {
            var metaData = new PackageMetaData
            {
                Id = Guid.NewGuid(),
                Owner = packageVersion.Id
            };

            packageVersion.MetaData = metaData.Id;
            return metaData;
        }

        /// <summary>
        /// Creates a <see cref="PackageInvitation" /> instance linked to the specified <see cref="Account" /> using only
        /// codegenerated properties.
        /// </summary>
        /// <param name="account">The owning account.</param>
        /// <returns>A configured <see cref="PackageInvitation" /> entity.</returns>
        private static PackageInvitation CreatePackageInvitation(Account account)
        {
            var invitation = new PackageInvitation
            {
                Id = Guid.NewGuid(),
                Owner = account.Id,
                Package = SysmlIsqQuantitiesPackage.Id,
                Target = StefanAccount.Id == account.Id ? RegisAccount.Id : StefanAccount.Id,
                PackageInvitationKind = PackageInvitationKind.MAINTAINER,
                Status = InvitationStatusKind.PENDING,
                ExperiesAt = DateTime.UtcNow.AddDays(14)
            };

            account.OwnedPackageInvitation = invitation.Id;
            return invitation;
        }
    }
}
