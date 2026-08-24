// ------------------------------------------------------------------------------------------------
// <copyright file="AccountsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages;
    using Mycelium.Forge.Models.Admin;
    using Mycelium.Forge.ViewModels.AdminAccounts;

    [TestFixture]
    public class AccountsTestFixture
    {
        private BunitContext context;
        private Mock<IAdminAccountsViewModel> viewModelMock;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IAdminAccountsViewModel>();

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Alex Rivera",
                ShortName = "alex.rivera",
                Email = "alex.rivera@example.com"
            };

            var accounts = new List<AdminAccountModel>
            {
                new(account, true, "Verified", "@starion (Admin)")
            };

            this.viewModelMock.SetupGet(x => x.Accounts).Returns(accounts);
            this.viewModelMock.SetupGet(x => x.FilteredAccounts).Returns(accounts);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyOnInitialized()
        {
            var accountsPage = this.context.Render<Accounts>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(accountsPage.Instance, Is.Not.Null);
                Assert.That(accountsPage.Instance.StatusFilterOptions, Has.Count.EqualTo(3));
                Assert.That(accountsPage.Instance.VerificationFilterOptions, Has.Count.EqualTo(3));
                this.viewModelMock.Verify(x => x.InitializeViewModel("", "All", "All"), Times.Once);
            }
        }
    }
}
