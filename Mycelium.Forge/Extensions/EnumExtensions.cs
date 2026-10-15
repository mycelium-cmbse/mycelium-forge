// ------------------------------------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    /// <summary>
    /// Provides extension methods for enumeration values.
    /// </summary>
    public static class EnumExtensions
    {
        /// <param name="value">The enumeration value to convert.</param>
        extension(Enum value)
        {
            /// <summary>
            /// Converts the enumeration value to a string representation with the first character in uppercase and remaining
            /// characters in lowercase.
            /// </summary>
            /// <returns>
            /// The string representation of the enumeration value formatted with first letter capitalized and remaining
            /// characters lowercase.
            /// </returns>
            public string ToUpperCaseFirst()
            {
                return value == null
                    ? string.Empty
                    : value.ToString().ToUpperCaseFirst();
            }
        }
    }
}
