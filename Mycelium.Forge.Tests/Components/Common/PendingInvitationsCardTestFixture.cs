// ------------------------------------------------------------------------------------------------
// <copyright file="PendingInvitationsCardTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Common
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using DynamicData;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Tests.TestHelpers;
    using Mycelium.Forge.ViewModels.Invitations;

    [TestFixture]
    public class PendingInvitationsCardTestFixture
    {
        private BunitContext context;
        private Mock<IInvitationManagerViewModel<IOrganizationInvitation>> viewModelMock;
        private SourceList<IOrganizationInvitation> pendingInvitations;
        private OrganizationInvitation activeInvitation;
        private OrganizationInvitation expiredInvitation;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();
            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.pendingInvitations = new SourceList<IOrganizationInvitation>();

            this.activeInvitation = new OrganizationInvitation
            {
                Id = Guid.NewGuid(),
                Organization = Guid.NewGuid(),
                Target = Guid.NewGuid(),
                OrganizationInvitationKind = OrganizationInvitationKind.MEMBER,
                Status = InvitationStatusKind.PENDING,
                ExperiesAt = DateTime.UtcNow.AddDays(3)
            };

            this.expiredInvitation = new OrganizationInvitation
            {
                Id = Guid.NewGuid(),
                Organization = Guid.NewGuid(),
                Target = Guid.NewGuid(),
                OrganizationInvitationKind = OrganizationInvitationKind.ADMINISTRATOR,
                Status = InvitationStatusKind.PENDING,
                ExperiesAt = DateTime.UtcNow.AddHours(-2)
            };

            this.pendingInvitations.AddRange([this.activeInvitation, this.expiredInvitation]);

            this.viewModelMock = new Mock<IInvitationManagerViewModel<IOrganizationInvitation>>();
            this.viewModelMock.Setup(x => x.PendingInvitations).Returns(this.pendingInvitations);
            this.viewModelMock.Setup(x => x.GetTargetName(this.activeInvitation)).Returns("alice.dev");
            this.viewModelMock.Setup(x => x.GetTargetName(this.expiredInvitation)).Returns("Unknown");
        }

        [TearDown]
        public async Task TearDown()
        {
            this.pendingInvitations.Dispose();
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyResendAndRevokeActions()
        {
            var card = this.context.Render<PendingInvitationsCard<IOrganizationInvitation>>(parameters => parameters
                .Add(p => p.ViewModel, this.viewModelMock.Object)
                .Add(p => p.RoleTextSelector, invitation => invitation.OrganizationInvitationKind.ToString())
                .Add(p => p.AlertDescription, "Invitation notice"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(card.Markup, Does.Contain("alice.dev"));
                Assert.That(card.Markup, Does.Contain("Unknown"));
                Assert.That(card.Markup, Does.Contain("Expired"));
                Assert.That(card.Markup, Does.Contain("Invitation notice"));
                Assert.That(card.FindComponents<BbButton>().Where(x => x.Instance.HasClass("resend-invitation-button")).ToList(), Has.Count.EqualTo(2));
                Assert.That(card.FindComponents<BbButton>().Where(x => x.Instance.HasClass("revoke-invitation-button")).ToList(), Has.Count.EqualTo(2));
            }

            var resendButton = card.Find(".resend-invitation-button");
            await card.InvokeAsync(() => resendButton.ClickAsync());

            var revokeButton = card.Find(".revoke-invitation-button");
            await card.InvokeAsync(() => revokeButton.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.ResendInvitation(this.activeInvitation, It.IsAny<CancellationToken>()), Times.Once);
                this.viewModelMock.Verify(x => x.RevokeInvitation(this.activeInvitation, It.IsAny<CancellationToken>()), Times.Once);
            }

            // Reactive SourceList update removes row without explicit parent render
            await card.InvokeAsync(() => this.pendingInvitations.Remove(this.expiredInvitation));

            Assert.That(card.FindComponents<BbButton>().Where(x => x.Instance.HasClass("resend-invitation-button")).ToList(), Has.Count.EqualTo(1));

            // CanManage = false hides action buttons
            card.Render(parameters => parameters.Add(p => p.CanManage, false));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(card.FindComponents<BbButton>().Where(x => x.Instance.HasClass("resend-invitation-button")).ToList(), Is.Empty);
                Assert.That(card.FindComponents<BbButton>().Where(x => x.Instance.HasClass("revoke-invitation-button")).ToList(), Is.Empty);
            }
        }
    }
}
