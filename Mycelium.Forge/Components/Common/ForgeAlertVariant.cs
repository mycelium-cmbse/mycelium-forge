// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeAlertVariant.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    /// <summary>
    /// Defines the visual styling variants for the ForgeAlert callout component.
    /// </summary>
    public enum ForgeAlertVariant
    {
        /// <summary>
        /// Default subtle surface with muted border and standard foreground text.
        /// </summary>
        Default,

        /// <summary>
        /// Informational surface with blue tint.
        /// </summary>
        Info,

        /// <summary>
        /// Success surface with green tint.
        /// </summary>
        Success,

        /// <summary>
        /// Warning surface with amber tint.
        /// </summary>
        Warning,

        /// <summary>
        /// Destructive or critical error surface with red tint.
        /// </summary>
        Danger,

        /// <summary>
        /// Secondary surface with subtle background and secondary text.
        /// </summary>
        Secondary
    }
}
