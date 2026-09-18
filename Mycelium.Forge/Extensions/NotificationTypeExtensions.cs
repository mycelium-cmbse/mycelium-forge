// ------------------------------------------------------------------------------------------------
// <copyright file="NotificationTypeExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using BlazorBlueprint.Components;

    using Mycelium.Forge.Model;

    /// <summary>
    /// Provides extension methods for converting <see cref="NotificationType" /> values to BlazorBlueprint UI representations.
    /// </summary>
    public static class NotificationTypeExtensions
    {
        /// <summary>
        /// Converts the specified <see cref="NotificationType" /> to its corresponding <see cref="ToastVariant" />.
        /// </summary>
        /// <param name="type">The <see cref="NotificationType" /> value to convert.</param>
        /// <returns>The matching <see cref="ToastVariant" />.</returns>
        public static ToastVariant ToToastVariant(this NotificationType type)
        {
            return type switch
            {
                NotificationType.Default => ToastVariant.Default,
                NotificationType.Success => ToastVariant.Success,
                NotificationType.Error => ToastVariant.Destructive,
                NotificationType.Info => ToastVariant.Info,
                NotificationType.Warning => ToastVariant.Warning,
                _ => ToastVariant.Default
            };
        }
    }
}
