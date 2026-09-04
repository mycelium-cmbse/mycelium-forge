// ------------------------------------------------------------------------------------------------
// <copyright file="CsvPropertyPermissionsDataLoader.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.DataLoaders
{
    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;

    /// <summary>
    /// Loads property-level permission definitions from a CSV file.
    /// </summary>
    public class CsvPropertyPermissionsDataLoader
    {
        /// <summary>
        /// Loads the property permission definitions from the specified CSV file, grouped by entity name.
        /// </summary>
        /// <param name="csvPath">The path to the property permissions CSV file.</param>
        /// <returns>A dictionary mapping entity names to their list of property permission definitions.</returns>
        public Dictionary<string, List<PropertyPermissionDefinition>> Load(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
            {
                throw new ArgumentException("CSV file path cannot be null or empty.", nameof(csvPath));
            }

            if (!File.Exists(csvPath))
            {
                throw new FileNotFoundException($"CSV file not found at: {csvPath}", csvPath);
            }

            var lines = File.ReadAllLines(csvPath);
            var result = new Dictionary<string, List<PropertyPermissionDefinition>>();

            for (var i = 1; i < lines.Length; i++)
            {
                var fields = CsvRolesDataLoader.ParseCsvLine(lines[i]);

                if (fields.Count < 3)
                {
                    continue;
                }

                var entityName = fields[0].Trim();

                if (string.IsNullOrWhiteSpace(entityName))
                {
                    continue;
                }

                var definition = new PropertyPermissionDefinition
                {
                    EntityName = entityName,
                    Property = fields.Count > 1 ? fields[1].Trim() : string.Empty,
                    RequiredPermission = fields.Count > 2 ? fields[2].Trim() : string.Empty,
                    Operation = fields.Count > 3 ? fields[3].Trim() : "Update"
                };

                if (!result.TryGetValue(entityName, out var list))
                {
                    list = [];
                    result.Add(entityName, list);
                }

                list.Add(definition);
            }

            return result;
        }
    }
}
