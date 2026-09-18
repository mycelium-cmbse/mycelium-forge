// ------------------------------------------------------------------------------------------------
// <copyright file="NotificationTypeExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Extensions
{
    using BlazorBlueprint.Components;

    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Model;

    [TestFixture]
    public class NotificationTypeExtensionsTestFixture
    {
        [Test]
        public void VerifyToToastVariant()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(NotificationType.Default.ToToastVariant(), Is.EqualTo(ToastVariant.Default));
                Assert.That(NotificationType.Success.ToToastVariant(), Is.EqualTo(ToastVariant.Success));
                Assert.That(NotificationType.Error.ToToastVariant(), Is.EqualTo(ToastVariant.Destructive));
                Assert.That(NotificationType.Info.ToToastVariant(), Is.EqualTo(ToastVariant.Info));
                Assert.That(NotificationType.Warning.ToToastVariant(), Is.EqualTo(ToastVariant.Warning));
                Assert.That(((NotificationType)999).ToToastVariant(), Is.EqualTo(ToastVariant.Default));
            }
        }
    }
}
