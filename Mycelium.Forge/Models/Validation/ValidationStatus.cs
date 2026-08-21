// ------------------------------------------------------------------------------------------------
// <copyright file="ValidationStatus.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Validation
{
    /// <summary>
    /// Specifies the evaluation status outcome of a validation rule or check.
    /// </summary>
    public enum ValidationStatus
    {
        /// <summary>
        /// The validation check passed successfully with no errors.
        /// </summary>
        Pass,

        /// <summary>
        /// The validation check completed with non-blocking warnings.
        /// </summary>
        Warning,

        /// <summary>
        /// An expected documentation or package file was not found.
        /// </summary>
        Missing,

        /// <summary>
        /// The validation check failed and prevents publishing.
        /// </summary>
        Fail
    }
}
