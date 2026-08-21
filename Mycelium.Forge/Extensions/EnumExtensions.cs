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
    using System;

    /// <summary>
    /// Provides extension methods for enumeration values.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts the enumeration value to a string representation with lowercase characters.
        /// </summary>
        /// <param name="value">The enumeration value to convert.</param>
        /// <returns>The string representation of the enumeration value formatted with lowercase characters.</returns>
        public static string ToLowerCaseFirst(this Enum value)
        {
            return value == null 
                ? string.Empty 
                : value.ToString().ToUpperCaseFirst();
        }
    }
}
