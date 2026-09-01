// ------------------------------------------------------------------------------------------------
// <copyright file="PermissionDefinition.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.DataLoaders.PermissionModels
{
    /// <summary>
    /// Represents a single permission parsed from a CSV column header.
    /// </summary>
    public class PermissionDefinition
    {
        /// <summary>
        /// Gets or sets the original CSV column header text.
        /// </summary>
        public string CsvHeader { get; set; }

        /// <summary>
        /// Gets or sets the PascalCase enum name derived from the header.
        /// </summary>
        public string EnumName { get; set; }
    }
}
