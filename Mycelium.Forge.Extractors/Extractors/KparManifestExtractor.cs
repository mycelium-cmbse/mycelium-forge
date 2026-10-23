// ------------------------------------------------------------------------------------------------
// <copyright file="KparManifestExtractor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Extractors
{
    using System.Text.Json;
    using System.Text.RegularExpressions;

    using ErrorOr;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Errors;
    using Mycelium.Forge.Extractors.Models;
    using Mycelium.Forge.Extractors.Models.Kpar;

    using SysML2.NET.Kpar;
    using SysML2.NET.ModelInterchange;

    /// <summary>
    /// Validates a kpar against the publish-time checks of design §9.1 (KerML 1.0 §10.3) and projects its descriptors onto a
    /// <see cref="KparManifest" />.
    /// </summary>
    public partial class KparManifestExtractor : IArtifactManifestExtractor
    {
        /// <summary>
        /// The resource the <see cref="IReader" /> assigns to a usage entry that declares none.
        /// </summary>
        /// <remarks>
        /// <see cref="InterchangeProjectUsage" /> initializes its <c>Resource</c> to <c>about:blank</c> by default. When
        /// <c>.project.json</c> omits the <c>resource</c> property, the deserializer leaves it set to this value rather than null.
        /// </remarks>
        private const string AbsentResource = "about:blank";

        /// <summary>
        /// The options making the <see cref="IReader" /> enforce the root descriptors, the existence of every indexed model file
        /// and the declared checksums, collecting every checksum mismatch.
        /// </summary>
        private static readonly ReadOptions ReadOptions = new()
        {
            ValidateRequiredDescriptors = true,
            ValidateIndexPaths = true,
            ValidateChecksums = true,
            ChecksumFailureBehavior = ChecksumFailureBehavior.Collect
        };

        /// <summary>
        /// The reader of the kpar archive format.
        /// </summary>
        private readonly IReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="KparManifestExtractor" /> class.
        /// </summary>
        /// <param name="reader">The reader of the kpar archive format.</param>
        public KparManifestExtractor(IReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Determines whether the extractor handles the specified artifact format.
        /// </summary>
        /// <param name="kind">The artifact format.</param>
        /// <returns>True if the extractor handles the format, otherwise false.</returns>
        public bool CanHandle(ArtifactKind kind)
        {
            return kind == ArtifactKind.Kpar;
        }

        /// <summary>
        /// Validates a kpar and extracts its manifest.
        /// </summary>
        /// <param name="artifact">The kpar content.</param>
        /// <returns>A result containing the <see cref="KparManifest" />, or one error for each failed validation check.</returns>
        public ErrorOr<IArtifactManifest> Extract(Stream artifact)
        {
            Archive archive;

            try
            {
                archive = this.reader.Read(artifact, ReadOptions);
            }
            catch (InvalidDataException exception)
            {
                return Error.Validation(KparErrorCodes.InvalidArchive, exception.Message);
            }
            catch (JsonException exception)
            {
                return Error.Validation(KparErrorCodes.InvalidDescriptor, exception.Message);
            }
            catch (Exception exception)
            {
                return Error.Unexpected(KparErrorCodes.UnexpectedError, exception.Message);
            }

            var errors = Validate(archive);

            if (errors.Count > 0)
            {
                return errors;
            }

            return CreateManifest(archive);
        }

        /// <summary>
        /// Gets the regular expression of the SemVer 2.0.0 grammar published at https://semver.org.
        /// </summary>
        /// <returns>The <see cref="Regex" />.</returns>
        [GeneratedRegex(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$")]
        private static partial Regex SemanticVersionRegex();

        /// <summary>
        /// Runs the checks that the <see cref="IReader" /> does not enforce.
        /// </summary>
        /// <param name="archive">The archive read by the <see cref="IReader" />.</param>
        /// <returns>One error for each failed check.</returns>
        private static List<Error> Validate(Archive archive)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(archive.Project.Name))
            {
                errors.Add(Error.Validation(KparErrorCodes.MissingName, "The .project.json has no name."));
            }

            if (string.IsNullOrWhiteSpace(archive.Project.Version))
            {
                errors.Add(Error.Validation(KparErrorCodes.MissingVersion, "The .project.json has no version."));
            }
            else if (!SemanticVersionRegex().IsMatch(archive.Project.Version))
            {
                errors.Add(Error.Validation(KparErrorCodes.InvalidVersion, $"The version '{archive.Project.Version}' is not SemVer 2.0.0."));
            }

            if (archive.Project.Usage != null && archive.Project.Usage.Any(usage => usage.Resource == null || usage.Resource.OriginalString == AbsentResource))
            {
                errors.Add(Error.Validation(KparErrorCodes.MissingUsageResource, "A usage entry of the .project.json has no resource."));
            }

            if (archive.Metadata.Created == default)
            {
                errors.Add(Error.Validation(KparErrorCodes.MissingCreated, "The .meta.json has no created timestamp."));
            }

            if (archive.Metadata.Index.Count == 0)
            {
                errors.Add(Error.Validation(KparErrorCodes.NoModels, "The .meta.json has no index or its index declares no root namespace."));
            }
            else
            {
                foreach (var duplicatePath in archive.Metadata.Index.Values.GroupBy(path => path).Where(group => group.Count() > 1))
                {
                    errors.Add(Error.Validation(KparErrorCodes.DuplicateModelPath, $"The archive path '{duplicatePath.Key}' is shared by several root namespaces."));
                }
            }

            foreach (var mismatch in archive.ChecksumMismatches)
            {
                errors.Add(Error.Validation(KparErrorCodes.ChecksumMismatch, $"The {mismatch.Algorithm} checksum of '{mismatch.Path}' is '{mismatch.Actual}' but the .meta.json declares '{mismatch.Expected}'."));
            }

            return errors;
        }

        /// <summary>
        /// Projects the descriptors of a validated archive onto a <see cref="KparManifest" />.
        /// </summary>
        /// <param name="archive">The validated archive.</param>
        /// <returns>The <see cref="KparManifest" />.</returns>
        private static KparManifest CreateManifest(Archive archive)
        {
            var manifest = new KparManifest
            {
                Name = archive.Project.Name,
                Description = archive.Project.Description,
                Version = archive.Project.Version,
                License = archive.Project.License,
                Maintainers = archive.Project.Maintainer ?? [],
                Website = archive.Project.Website,
                Topics = archive.Project.Topic ?? [],
                Metamodel = archive.Metadata.Metamodel?.ToString(),
                Created = archive.Metadata.Created,
                ModelPaths = archive.Metadata.Index
            };

            foreach (var usage in archive.Project.Usage ?? [])
            {
                manifest.Dependencies.Add(new KparDependency { Resource = usage.Resource.ToString(), VersionConstraint = usage.VersionConstraint });
            }

            foreach (var checksum in archive.Metadata.Checksum ?? [])
            {
                manifest.Checksums.Add(checksum.Key, new KparChecksum { Algorithm = checksum.Value.Algorithm.ToString(), Value = checksum.Value.Value });
            }

            return manifest;
        }
    }
}
