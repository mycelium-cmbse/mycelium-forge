// ------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="DateTime" /> operations.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Ensures the specified <see cref="DateTime" /> is converted to Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="value">The date and time to convert.</param>
        /// <returns>The <see cref="DateTime" /> in UTC.</returns>
        private static DateTime ToUtc(DateTime value)
        {
            return value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        }

        /// <summary>
        /// Formats the specified elapsed value and time unit into a human-readable relative time string.
        /// </summary>
        /// <param name="value">The number of units elapsed.</param>
        /// <param name="unit">The name of the time unit (e.g., 'minute', 'hour', 'day', 'week', 'month', 'year').</param>
        /// <returns>A string formatted as '{value} {unit}(s) ago'.</returns>
        private static string FormatTimeAgoUnit(int value, string unit)
        {
            return value <= 1 ? $"1 {unit} ago" : $"{value} {unit}s ago";
        }

        /// <param name="dateTime">The date and time to format.</param>
        extension(DateTime dateTime)
        {
            /// <summary>
            /// Formats a <see cref="DateTime" /> into a human-readable relative time span string compared to the current UTC time.
            /// </summary>
            /// <returns>A string describing the elapsed time relative to now (e.g., 'just now', '2 weeks ago', '1 month ago').</returns>
            public string ToTimeAgo()
            {
                return dateTime.ToTimeAgo(DateTime.UtcNow);
            }

            /// <summary>
            /// Formats a <see cref="DateTime" /> into a human-readable relative time span string compared to a specified reference
            /// date and time.
            /// </summary>
            /// <param name="relativeTo">The reference date and time to calculate the relative duration against.</param>
            /// <returns>A string describing the elapsed time relative to the reference time.</returns>
            public string ToTimeAgo(DateTime relativeTo)
            {
                var utcDateTime = ToUtc(dateTime);
                var utcRelativeTo = ToUtc(relativeTo);

                var elapsed = utcRelativeTo - utcDateTime;

                if (elapsed.TotalSeconds < 60)
                {
                    return "just now";
                }

                if (elapsed.TotalMinutes < 60)
                {
                    return FormatTimeAgoUnit((int)elapsed.TotalMinutes, "minute");
                }

                if (elapsed.TotalHours < 24)
                {
                    return FormatTimeAgoUnit((int)elapsed.TotalHours, "hour");
                }

                if (elapsed.TotalDays < 7)
                {
                    return FormatTimeAgoUnit((int)elapsed.TotalDays, "day");
                }

                if (elapsed.TotalDays < 30)
                {
                    return FormatTimeAgoUnit((int)(elapsed.TotalDays / 7), "week");
                }

                if (elapsed.TotalDays < 365)
                {
                    return FormatTimeAgoUnit((int)(elapsed.TotalDays / 30), "month");
                }

                return FormatTimeAgoUnit((int)(elapsed.TotalDays / 365), "year");
            }
        }
    }
}
