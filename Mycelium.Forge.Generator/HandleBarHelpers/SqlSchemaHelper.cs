// ------------------------------------------------------------------------------------------------
// <copyright file="SqlSchemaHelper.cs" company="Starion Group S.A.">
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

    using uml4net.CommonStructure;
    using uml4net.Extensions;
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// A Handlebars block helper for the SQL schema generation.
    /// </summary>
    public static class SqlSchemaHelper
    {
        /// <summary>
        /// Registers the SQL schema Handlebars helpers.
        /// </summary>
        /// <param name="handlebars">The <see cref="IHandlebars" /> context with which the helpers need to be registered.</param>
        public static void RegisterSqlSchemaHelpers(this IHandlebars handlebars)
        {
            ArgumentNullException.ThrowIfNull(handlebars);

            handlebars.RegisterHelper("Forge.SQL.WriteBasicTableDefinitions", WriteBasicTableDefinitions);
            handlebars.RegisterHelper("Forge.SQL.WriteBasicTableThingConstraints", WriteBasicTableThingConstraints);
            handlebars.RegisterHelper("Forge.SQL.WriteManyToManyTableDefinitionsAndConstraints", WriteManyToManyTableDefinitionsAndConstraints);
            handlebars.RegisterHelper("Forge.SQL.WriteNormalReferenceConstraints", WriteNormalReferenceConstraints);
            handlebars.RegisterHelper("Forge.SQL.WriteIndexableValuePropertyIndexes", WriteIndexableValuePropertyIndexes);
            handlebars.RegisterHelper("Forge.SQL.DeleteBaseTableTriggerFunctions", DeleteBaseTableTriggerFunctions);
            handlebars.RegisterHelper("Forge.SQL.WriteBasicTableThingDeleteTriggers", WriteBasicTableThingDeleteTriggers);
            handlebars.RegisterHelper("Forge.SQL.WriteBaseTableDeleteTriggers", WriteBaseTableDeleteTriggers);
        }

        /// <summary>
        /// Writes basic table definitions for an <see cref="IClass" />.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteBasicTableDefinitions(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            var @class = arguments.QuerySingle<IClass>("Forge.SQL.WriteBasicTableDefinitions");

            if (@class.IsThingClass())
            {
                return;
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"CREATE TABLE forge.{@class.QuerySqlTableName()} (");
            stringBuilder.AppendLine("    id uuid NOT NULL,");

            foreach (var property in @class.QuerySqlSingleReferenceProperties())
            {
                // Preserves property UML multiplicity nullability without overriding non-owned composite properties
                var isNullable = property.QueryIsNullable();
                stringBuilder.AppendLine($"    {property.QuerySqlAttributeName()} {property.QuerySqlTypeName()}{(isNullable ? "" : " NOT NULL")},");
            }

            stringBuilder.AppendLine("    PRIMARY KEY (id)");
            stringBuilder.AppendLine(");");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"ALTER TABLE forge.{@class.QuerySqlTableName()} SET (autovacuum_vacuum_scale_factor = 0.0);");
            stringBuilder.AppendLine($"ALTER TABLE forge.{@class.QuerySqlTableName()} SET (autovacuum_vacuum_threshold = 2500);");
            stringBuilder.AppendLine($"ALTER TABLE forge.{@class.QuerySqlTableName()} SET (autovacuum_analyze_scale_factor = 0.0);");
            stringBuilder.AppendLine($"ALTER TABLE forge.{@class.QuerySqlTableName()} SET (autovacuum_analyze_threshold = 2500);");
            stringBuilder.AppendLine();

            writer.WriteSafeString(stringBuilder);
        }

        /// <summary>
        /// Writes the Thing foreign key constraint for an <see cref="IClass" />.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteBasicTableThingConstraints(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            var @class = arguments.QuerySingle<IClass>("Forge.SQL.WriteBasicTableThingConstraints");

            if (@class.IsThingClass())
            {
                return;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"ALTER TABLE forge.{@class.QuerySqlTableName()} ADD CONSTRAINT {@class.QuerySqlTableName()}_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;");

            writer.WriteSafeString(stringBuilder);
        }

        /// <summary>
        /// Writes many-to-many junction table definitions and constraints for an <see cref="IClass" />.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteManyToManyTableDefinitionsAndConstraints(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            var @class = arguments.QuerySingle<IClass>("Forge.SQL.WriteManyToManyTableDefinitionsAndConstraints");

            if (@class.IsThingClass())
            {
                return;
            }

            var stringBuilder = new StringBuilder();
            var ownedManyToManyProperties = @class.QueryOwnedManyToManyProperties();

            foreach (var property in ownedManyToManyProperties)
            {
                var ownerName = (property.Owner as INamedElement)?.Name ?? property.Namespace?.Name ?? string.Empty;
                var tableName = property.QueryManyToManyTableName();
                var sourceColumn = property.QueryManyToManySourcePropertyName();
                var targetColumn = property.QueryManyToManyTargetPropertyName();
                var sourceTable = property.QueryManyToManySourcePropertyTypeName().ToSnakeCase();
                var targetTable = property.QueryManyToManyTargetPropertyTypeName().ToSnakeCase();

                stringBuilder.AppendLine($"CREATE TABLE forge.{tableName} (");
                stringBuilder.AppendLine($"    {sourceColumn} uuid NOT NULL,");
                stringBuilder.AppendLine($"    {targetColumn} uuid NOT NULL,");
                stringBuilder.AppendLine($"    PRIMARY KEY ({sourceColumn}, {targetColumn})");
                stringBuilder.AppendLine(");");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"ALTER TABLE forge.{tableName} SET (autovacuum_vacuum_scale_factor = 0.0);");
                stringBuilder.AppendLine($"ALTER TABLE forge.{tableName} SET (autovacuum_vacuum_threshold = 2500);");
                stringBuilder.AppendLine($"ALTER TABLE forge.{tableName} SET (autovacuum_analyze_scale_factor = 0.0);");
                stringBuilder.AppendLine($"ALTER TABLE forge.{tableName} SET (autovacuum_analyze_threshold = 2500);");
                stringBuilder.AppendLine();

                stringBuilder.AppendLine($"ALTER TABLE forge.{tableName} ADD CONSTRAINT {sourceTable}_fk_source FOREIGN KEY ({sourceColumn}) REFERENCES forge.{sourceTable} (id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;");

                stringBuilder.AppendLine($"CREATE INDEX idx_{ownerName.ToSnakeCase()}_{property.Name.ToSnakeCase()}_{sourceColumn} ON forge.{tableName} ({sourceColumn});");

                stringBuilder.AppendLine($"ALTER TABLE forge.{tableName} ADD CONSTRAINT {targetTable}_fk_target FOREIGN KEY ({targetColumn}) REFERENCES forge.{targetTable} (id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;");

                stringBuilder.AppendLine($"CREATE INDEX idx_{ownerName.ToSnakeCase()}_{property.Name.ToSnakeCase()}_{targetColumn} ON forge.{tableName} ({targetColumn});");

                stringBuilder.AppendLine();
            }

            writer.WriteSafeString(stringBuilder);
        }

        /// <summary>
        /// Writes normal single reference foreign key constraints and indexes for an <see cref="IClass" />.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteNormalReferenceConstraints(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            var @class = arguments.QuerySingle<IClass>("Forge.SQL.WriteNormalReferenceConstraints");

            if (@class.IsThingClass())
            {
                return;
            }

            var stringBuilder = new StringBuilder();

            foreach (var property in @class.QuerySqlSingleReferenceProperties())
            {
                stringBuilder.AppendLine($"ALTER TABLE forge.{@class.QuerySqlTableName()} ADD CONSTRAINT {@class.QuerySqlTableName()}_{property.QuerySqlAttributeName()}_fk_source FOREIGN KEY ({property.QuerySqlAttributeName()}) REFERENCES forge.{property.QueryTypeName().ToSnakeCase()} (id){(property.Opposite?.IsComposite ?? false ? " ON DELETE CASCADE" : string.Empty)} ON UPDATE CASCADE DEFERRABLE;");

                stringBuilder.AppendLine($"CREATE INDEX idx_{@class.QuerySqlTableName()}_{property.QuerySqlAttributeName()} ON forge.{@class.QuerySqlTableName()} ({property.QuerySqlAttributeName()});");
            }

            writer.WriteSafeString(stringBuilder);
        }

        /// <summary>
        /// Writes indexes for value properties marked as indexable (<see cref="PropertyExtension.QueryIsIndexable" />).
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteIndexableValuePropertyIndexes(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            var @class = arguments.QuerySingle<IClass>("Forge.SQL.WriteIndexableValuePropertyIndexes");

            if (@class.IsThingClass())
            {
                return;
            }

            var indexableValueProperties = @class.OwnedAttribute
                .Where(x => x.QueryIsDataType() && x.QueryIsIndexable())
                .OrderBy(x => x.Name)
                .ToList();

            if (indexableValueProperties.Count == 0)
            {
                return;
            }

            var stringBuilder = new StringBuilder();
            var whereClause = QueryIndexWhereClause(@class);

            foreach (var property in indexableValueProperties)
            {
                stringBuilder.AppendLine($"CREATE INDEX idx_{@class.QuerySqlTableName()}_{property.QuerySqlAttributeName()} ON forge.thing ((data ->> '{property.Name.LowerCaseFirstLetter()}')){whereClause};");
            }

            writer.WriteSafeString(stringBuilder);
        }

        /// <summary>
        /// Queries the SQL WHERE clause for an index on an <see cref="IClass" /> based on its concrete subclasses.
        /// </summary>
        /// <param name="class">The subject <see cref="IClass" />.</param>
        /// <returns>The SQL WHERE clause string, or empty if unrestricted.</returns>
        private static string QueryIndexWhereClause(IClass @class)
        {
            if (!@class.IsAbstract)
            {
                return $" WHERE class_kind = '{@class.Name}'";
            }

            var concreteSubclasses = @class.QueryConcreteSubclasses();

            return concreteSubclasses.Count switch
            {
                0 => string.Empty,
                1 => $" WHERE class_kind = '{concreteSubclasses[0].Name}'",
                _ => $" WHERE class_kind IN ('{string.Join("', '", concreteSubclasses.Select(x => x.Name))}')"
            };
        }

        /// <summary>
        /// Writes delete trigger functions for base tables.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void DeleteBaseTableTriggerFunctions(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            var classes = arguments.QuerySingle<IEnumerable<IClass>>("Forge.SQL.DeleteBaseTableTriggerFunctions");

            var stringBuilder = new StringBuilder();

            foreach (var className in classes.Where(x => x.QueryAllSpecializations().Count != 0 && x.QueryDerivesFrom(ModelConstants.ThingName)).Select(@class => @class.Name))
            {
                var sql = $$"""
                            CREATE OR REPLACE FUNCTION forge.{{className.ToSnakeCase()}}_delete()
                                RETURNS trigger
                                LANGUAGE plpgsql
                                AS $$
                                BEGIN
                                    EXECUTE 'DELETE FROM forge.{{className.ToSnakeCase()}} WHERE id = $1' USING OLD.id;
                                    RETURN OLD;
                                END;
                            $$;
                            """;

                stringBuilder.AppendLine(sql);
                stringBuilder.AppendLine();
            }

            writer.WriteSafeString(stringBuilder);
        }

        /// <summary>
        /// Writes delete triggers to the Thing table for an <see cref="IClass" />.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteBasicTableThingDeleteTriggers(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            var @class = arguments.QuerySingle<IClass>("Forge.SQL.WriteBasicTableThingDeleteTriggers");

            if (@class.IsThingClass())
            {
                return;
            }

            var txt = $$"""
                        CREATE OR REPLACE TRIGGER trg_thing_delete
                            AFTER DELETE ON forge.{{@class.QuerySqlTableName()}}
                            FOR EACH ROW
                                EXECUTE FUNCTION forge.thing_delete();


                        """;

            writer.WriteSafeString(txt);
        }

        /// <summary>
        /// Writes delete triggers to base tables for an <see cref="IClass" />.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteBaseTableDeleteTriggers(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            var @class = arguments.QuerySingle<IClass>("Forge.SQL.WriteBaseTableDeleteTriggers");

            if (@class.IsThingClass())
            {
                return;
            }

            foreach (var baseClassName in @class.Generalization.Select(x => x.General).OfType<IClass>().Where(x => x.QueryDerivesFrom(ModelConstants.ThingName)).Select(baseClass => baseClass.Name))
            {
                var txt = $$"""
                            CREATE OR REPLACE TRIGGER trg_{{baseClassName.ToSnakeCase()}}_on_{{@class.QuerySqlTableName()}}_delete
                                AFTER DELETE ON forge.{{@class.QuerySqlTableName()}}
                                FOR EACH ROW
                                    EXECUTE FUNCTION forge.{{baseClassName.ToSnakeCase()}}_delete();


                            """;

                writer.WriteSafeString(txt);
            }
        }
    }
}
