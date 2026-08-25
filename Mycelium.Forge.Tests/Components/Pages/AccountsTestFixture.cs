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

            this.viewModelMock.Setup(x => x.Accounts).Returns(accounts);
            this.viewModelMock.Setup(x => x.FilteredAccounts).Returns(accounts);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyGetAccountsCountText()
        {
            var accountsPage = this.context.Render<Accounts>();

            var singleCount = accountsPage.Instance.GetAccountsCountText();

            this.viewModelMock.Setup(x => x.FilteredAccounts).Returns(
            [
                new AdminAccountModel(new Account { Name = "User 1" }),
                new AdminAccountModel(new Account { Name = "User 2" })
            ]);

            var multipleCount = accountsPage.Instance.GetAccountsCountText();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(singleCount, Is.EqualTo("1 account"));
                Assert.That(multipleCount, Is.EqualTo("2 accounts"));
            }
        }

        [Test]
        public void VerifyOnAccountMenu()
        {
            var account = new AdminAccountModel(new Account { Name = "Alex Rivera" });

            Assert.DoesNotThrow(() => Accounts.OnAccountMenu(account));
        }

        [Test]
        public void VerifyOnInitialized()
        {
            var accountsPage = this.context.Render<Accounts>();
            Assert.That(accountsPage.Instance, Is.Not.Null);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(Accounts.StatusFilterOptions, Has.Count.EqualTo(3));
                Assert.That(Accounts.VerificationFilterOptions, Has.Count.EqualTo(3));
                this.viewModelMock.Verify(x => x.InitializeViewModel(string.Empty, "All", "All"), Times.Once);
            }
        }

        [Test]
        public void VerifyOnSearchInputChanged()
        {
            var accountsPage = this.context.Render<Accounts>();

            accountsPage.Instance.OnSearchInputChanged("alex");
            var query = accountsPage.Instance.SearchQuery;

            accountsPage.Instance.OnSearchInputChanged(null);
            var nullQuery = accountsPage.Instance.SearchQuery;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(query, Is.EqualTo("alex"));
                Assert.That(nullQuery, Is.EqualTo(string.Empty));
                this.viewModelMock.Verify(x => x.ApplyFilters("alex", "All", "All"), Times.Once);
                this.viewModelMock.Verify(x => x.ApplyFilters(string.Empty, "All", "All"), Times.Once);
            }
        }

        [Test]
        public void VerifyOnStatusFilterChanged()
        {
            var accountsPage = this.context.Render<Accounts>();

            accountsPage.Instance.OnStatusFilterChanged("Active");
            var status = accountsPage.Instance.SelectedStatusFilter;

            accountsPage.Instance.OnStatusFilterChanged(null);
            var nullStatus = accountsPage.Instance.SelectedStatusFilter;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(status, Is.EqualTo("Active"));
                Assert.That(nullStatus, Is.EqualTo("All"));
                this.viewModelMock.Verify(x => x.ApplyFilters(string.Empty, "Active", "All"), Times.Once);
                this.viewModelMock.Verify(x => x.ApplyFilters(string.Empty, "All", "All"), Times.Once);
            }
        }

        [Test]
        public void VerifyOnTransfer()
        {
            var accountsPage = this.context.Render<Accounts>();

            accountsPage.Instance.OnTransfer();

            this.viewModelMock.Verify(x => x.TransferOwnership(), Times.Once);
        }

        [Test]
        public void VerifyOnVerificationFilterChanged()
        {
            var accountsPage = this.context.Render<Accounts>();

            accountsPage.Instance.OnVerificationFilterChanged("Verified");
            var verification = accountsPage.Instance.SelectedVerificationFilter;

            accountsPage.Instance.OnVerificationFilterChanged(null);
            var nullVerification = accountsPage.Instance.SelectedVerificationFilter;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(verification, Is.EqualTo("Verified"));
                Assert.That(nullVerification, Is.EqualTo("All"));
                this.viewModelMock.Verify(x => x.ApplyFilters(string.Empty, "All", "Verified"), Times.Once);
                this.viewModelMock.Verify(x => x.ApplyFilters(string.Empty, "All", "All"), Times.Once);
            }
        }

        [Test]
        public void VerifyRenderingWithEmptyAndNonActiveAccounts()
        {
            this.viewModelMock.Setup(x => x.FilteredAccounts).Returns([]);
            var emptyPage = this.context.Render<Accounts>();

            var pendingAccount = new AdminAccountModel(
                new Account
                {
                    Name = "Pending User",
                    ShortName = "pending.user",
                    Email = "pending@example.com"
                },
                false,
                "Pending",
                "None",
                ScopeStatusKind.DEACTIVATED);

            this.viewModelMock.Setup(x => x.FilteredAccounts).Returns([pendingAccount]);
            var pendingPage = this.context.Render<Accounts>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyPage.Markup, Does.Contain("No accounts match the current filter."));
                Assert.That(pendingPage.Markup, Does.Contain("Pending"));
                Assert.That(pendingPage.Markup, Does.Contain("DEACTIVATED"));
            }
        }
    }
}
