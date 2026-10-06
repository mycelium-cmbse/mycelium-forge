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
        /// Formats the specified duration value and time unit into a relative time unit string.
        /// </summary>
        /// <param name="value">The number of units.</param>
        /// <param name="unit">The name of the time unit (e.g., 'minute', 'hour', 'day', 'week', 'month', 'year').</param>
        /// <returns>A string formatted as '{value} {unit}(s)'.</returns>
        private static string FormatTimeUnit(int value, string unit)
        {
            return value <= 1 ? $"1 {unit}" : $"{value} {unit}s";
        }

        /// <summary>
        /// Formats a time span duration into a relative unit description string.
        /// </summary>
        /// <param name="duration">The time span duration to format.</param>
        /// <returns>A string describing the relative duration in human-readable units.</returns>
        private static string FormatRelativeDuration(TimeSpan duration)
        {
            if (duration.TotalSeconds < 60)
            {
                return "just now";
            }

            if (duration.TotalMinutes < 60)
            {
                return FormatTimeUnit((int)duration.TotalMinutes, "minute");
            }

            if (duration.TotalHours < 24)
            {
                return FormatTimeUnit((int)duration.TotalHours, "hour");
            }

            if (duration.TotalDays < 7)
            {
                return FormatTimeUnit((int)duration.TotalDays, "day");
            }

            if (duration.TotalDays < 30)
            {
                return FormatTimeUnit((int)(duration.TotalDays / 7), "week");
            }

            if (duration.TotalDays < 365)
            {
                return FormatTimeUnit((int)(duration.TotalDays / 30), "month");
            }

            return FormatTimeUnit((int)(duration.TotalDays / 365), "year");
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
                var elapsed = ToUtc(relativeTo) - ToUtc(dateTime);
                var durationText = FormatRelativeDuration(elapsed);

                return durationText == "just now" ? durationText : $"{durationText} ago";
            }

            /// <summary>
            /// Formats a <see cref="DateTime" /> into a human-readable relative future time span string compared to the current UTC
            /// time.
            /// </summary>
            /// <returns>A string describing the remaining time relative to now (e.g., 'just now', '2 weeks', '1 month').</returns>
            public string ToTimeToCome()
            {
                return dateTime.ToTimeToCome(DateTime.UtcNow);
            }

            /// <summary>
            /// Formats a <see cref="DateTime" /> into a human-readable relative future time span string compared to a specified
            /// reference
            /// date and time.
            /// </summary>
            /// <param name="relativeTo">The reference date and time to calculate the relative duration against.</param>
            /// <returns>A string describing the remaining time relative to the reference time.</returns>
            public string ToTimeToCome(DateTime relativeTo)
            {
                var remaining = ToUtc(dateTime) - ToUtc(relativeTo);

                return FormatRelativeDuration(remaining);
            }
        }
    }
}
