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
    public class CsvRolesDataLoader
    {
        /// <summary>
        /// Loads and parses the specified CSV file into a <see cref="RolePermissionModel" />.
        /// </summary>
        /// <param name="csvPath">The path to the CSV file.</param>
        /// <returns>A parsed <see cref="RolePermissionModel" />.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="csvPath" /> is null or whitespace.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the CSV file does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the CSV format is invalid.</exception>
        public RolePermissionModel Load(string csvPath)
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

            var headerFields = ParseCsvLine(lines[0]);

            var hasInherits = headerFields.Count > 1 && string.Equals(headerFields[1].Trim(), nameof(RoleDefinition.Inherits), StringComparison.OrdinalIgnoreCase);
            var skipCount = hasInherits ? 3 : 2;

            // Columns after role metadata (Role, [Inherits], Summary) are permission headers
            var permissionHeaders = headerFields.Skip(skipCount).ToList();

            var permissions = permissionHeaders
                .Select(header => new PermissionDefinition
                {
                    CsvHeader = header.Trim(),
                    EnumName = ConvertHeaderToEnumName(header.Trim())
                })
                .ToList();

            var roles = new List<RoleDefinition>();

            for (var i = 1; i < lines.Count; i++)
            {
                var fields = ParseCsvLine(lines[i]);

                var roleName = fields[0].Trim();

                if (string.IsNullOrWhiteSpace(roleName))
                {
                    continue;
                }

                var inherits = hasInherits && fields.Count > 1 ? fields[1].Trim() : string.Empty;

                var summary = hasInherits
                    ? fields.Count > 2 ? fields[2].Trim() : string.Empty
                    : fields.Count > 1
                        ? fields[1].Trim()
                        : string.Empty;

                var granted = new List<string>();

                for (var j = 0; j < permissionHeaders.Count; j++)
                {
                    var cellIndex = j + skipCount;
                    var cellValue = cellIndex < fields.Count ? fields[cellIndex].Trim() : string.Empty;

                    if (string.Equals(cellValue, "X", StringComparison.OrdinalIgnoreCase))
                    {
                        granted.Add(permissions[j].EnumName);
                    }
                }

                roles.Add(new RoleDefinition
                {
                    Name = roleName,
                    Inherits = inherits,
                    Summary = summary,
                    GrantedPermissions = granted
                });
            }

            var roleByName = roles.ToDictionary(r => r.Name, StringComparer.OrdinalIgnoreCase);

            foreach (var role in roles)
            {
                if (!string.IsNullOrWhiteSpace(role.Inherits) && roleByName.TryGetValue(role.Inherits, out var parentRole))
                {
                    foreach (var parentPermission in parentRole.GrantedPermissions)
                    {
                        if (!role.GrantedPermissions.Contains(parentPermission))
                        {
                            role.GrantedPermissions.Add(parentPermission);
                        }
                    }
                }
            }

            var policies = permissions.Select(p => new PolicyMapping
            {
                PermissionEnumName = p.EnumName,
                AllowedRoles =
                [
                    .. roles
                        .Where(r => r.GrantedPermissions.Contains(p.EnumName))
                        .Select(r => r.Name)
                ]
            }).ToList();

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

            var words = Regex.Split(trimmed, @"[\s/\-]+")
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

            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        currentField.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
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
            }

            result.Add(currentField.ToString());
            return result;
        }
    }
}
