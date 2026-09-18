// ------------------------------------------------------------------------------------------------
// <copyright file="INotificationService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using DynamicData;

    using Mycelium.Forge.Model;

    /// <summary>
    /// Provides notification management and reactive notification stream operations.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Gets the reactive source list containing <see cref="NotificationContent" /> instances.
        /// </summary>
        SourceList<NotificationContent> Results { get; }

        /// <summary>
        /// Queues a notification to be displayed.
        /// </summary>
        /// <param name="notification">The <see cref="NotificationContent" /> to queue.</param>
        void AddNotification(NotificationContent notification);

        /// <summary>
        /// Queues a notification to be displayed.
        /// </summary>
        /// <param name="message">The notification body message.</param>
        /// <param name="title">The optional notification title.</param>
        /// <param name="type">The notification type.</param>
        void AddNotification(string message, string title = null, NotificationType type = NotificationType.Default);
    }
}
