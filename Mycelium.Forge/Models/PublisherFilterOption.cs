// ------------------------------------------------------------------------------------------------
// <copyright file="PublisherFilterOption.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a publisher filter option displayed in the publisher chip selector.
    /// </summary>
    /// <param name="Key">The filter identifier (e.g., "all", "@starion", "@mycelium").</param>
    /// <param name="Label">The display label with count (e.g., "All (5)", "@starion (3)").</param>
    public record PublisherFilterOption(
        string Key,
        string Label);
}
