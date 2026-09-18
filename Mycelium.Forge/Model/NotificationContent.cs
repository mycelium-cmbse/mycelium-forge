// ------------------------------------------------------------------------------------------------
// <copyright file="NotificationContent.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Model
{
    /// <summary>
    /// Represents a notification record containing a message, an optional title, and a notification type.
    /// </summary>
    /// <param name="Message">The notification body message.</param>
    /// <param name="Title">The optional notification title.</param>
    /// <param name="Type">The notification severity type.</param>
    public record NotificationContent(string Message, string Title = null, NotificationType Type = NotificationType.Default);
}
