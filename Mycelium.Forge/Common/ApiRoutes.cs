// ------------------------------------------------------------------------------------------------
// <copyright file="ApiRoutes.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Defines API route path constants used for endpoints and HTTP client requests.
    /// </summary>
    public static class ApiRoutes
    {
        /// <summary>
        /// The route path for staging package archive uploads.
        /// </summary>
        public const string UploadFileForStage = "/api/v1/upload/stage";

        /// <summary>
        /// The route template for determining whether a scope with the specified short name exists.
        /// </summary>
        public const string ScopeShortNameExists = "/api/v1/internal/scopes/shortnames/{shortName}/exists";
    }
}
