// ------------------------------------------------------------------------------------------------
// <copyright file="NotificationServiceTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Services
{
    using Mycelium.Forge.Model;
    using Mycelium.Forge.Services;

    [TestFixture]
    public class NotificationServiceTestFixture
    {
        private NotificationService notificationService;

        [SetUp]
        public void SetUp()
        {
            this.notificationService = new NotificationService();
        }

        [Test]
        public void VerifyAddNotification()
        {
            Assert.That(this.notificationService.Results.Items, Has.Count.EqualTo(0));

            this.notificationService.AddNotification(null);
            Assert.That(this.notificationService.Results.Items, Has.Count.EqualTo(0));

            var record = new NotificationContent("Operation completed successfully.", "Success Title", NotificationType.Success);
            this.notificationService.AddNotification(record);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.notificationService.Results.Items, Has.Count.EqualTo(1));
                Assert.That(this.notificationService.Results.Items[0].Message, Is.EqualTo("Operation completed successfully."));
                Assert.That(this.notificationService.Results.Items[0].Title, Is.EqualTo("Success Title"));
                Assert.That(this.notificationService.Results.Items[0].Type, Is.EqualTo(NotificationType.Success));
            }

            this.notificationService.AddNotification("Error occurred.", "Error Title", NotificationType.Error);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.notificationService.Results.Items, Has.Count.EqualTo(2));
                Assert.That(this.notificationService.Results.Items[1].Message, Is.EqualTo("Error occurred."));
                Assert.That(this.notificationService.Results.Items[1].Title, Is.EqualTo("Error Title"));
                Assert.That(this.notificationService.Results.Items[1].Type, Is.EqualTo(NotificationType.Error));
            }

            this.notificationService.AddNotification("Default message.");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.notificationService.Results.Items, Has.Count.EqualTo(3));
                Assert.That(this.notificationService.Results.Items[2].Message, Is.EqualTo("Default message."));
                Assert.That(this.notificationService.Results.Items[2].Title, Is.Null);
                Assert.That(this.notificationService.Results.Items[2].Type, Is.EqualTo(NotificationType.Default));
            }
        }
    }
}
