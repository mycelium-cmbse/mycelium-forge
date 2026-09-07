// ------------------------------------------------------------------------------------------------
// <copyright file="DefaultResourceLocator.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.DataLoaders
{
    /// <summary>
    /// Locates default generator resource files across conventional runtime and project directories.
    /// </summary>
    public static class DefaultResourceLocator
    {
        /// <summary>
        /// Attempts to locate the specified resource file in common runtime and development directories.
        /// </summary>
        /// <param name="resourceFileName">The name of the resource file to locate (e.g. <c>forge-entity-permissions.csv</c>).</param>
        /// <param name="resolvedPath">
        /// When this method returns <c>true</c>, contains the full path of the located resource;
        /// otherwise empty string.
        /// </param>
        /// <returns><c>true</c> if the resource was located; otherwise <c>false</c>.</returns>
        public static bool TryLocate(string resourceFileName, out string resolvedPath)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var candidates = new[]
            {
                Path.Combine(baseDirectory, "Resources", resourceFileName),
                Path.Combine(baseDirectory, resourceFileName),
                Path.GetFullPath(Path.Combine(baseDirectory, "../../../../Mycelium.Forge.Generator/Resources", resourceFileName)),
                Path.GetFullPath(Path.Combine(baseDirectory, "../../../Mycelium.Forge.Generator/Resources", resourceFileName))
            };

            foreach (var candidate in candidates.Where(File.Exists))
            {
                resolvedPath = candidate;
                return true;
            }

            resolvedPath = string.Empty;
            return false;
        }

        /// <summary>
        /// Attempts to locate the default entity permissions CSV resource and executes the specified loader action if found.
        /// </summary>
        /// <param name="loadAction">The action to execute with the resolved CSV path.</param>
        public static void TryLoadDefaultEntityPermissions(Action<string> loadAction)
        {
            ArgumentNullException.ThrowIfNull(loadAction);

            if (TryLocate("forge-entity-permissions.csv", out var resourcePath))
            {
                loadAction(resourcePath);
            }
        }
    }
}
