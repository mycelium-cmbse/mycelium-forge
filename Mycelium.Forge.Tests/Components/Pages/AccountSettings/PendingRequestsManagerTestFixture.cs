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
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.AccountSettings;
    using Mycelium.Forge.ViewModels.AccountSettings;
    using Mycelium.Forge.ViewModels.Rows;

    [TestFixture]
    public class PendingRequestsManagerTestFixture
    {
        private BunitContext context;
        private Mock<IPendingRequestsManagerViewModel> viewModelMock;
        private OrganizationInvitationRowViewModel testRow;
        private Organization testOrganization;
        private OrganizationInvitation testInvitation;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IPendingRequestsManagerViewModel>();

            var orgId = Guid.NewGuid();

            this.testOrganization = new Organization
            {
                Id = orgId,
                Name = "Starion Group",
                ShortName = "starion"
            };

            this.testInvitation = new OrganizationInvitation
            {
                Id = Guid.NewGuid(),
                Organization = orgId,
                Target = Guid.NewGuid(),
                Status = InvitationStatusKind.PENDING,
                OrganizationInvitationKind = OrganizationInvitationKind.ADMINISTRATOR,
                ExperiesAt = DateTime.UtcNow.AddDays(7)
            };

            this.testRow = OrganizationInvitationRowViewModel.GenerateRows([this.testInvitation], [this.testOrganization]).First();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyOnAcceptInvitation()
        {
            this.viewModelMock.Setup(x => x.PendingInvitations).Returns([this.testRow]);
            var component = this.context.Render<PendingRequestsManager>(parameters => parameters.Add(x => x.ViewModel, this.viewModelMock.Object));

            var acceptButton = component.Find(".pending-accept-button");
            await component.InvokeAsync(() => acceptButton.ClickAsync());

            this.viewModelMock.Verify(x => x.AcceptInvitation(this.testRow), Times.Once);
        }

        [Test]
        public async Task VerifyOnRejectInvitation()
        {
            this.viewModelMock.Setup(x => x.PendingInvitations).Returns([this.testRow]);
            var component = this.context.Render<PendingRequestsManager>(parameters => parameters.Add(x => x.ViewModel, this.viewModelMock.Object));

            var rejectButton = component.Find(".pending-reject-button");
            await component.InvokeAsync(() => rejectButton.ClickAsync());

            this.viewModelMock.Verify(x => x.RejectInvitation(this.testRow), Times.Once);
        }

        [Test]
        public void VerifyRendering()
        {
            // Case 1: ViewModel is null or empty
            this.viewModelMock.Setup(x => x.PendingInvitations).Returns([]);

            var component = this.context.Render<PendingRequestsManager>(parameters => parameters
                .Add(x => x.ViewModel, this.viewModelMock.Object)
                .Add(x => x.Class, "test-class"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(component.Markup, Does.Contain("No pending invitations."));
                Assert.That(component.Markup, Does.Contain("test-class"));
            }

            // Case 2: ViewModel has invitations
            this.viewModelMock.Setup(x => x.PendingInvitations).Returns([this.testRow]);

            var populatedComponent = this.context.Render<PendingRequestsManager>(parameters => parameters
                .Add(x => x.ViewModel, this.viewModelMock.Object));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(populatedComponent.Markup, Does.Contain("Starion Group"));
                Assert.That(populatedComponent.Markup, Does.Contain("starion"));
                Assert.That(populatedComponent.Markup, Does.Not.Contain("No pending invitations."));
            }
        }
    }
}
