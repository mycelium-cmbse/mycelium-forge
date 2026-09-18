// ------------------------------------------------------------------------------------------------
// <copyright file="NotificationComponentTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Common
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Model;
    using Mycelium.Forge.Services;

    [TestFixture]
    public class NotificationComponentTestFixture
    {
        private BunitContext context;
        private NotificationService notificationService;
        private ToastService toastService;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.notificationService = new NotificationService();
            this.context.Services.AddSingleton<INotificationService>(this.notificationService);
            this.toastService = this.context.Services.GetRequiredService<ToastService>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyDisplayToastNotification()
        {
            var notificationComponent = this.context.Render<NotificationComponent>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(notificationComponent.Instance, Is.Not.Null);
                Assert.That(notificationComponent.Instance.NotificationService, Is.Not.Null);
                Assert.That(notificationComponent.Instance.ToastService, Is.Not.Null);
                Assert.That(this.toastService.Toasts, Has.Count.EqualTo(0));
            }

            this.notificationService.AddNotification(null);
            this.notificationService.AddNotification(new NotificationContent(string.Empty));
            this.notificationService.AddNotification(new NotificationContent("   "));
            Assert.That(this.toastService.Toasts, Has.Count.EqualTo(0));

            this.notificationService.AddNotification(new NotificationContent("Item saved.", "Saved", NotificationType.Success));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.toastService.Toasts, Has.Count.EqualTo(1));
                Assert.That(this.notificationService.Results.Items, Has.Count.EqualTo(0));
            }

            this.notificationService.AddNotification("An error occurred.", "Failed", NotificationType.Error);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.toastService.Toasts, Has.Count.EqualTo(2));
                Assert.That(this.notificationService.Results.Items, Has.Count.EqualTo(0));
            }

            this.notificationService.AddNotification("Information note.", "Note", NotificationType.Info);
            Assert.That(this.toastService.Toasts, Has.Count.EqualTo(3));

            this.notificationService.AddNotification("Warning message.", "Warning", NotificationType.Warning);
            Assert.That(this.toastService.Toasts, Has.Count.EqualTo(4));

            this.notificationService.AddNotification("Custom type.", null, (NotificationType)999);
            Assert.That(this.toastService.Toasts, Has.Count.EqualTo(5));

            notificationComponent.Instance.Dispose();

            this.notificationService.AddNotification("After dispose.", "Ignored", NotificationType.Success);
            Assert.That(this.toastService.Toasts, Has.Count.EqualTo(5));
        }
    }
}
