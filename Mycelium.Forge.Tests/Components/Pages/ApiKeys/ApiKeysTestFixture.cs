// ------------------------------------------------------------------------------------------------
// <copyright file="ApiKeysTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.ApiKeys
{
    using System;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.ApiKeys;
    using Mycelium.Forge.Models.ApiKey;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.ViewModels.ApiKeys;

    [TestFixture]
    public class ApiKeysTestFixture
    {
        private BunitContext context;
        private Mock<IApiKeysViewModel> viewModelMock;
        private DialogService dialogService;
        private APIKey testApiKey;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IApiKeysViewModel>();

            this.testApiKey = new APIKey
            {
                Id = Guid.NewGuid(),
                Name = "CI Token",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddYears(1),
                LastUsedAt = DateTime.UtcNow
            };

            this.viewModelMock.Setup(x => x.ApiKeys).Returns(
            [
                new ApiKeyModel(this.testApiKey, "@starion", "publish", "secret")
            ]);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.dialogService = this.context.Services.GetRequiredService<DialogService>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyHandleCreateApiKey()
        {
            var result = new CreateApiKeyResult { KeyName = "Deployment Key", Scope = "@starion", Expiration = "30 days" };
            var apiKey = new APIKey { Id = Guid.NewGuid(), Name = "Deployment Key" };
            var createdKeyModel = new ApiKeyModel(apiKey, "@starion", "publish", "myc_live_secret12345");

            this.viewModelMock.Setup(x => x.CreateApiKey(result)).Returns(createdKeyModel);

            var apiKeysPage = this.context.Render<ApiKeys>();
            _ = apiKeysPage.Instance.HandleCreateApiKey(result);

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.CreateApiKey(result), Times.Once);
                Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
            }
        }

        [Test]
        public void VerifyOnCreateKey()
        {
            var apiKeysPage = this.context.Render<ApiKeys>();
            var createKeyButton = apiKeysPage.Find("#api-keys-create-key-button");

            _ = apiKeysPage.InvokeAsync(() => createKeyButton.ClickAsync());

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
        }

        [Test]
        public void VerifyOnInitialized()
        {
            var apiKeysPage = this.context.Render<ApiKeys>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(apiKeysPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel(), Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnRevokeKey()
        {
            var apiKeysPage = this.context.Render<ApiKeys>();
            var revokeButton = apiKeysPage.Find(".revoke-api-key-button");

            await apiKeysPage.InvokeAsync(() => revokeButton.ClickAsync());

            this.viewModelMock.Verify(x => x.RevokeApiKey(this.testApiKey.Id), Times.Once);
        }
    }
}
