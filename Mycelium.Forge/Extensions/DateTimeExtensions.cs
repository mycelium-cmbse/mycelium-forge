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
                var utcDateTime = dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime();
                var utcRelativeTo = relativeTo.Kind == DateTimeKind.Utc ? relativeTo : relativeTo.ToUniversalTime();

                var elapsed = utcRelativeTo - utcDateTime;

                if (elapsed.TotalSeconds < 60)
                {
                    return "just now";
                }

                if (elapsed.TotalMinutes < 60)
                {
                    var minutes = (int)elapsed.TotalMinutes;
                    return minutes == 1 ? "1 minute ago" : $"{minutes} minutes ago";
                }

                if (elapsed.TotalHours < 24)
                {
                    var hours = (int)elapsed.TotalHours;
                    return hours == 1 ? "1 hour ago" : $"{hours} hours ago";
                }

                if (elapsed.TotalDays < 7)
                {
                    var days = (int)elapsed.TotalDays;
                    return days == 1 ? "1 day ago" : $"{days} days ago";
                }

                if (elapsed.TotalDays < 30)
                {
                    var weeks = (int)(elapsed.TotalDays / 7);
                    return weeks == 1 ? "1 week ago" : $"{weeks} weeks ago";
                }

                if (elapsed.TotalDays < 365)
                {
                    var months = (int)(elapsed.TotalDays / 30);
                    return months <= 1 ? "1 month ago" : $"{months} months ago";
                }

                var years = (int)(elapsed.TotalDays / 365);
                return years <= 1 ? "1 year ago" : $"{years} years ago";
            }
        }
    }
}
