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
            handlebars.RegisterHelper("Forge.SQL.DeleteBaseTableTriggerFunctions", DeleteBaseTableTriggerFunctions);
            handlebars.RegisterHelper("Forge.SQL.WriteBasicTableThingDeleteTriggers", WriteBasicTableThingDeleteTriggers);
            handlebars.RegisterHelper("Forge.SQL.WriteBaseTableDeleteTriggers", WriteBaseTableDeleteTriggers);
            handlebars.RegisterHelper("Forge.SQL.ModelVersion", WriteModelVersion);
        }

        /// <summary>
        /// Writes basic table definitions for an <see cref="IClass" />.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteBasicTableDefinitions(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("Forge.SQL.WriteBasicTableDefinitions - context is supposed to be IClass");
            }

            if (@class.IsThingClass())
            {
                return;
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"CREATE TABLE \"Forge\".\"{@class.QuerySqlTableName()}\" (");
            stringBuilder.AppendLine("    \"id\" uuid NOT NULL,");

            foreach (var property in @class.QuerySqlSingleReferenceProperties())
            {
                // Preserves property UML multiplicity nullability without overriding non-owned composite properties
                var isNullable = property.QueryIsNullable();
                stringBuilder.AppendLine($"    \"{property.QuerySqlAttributeName()}\" {property.QuerySqlTypeName()}{(isNullable ? "" : " NOT NULL")},");
            }

            stringBuilder.AppendLine("    PRIMARY KEY (\"id\")");
            stringBuilder.AppendLine(");");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"ALTER TABLE \"Forge\".\"{@class.QuerySqlTableName()}\" SET (autovacuum_vacuum_scale_factor = 0.0);");
            stringBuilder.AppendLine($"ALTER TABLE \"Forge\".\"{@class.QuerySqlTableName()}\" SET (autovacuum_vacuum_threshold = 2500);");
            stringBuilder.AppendLine($"ALTER TABLE \"Forge\".\"{@class.QuerySqlTableName()}\" SET (autovacuum_analyze_scale_factor = 0.0);");
            stringBuilder.AppendLine($"ALTER TABLE \"Forge\".\"{@class.QuerySqlTableName()}\" SET (autovacuum_analyze_threshold = 2500);");
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
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("Forge.SQL.WriteBasicTableThingConstraints - context is supposed to be IClass");
            }

            if (@class.IsThingClass())
            {
                return;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"ALTER TABLE \"Forge\".\"{@class.QuerySqlTableName()}\" ADD CONSTRAINT \"{@class.QuerySqlTableName()}_Thing_FK_Source\" FOREIGN KEY (\"id\") REFERENCES \"Forge\".\"Thing\" (\"id\") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;");

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
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("Forge.SQL.WriteManyToManyTableDefinitionsAndConstraints - context is supposed to be IClass");
            }

            if (@class.IsThingClass())
            {
                return;
            }

            var stringBuilder = new StringBuilder();
            var ownedManyToManyProperties = @class.QueryOwnedManyToManyProperties();

            foreach (var property in ownedManyToManyProperties)
            {
                var ownerName = (property.Owner as INamedElement)?.Name ?? property.Namespace?.Name ?? string.Empty;

                stringBuilder.AppendLine($"CREATE TABLE \"Forge\".\"{property.QueryManyToManyTableName()}\" (");
                stringBuilder.AppendLine($"    \"{property.QueryManyToManySourcePropertyName()}\" uuid NOT NULL,");
                stringBuilder.AppendLine($"    \"{property.QueryManyToManyTargetPropertyName()}\" uuid NOT NULL,");
                stringBuilder.AppendLine($"    PRIMARY KEY (\"{property.QueryManyToManySourcePropertyName()}\", \"{property.QueryManyToManyTargetPropertyName()}\")");
                stringBuilder.AppendLine(");");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"ALTER TABLE \"Forge\".\"{property.QueryManyToManyTableName()}\" SET (autovacuum_vacuum_scale_factor = 0.0);");
                stringBuilder.AppendLine($"ALTER TABLE \"Forge\".\"{property.QueryManyToManyTableName()}\" SET (autovacuum_vacuum_threshold = 2500);");
                stringBuilder.AppendLine($"ALTER TABLE \"Forge\".\"{property.QueryManyToManyTableName()}\" SET (autovacuum_analyze_scale_factor = 0.0);");
                stringBuilder.AppendLine($"ALTER TABLE \"Forge\".\"{property.QueryManyToManyTableName()}\" SET (autovacuum_analyze_threshold = 2500);");
                stringBuilder.AppendLine();

                stringBuilder.AppendLine($"ALTER TABLE \"Forge\".\"{property.QueryManyToManyTableName()}\" ADD CONSTRAINT \"{property.QueryManyToManySourcePropertyTypeName()}_FK_Source\" FOREIGN KEY (\"{property.QueryManyToManySourcePropertyName()}\") REFERENCES \"Forge\".\"{property.QueryManyToManySourcePropertyTypeName()}\" (\"id\") ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;");

                stringBuilder.AppendLine(
                    $"CREATE INDEX \"idx_{ownerName.CapitalizeFirstLetter()}_{property.Name.LowerCaseFirstLetter()}_{property.QueryManyToManySourcePropertyName()}\" ON \"Forge\".\"{property.QueryManyToManyTableName()}\" (\"{property.QueryManyToManySourcePropertyName()}\");");

                stringBuilder.AppendLine($"ALTER TABLE \"Forge\".\"{property.QueryManyToManyTableName()}\" ADD CONSTRAINT \"{property.QueryManyToManyTargetPropertyTypeName()}_FK_Target\" FOREIGN KEY (\"{property.QueryManyToManyTargetPropertyName()}\") REFERENCES \"Forge\".\"{property.QueryManyToManyTargetPropertyTypeName()}\" (\"id\") ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;");

                stringBuilder.AppendLine(
                    $"CREATE INDEX \"idx_{ownerName.CapitalizeFirstLetter()}_{property.Name.LowerCaseFirstLetter()}_{property.QueryManyToManyTargetPropertyName()}\" ON \"Forge\".\"{property.QueryManyToManyTableName()}\" (\"{property.QueryManyToManyTargetPropertyName()}\");");

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
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("Forge.SQL.WriteNormalReferenceConstraints - context is supposed to be IClass");
            }

            if (@class.IsThingClass())
            {
                return;
            }

            var stringBuilder = new StringBuilder();

            foreach (var property in @class.QuerySqlSingleReferenceProperties())
            {
                stringBuilder.AppendLine($"ALTER TABLE \"Forge\".\"{@class.QuerySqlTableName()}\" ADD CONSTRAINT \"{@class.QuerySqlTableName()}_{property.QuerySqlAttributeName()}_FK_Source\" FOREIGN KEY (\"{property.QuerySqlAttributeName()}\") REFERENCES \"Forge\".\"{property.QueryTypeName()}\" (\"id\"){(property.Opposite?.IsComposite ?? false ? " ON DELETE CASCADE" : string.Empty)} ON UPDATE CASCADE DEFERRABLE;");

                stringBuilder.AppendLine(
                    $"CREATE INDEX \"idx_{@class.QuerySqlTableName()}_{property.QuerySqlAttributeName()}\" ON \"Forge\".\"{@class.QuerySqlTableName()}\" (\"{property.QuerySqlAttributeName()}\");");
            }

            writer.WriteSafeString(stringBuilder);
        }

        /// <summary>
        /// Writes delete trigger functions for base tables.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void DeleteBaseTableTriggerFunctions(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (context.Value is not IEnumerable<IClass> classes)
            {
                throw new ArgumentException("Forge.SQL.DeleteBaseTableTriggerFunctions - context is supposed to be IEnumerable<IClass>");
            }

            var stringBuilder = new StringBuilder();

            foreach (var className in classes.Where(x => x.QueryAllSpecializations().Count != 0 && x.QueryDerivesFrom("Thing")).Select(@class => @class.Name))
            {
                var sql = $$"""
                            CREATE OR REPLACE FUNCTION "Forge".{{className.ToLower()}}_delete()
                                RETURNS trigger
                                LANGUAGE plpgsql
                                AS $$
                                BEGIN
                                    EXECUTE 'DELETE FROM "Forge"."{{className}}" WHERE id = $1' USING OLD.id;
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
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("Forge.SQL.WriteBasicTableThingDeleteTriggers - context is supposed to be IClass");
            }

            if (@class.IsThingClass())
            {
                return;
            }

            var txt = $$"""
                        CREATE OR REPLACE TRIGGER trg_thing_delete
                            AFTER DELETE ON "Forge"."{{@class.QuerySqlTableName()}}"
                            FOR EACH ROW
                                EXECUTE FUNCTION "Forge".thing_delete();


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
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("Forge.SQL.WriteBaseTableDeleteTriggers - context is supposed to be IClass");
            }

            if (@class.IsThingClass())
            {
                return;
            }

            foreach (var baseClassName in @class.Generalization.Select(x => x.General).OfType<IClass>().Where(x => x.QueryDerivesFrom("Thing")).Select(baseClass => baseClass.Name))
            {
                var txt = $$"""
                            CREATE OR REPLACE TRIGGER trg_{{baseClassName.ToLower()}}_on_{{@class.QuerySqlTableName().ToLower()}}_delete
                                AFTER DELETE ON "Forge"."{{@class.QuerySqlTableName()}}"
                                FOR EACH ROW
                                    EXECUTE FUNCTION "Forge".{{baseClassName.ToLower()}}_delete();


                            """;

                writer.WriteSafeString(txt);
            }
        }

        /// <summary>
        /// Writes the model version.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteModelVersion(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            writer.Write("0.1.0");
        }
    }
}
