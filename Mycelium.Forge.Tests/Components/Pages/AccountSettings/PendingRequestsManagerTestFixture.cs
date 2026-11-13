// ------------------------------------------------------------------------------------------------
// <copyright file="PendingRequestsManagerTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.AccountSettings
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Icons.Lucide.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Components.Pages.AccountSettings;
    using Mycelium.Forge.ViewModels.AccountSettings;
    using Mycelium.Forge.ViewModels.Rows;

    [TestFixture]
    public class PendingRequestsManagerTestFixture
    {
        private BunitContext context;
        private Mock<IPendingRequestsManagerViewModel> viewModelMock;
        private OrganizationInvitationRowViewModel testRow;
        private PackageInvitationRowViewModel testPackageRow;
        private PackageInvitationRowViewModel testPrivatePackageRow;
        private Organization testOrganization;
        private Package testPackage;
        private OrganizationInvitation testInvitation;
        private PackageInvitation testPackageInvitation;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IPendingRequestsManagerViewModel>();

            var organizationId = Guid.NewGuid();
            var packageId = Guid.NewGuid();

            this.testOrganization = new Organization
            {
                Id = organizationId,
                Name = "Starion Group",
                ShortName = "starion"
            };

            this.testPackage = new Package
            {
                Id = packageId,
                Name = "ECSS Power",
                ShortName = "ecss-power",
                Owner = organizationId
            };

            this.testInvitation = new OrganizationInvitation
            {
                Id = Guid.NewGuid(),
                Organization = organizationId,
                Target = Guid.NewGuid(),
                Status = InvitationStatusKind.PENDING,
                OrganizationInvitationKind = OrganizationInvitationKind.ADMINISTRATOR,
                ExperiesAt = DateTime.UtcNow.AddDays(7)
            };

            this.testPackageInvitation = new PackageInvitation
            {
                Id = Guid.NewGuid(),
                Package = packageId,
                Target = Guid.NewGuid(),
                Status = InvitationStatusKind.PENDING,
                PackageInvitationKind = PackageInvitationKind.MAINTAINER,
                ExperiesAt = DateTime.UtcNow.AddDays(7)
            };

            var privatePackageInvitation = new PackageInvitation
            {
                Id = Guid.NewGuid(),
                Package = Guid.NewGuid(),
                Target = Guid.NewGuid(),
                Status = InvitationStatusKind.PENDING,
                PackageInvitationKind = PackageInvitationKind.OWNER,
                ExperiesAt = DateTime.UtcNow.AddDays(7)
            };

            this.testRow = OrganizationInvitationRowViewModel.GenerateRows([this.testInvitation], [this.testOrganization]).First();
            this.testPackageRow = PackageInvitationRowViewModel.GenerateRows([this.testPackageInvitation], [this.testPackage, this.testOrganization]).First();
            this.testPrivatePackageRow = PackageInvitationRowViewModel.GenerateRows([privatePackageInvitation], []).First();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyOnAcceptInvitation()
        {
            this.viewModelMock.Setup(x => x.PendingInvitations).Returns([this.testRow, this.testPackageRow]);
            var component = this.context.Render<PendingRequestsManager>(parameters => parameters.Add(x => x.ViewModel, this.viewModelMock.Object));

            var acceptButtons = component.FindAll(".pending-accept-button");
            await component.InvokeAsync(() => acceptButtons[0].ClickAsync());
            await component.InvokeAsync(() => acceptButtons[1].ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.AcceptInvitation(this.testInvitation, It.IsAny<CancellationToken>()), Times.Once);
                this.viewModelMock.Verify(x => x.AcceptInvitation(this.testPackageInvitation, It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnRejectInvitation()
        {
            this.viewModelMock.Setup(x => x.PendingInvitations).Returns([this.testRow, this.testPackageRow]);
            var component = this.context.Render<PendingRequestsManager>(parameters => parameters.Add(x => x.ViewModel, this.viewModelMock.Object));

            var rejectButtons = component.FindAll(".pending-reject-button");
            await component.InvokeAsync(() => rejectButtons[0].ClickAsync());
            await component.InvokeAsync(() => rejectButtons[1].ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.RejectInvitation(this.testInvitation, It.IsAny<CancellationToken>()), Times.Once);
                this.viewModelMock.Verify(x => x.RejectInvitation(this.testPackageInvitation, It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Test]
        public void VerifyRendering()
        {
            // Case 1: ViewModel is empty
            this.viewModelMock.Setup(x => x.PendingInvitations).Returns([]);

            var component = this.context.Render<PendingRequestsManager>(parameters =>
            {
                parameters.Add(x => x.ViewModel, this.viewModelMock.Object);
                parameters.Add(x => x.Class, "test-class");
            });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(component.Markup, Does.Contain("No pending invitations."));
                Assert.That(component.Markup, Does.Contain("test-class"));
                Assert.That(component.FindComponents<PendingRequestRow>(), Is.Empty);
            }

            // Case 2: ViewModel has organization and package invitations
            this.viewModelMock.Setup(x => x.PendingInvitations).Returns([this.testRow, this.testPackageRow, this.testPrivatePackageRow]);

            var populatedComponent = this.context.Render<PendingRequestsManager>(parameters => parameters.Add(x => x.ViewModel, this.viewModelMock.Object));
            var rows = populatedComponent.FindComponents<PendingRequestRow>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(rows, Has.Count.EqualTo(3));
                Assert.That(rows[0].FindComponents<CollaboratorAvatar>(), Has.Count.EqualTo(1));
                Assert.That(rows[0].FindComponents<LucideIcon>(), Is.Empty);
                Assert.That(rows[0].Find("a").GetAttribute("href"), Is.EqualTo("/organizations/starion"));
                Assert.That(rows[0].Markup, Does.Contain("Starion Group"));
                Assert.That(rows[0].Markup, Does.Contain("starion"));

                Assert.That(rows[1].FindComponents<CollaboratorAvatar>(), Is.Empty);
                Assert.That(rows[1].FindComponents<LucideIcon>(), Has.Count.EqualTo(1));
                Assert.That(rows[1].Find("a").GetAttribute("href"), Is.EqualTo("/packages/starion/ecss-power"));
                Assert.That(rows[1].Markup, Does.Contain("@starion/ecss-power"));
                Assert.That(rows[1].Markup, Does.Contain("ECSS Power"));

                Assert.That(rows[2].FindComponents<CollaboratorAvatar>(), Is.Empty);
                Assert.That(rows[2].FindComponents<LucideIcon>(), Has.Count.EqualTo(1));
                Assert.That(rows[2].FindAll("a"), Is.Empty);
                Assert.That(rows[2].Markup, Does.Contain("Private package"));
                Assert.That(rows[2].Markup, Does.Contain("Package invitation"));
                Assert.That(populatedComponent.Markup, Does.Not.Contain("No pending invitations."));
            }
        }
    }
}
