// ------------------------------------------------------------------------------------------------
// <copyright file="UrlParameterNames.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Defines URL query parameter name constants used across application routing and queries.
    /// </summary>
    public static class UrlParameterNames
    {
        /// <summary>
        /// The search query parameter name.
        /// </summary>
        public const string Query = "q";

        /// <summary>
        /// The sort order parameter name.
        /// </summary>
        public const string Sort = "sort";

        /// <summary>
        /// The format filter parameter name.
        /// </summary>
        public const string Format = "format";

        /// <summary>
        /// The category filter parameter name.
        /// </summary>
        public const string Category = "category";

        /// <summary>
        /// The email address parameter name.
        /// </summary>
        public const string Email = "email";
    }
}
