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
            var lines = ReadLines(csvPath);
            var result = new Dictionary<string, EntityPermissionDefinition>();

            for (var i = 1; i < lines.Count; i++)
            {
                var fields = CsvRolesDataLoader.ParseCsvLine(lines[i]);

                if (TryParseRow(fields, out var definition))
                {
                    result.Add(definition.EntityName, definition);
                }
            }

            return result;
        }

        /// <summary>
        /// Reads all non-empty lines from the specified CSV file.
        /// </summary>
        /// <param name="csvPath">The path to the CSV file.</param>
        /// <returns>A list of non-empty lines.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="csvPath" /> is null or whitespace.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the CSV file does not exist.</exception>
        private static List<string> ReadLines(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
            {
                throw new ArgumentException("The CSV file path must be provided.", nameof(csvPath));
            }

            if (!File.Exists(csvPath))
            {
                throw new FileNotFoundException($"CSV file not found: {csvPath}", csvPath);
            }

            return File.ReadAllLines(csvPath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();
        }

        /// <summary>
        /// Attempts to parse an <see cref="EntityPermissionDefinition" /> from the CSV row fields.
        /// </summary>
        /// <param name="fields">The parsed CSV row fields.</param>
        /// <param name="definition">When this method returns, contains the parsed definition, or null if parsing failed.</param>
        /// <returns>True if the row was successfully parsed; otherwise false.</returns>
        private static bool TryParseRow(List<string> fields, out EntityPermissionDefinition definition)
        {
            if (fields.Count < 5 || string.IsNullOrWhiteSpace(fields[0]))
            {
                definition = null;
                return false;
            }

            definition = new EntityPermissionDefinition
            {
                EntityName = fields[0].Trim(),
                CreatePermission = GetField(fields, 1),
                ReadPermission = GetField(fields, 2),
                UpdatePermission = GetField(fields, 3),
                DeletePermission = GetField(fields, 4),
                OwnerProperty = GetField(fields, 5),
                MaintainerProperty = GetField(fields, 6),
                VisibilityProperty = GetField(fields, 7)
            };

            return true;
        }

        /// <summary>
        /// Gets the trimmed field value at the specified index or an empty string if out of bounds.
        /// </summary>
        /// <param name="fields">The parsed CSV row fields.</param>
        /// <param name="index">The zero-based field index.</param>
        /// <returns>The trimmed field value or empty string.</returns>
        private static string GetField(List<string> fields, int index)
        {
            return index < fields.Count ? fields[index].Trim() : string.Empty;
        }
    }
}
