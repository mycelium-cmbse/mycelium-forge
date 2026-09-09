// ------------------------------------------------------------------------------------------------
// <copyright file="CsvRolesDataLoader.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.DataLoaders
{
    using System.Text;
    using System.Text.RegularExpressions;

    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;

    /// <summary>
    /// Loads a CSV file containing role and permission definitions and produces a <see cref="RolePermissionModel" />.
    /// </summary>
    public static partial class CsvRolesDataLoader
    {
        /// <summary>
        /// Loads and parses the specified CSV file into a <see cref="RolePermissionModel" />.
        /// </summary>
        /// <param name="csvPath">The path to the CSV file.</param>
        /// <returns>A parsed <see cref="RolePermissionModel" />.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="csvPath" /> is null or whitespace.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the CSV file does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the CSV format is invalid.</exception>
        public static RolePermissionModel Load(string csvPath)
        {
            var lines = ReadAndValidateLines(csvPath);

            var headerFields = ParseCsvLine(lines[0]);

            var hasInherits = headerFields.Count > 1 && string.Equals(headerFields[1].Trim(), nameof(RoleDefinition.Inherits), StringComparison.OrdinalIgnoreCase);
            var skipCount = hasInherits ? 3 : 2;

            var permissions = ParsePermissions(headerFields, skipCount);
            var roles = ParseRoles(lines, permissions, hasInherits, skipCount);

            ApplyInheritance(roles);

            var policies = BuildPolicies(roles, permissions);

            return new RolePermissionModel
            {
                Roles = roles,
                Permissions = permissions,
                Policies = policies
            };
        }

        /// <summary>
        /// Converts a CSV column header like "View all organizations" or "ViewAllOrganizations" to a PascalCase enum name.
        /// </summary>
        /// <param name="header">The raw CSV column header text.</param>
        /// <returns>The PascalCase enum name.</returns>
        public static string ConvertHeaderToEnumName(string header)
        {
            if (string.IsNullOrWhiteSpace(header))
            {
                return string.Empty;
            }

            var trimmed = header.Trim();

            if (!trimmed.Contains(' ') && !trimmed.Contains('-') && !trimmed.Contains('/'))
            {
                return trimmed;
            }

            var words = WordSplitRegex().Split(trimmed)
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .Select(w => char.ToUpperInvariant(w[0]) + w.Substring(1));

            return string.Concat(words);
        }

        /// <summary>
        /// Parses a single CSV line, handling quoted fields containing commas.
        /// </summary>
        /// <param name="line">The raw CSV line.</param>
        /// <returns>A list of parsed field values.</returns>
        public static List<string> ParseCsvLine(string line)
        {
            var result = new List<string>();
            var inQuotes = false;
            var currentField = new StringBuilder();
            var i = 0;

            while (i < line.Length)
            {
                var c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        currentField.Append('"');
                        i += 2;
                        continue;
                    }

                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(currentField.ToString());
                    currentField.Clear();
                }
                else
                {
                    currentField.Append(c);
                }

                i++;
            }

            result.Add(currentField.ToString());
            return result;
        }

        /// <summary>
        /// Gets the compiled regular expression used to split words.
        /// </summary>
        /// <returns>A compiled <see cref="Regex" /> instance.</returns>
        [GeneratedRegex(@"[\s/\-]+", RegexOptions.None, 1000)]
        private static partial Regex WordSplitRegex();

        /// <summary>
        /// Reads and validates the lines from the specified CSV file.
        /// </summary>
        /// <param name="csvPath">The path to the CSV file.</param>
        /// <returns>A list of non-empty lines from the CSV file.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="csvPath" /> is null or whitespace.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the CSV file does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the CSV contains fewer than two lines.</exception>
        private static List<string> ReadAndValidateLines(string csvPath)
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

            if (lines.Count < 2)
            {
                throw new InvalidOperationException("CSV must contain at least a header row and one data row.");
            }

            return lines;
        }

        /// <summary>
        /// Parses permission definitions from the CSV header fields.
        /// </summary>
        /// <param name="headerFields">The parsed header fields.</param>
        /// <param name="skipCount">The number of role metadata columns to skip.</param>
        /// <returns>A list of <see cref="PermissionDefinition" /> instances.</returns>
        private static List<PermissionDefinition> ParsePermissions(List<string> headerFields, int skipCount)
        {
            return headerFields.Skip(skipCount)
                .Select(header => new PermissionDefinition
                {
                    CsvHeader = header.Trim(),
                    EnumName = ConvertHeaderToEnumName(header.Trim())
                })
                .ToList();
        }

        /// <summary>
        /// Parses role definitions from the CSV lines.
        /// </summary>
        /// <param name="lines">The CSV lines.</param>
        /// <param name="permissions">The parsed permission definitions.</param>
        /// <param name="hasInherits">A value indicating whether the CSV has an Inherits column.</param>
        /// <param name="skipCount">The number of columns before permissions.</param>
        /// <returns>A list of parsed <see cref="RoleDefinition" /> instances.</returns>
        private static List<RoleDefinition> ParseRoles(List<string> lines, List<PermissionDefinition> permissions, bool hasInherits, int skipCount)
        {
            var roles = new List<RoleDefinition>();

            for (var i = 1; i < lines.Count; i++)
            {
                var fields = ParseCsvLine(lines[i]);

                if (fields.Count == 0)
                {
                    continue;
                }

                var roleName = fields[0].Trim();

                if (string.IsNullOrWhiteSpace(roleName))
                {
                    continue;
                }

                roles.Add(new RoleDefinition
                {
                    Name = roleName,
                    Inherits = ExtractInherits(fields, hasInherits),
                    Summary = ExtractSummary(fields, hasInherits),
                    GrantedPermissions = ExtractGrantedPermissions(fields, permissions, skipCount)
                });
            }

            return roles;
        }

        /// <summary>
        /// Extracts the inherits role name from the CSV row fields.
        /// </summary>
        /// <param name="fields">The parsed CSV fields.</param>
        /// <param name="hasInherits">A value indicating whether the CSV has an Inherits column.</param>
        /// <returns>The inherits role name, or empty string.</returns>
        private static string ExtractInherits(List<string> fields, bool hasInherits)
        {
            if (hasInherits && fields.Count > 1)
            {
                return fields[1].Trim();
            }

            return string.Empty;
        }

        /// <summary>
        /// Extracts the summary from the CSV row fields.
        /// </summary>
        /// <param name="fields">The parsed CSV fields.</param>
        /// <param name="hasInherits">A value indicating whether the CSV has an Inherits column.</param>
        /// <returns>The extracted summary string.</returns>
        private static string ExtractSummary(List<string> fields, bool hasInherits)
        {
            var summaryIndex = hasInherits ? 2 : 1;
            return fields.Count > summaryIndex ? fields[summaryIndex].Trim() : string.Empty;
        }

        /// <summary>
        /// Extracts granted permissions for a role row.
        /// </summary>
        /// <param name="fields">The parsed CSV fields.</param>
        /// <param name="permissions">The list of permission definitions.</param>
        /// <param name="skipCount">The number of columns before permissions.</param>
        /// <returns>A list of granted permission enum names.</returns>
        private static List<string> ExtractGrantedPermissions(List<string> fields, List<PermissionDefinition> permissions, int skipCount)
        {
            var granted = new List<string>();

            for (var j = 0; j < permissions.Count; j++)
            {
                var cellIndex = j + skipCount;
                var cellValue = cellIndex < fields.Count ? fields[cellIndex].Trim() : string.Empty;

                if (string.Equals(cellValue, "X", StringComparison.OrdinalIgnoreCase))
                {
                    granted.Add(permissions[j].EnumName);
                }
            }

            return granted;
        }

        /// <summary>
        /// Resolves and applies inherited permissions across roles.
        /// </summary>
        /// <param name="roles">The list of role definitions.</param>
        private static void ApplyInheritance(List<RoleDefinition> roles)
        {
            var roleByName = roles.ToDictionary(r => r.Name, StringComparer.OrdinalIgnoreCase);

            foreach (var role in roles)
            {
                if (!string.IsNullOrWhiteSpace(role.Inherits) && roleByName.TryGetValue(role.Inherits, out var parentRole))
                {
                    InheritPermissions(role, parentRole);
                }
            }
        }

        /// <summary>
        /// Inherits permissions from a parent role into a child role.
        /// </summary>
        /// <param name="role">The child role definition.</param>
        /// <param name="parentRole">The parent role definition.</param>
        private static void InheritPermissions(RoleDefinition role, RoleDefinition parentRole)
        {
            foreach (var parentPermission in parentRole.GrantedPermissions.Where(parentPermission => !role.GrantedPermissions.Contains(parentPermission)))
            {
                role.GrantedPermissions.Add(parentPermission);
            }
        }

        /// <summary>
        /// Builds policy mappings from permissions and roles.
        /// </summary>
        /// <param name="roles">The list of role definitions.</param>
        /// <param name="permissions">The list of permission definitions.</param>
        /// <returns>A list of <see cref="PolicyMapping" /> instances.</returns>
        private static List<PolicyMapping> BuildPolicies(List<RoleDefinition> roles, List<PermissionDefinition> permissions)
        {
            return permissions.Select(p => new PolicyMapping
            {
                PermissionEnumName = p.EnumName,
                AllowedRoles =
                [
                    .. roles
                        .Where(r => r.GrantedPermissions.Contains(p.EnumName))
                        .Select(r => r.Name)
                ]
            }).ToList();
        }
    }
}
