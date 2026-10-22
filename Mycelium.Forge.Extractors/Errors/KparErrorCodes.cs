// ------------------------------------------------------------------------------------------------
// <copyright file="KparErrorCodes.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Errors
{
    /// <summary>
    /// Defines the error codes returned when a kpar fails a publish-time validation check (design §9.1, KerML 1.0 §10.3).
    /// </summary>
    public static class KparErrorCodes
    {
        /// <summary>
        /// The archive is not a valid ZIP, or its top-level <c>.project.json</c> / <c>.meta.json</c> are not exactly one each, or
        /// an
        /// indexed model interchange file is missing from the archive.
        /// </summary>
        public const string InvalidArchive = "Kpar.InvalidArchive";

        /// <summary>
        /// The <c>.project.json</c> or <c>.meta.json</c> does not conform to its schema.
        /// </summary>
        public const string InvalidDescriptor = "Kpar.InvalidDescriptor";

        /// <summary>
        /// The mandatory <c>name</c> of <c>.project.json</c> is missing.
        /// </summary>
        public const string MissingName = "Kpar.MissingName";

        /// <summary>
        /// The mandatory <c>version</c> of <c>.project.json</c> is missing.
        /// </summary>
        public const string MissingVersion = "Kpar.MissingVersion";

        /// <summary>
        /// The <c>version</c> of <c>.project.json</c> is not SemVer 2.0.0.
        /// </summary>
        public const string InvalidVersion = "Kpar.InvalidVersion";

        /// <summary>
        /// The mandatory <c>created</c> of <c>.meta.json</c> is missing.
        /// </summary>
        public const string MissingCreated = "Kpar.MissingCreated";

        /// <summary>
        /// The mandatory <c>index</c> of <c>.meta.json</c> is missing or declares no root namespace, so the archive holds no model
        /// interchange file.
        /// </summary>
        public const string NoModels = "Kpar.NoModels";

        /// <summary>
        /// Two root namespaces of the <c>index</c> resolve to the same archive path.
        /// </summary>
        public const string DuplicateModelPath = "Kpar.DuplicateModelPath";

        /// <summary>
        /// A <c>usage[]</c> entry of <c>.project.json</c> has no <c>resource</c> IRI.
        /// </summary>
        public const string MissingUsageResource = "Kpar.MissingUsageResource";

        /// <summary>
        /// The checksum of a model interchange file differs from the one declared in <c>.meta.json</c>.
        /// </summary>
        public const string ChecksumMismatch = "Kpar.ChecksumMismatch";
    }
}
