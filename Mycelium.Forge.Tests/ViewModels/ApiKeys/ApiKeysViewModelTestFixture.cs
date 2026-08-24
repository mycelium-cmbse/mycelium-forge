// ------------------------------------------------------------------------------------------------
// <copyright file="ApiKeysViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.ApiKeys
{
    using System;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models.ApiKey;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.ViewModels.ApiKeys;

    [TestFixture]
    public class ApiKeysViewModelTestFixture
    {
        private ApiKeysViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new ApiKeysViewModel();
        }

        [Test]
        public void VerifyCreateApiKeyWithModel()
        {
            this.viewModel.InitializeViewModel();
            var initialCount = this.viewModel.ApiKeys.Count;

            var apiKey = new APIKey { Id = Guid.NewGuid(), Name = "Custom Key" };
            var model = new ApiKeyModel(apiKey);

            this.viewModel.CreateApiKey(model);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.ApiKeys, Has.Count.EqualTo(initialCount + 1));
                Assert.That(this.viewModel.ApiKeys, Contains.Item(model));
            }
        }

        [Test]
        public void VerifyCreateApiKeyWithResult()
        {
            this.viewModel.InitializeViewModel();
            var initialCount = this.viewModel.ApiKeys.Count;

            var result = new CreateApiKeyResult
            {
                KeyName = "CLI Publishing Key",
                Scope = "@starion",
                Expiration = "30 days",
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                Permissions =
                [
                    new ApiKeyPermissionModel { Name = "publish", IsGranted = true }
                ]
            };

            var createdModel = this.viewModel.CreateApiKey(result);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.ApiKeys, Has.Count.EqualTo(initialCount + 1));
                Assert.That(createdModel.Name, Is.EqualTo("CLI Publishing Key"));
                Assert.That(createdModel.PermissionsText, Is.EqualTo("publish"));
                Assert.That(createdModel.SecretToken, Does.StartWith("forge_pat_"));
            }
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.InitializeViewModel();

            Assert.That(this.viewModel.ApiKeys, Has.Count.GreaterThan(0));
        }

        [Test]
        public void VerifyRevokeApiKey()
        {
            this.viewModel.InitializeViewModel();
            var initialCount = this.viewModel.ApiKeys.Count;
            var targetId = this.viewModel.ApiKeys[0].Id;

            this.viewModel.RevokeApiKey(targetId);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.ApiKeys, Has.Count.EqualTo(initialCount - 1));
                Assert.That(this.viewModel.ApiKeys.Exists(x => x.Id == targetId), Is.False);
            }
        }
    }
}
