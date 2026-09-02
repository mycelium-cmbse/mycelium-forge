// ------------------------------------------------------------------------------------------------
// <copyright file="CsvEntityBehaviorsDataLoader.cs" company="Starion Group S.A.">
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
    /// Loads specialized entity permission behavior configurations from a CSV file.
    /// </summary>
    public class CsvEntityBehaviorsDataLoader
    {
        /// <summary>
        /// Loads the entity behavior definitions from the specified CSV file.
        /// </summary>
        /// <param name="csvPath">The path to the entity behaviors CSV file.</param>
        /// <returns>A dictionary mapping entity names to their behavior definitions.</returns>
        public Dictionary<string, EntityBehaviorDefinition> Load(string csvPath)
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
            var result = new Dictionary<string, EntityBehaviorDefinition>();

            for (var i = 1; i < lines.Length; i++)
            {
                var fields = CsvRolesDataLoader.ParseCsvLine(lines[i]);

                if (fields.Count < 2)
                {
                    continue;
                }

                var entityName = fields[0].Trim();

                if (string.IsNullOrWhiteSpace(entityName))
                {
                    continue;
                }

                var behaviorType = fields[1].Trim();
                var configRaw = fields.Count > 2 ? fields[2].Trim() : string.Empty;
                var configDict = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(configRaw))
                {
                    var pairs = configRaw.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    foreach (var pair in pairs)
                    {
                        var parts = pair.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                        if (parts.Length == 2)
                        {
                            configDict.Add(parts[0], parts[1]);
                        }
                        else if (parts.Length == 1)
                        {
                            configDict.Add(parts[0], "true");
                        }
                    }
                }

                var definition = new EntityBehaviorDefinition
                {
                    EntityName = entityName,
                    BehaviorType = behaviorType,
                    Configuration = configDict
                };

                result.Add(entityName, definition);
            }

            return result;
        }
    }
}
