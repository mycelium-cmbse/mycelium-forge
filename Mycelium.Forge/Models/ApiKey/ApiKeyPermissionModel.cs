// ------------------------------------------------------------------------------------------------
// <copyright file="ApiKeyPermissionModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.ApiKey
{
    /// <summary>
    /// Represents an API key permission option with its identifier, descriptive label, and selection state.
    /// </summary>
    public class ApiKeyPermissionModel
    {
        /// <summary>
        /// Gets or sets the programmatic identifier of the permission.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the descriptive display label of the permission.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this permission is selected or granted.
        /// </summary>
        public bool IsGranted { get; set; }
    }
}
