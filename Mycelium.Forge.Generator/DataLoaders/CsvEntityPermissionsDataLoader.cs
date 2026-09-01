// ------------------------------------------------------------------------------------------------
// <copyright file="CsvEntityPermissionsDataLoader.cs" company="Starion Group S.A.">
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
    /// Loads the declarative entity CRUD permissions and ownership properties CSV.
    /// </summary>
    public class CsvEntityPermissionsDataLoader
    {
        /// <summary>
        /// Loads the specified CSV file and returns a dictionary of <see cref="EntityPermissionDefinition" /> keyed by entity
        /// name.
        /// </summary>
        /// <param name="csvPath">The path to the entity permissions CSV file.</param>
        /// <returns>A dictionary of entity permission definitions.</returns>
        public Dictionary<string, EntityPermissionDefinition> Load(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
            {
                throw new ArgumentException("The CSV file path must be provided.", nameof(csvPath));
            }

            if (!File.Exists(csvPath))
            {
                throw new FileNotFoundException($"CSV file not found: {csvPath}", csvPath);
            }

            var lines = File.ReadAllLines(csvPath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();

            var result = new Dictionary<string, EntityPermissionDefinition>();

            for (var i = 1; i < lines.Count; i++)
            {
                var fields = CsvRolesDataLoader.ParseCsvLine(lines[i]);

                if (fields.Count < 5)
                {
                    continue;
                }

                var entityName = fields[0].Trim();

                if (string.IsNullOrWhiteSpace(entityName))
                {
                    continue;
                }

                var definition = new EntityPermissionDefinition
                {
                    EntityName = entityName,
                    CreatePermission = fields.Count > 1 ? fields[1].Trim() : string.Empty,
                    ReadPermission = fields.Count > 2 ? fields[2].Trim() : string.Empty,
                    UpdatePermission = fields.Count > 3 ? fields[3].Trim() : string.Empty,
                    DeletePermission = fields.Count > 4 ? fields[4].Trim() : string.Empty,
                    OwnerProperty = fields.Count > 5 ? fields[5].Trim() : string.Empty,
                    MaintainerProperty = fields.Count > 6 ? fields[6].Trim() : string.Empty,
                    VisibilityProperty = fields.Count > 7 ? fields[7].Trim() : string.Empty
                };

                result.Add(entityName, definition);
            }

            return result;
        }
    }
}
