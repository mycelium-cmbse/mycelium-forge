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

            handlebars.RegisterHelper("Forge.SQL.WriteBasicTableDefinitions", (writer, context, _) =>
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
                    stringBuilder.AppendLine($"    \"{property.QuerySqlAttributeName()}\" {property.QuerySqlTypeName()}{(property.QueryIsNullable() ? "" : " NOT NULL")},");
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
            });

            handlebars.RegisterHelper("Forge.SQL.WriteBasicTableThingConstraints", (writer, context, _) =>
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
            });

            handlebars.RegisterHelper("Forge.SQL.WriteManyToManyTableDefinitionsAndConstraints", (writer, context, _) =>
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
            });

            handlebars.RegisterHelper("Forge.SQL.WriteNormalReferenceConstraints", (writer, context, _) =>
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
            });

            handlebars.RegisterHelper("Forge.SQL.DeleteBaseTableTriggerFunctions", (writer, context, _) =>
            {
                if (context.Value is not IEnumerable<IClass> classes)
                {
                    throw new ArgumentException("Forge.SQL.DeleteBaseTableTriggerFunctions - context is supposed to be IEnumerable<IClass>");
                }

                var stringBuilder = new StringBuilder();

                foreach (var @class in classes.Where(x => x.QueryAllSpecializations().Count != 0 && x.QueryDerivesFrom("Thing")))
                {
                    var sql = $$"""
                                CREATE OR REPLACE FUNCTION "Forge".{{@class.Name.ToLower()}}_delete()
                                    RETURNS trigger
                                    LANGUAGE plpgsql
                                    AS $$
                                    BEGIN
                                        DELETE FROM "Forge"."{{@class.Name}}"
                                        WHERE id = OLD.id;
                                        RETURN OLD;
                                    END;
                                $$;
                                """;

                    stringBuilder.AppendLine(sql);
                    stringBuilder.AppendLine();
                }

                writer.WriteSafeString(stringBuilder);
            });

            handlebars.RegisterHelper("Forge.SQL.WriteBasicTableThingDeleteTriggers", (writer, context, _) =>
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
            });

            handlebars.RegisterHelper("Forge.SQL.WriteBaseTableDeleteTriggers", (writer, context, _) =>
            {
                if (context.Value is not IClass @class)
                {
                    throw new ArgumentException("Forge.SQL.WriteBaseTableDeleteTriggers - context is supposed to be IClass");
                }

                if (@class.IsThingClass())
                {
                    return;
                }

                foreach (var baseClass in @class.Generalization.Select(x => x.General).OfType<IClass>().Where(x => x.QueryDerivesFrom("Thing")))
                {
                    var txt = $$"""
                                CREATE OR REPLACE TRIGGER trg_{{baseClass.Name.ToLower()}}_on_{{@class.QuerySqlTableName().ToLower()}}_delete
                                    AFTER DELETE ON "Forge"."{{@class.QuerySqlTableName()}}"
                                    FOR EACH ROW
                                        EXECUTE FUNCTION "Forge".{{baseClass.Name.ToLower()}}_delete();


                                """;

                    writer.WriteSafeString(txt);
                }
            });

            handlebars.RegisterHelper("Forge.SQL.ModelVersion", (writer, _, _) => { writer.Write("0.1.0"); });
        }
    }
}
