// ------------------------------------------------------------------------------------------------
// <copyright file="IntegerExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using System.Globalization;

    /// <summary>
    /// Provides extension methods for integer formatting and manipulation.
    /// </summary>
    public static class IntegerExtensions
    {
        /// <param name="value">The integer value to format.</param>
        extension(int value)
        {
            /// <summary>
            /// Formats an integer value into a compact metric string representation (e.g., 600 -> "600", 1600 -> "1.6k", 16000 ->
            /// "16k").
            /// </summary>
            /// <returns>A compact metric string representation of the integer.</returns>
            public string ToCompactMetric()
            {
                var absoluteValue = Math.Abs(value);

                return absoluteValue switch
                {
                    < 1000 => value.ToString(CultureInfo.InvariantCulture),
                    < 1000000 => (value / 1000.0).ToString("0.#k", CultureInfo.InvariantCulture),
                    < 1000000000 => (value / 1000000.0).ToString("0.#M", CultureInfo.InvariantCulture),
                    _ => (value / 1000000000.0).ToString("0.#B", CultureInfo.InvariantCulture)
                };
            }
        }
    }
}
