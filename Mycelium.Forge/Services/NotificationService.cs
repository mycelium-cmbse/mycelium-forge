// ------------------------------------------------------------------------------------------------
// <copyright file="NotificationService.cs" company="Starion Group S.A.">
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

    using ReactiveUI;

    /// <summary>
    /// Default implementation of <see cref="INotificationService" /> providing reactive notification queueing.
    /// </summary>
    public class NotificationService : ReactiveObject, INotificationService
    {
        /// <summary>
        /// Gets the reactive source list containing <see cref="NotificationContent" /> instances.
        /// </summary>
        public SourceList<NotificationContent> Results { get; } = new();

        /// <summary>
        /// Queues a notification to be displayed.
        /// </summary>
        /// <param name="notification">The <see cref="NotificationContent" /> to queue.</param>
        public void AddNotification(NotificationContent notification)
        {
            if (notification == null)
            {
                return;
            }

            this.Results.Add(notification);
        }

        /// <summary>
        /// Queues a notification to be displayed.
        /// </summary>
        /// <param name="message">The notification body message.</param>
        /// <param name="title">The optional notification title.</param>
        /// <param name="type">The notification type.</param>
        public void AddNotification(string message, string title = null, NotificationType type = NotificationType.Default)
        {
            this.AddNotification(new NotificationContent(message, title, type));
        }
    }
}
