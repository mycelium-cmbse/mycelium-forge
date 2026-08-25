// ------------------------------------------------------------------------------------------------
// <copyright file="VerificationFilterOption.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Enums
{
    /// <summary>
    /// Specifies the verification status filter options for administrator account filtering.
    /// </summary>
    public enum VerificationFilterOption
    {
        /// <summary>
        /// Include all accounts regardless of verification status.
        /// </summary>
        All,

        /// <summary>
        /// Filter to verified accounts only.
        /// </summary>
        Verified,

        /// <summary>
        /// Filter to pending verification accounts only.
        /// </summary>
        Pending
    }
}
