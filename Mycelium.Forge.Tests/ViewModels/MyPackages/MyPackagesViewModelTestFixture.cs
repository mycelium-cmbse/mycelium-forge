// ------------------------------------------------------------------------------------------------
// <copyright file="MyPackagesViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.MyPackages
{
    using System.Threading.Tasks;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.MyPackages;

    [TestFixture]
    public class MyPackagesViewModelTestFixture
    {
        private MyPackagesViewModel viewModel;

        private Mock<IUserService> userServiceMock;

        [SetUp]
        public void SetUp()
        {
            this.userServiceMock = new Mock<IUserService>();

            this.userServiceMock
                .Setup(s => s.GetCurrentUser(It.IsAny<bool>()))
                .ReturnsAsync(SeedData.RegisAccount);

            this.userServiceMock
                .Setup(s => s.CurrentRoles)
                .Returns([RoleKind.InstallationAdministrator, RoleKind.OrganizationAdministrator, RoleKind.Account]);

            this.viewModel = new MyPackagesViewModel(this.userServiceMock.Object);
        }

        [Test]
        public async Task VerifyInitializeViewModel()
        {
            await this.viewModel.InitializeViewModel();

            Assert.That(this.viewModel.Packages, Has.Count.GreaterThan(0));
        }
    }
}
