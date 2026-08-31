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
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// A Handlebars block helper for Data Access Object (DAO) code generation.
    /// </summary>
    public static class DaoHelper
    {
        /// <summary>
        /// The error message used when the Handlebars context is not an <see cref="IClass" />.
        /// </summary>
        private const string ContextMustBeIClass = "context is supposed to be an IClass";

        /// <summary>
        /// The name of the base class in the Thing hierarchy, used for generating SQL queries and commands.
        /// </summary>
        private const string ThingName = "Thing";

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
                throw new ArgumentException(ContextMustBeIClass, nameof(context));
            }

            var valueTypeAndSingleReferenceProperties = new StringBuilder();

            valueTypeAndSingleReferenceProperties.AppendLine("                -- Thing");
            valueTypeAndSingleReferenceProperties.AppendLine("                INSERT INTO \"Forge\".\"Thing\" (\"id\", \"classKind\", \"data\")");
            valueTypeAndSingleReferenceProperties.AppendLine("                VALUES (@id, @classKind, @data);");

            var allSuperClassesThatDeriveFromThing = @class.QuerySuperClassesDerivingFromThing();

            foreach (var thingDerivedClass in allSuperClassesThatDeriveFromThing)
            {
                var sqlColumns = new StringBuilder("\"id\"");
                var sqlParameters = new StringBuilder("@id");

                var singleReferenceProperties = thingDerivedClass
                    .QuerySqlSingleReferenceProperties()
                    .Distinct()
                    .ToList();

                foreach (var singleReferenceProperty in singleReferenceProperties)
                {
                    sqlColumns.Append($", \"{singleReferenceProperty.QuerySqlAttributeName()}\"");
                    sqlParameters.Append($", @{singleReferenceProperty.QuerySqlAttributeName()}");
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
                throw new ArgumentException(ContextMustBeIClass, nameof(context));
            }

            var allSuperClassesThatDeriveFromThing = @class.QuerySuperClassesDerivingFromThing();
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
                throw new ArgumentException(ContextMustBeIClass, nameof(context));
            }

            var allSuperClassesThatDeriveFromThing = @class.QuerySuperClassesDerivingFromThing();
            var result = new StringBuilder();
            var dtoName = @class.Name.LowerCaseFirstLetter();

            foreach (var thingDerivedClass in allSuperClassesThatDeriveFromThing)
            {
                var properties = thingDerivedClass.QueryManyToManyProperties();

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
                throw new ArgumentException(ContextMustBeIClass, nameof(context));
            }

            var valueTypeAndSingleReferenceProperties = new StringBuilder();

            valueTypeAndSingleReferenceProperties.AppendLine("                -- Thing");
            valueTypeAndSingleReferenceProperties.AppendLine("                UPDATE \"Forge\".\"Thing\" SET \"data\" = @data WHERE \"id\" = @id;");

            var allSuperClassesThatDeriveFromThing = @class.QuerySuperClassesDerivingFromThing();

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
                throw new ArgumentException(ContextMustBeIClass, nameof(context));
            }

            var allSuperClassesThatDeriveFromThing = @class.QuerySuperClassesDerivingFromThing();
            var result = new StringBuilder();
            var dtoName = @class.Name.LowerCaseFirstLetter();

            foreach (var thingDerivedClass in allSuperClassesThatDeriveFromThing)
            {
                var properties = thingDerivedClass.QueryManyToManyProperties();

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
                throw new ArgumentException(ContextMustBeIClass, nameof(context));
            }

            var hierarchyClasses = @class.QueryThingHierarchyClasses();
            var sql = new StringBuilder();

            var valueProperties = hierarchyClasses
                .SelectMany(x => x.QueryAllProperties())
                .Distinct()
                .Where(x => x.QueryIsDataType())
                .Where(x => !x.IsDerived && !x.IsDerivedUnion && !x.IsThingAttribute())
                .OrderBy(x => x.Name)
                .ToList();

            var referenceProperties = hierarchyClasses
                .SelectMany(x => x.QueryPropertiesThatAreOwnedAndUsableAndInheritedFromDirectNonDerivesFromClasses(ThingName).Union(x.QueryOppositeCompositeProperties()))
                .Distinct()
                .Where(x => !x.QueryIsDataType())
                .Where(x => !x.QueryIsMemberOfManyToMany())
                .OrderBy(x => x.Name)
                .ToList();

            var manyToManyReferenceProperties = hierarchyClasses
                .SelectMany(x => x.QueryPropertiesThatAreOwnedAndUsableAndInheritedFromDirectNonDerivesFromClasses(ThingName))
                .Distinct()
                .Where(x => !x.QueryIsDataType())
                .Where(x => x.QueryIsMemberOfManyToMany())
                .OrderBy(x => x.Name)
                .ToList();

            sql.AppendLine("                    -- READ Thing");
            sql.AppendLine("                    SELECT");
            sql.AppendLine("                       \"Thing\".\"id\" AS \"id\",");
            sql.AppendLine("                       \"Thing\".\"classKind\" AS \"classKind\",");

            AppendValueColumns(sql, valueProperties);
            AppendReferenceColumns(sql, referenceProperties, hierarchyClasses);
            AppendManyToManyColumns(sql, manyToManyReferenceProperties);

            var lastCommaIndex = sql.ToString().LastIndexOf(',');

            if (lastCommaIndex >= 0)
            {
                sql.Remove(lastCommaIndex, 1);
            }

            sql.AppendLine("                    FROM \"Forge\".\"Thing\" AS \"Thing\"");

            AppendJoins(sql, hierarchyClasses);
            AppendLateralJoins(sql, referenceProperties, manyToManyReferenceProperties, @class);

            sql.AppendLine();
            sql.AppendLine($"                    WHERE \"{@class.Name}\".\"id\" = ANY(@include);");

            writer.WriteSafeString(sql);
        }

        /// <summary>
        /// Appends value column selections from JSONB data to the SQL SELECT statement.
        /// </summary>
        /// <param name="sql">The <see cref="StringBuilder" /> for SQL generation.</param>
        /// <param name="valueProperties">The list of value type properties.</param>
        private static void AppendValueColumns(StringBuilder sql, IEnumerable<IProperty> valueProperties)
        {
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
        }

        /// <summary>
        /// Appends single and composite reference column selections to the SQL SELECT statement.
        /// </summary>
        /// <param name="sql">The <see cref="StringBuilder" /> for SQL generation.</param>
        /// <param name="referenceProperties">The list of reference properties.</param>
        /// <param name="hierarchyClasses">The list of classes in the Thing hierarchy.</param>
        private static void AppendReferenceColumns(StringBuilder sql, IEnumerable<IProperty> referenceProperties, IEnumerable<IClass> hierarchyClasses)
        {
            var hierarchyClassesList = hierarchyClasses.ToList();

            foreach (var property in referenceProperties)
            {
                var propertyName = property.Name.LowerCaseFirstLetter();

                var ownerClass = hierarchyClassesList
                    .FirstOrDefault(c => c.QueryPropertiesThatAreOwnedAndUsableAndInheritedFromDirectNonDerivesFromClasses(ThingName).Contains(property) || c.QueryOppositeCompositeProperties().Contains(property));

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
        }

        /// <summary>
        /// Appends many-to-many reference column selections to the SQL SELECT statement.
        /// </summary>
        /// <param name="sql">The <see cref="StringBuilder" /> for SQL generation.</param>
        /// <param name="manyToManyReferenceProperties">The list of many-to-many reference properties.</param>
        private static void AppendManyToManyColumns(StringBuilder sql, IEnumerable<IProperty> manyToManyReferenceProperties)
        {
            foreach (var property in manyToManyReferenceProperties)
            {
                var propertyName = property.Name.LowerCaseFirstLetter();
                sql.AppendLine($"                       COALESCE(\"{property.QueryManyToManyTableName()}\".\"{propertyName}\",'{{}}'::uuid[]) AS \"{propertyName}\",");
            }
        }

        /// <summary>
        /// Appends INNER JOIN statements for superclasses in the Thing hierarchy to the SQL query.
        /// </summary>
        /// <param name="sql">The <see cref="StringBuilder" /> for SQL generation.</param>
        /// <param name="hierarchyClasses">The list of classes in the Thing hierarchy.</param>
        private static void AppendJoins(StringBuilder sql, IEnumerable<IClass> hierarchyClasses)
        {
            foreach (var usedClassName in hierarchyClasses
                         .Where(x => !x.IsThingClass())
                         .Select(x => x.Name.CapitalizeFirstLetter()))
            {
                sql.AppendLine();
                sql.AppendLine($"                    -- READ {usedClassName}");
                sql.AppendLine($"                    INNER JOIN \"Forge\".\"{usedClassName}\" AS \"{usedClassName}\"");
                sql.AppendLine($"                            ON \"{usedClassName}\".\"id\" = \"Thing\".\"id\"");
            }
        }

        /// <summary>
        /// Appends LEFT JOIN LATERAL subqueries for reference collections and many-to-many tables.
        /// </summary>
        /// <param name="sql">The <see cref="StringBuilder" /> for SQL generation.</param>
        /// <param name="referenceProperties">The list of reference properties.</param>
        /// <param name="manyToManyReferenceProperties">The list of many-to-many properties.</param>
        /// <param name="class">The current subject class.</param>
        private static void AppendLateralJoins(StringBuilder sql, IEnumerable<IProperty> referenceProperties, IEnumerable<IProperty> manyToManyReferenceProperties, IClass @class)
        {
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
                throw new ArgumentException(ContextMustBeIClass, nameof(context));
            }

            var hierarchyClasses = @class.QueryThingHierarchyClasses();

            var allProperties = hierarchyClasses
                .SelectMany(x => x.QueryPropertiesThatAreOwnedAndUsableAndInheritedFromDirectNonDerivesFromClasses(ThingName).Union(x.QueryOppositeCompositeProperties()))
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
                var isLast = i == allProperties.Count - 1;
                var comma = isLast ? string.Empty : ",";

                mapBuilder.Append(FormatDtoPropertyMapping(property, comma));

                if (!isLast)
                {
                    mapBuilder.AppendLine();
                }
            }

            writer.WriteSafeString(mapBuilder);
        }

        /// <summary>
        /// Formats the mapping line for a single DTO property from the data reader.
        /// </summary>
        /// <param name="property">The property to map.</param>
        /// <param name="comma">The trailing comma if not the last property.</param>
        /// <returns>The formatted mapping code string.</returns>
        private static string FormatDtoPropertyMapping(IProperty property, string comma)
        {
            var propName = property.Name.LowerCaseFirstLetter();
            var propCSharpName = property.Name.CapitalizeFirstLetter();

            if (!property.QueryIsDataType())
            {
                return FormatReferencePropertyMapping(property, propName, propCSharpName, comma);
            }

            return FormatDataTypePropertyMapping(property, propName, propCSharpName, comma);
        }

        /// <summary>
        /// Formats the mapping line for an entity reference property.
        /// </summary>
        /// <param name="property">The reference property.</param>
        /// <param name="propName">The lowerCamelCase property name.</param>
        /// <param name="propCSharpName">The PascalCase property name.</param>
        /// <param name="comma">The trailing comma if not the last property.</param>
        /// <returns>The formatted mapping code string.</returns>
        private static string FormatReferencePropertyMapping(IProperty property, string propName, string propCSharpName, string comma)
        {
            if (property.QueryIsEnumerable())
            {
                return $"                {propCSharpName} = [.. (Guid[])reader[\"{propName}\"]]{comma}";
            }

            if (property.QueryIsNullable())
            {
                return $"                {propCSharpName} = reader[\"{propName}\"] is DBNull ? null : (Guid)reader[\"{propName}\"]{comma}";
            }

            return $"                {propCSharpName} = (Guid)reader[\"{propName}\"]{comma}";
        }

        /// <summary>
        /// Formats the mapping line for a value/data type property.
        /// </summary>
        /// <param name="property">The data type property.</param>
        /// <param name="propName">The lowerCamelCase property name.</param>
        /// <param name="propCSharpName">The PascalCase property name.</param>
        /// <param name="comma">The trailing comma if not the last property.</param>
        /// <returns>The formatted mapping code string.</returns>
        private static string FormatDataTypePropertyMapping(IProperty property, string propName, string propCSharpName, string comma)
        {
            if (property.QueryIsNullable())
            {
                return $"                {propCSharpName} = reader[\"{propName}\"] is DBNull ? null : {property.QueryReadConversion()}{comma}";
            }

            return $"                {propCSharpName} = {property.QueryReadConversion()}{comma}";
        }
    }
}
