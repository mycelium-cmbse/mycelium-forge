// ------------------------------------------------------------------------------------------------
// <copyright file="ServiceHelper.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal
{
    using System.Collections;
    using System.Text;

    using Microsoft.Extensions.Logging;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Common.Comparers;

    /// <summary>
    /// Provides helper methods for domain service operations, such as logging property changes.
    /// </summary>
    public static class ServiceHelper
    {
        /// <summary>
        /// Formats a property change value for logging representation.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A string representation of the formatted value.</returns>
        public static string FormatChangeValue(object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is string stringValue)
            {
                return $"\"{stringValue}\"";
            }

            if (value is IEnumerable enumerable)
            {
                var items = enumerable.Cast<object>().Select(FormatChangeValue);
                return $"[{string.Join(", ", items)}]";
            }

            return value.ToString();
        }

        /// <summary>
        /// Logs the differences between an original and updated <see cref="IThing" /> entity.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IThing" /> being updated.</typeparam>
        /// <param name="logger">The <see cref="ILogger" /> instance to use for logging.</param>
        /// <param name="comparer">The <see cref="IDtoComparer{T}" /> used to compare instances.</param>
        /// <param name="originalThing">The original entity state before the update.</param>
        /// <param name="updatedThing">The updated entity state after the update.</param>
        public static void LogChanges<T>(ILogger logger, IDtoComparer<T> comparer, T originalThing, T updatedThing) where T : IThing
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(comparer);

            // If logging at the Information level is not enabled, skip the comparison and logging to avoid unnecessary computation.
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            var changes = comparer.Compare(originalThing, updatedThing).ToList();

            if (changes.Count == 0)
            {
                return;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Updating {typeof(T).Name} {updatedThing.Id} with {changes.Count} changes:");

            foreach (var change in changes)
            {
                var oldValueFormatted = FormatChangeValue(change.OldValue);
                var newValueFormatted = FormatChangeValue(change.NewValue);
                stringBuilder.AppendLine($"   -> {change.PropertyName} changed from {oldValueFormatted} to {newValueFormatted}");
            }

            logger.LogInformation("{Message}", stringBuilder.ToString());
        }
    }
}
