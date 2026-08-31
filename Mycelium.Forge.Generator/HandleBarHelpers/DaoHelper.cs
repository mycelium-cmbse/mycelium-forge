// ------------------------------------------------------------------------------------------------
// <copyright file="DaoHelper.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers
{
    using System.Text;

    using HandlebarsDotNet;

    using Mycelium.Forge.Generator.Extensions;

    using uml4net.Classification;
    using uml4net.CommonStructure;
    using uml4net.Extensions;
    using uml4net.SimpleClassifiers;
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// A Handlebars block helper for Data Access Object (DAO) code generation.
    /// </summary>
    public static class DaoHelper
    {
        /// <summary>
        /// Registers the DAO Handlebars helpers with the specified Handlebars context.
        /// </summary>
        /// <param name="handlebars">The <see cref="IHandlebars" /> context.</param>
        public static void RegisterDaoHelper(this IHandlebars handlebars)
        {
            ArgumentNullException.ThrowIfNull(handlebars);

            handlebars.RegisterHelper("Dao.CreateAsyncWriteValueTypeAndSingleReferenceProperties", CreateAsyncWriteValueTypeAndSingleReferenceProperties);
            handlebars.RegisterHelper("Dao.CreateAndUpdateAsyncWriteCommandParameters", CreateAndUpdateAsyncWriteCommandParameters);
            handlebars.RegisterHelper("Dao.CreateAsyncAppendMultiReferencePropertiesToSqlBuilder", CreateAsyncAppendMultiReferencePropertiesToSqlBuilder);
            handlebars.RegisterHelper("Dao.UpdateAsyncWriteValueTypeAndSingleReferenceProperties", UpdateAsyncWriteValueTypeAndSingleReferenceProperties);
            handlebars.RegisterHelper("Dao.UpdateAsyncAppendMultiReferencePropertiesToSqlBuilder", UpdateAsyncAppendMultiReferencePropertiesToSqlBuilder);
            handlebars.RegisterHelper("Dao.ReadAsyncWriteReadSQL", ReadAsyncWriteReadSql);
            handlebars.RegisterHelper("Dao.WriteMapToDto", WriteMapToDto);
        }

        /// <summary>
        /// Writes the INSERT statements for the root Thing and superclass tables for <c>CreateAsync</c>.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" /> containing an <see cref="IClass" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void CreateAsyncWriteValueTypeAndSingleReferenceProperties(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("context is supposed to be an IClass", nameof(context));
            }

            var valueTypeAndSingleReferenceProperties = new StringBuilder();

            valueTypeAndSingleReferenceProperties.AppendLine("                -- Thing");
            valueTypeAndSingleReferenceProperties.AppendLine("                INSERT INTO \"Forge\".\"Thing\" (\"id\", \"classKind\", \"data\")");
            valueTypeAndSingleReferenceProperties.AppendLine("                VALUES (@id, @classKind, @data);");

            var allSuperClassesThatDeriveFromThing = @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(x => x.QueryDerivesFrom("Thing"))
                .Reverse()
                .ToList();

            foreach (var thingDerivedClass in allSuperClassesThatDeriveFromThing)
            {
                var sqlColumns = "\"id\"";
                var sqlParameters = "@id";

                var singleReferenceProperties = thingDerivedClass
                    .QuerySqlSingleReferenceProperties()
                    .Distinct()
                    .ToList();

                foreach (var singleReferenceProperty in singleReferenceProperties)
                {
                    sqlColumns += $", \"{singleReferenceProperty.QuerySqlAttributeName()}\"";
                    sqlParameters += $", @{singleReferenceProperty.QuerySqlAttributeName()}";
                }

                valueTypeAndSingleReferenceProperties.AppendLine();
                valueTypeAndSingleReferenceProperties.AppendLine($"                -- {thingDerivedClass.Name}");
                valueTypeAndSingleReferenceProperties.AppendLine($"                INSERT INTO \"Forge\".\"{thingDerivedClass.Name}\" ({sqlColumns})");
                valueTypeAndSingleReferenceProperties.AppendLine($"                VALUES ({sqlParameters});");
            }

            writer.WriteSafeString(valueTypeAndSingleReferenceProperties);
        }

        /// <summary>
        /// Writes NpgsqlCommand parameter bindings for all single reference properties.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" /> containing an <see cref="IClass" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void CreateAndUpdateAsyncWriteCommandParameters(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("context is supposed to be an IClass", nameof(context));
            }

            var allSuperClassesThatDeriveFromThing = @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(x => x.QueryDerivesFrom("Thing"))
                .Reverse()
                .ToList();

            var commandParametersToWrite = new StringBuilder();
            var dtoName = @class.Name.LowerCaseFirstLetter();

            foreach (var thingDerivedClass in allSuperClassesThatDeriveFromThing)
            {
                var singleReferenceProperties = thingDerivedClass
                    .QuerySqlSingleReferenceProperties()
                    .Distinct()
                    .ToList();

                foreach (var singleReferenceProperty in singleReferenceProperties)
                {
                    var paramName = singleReferenceProperty.QuerySqlAttributeName();
                    var propCSharpName = singleReferenceProperty.Name.CapitalizeFirstLetter();

                    if (singleReferenceProperty.QueryIsNullable())
                    {
                        commandParametersToWrite.AppendLine(
                            $"                command.Parameters.Add(new NpgsqlParameter(\"@{paramName}\", NpgsqlDbType.Uuid) {{ Value = (object){dtoName}.{propCSharpName} ?? DBNull.Value }});");
                    }
                    else
                    {
                        commandParametersToWrite.AppendLine(
                            $"                command.Parameters.Add(new NpgsqlParameter(\"@{paramName}\", NpgsqlDbType.Uuid) {{ Value = {dtoName}.{propCSharpName} }});");
                    }
                }
            }

            if (commandParametersToWrite.Length > 0)
            {
                writer.WriteSafeString("\r\n");
                writer.WriteSafeString(commandParametersToWrite.ToString().TrimEnd());
            }
        }

        /// <summary>
        /// Appends multi-reference property INSERT statements to the SQL builder in <c>CreateAsync</c>.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" /> containing an <see cref="IClass" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void CreateAsyncAppendMultiReferencePropertiesToSqlBuilder(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("context is supposed to be an IClass", nameof(context));
            }

            var allSuperClassesThatDeriveFromThing = @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(x => x.QueryDerivesFrom("Thing"))
                .Reverse()
                .ToList();

            var result = new StringBuilder();
            var dtoName = @class.Name.LowerCaseFirstLetter();

            foreach (var thingDerivedClass in allSuperClassesThatDeriveFromThing)
            {
                var properties = thingDerivedClass
                    .QueryPropertiesThatAreOwnedAndUsableAndInheritedFromDirectNonDerivesFromClasses("Thing")
                    .Where(x => x.QueryIsMemberOfManyToMany())
                    .ToList();

                if (properties.Count == 0)
                {
                    continue;
                }

                result.AppendLine($"                // {thingDerivedClass.Name} Multi Reference Properties");

                foreach (var property in properties)
                {
                    var propName = property.Name.LowerCaseFirstLetter();
                    var propCSharpName = property.Name.CapitalizeFirstLetter();
                    var manyToManyTableName = property.QueryManyToManyTableName();
                    var sourcePropName = property.QueryManyToManySourcePropertyName();
                    var targetPropName = property.QueryManyToManyTargetPropertyName();

                    result.AppendLine($"                var {propName}Counter = 0;");
                    result.AppendLine();
                    result.AppendLine($"                foreach (var item in {dtoName}.{propCSharpName})");
                    result.AppendLine("                {");
                    result.AppendLine($"                    sqlBuilder.AppendLine(\"INSERT INTO \\\"Forge\\\".\\\"{manyToManyTableName}\\\"(\\\"{sourcePropName}\\\",\\\"{targetPropName}\\\")\");");
                    result.AppendLine($"                    sqlBuilder.AppendLine($\"VALUES (@id, @{propName}{{{propName}Counter}});\");");
                    result.AppendLine("                    sqlBuilder.AppendLine(\"\");");
                    result.AppendLine();
                    result.AppendLine($"                    command.Parameters.Add(new NpgsqlParameter($\"@{propName}{{{propName}Counter}}\", NpgsqlDbType.Uuid) {{ Value = item }});");
                    result.AppendLine($"                    {propName}Counter++;");
                    result.AppendLine("                }");
                    result.AppendLine();
                }
            }

            if (result.Length > 0)
            {
                writer.WriteSafeString("\r\n\r\n");
                writer.WriteSafeString(result.ToString().TrimEnd());
            }
        }

        /// <summary>
        /// Writes UPDATE statements for the root Thing and superclass tables for <c>UpdateAsync</c>.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" /> containing an <see cref="IClass" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void UpdateAsyncWriteValueTypeAndSingleReferenceProperties(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("context is supposed to be an IClass", nameof(context));
            }

            var valueTypeAndSingleReferenceProperties = new StringBuilder();

            valueTypeAndSingleReferenceProperties.AppendLine("                -- Thing");
            valueTypeAndSingleReferenceProperties.AppendLine("                UPDATE \"Forge\".\"Thing\" SET \"data\" = @data WHERE \"id\" = @id;");

            var allSuperClassesThatDeriveFromThing = @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(x => x.QueryDerivesFrom("Thing"))
                .Reverse()
                .ToList();

            foreach (var thingDerivedClass in allSuperClassesThatDeriveFromThing)
            {
                var singleReferenceProperties = thingDerivedClass
                    .QuerySqlSingleReferenceProperties()
                    .Distinct()
                    .ToList();

                var sqlSetters = singleReferenceProperties
                    .Select(prop => $"\"{prop.QuerySqlAttributeName()}\" = @{prop.QuerySqlAttributeName()}")
                    .ToList();

                if (sqlSetters.Count != 0)
                {
                    valueTypeAndSingleReferenceProperties.AppendLine();
                    valueTypeAndSingleReferenceProperties.AppendLine($"                -- {thingDerivedClass.Name}");
                    valueTypeAndSingleReferenceProperties.AppendLine($"                UPDATE \"Forge\".\"{thingDerivedClass.Name}\"");
                    valueTypeAndSingleReferenceProperties.Append("                SET");

                    for (var i = 0; i < sqlSetters.Count; i++)
                    {
                        valueTypeAndSingleReferenceProperties.AppendLine();
                        valueTypeAndSingleReferenceProperties.Append($"                    {sqlSetters[i]}{(i < sqlSetters.Count - 1 ? "," : string.Empty)}");
                    }

                    valueTypeAndSingleReferenceProperties.AppendLine();
                    valueTypeAndSingleReferenceProperties.AppendLine("                WHERE \"id\" = @id;");
                }
            }

            writer.WriteSafeString(valueTypeAndSingleReferenceProperties);
        }

        /// <summary>
        /// Appends multi-reference property MERGE/DELETE statements to the SQL builder in <c>UpdateAsync</c>.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" /> containing an <see cref="IClass" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void UpdateAsyncAppendMultiReferencePropertiesToSqlBuilder(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("context is supposed to be an IClass", nameof(context));
            }

            var allSuperClassesThatDeriveFromThing = @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(x => x.QueryDerivesFrom("Thing"))
                .Reverse()
                .ToList();

            var result = new StringBuilder();
            var dtoName = @class.Name.LowerCaseFirstLetter();

            foreach (var thingDerivedClass in allSuperClassesThatDeriveFromThing)
            {
                var properties = thingDerivedClass
                    .QueryPropertiesThatAreOwnedAndUsableAndInheritedFromDirectNonDerivesFromClasses("Thing")
                    .Where(x => x.QueryIsMemberOfManyToMany())
                    .ToList();

                if (properties.Count == 0)
                {
                    continue;
                }

                result.AppendLine($"                // {thingDerivedClass.Name} Multi Reference Properties");

                foreach (var property in properties)
                {
                    var propName = property.Name.LowerCaseFirstLetter();
                    var propCSharpName = property.Name.CapitalizeFirstLetter();
                    var manyToManyTableName = property.QueryManyToManyTableName();
                    var sourcePropName = property.QueryManyToManySourcePropertyName();
                    var targetPropName = property.QueryManyToManyTargetPropertyName();

                    result.AppendLine();
                    result.AppendLine("                sqlBuilder.AppendLine(");
                    result.AppendLine("                \"\"\"");
                    result.AppendLine($"                WITH \"wanted_{manyToManyTableName}\" AS (");
                    result.AppendLine($"                    SELECT @id AS \"{sourcePropName}\", unnest(@{propName}::uuid[]) AS \"{targetPropName}\"");
                    result.AppendLine("                )");
                    result.AppendLine($"                MERGE INTO \"Forge\".\"{manyToManyTableName}\" \"current_{manyToManyTableName}\"");
                    result.AppendLine($"                    USING \"wanted_{manyToManyTableName}\"");
                    result.AppendLine($"                    ON \"current_{manyToManyTableName}\".\"{sourcePropName}\" = \"wanted_{manyToManyTableName}\".\"{sourcePropName}\" AND \"current_{manyToManyTableName}\".\"{targetPropName}\" = \"wanted_{manyToManyTableName}\".\"{targetPropName}\"");
                    result.AppendLine("                WHEN NOT MATCHED THEN");
                    result.AppendLine($"                    INSERT (\"{sourcePropName}\", \"{targetPropName}\")");
                    result.AppendLine($"                        VALUES (\"wanted_{manyToManyTableName}\".\"{sourcePropName}\", \"wanted_{manyToManyTableName}\".\"{targetPropName}\");");
                    result.AppendLine();
                    result.AppendLine($"                DELETE FROM \"Forge\".\"{manyToManyTableName}\"");
                    result.AppendLine($"                    WHERE  \"{sourcePropName}\" = @id");
                    result.AppendLine($"                      AND  \"{targetPropName}\" NOT IN (SELECT unnest(@{propName}::uuid[]));");
                    result.AppendLine("                \"\"\");");
                    result.AppendLine();
                    result.AppendLine($"                command.Parameters.Add(new NpgsqlParameter(\"@{propName}\", (NpgsqlDbType)((int)NpgsqlDbType.Array | (int)NpgsqlDbType.Uuid)) {{ Value = ({dtoName}.{propCSharpName} ?? []).ToArray() }});");
                    result.AppendLine();
                }
            }

            if (result.Length > 0)
            {
                writer.WriteSafeString("\r\n\r\n");
                writer.WriteSafeString(result.ToString().TrimEnd());
            }
        }

        /// <summary>
        /// Generates the SELECT query SQL for <c>ReadAsync</c>.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" /> containing an <see cref="IClass" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void ReadAsyncWriteReadSql(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("context is supposed to be an IClass", nameof(context));
            }

            var classAndAllItsSuperClassesThatDeriveFromThing = @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(x => x.QueryDerivesFrom("Thing") || x.IsThingClass())
                .Reverse()
                .ToList();

            var sql = new StringBuilder();

            var valueProperties = classAndAllItsSuperClassesThatDeriveFromThing
                .SelectMany(x => x.QueryAllProperties())
                .Distinct()
                .Where(x => x.QueryIsDataType())
                .Where(x => !x.IsDerived && !x.IsDerivedUnion && !x.IsThingAttribute())
                .OrderBy(x => x.Name)
                .ToList();

            var referenceProperties = classAndAllItsSuperClassesThatDeriveFromThing
                .SelectMany(x => x.QueryPropertiesThatAreOwnedAndUsableAndInheritedFromDirectNonDerivesFromClasses("Thing").Union(x.QueryOppositeCompositeProperties()))
                .Distinct()
                .Where(x => !x.QueryIsDataType())
                .Where(x => !x.QueryIsMemberOfManyToMany())
                .OrderBy(x => x.Name)
                .ToList();

            var manyToManyReferenceProperties = classAndAllItsSuperClassesThatDeriveFromThing
                .SelectMany(x => x.QueryPropertiesThatAreOwnedAndUsableAndInheritedFromDirectNonDerivesFromClasses("Thing"))
                .Distinct()
                .Where(x => !x.QueryIsDataType())
                .Where(x => x.QueryIsMemberOfManyToMany())
                .OrderBy(x => x.Name)
                .ToList();

            sql.AppendLine("                    -- READ Thing");
            sql.AppendLine("                    SELECT");
            sql.AppendLine("                       \"Thing\".\"id\" AS \"id\",");
            sql.AppendLine("                       \"Thing\".\"classKind\" AS \"classKind\",");

            foreach (var property in valueProperties)
            {
                var suffix = property.QueryJsonbSelectDataTypeSuffix();
                var propertyName = property.Name.LowerCaseFirstLetter();

                if (property.Type?.Name is "UnlimitedNatural" or "String")
                {
                    suffix = string.Empty;
                }

                sql.AppendLine($"                       (\"Thing\".\"data\"->>'{property.Name}'){suffix} AS \"{propertyName}\",");
            }

            foreach (var property in referenceProperties)
            {
                var propertyName = property.Name.LowerCaseFirstLetter();

                var ownerClass = classAndAllItsSuperClassesThatDeriveFromThing
                    .FirstOrDefault(c => c.QueryPropertiesThatAreOwnedAndUsableAndInheritedFromDirectNonDerivesFromClasses("Thing").Contains(property) || c.QueryOppositeCompositeProperties().Contains(property));

                var ownerName = ownerClass?.Name ?? (property.Owner as INamedElement)?.Name ?? property.Namespace?.Name ?? string.Empty;

                if (property.QueryIsEnumerable())
                {
                    sql.AppendLine($"                       COALESCE(\"{ownerName.CapitalizeFirstLetter()}_{property.Name.CapitalizeFirstLetter()}\".\"{propertyName}\",'{{}}'::uuid[]) AS \"{propertyName}\",");
                }
                else if (property.IsComposite)
                {
                    sql.AppendLine($"                       \"{ownerName.CapitalizeFirstLetter()}_{property.Name.CapitalizeFirstLetter()}\".\"{propertyName}\"::uuid AS \"{propertyName}\",");
                }
                else
                {
                    sql.AppendLine($"                       \"{ownerName.CapitalizeFirstLetter()}\".\"{propertyName}\" AS \"{propertyName}\",");
                }
            }

            foreach (var property in manyToManyReferenceProperties)
            {
                var propertyName = property.Name.LowerCaseFirstLetter();
                sql.AppendLine($"                       COALESCE(\"{property.QueryManyToManyTableName()}\".\"{propertyName}\",'{{}}'::uuid[]) AS \"{propertyName}\",");
            }

            var lastCommaIndex = sql.ToString().LastIndexOf(',');

            if (lastCommaIndex >= 0)
            {
                sql.Remove(lastCommaIndex, 1);
            }

            sql.AppendLine("                    FROM \"Forge\".\"Thing\" AS \"Thing\"");

            foreach (var usedClass in classAndAllItsSuperClassesThatDeriveFromThing.Where(x => !x.IsThingClass()))
            {
                sql.AppendLine();
                sql.AppendLine($"                    -- READ {usedClass.Name.CapitalizeFirstLetter()}");
                sql.AppendLine($"                    INNER JOIN \"Forge\".\"{usedClass.Name.CapitalizeFirstLetter()}\" AS \"{usedClass.Name.CapitalizeFirstLetter()}\"");
                sql.AppendLine($"                            ON \"{usedClass.Name.CapitalizeFirstLetter()}\".\"id\" = \"Thing\".\"id\"");
            }

            foreach (var property in referenceProperties)
            {
                var propName = property.Name.LowerCaseFirstLetter();
                var ownerName = (property.Owner as INamedElement)?.Name ?? property.Namespace?.Name ?? string.Empty;

                if (property.QueryIsEnumerable())
                {
                    sql.AppendLine();
                    sql.AppendLine($"                    -- READ {ownerName.CapitalizeFirstLetter()}.{propName}");
                    sql.AppendLine("                    LEFT JOIN LATERAL(");
                    sql.AppendLine("                        SELECT");
                    sql.AppendLine($"                            COALESCE(array_agg(\"{property.Type?.Name.CapitalizeFirstLetter()}\".\"id\"::uuid), '{{}}'::uuid[]) AS \"{propName}\"");
                    sql.AppendLine($"                        FROM \"Forge\".\"{property.Type?.Name.CapitalizeFirstLetter()}\" AS \"{property.Type?.Name.CapitalizeFirstLetter()}\"");
                    sql.AppendLine($"                        WHERE \"{property.Type?.Name.CapitalizeFirstLetter()}\".\"{property.Opposite?.Name.LowerCaseFirstLetter() ?? "owner"}\" = \"{ownerName.CapitalizeFirstLetter()}\".\"id\"");
                    sql.AppendLine($"                    ) AS \"{ownerName.CapitalizeFirstLetter()}_{property.Name.CapitalizeFirstLetter()}\" ON true");
                }
                else if (property.IsComposite)
                {
                    sql.AppendLine();
                    sql.AppendLine($"                    -- READ {ownerName.CapitalizeFirstLetter()}.{propName}");
                    sql.AppendLine("                    LEFT JOIN LATERAL(");
                    sql.AppendLine("                        SELECT");
                    sql.AppendLine($"                            \"{property.Type?.Name.CapitalizeFirstLetter()}\".\"id\"::uuid AS \"{propName}\"");
                    sql.AppendLine($"                        FROM \"Forge\".\"{property.Type?.Name.CapitalizeFirstLetter()}\" AS \"{property.Type?.Name.CapitalizeFirstLetter()}\"");
                    sql.AppendLine($"                        WHERE \"{property.Type?.Name.CapitalizeFirstLetter()}\".\"{property.Opposite?.Name.LowerCaseFirstLetter() ?? "owner"}\" = \"{ownerName.CapitalizeFirstLetter()}\".\"id\"");
                    sql.AppendLine($"                    ) AS \"{ownerName.CapitalizeFirstLetter()}_{property.Name.CapitalizeFirstLetter()}\" ON true");
                }
            }

            foreach (var property in manyToManyReferenceProperties)
            {
                var manyToManyTableName = property.QueryManyToManyTableName();
                var propName = property.Name.LowerCaseFirstLetter();
                var ownerName = (property.Owner as INamedElement)?.Name ?? property.Namespace?.Name ?? string.Empty;

                sql.AppendLine();
                sql.AppendLine($"                    -- READ {ownerName.CapitalizeFirstLetter()}.{propName}");
                sql.AppendLine("                    LEFT JOIN LATERAL(");
                sql.AppendLine("                        SELECT");
                sql.AppendLine($"                                \"{manyToManyTableName}\".\"{property.QueryManyToManySourcePropertyName()}\" AS \"id\",");
                sql.AppendLine("                           COALESCE(");
                sql.AppendLine($"                               array_agg(\"{manyToManyTableName}\".\"{property.QueryManyToManyTargetPropertyName()}\"::uuid), '{{}}'::uuid[]");
                sql.AppendLine($"                           ) AS \"{propName}\"");
                sql.AppendLine($"                       FROM \"Forge\".\"{manyToManyTableName}\" AS \"{manyToManyTableName}\"");
                sql.AppendLine($"                       WHERE \"{manyToManyTableName}\".\"{property.QueryManyToManySourcePropertyName()}\" = \"{@class.Name.CapitalizeFirstLetter()}\".\"id\"");
                sql.AppendLine("                       GROUP BY 1");
                sql.AppendLine($"                    ) AS \"{manyToManyTableName}\" ON true");
            }

            sql.AppendLine();
            sql.AppendLine($"                    WHERE \"{@class.Name}\".\"id\" = ANY(@include);");

            writer.WriteSafeString(sql);
        }

        /// <summary>
        /// Writes DTO property mappings from the DataReader into the object initializer of <c>MapToDto</c>.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" /> containing an <see cref="IClass" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteMapToDto(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("context is supposed to be an IClass", nameof(context));
            }

            var classAndAllItsSuperClassesThatDeriveFromThing = @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(x => x.QueryDerivesFrom("Thing") || x.IsThingClass())
                .Reverse()
                .ToList();

            var allProperties = classAndAllItsSuperClassesThatDeriveFromThing
                .SelectMany(x => x.QueryPropertiesThatAreOwnedAndUsableAndInheritedFromDirectNonDerivesFromClasses("Thing").Union(x.QueryOppositeCompositeProperties()))
                .Distinct()
                .Where(x => !x.IsDerived && !x.IsDerivedUnion && !x.IsThingAttribute())
                .OrderBy(x => x.Name)
                .ToList();

            var mapBuilder = new StringBuilder();

            if (allProperties.Count > 0)
            {
                mapBuilder.AppendLine(",");
            }

            for (var i = 0; i < allProperties.Count; i++)
            {
                var property = allProperties[i];
                var propName = property.Name.LowerCaseFirstLetter();
                var propCSharpName = property.Name.CapitalizeFirstLetter();
                var isLast = i == allProperties.Count - 1;
                var comma = isLast ? string.Empty : ",";

                if (!property.QueryIsDataType())
                {
                    if (property.QueryIsEnumerable())
                    {
                        mapBuilder.Append($"                {propCSharpName} = [.. (Guid[])reader[\"{propName}\"]]{comma}");
                    }
                    else
                    {
                        if (property.QueryIsNullable())
                        {
                            mapBuilder.Append($"                {propCSharpName} = reader[\"{propName}\"] is DBNull ? null : (Guid)reader[\"{propName}\"]{comma}");
                        }
                        else
                        {
                            mapBuilder.Append($"                {propCSharpName} = (Guid)reader[\"{propName}\"]{comma}");
                        }
                    }
                }
                else
                {
                    if (property.QueryIsNullable())
                    {
                        mapBuilder.Append($"                {propCSharpName} = reader[\"{propName}\"] is DBNull ? null : {property.GetReadConversion()}{comma}");
                    }
                    else
                    {
                        mapBuilder.Append($"                {propCSharpName} = {property.GetReadConversion()}{comma}");
                    }
                }

                if (!isLast)
                {
                    mapBuilder.AppendLine();
                }
            }

            writer.WriteSafeString(mapBuilder);
        }

        /// <summary>
        /// Returns a JSONB select data type suffix that can be used in a SQL select query.
        /// </summary>
        /// <param name="property">The property to get the suffix for.</param>
        /// <returns>The data type suffix string.</returns>
        private static string QueryJsonbSelectDataTypeSuffix(this IProperty property)
        {
            ArgumentNullException.ThrowIfNull(property);

            var typeName = property.QuerySqlTypeName();

            if (typeName is "text" or "timestamp" or "")
            {
                return string.Empty;
            }

            if (property.QueryIsEnumerable() && !property.IsComposite)
            {
                return string.Empty;
            }

            return $"::{typeName}";
        }

        /// <summary>
        /// Returns a string representation of a type conversion expression for reading a property from an NpgsqlDataReader.
        /// </summary>
        /// <param name="property">The property to get the conversion expression for.</param>
        /// <returns>The conversion expression string.</returns>
        private static string GetReadConversion(this IProperty property)
        {
            ArgumentNullException.ThrowIfNull(property);

            var propName = property.Name.LowerCaseFirstLetter();
            var typeName = property.QuerySqlTypeName();

            if (typeName == "timestamp")
            {
                return property.QueryIsNullable()
                    ? $"reader[\"{propName}\"] is DBNull ? null : DateTime.Parse(reader[\"{propName}\"].ToString())"
                    : $"DateTime.Parse((string)reader[\"{propName}\"])";
            }

            if (typeName == "date")
            {
                return property.QueryIsNullable()
                    ? $"reader[\"{propName}\"] is DBNull ? null : DateOnly.Parse(reader[\"{propName}\"].ToString())"
                    : $"DateOnly.Parse((string)reader[\"{propName}\"])";
            }

            if (property.Type is IEnumeration enumerationType)
            {
                if (property.QueryIsEnumerable())
                {
                    return $"JsonSerializer.Deserialize<List<{enumerationType.Name}>>((string)reader[\"{propName}\"])";
                }

                return $"{enumerationType.Name}Provider.Parse((string)reader[\"{propName}\"])";
            }

            if (property.QueryIsEnumerable() && !property.IsComposite)
            {
                return $"JsonSerializer.Deserialize<List<{property.QueryCSharpTypeName()}>>((string)reader[\"{propName}\"])";
            }

            var csharpType = property.QueryCSharpTypeName();
            return $"({csharpType})reader[\"{propName}\"]";
        }
    }
}
