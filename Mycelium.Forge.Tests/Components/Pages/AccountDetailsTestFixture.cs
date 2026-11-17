// ------------------------------------------------------------------------------------------------
// <copyright file="AccountDetailsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages;
    using Mycelium.Forge.ViewModels.AccountDetails;

    [TestFixture]
    public class AccountDetailsTestFixture
    {
        private BunitContext context;
        private Mock<IAccountDetailsViewModel> viewModelMock;
        private Account account;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IAccountDetailsViewModel>();

            this.account = new Account
            {
                Id = Guid.NewGuid(),
                Name = "R. Andre",
                ShortName = "r-andre",
                Origin = "Toulouse, France",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            this.viewModelMock.Setup(x => x.Account).Returns(this.account);
            this.viewModelMock.Setup(x => x.Packages).Returns([]);
            this.viewModelMock.Setup(x => x.IsCurrentUser).Returns(true);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyOnParametersSet()
        {
            var blankAccountDetailsPage = this.context.Render<AccountDetails>(parameters => parameters.Add(p => p.ShortName, "   "));
            var accountDetailsPage = this.context.Render<AccountDetails>(parameters => parameters.Add(p => p.ShortName, "r-andre"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(blankAccountDetailsPage.Instance, Is.Not.Null);
                Assert.That(accountDetailsPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel("   ", It.IsAny<CancellationToken>()), Times.Never);
                this.viewModelMock.Verify(x => x.InitializeViewModel("r-andre", It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Test]
        public void VerifyRendering()
        {
            this.viewModelMock.Setup(x => x.Account).Returns((IAccount)null!);
            var nullAccountDetailsPage = this.context.Render<AccountDetails>();

            this.viewModelMock.Setup(x => x.Account).Returns(this.account);
            var activeAccountDetailsPage = this.context.Render<AccountDetails>();

            this.viewModelMock.Setup(x => x.IsCurrentUser).Returns(false);
            var otherAccountDetailsPage = this.context.Render<AccountDetails>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nullAccountDetailsPage.Markup.Trim(), Has.Length.LessThan(5));
                Assert.That(activeAccountDetailsPage.Markup, Does.Contain("R. Andre"));
                Assert.That(activeAccountDetailsPage.Markup, Does.Contain("@r-andre"));
                Assert.That(activeAccountDetailsPage.FindAll("#scope-details-settings-button"), Has.Count.EqualTo(1));
                Assert.That(otherAccountDetailsPage.FindAll("#scope-details-settings-button"), Has.Count.EqualTo(0));
            }
        }
    }
}
