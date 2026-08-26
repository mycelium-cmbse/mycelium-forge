// ------------------------------------------------------------------------------------------------
// <copyright file="PropertyChange.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Comparers
{
    /// <summary>
    /// Represents a change detected between an original property value and an updated property value of a DTO.
    /// </summary>
    /// <param name="PropertyName">The name of the property that changed.</param>
    /// <param name="OldValue">The original value of the property before the change.</param>
    /// <param name="NewValue">The updated value of the property after the change.</param>
    public record PropertyChange(string PropertyName, object OldValue, object NewValue);
}
