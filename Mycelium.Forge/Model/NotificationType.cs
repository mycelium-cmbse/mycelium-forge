// ------------------------------------------------------------------------------------------------
// <copyright file="NotificationType.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Model
{
    /// <summary>
    /// Specifies the severity type of a notification message.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Indicates a default notification.
        /// </summary>
        Default,

        /// <summary>
        /// Indicates a success notification.
        /// </summary>
        Success,

        /// <summary>
        /// Indicates an error notification.
        /// </summary>
        Error,

        /// <summary>
        /// Indicates an informational notification.
        /// </summary>
        Info,

        /// <summary>
        /// Indicates a warning notification.
        /// </summary>
        Warning
    }
}
