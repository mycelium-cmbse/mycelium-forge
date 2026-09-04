// ------------------------------------------------------------------------------------------------
// <copyright file="JsonEntityBehaviorsDataLoader.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.DataLoaders
{
    using System.Text.Json;

    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;

    /// <summary>
    /// Loads specialized entity permission behavior configurations from a JSON file.
    /// </summary>
    public class JsonEntityBehaviorsDataLoader
    {
        /// <summary>
        /// The delimiter used to join list values.
        /// </summary>
        private const string ListDelimiter = ",";

        /// <summary>
        /// Loads the entity behavior definitions from the specified JSON file.
        /// </summary>
        /// <param name="jsonPath">The path to the entity behaviors JSON file.</param>
        /// <returns>A dictionary mapping entity names to their behavior definitions.</returns>
        public Dictionary<string, EntityBehaviorDefinition> Load(string jsonPath)
        {
            if (string.IsNullOrWhiteSpace(jsonPath))
            {
                throw new ArgumentException("JSON file path cannot be null or empty.", nameof(jsonPath));
            }

            if (!File.Exists(jsonPath))
            {
                throw new FileNotFoundException($"JSON file not found at: {jsonPath}", jsonPath);
            }

            var jsonText = File.ReadAllText(jsonPath);
            List<EntityBehaviorItem> behaviorItems;

            if (jsonText.TrimStart().StartsWith("["))
            {
                behaviorItems = JsonSerializer.Deserialize<List<EntityBehaviorItem>>(jsonText) ?? [];
            }
            else
            {
                var document = JsonSerializer.Deserialize<ForgeEntityBehaviorsDocument>(jsonText);
                behaviorItems = document?.Behaviors ?? [];
            }

            var result = new Dictionary<string, EntityBehaviorDefinition>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in behaviorItems)
            {
                var entityName = !string.IsNullOrWhiteSpace(item.Entity) ? item.Entity : item.EntityName;

                if (string.IsNullOrWhiteSpace(entityName))
                {
                    continue;
                }

                var definition = new EntityBehaviorDefinition
                {
                    EntityName = entityName,
                    BehaviorType = item.BehaviorType
                };

                foreach (var pair in item.Configuration)
                {
                    definition.Configuration.Add(pair.Key, ResolveConfigurationValue(pair.Value));
                }

                result.Add(entityName, definition);
            }

            return result;
        }

        /// <summary>
        /// Resolves a <see cref="JsonElement" /> configuration value into a string.
        /// </summary>
        /// <param name="element">The <see cref="JsonElement" /> to resolve.</param>
        /// <returns>The resolved string representation.</returns>
        private static string ResolveConfigurationValue(JsonElement element)
        {
            if (element.ValueKind != JsonValueKind.Array)
            {
                var stringValue = element.GetString();
                return stringValue ?? element.ToString();
            }

            var values = new List<string>();

            foreach (var arrayItem in element.EnumerateArray())
            {
                var stringValue = arrayItem.GetString();

                if (!string.IsNullOrWhiteSpace(stringValue))
                {
                    values.Add(stringValue.Trim());
                }
            }

            return string.Join(ListDelimiter, values);
        }
    }
}
