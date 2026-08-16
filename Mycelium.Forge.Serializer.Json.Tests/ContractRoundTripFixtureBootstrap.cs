// ------------------------------------------------------------------------------------------------
// <copyright file="ContractRoundTripFixtureBootstrap.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Serializer.Json.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Not part of the regular suite: run manually to (re)generate <c>TestData/all-dto-types-and-enum-variations.json</c>,
    /// the fixture <see cref="ContractRoundTripTestFixture"/> reads. Regenerating hand-writes every
    /// property through the real, compiler-checked DTO classes and the real <see cref="Serializer"/> -
    /// far less error-prone than hand-authoring the JSON text directly against the wire format (camelCase
    /// keys, <c>@type</c>/<c>@id</c> markers, reference properties wrapped as <c>{ "@id": ... }</c>) - then
    /// the output is reviewed and committed. The same "regenerate once, hand-review, commit" workflow
    /// already used for the golden files under Mycelium.Forge.Generator.Tests/Expected/.
    /// </summary>
    [TestFixture]
    [Explicit("Writes TestData/all-dto-types-and-enum-variations.json back into the source tree; run manually after a model change, review the diff, and commit it.")]
    public class ContractRoundTripFixtureBootstrap
    {
        private static Guid G(int n)
        {
            return new Guid($"00000000-0000-0000-0000-{n:D12}");
        }

        [Test]
        public void Write_the_contract_round_trip_fixture()
        {
            var epoch = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var things = new List<IThing>
            {
                new APIKey
                {
                    Id = G(1),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    ExpiresAt = epoch.AddYears(1),
                    LastUsedAt = epoch.AddDays(3),
                    Name = "ci-publish-key",
                    Permissions = G(2),
                    RevokedAt = epoch.AddYears(2),
                    SecretHash = new List<byte> { 0x1a, 0x2b, 0x3c, 0x4d },
                },
                new Account
                {
                    Id = G(3),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    Address = new List<Guid> { G(4), G(5) },
                    ApiKey = new List<Guid> { G(1) },
                    AvatarBlobReference = "blob://avatars/account-3",
                    BillingEmail = "billing@example.com",
                    DefaultPackageVisibility = VisibilityKind.PUBLIC,
                    Email = "account-3@example.com",
                    Name = "jane-doe",
                    ShortName = "jane-doe",
                    Origin = "https://example.com/jane-doe",
                    OwnedOrganizationInvitation = new List<Guid> { G(6) },
                    OwnedPackage = new List<Guid> { G(7) },
                    OwnedPackageInvitation = G(8),
                    PrimaryAddress = G(4),
                    ProfileLink = new List<Guid> { G(9) },
                    Status = ScopeStatusKind.ACTIVE,
                    Website = "https://jane-doe.example.com",
                },
                new Address
                {
                    Id = G(4),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    AddressLine1 = "1 Infinite Loop",
                    AddressLine2 = "Suite 100",
                    Country = G(10),
                    Locality = "Cupertino",
                    PostalCode = "95014",
                    Region = "CA",
                },
                new Country
                {
                    Id = G(10),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    Alpha2Code = "US",
                    Alpha3Code = "USA",
                    Name = "United States of America",
                    NumericCode = "840",
                },
                new Forge
                {
                    Id = G(11),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    Account = new List<Guid> { G(3) },
                    Administrator = new List<Guid> { G(3) },
                    Country = new List<Guid> { G(10) },
                    Description = "The Mycelium Forge instance",
                    Name = "mycelium-forge",
                    ShortName = "mycelium-forge",
                    Organization = new List<Guid> { G(12) },
                    PackageType = new List<Guid> { G(13) },
                    ProfileType = new List<Guid> { G(14) },
                },
                new Organization
                {
                    Id = G(12),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    Address = new List<Guid> { G(4) },
                    Administrator = new List<Guid> { G(3) },
                    BillingEmail = "billing@starion-group.example",
                    DefaultPackageVisibility = VisibilityKind.INTERNAL,
                    Email = "contact@starion-group.example",
                    LogoBlobReference = "blob://logos/organization-12",
                    Member = new List<Guid> { G(3) },
                    Name = "starion-group",
                    ShortName = "starion-group",
                    Origin = "https://example.com/starion-group",
                    OwnedPackage = new List<Guid> { G(7) },
                    PrimaryAddress = G(4),
                    ProfileLink = new List<Guid> { G(9) },
                    Status = ScopeStatusKind.DEACTIVATED,
                    Website = "https://starion-group.example",
                },
                new OrganizationInvitation
                {
                    Id = G(6),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    ExperisAt = epoch.AddDays(14),
                    Organization = G(12),
                    OrganizationInvitationKind = OrganizationInvitationKind.MEMBER,
                    Status = InvitationStatusKind.PENDING,
                    Target = G(3),
                },
                new OrganizationInvitation
                {
                    Id = G(15),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    ExperisAt = epoch.AddDays(14),
                    Organization = G(12),
                    OrganizationInvitationKind = OrganizationInvitationKind.ADMINISTRATOR,
                    Status = InvitationStatusKind.ACCEPTED,
                    Target = G(3),
                },
                new Package
                {
                    Id = G(7),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    Listed = true,
                    Name = "mycelium-forge-client",
                    ShortName = "mycelium-forge-client",
                    PackageMaintainer = new List<Guid> { G(3) },
                    PackageOwner = new List<Guid> { G(3) },
                    PackageType = G(13),
                    Version = new List<Guid> { G(16) },
                    Visibility = VisibilityKind.PRIVATE,
                },
                new PackageInvitation
                {
                    Id = G(8),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    ExperisAt = epoch.AddDays(7),
                    Package = G(7),
                    PackageInvitationKind = PackageInvitationKind.OWNER,
                    Status = InvitationStatusKind.REJECTED,
                    Target = G(3),
                },
                new PackageInvitation
                {
                    Id = G(17),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    ExperisAt = epoch.AddDays(7),
                    Package = G(7),
                    PackageInvitationKind = PackageInvitationKind.MAINTAINER,
                    Status = InvitationStatusKind.REVOKED,
                    Target = G(3),
                },
                new PackageMetaData
                {
                    Id = G(18),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                },
                new PackageType
                {
                    Id = G(13),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    Description = "A NuGet-style .nupkg package",
                    Name = "nuget",
                },
                new PackageVersion
                {
                    Id = G(16),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    DownloadCount = 42,
                    Listed = true,
                    MetaData = G(18),
                    PublicationDate = epoch.AddDays(1),
                    Version = "1.0.0",
                },
                new ProfileLink
                {
                    Id = G(9),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    ProfileType = G(14),
                    Uri = "https://github.com/jane-doe",
                },
                new ProfileType
                {
                    Id = G(14),
                    CreatedAt = epoch,
                    ModifiedAt = epoch,
                    LogoBlobReference = "blob://logos/profile-type-14",
                    Name = "GitHub",
                },
            };

            var serializer = new Serializer();
            var jsonWriterOptions = new JsonWriterOptions { Indented = true };

            using var stream = new MemoryStream();
            serializer.Serialize(things, stream, jsonWriterOptions);

            var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

            var targetPath = Path.Combine(
                TestContext.CurrentContext.TestDirectory, "..", "..", "..", "TestData", "all-dto-types-and-enum-variations.json");

            File.WriteAllText(Path.GetFullPath(targetPath), json);

            Assert.That(Path.GetFullPath(targetPath), Does.Exist);
        }
    }
}
