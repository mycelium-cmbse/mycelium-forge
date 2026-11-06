// ------------------------------------------------------------------------------------------------
// <copyright file="SemanticVersionComparer.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using Mycelium.Forge.Extractors.Extractors;

    /// <summary>
    /// Implements SemVer 2.0.0 precedence comparison for semantic version strings.
    /// </summary>
    /// <remarks>
    /// Maintained as a zero-dependency implementation complying with SemVer 2.0.0 specification section 11 precedence rules,
    /// avoiding external package dependencies (such as Semver or NuGet.Versioning) for core release ordering and validation.
    /// </remarks>
    public static class SemanticVersionComparer
    {
        /// <summary>
        /// Compares two SemVer 2.0.0 formatted version strings for precedence.
        /// </summary>
        /// <param name="versionA">The first semantic version string.</param>
        /// <param name="versionB">The second semantic version string.</param>
        /// <returns>
        /// A signed number indicating relative values: less than zero if <paramref name="versionA" /> precedes
        /// <paramref name="versionB" />; zero if equal; greater than zero if <paramref name="versionA" /> succeeds
        /// <paramref name="versionB" />.
        /// </returns>
        public static int Compare(string versionA, string versionB)
        {
            if (string.Equals(versionA, versionB, StringComparison.Ordinal))
            {
                return 0;
            }

            var matchA = KparManifestExtractor.SemanticVersionRegex().Match(versionA ?? string.Empty);
            var matchB = KparManifestExtractor.SemanticVersionRegex().Match(versionB ?? string.Empty);

            if (!matchA.Success || !matchB.Success)
            {
                return string.Compare(versionA, versionB, StringComparison.OrdinalIgnoreCase);
            }

            var majorComparison = long.Parse(matchA.Groups[1].Value).CompareTo(long.Parse(matchB.Groups[1].Value));

            if (majorComparison != 0)
            {
                return majorComparison;
            }

            var minorComparison = long.Parse(matchA.Groups[2].Value).CompareTo(long.Parse(matchB.Groups[2].Value));

            if (minorComparison != 0)
            {
                return minorComparison;
            }

            var patchComparison = long.Parse(matchA.Groups[3].Value).CompareTo(long.Parse(matchB.Groups[3].Value));

            if (patchComparison != 0)
            {
                return patchComparison;
            }

            var preReleaseA = matchA.Groups[4].Success ? matchA.Groups[4].Value : null;
            var preReleaseB = matchB.Groups[4].Success ? matchB.Groups[4].Value : null;

            return ComparePreRelease(preReleaseA, preReleaseB);
        }

        /// <summary>
        /// Determines whether the new version represents a major version bump relative to the previous version.
        /// </summary>
        /// <param name="newVersion">The new semantic version string.</param>
        /// <param name="previousVersion">The previous semantic version string.</param>
        /// <returns><see langword="true" /> if the major version number has increased; otherwise, <see langword="false" />.</returns>
        public static bool IsMajorBump(string newVersion, string previousVersion)
        {
            var matchNew = KparManifestExtractor.SemanticVersionRegex().Match(newVersion ?? string.Empty);
            var matchPrev = KparManifestExtractor.SemanticVersionRegex().Match(previousVersion ?? string.Empty);

            if (!matchNew.Success || !matchPrev.Success)
            {
                return false;
            }

            return long.Parse(matchNew.Groups[1].Value) > long.Parse(matchPrev.Groups[1].Value);
        }

        /// <summary>
        /// Compares the pre-release identifier strings according to SemVer 2.0.0 precedence rules.
        /// </summary>
        /// <param name="preReleaseA">The optional pre-release string for version A.</param>
        /// <param name="preReleaseB">The optional pre-release string for version B.</param>
        /// <returns>A signed integer indicating precedence.</returns>
        private static int ComparePreRelease(string preReleaseA, string preReleaseB)
        {
            if (preReleaseA == null && preReleaseB == null)
            {
                return 0;
            }

            if (preReleaseA == null)
            {
                return 1;
            }

            if (preReleaseB == null)
            {
                return -1;
            }

            var partsA = preReleaseA.Split('.');
            var partsB = preReleaseB.Split('.');
            var minLength = Math.Min(partsA.Length, partsB.Length);

            for (var index = 0; index < minLength; index++)
            {
                var partComparison = CompareIdentifier(partsA[index], partsB[index]);

                if (partComparison != 0)
                {
                    return partComparison;
                }
            }

            return partsA.Length.CompareTo(partsB.Length);
        }

        /// <summary>
        /// Compares two individual dot-separated pre-release identifiers.
        /// </summary>
        /// <param name="identifierA">The first identifier part.</param>
        /// <param name="identifierB">The second identifier part.</param>
        /// <returns>A signed integer indicating precedence.</returns>
        private static int CompareIdentifier(string identifierA, string identifierB)
        {
            var isNumericA = long.TryParse(identifierA, out var numA);
            var isNumericB = long.TryParse(identifierB, out var numB);

            if (isNumericA && isNumericB)
            {
                return numA.CompareTo(numB);
            }

            if (isNumericA)
            {
                return -1;
            }

            if (isNumericB)
            {
                return 1;
            }

            return string.Compare(identifierA, identifierB, StringComparison.Ordinal);
        }
    }
}
