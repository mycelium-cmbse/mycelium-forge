// ------------------------------------------------------------------------------------------------
// <copyright file="BaseGarageIntegrationTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Storage.Tests.Helper
{
    using Mycelium.Forge.Storage.Stores;

    using NUnit.Framework;

    public abstract class BaseGarageIntegrationTestFixture
    {
        private readonly GarageContainer garage = new();

        public ArtifactStore ArtifactStore => this.garage.ArtifactStore;

        [OneTimeSetUp]
        public async Task InitializeGarageContainerAsync()
        {
            await this.garage.StartAsync();
        }

        [OneTimeTearDown]
        public async Task CleanupGarageContainerAsync()
        {
            await this.garage.DisposeAsync();
        }
    }
}
