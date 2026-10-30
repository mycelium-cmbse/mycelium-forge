// ------------------------------------------------------------------------------------------------
// <copyright file="CapellaErrorCodes.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Errors
{
    /// <summary>
    /// Provides the error code constants returned when Capella archive validation fails.
    /// </summary>
    public static class CapellaErrorCodes
    {
        /// <summary>
        /// Indicates that the artifact stream is not a valid ZIP archive, does not contain a semantic model file (.capella or
        /// .melodymodeller), or contains multiple model files.
        /// </summary>
        public const string InvalidArchive = "Capella.InvalidArchive";

        /// <summary>
        /// Indicates that the semantic model XML descriptor file cannot be parsed or deserialized.
        /// </summary>
        public const string InvalidDescriptor = "Capella.InvalidDescriptor";

        /// <summary>
        /// Indicates that the root Project or Library element is missing the name attribute or it is empty/whitespace.
        /// </summary>
        public const string MissingName = "Capella.MissingName";

        /// <summary>
        /// Indicates that an unexpected error occurred while processing the archive.
        /// </summary>
        public const string UnexpectedError = "Capella.UnexpectedError";
    }
}
