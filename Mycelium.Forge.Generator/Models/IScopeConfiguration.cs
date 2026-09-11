// ------------------------------------------------------------------------------------------------
// <copyright file="IScopeConfiguration.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Models
{
    /// <summary>
    /// Represents an entity behavior configuration that depends on a scope entity domain service.
    /// </summary>
    public interface IScopeConfiguration
    {
        /// <summary>
        /// Gets the scope entity name.
        /// </summary>
        string ScopeEntity { get; }

        /// <summary>
        /// Gets the injected domain service field name for the scope entity.
        /// </summary>
        string ScopeServiceField { get; }
    }
}
