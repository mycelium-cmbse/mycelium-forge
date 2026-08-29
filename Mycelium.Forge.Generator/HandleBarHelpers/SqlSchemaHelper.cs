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

    using uml4net.Classification;
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
            handlebars.RegisterHelper("Forge.SQL.WriteUniversalAttributeIndexes", WriteUniversalAttributeIndexes);
            handlebars.RegisterHelper("Forge.SQL.WriteClassAttributeIndexes", WriteClassAttributeIndexes);
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
        /// Writes the shared composite indexes for the universal <see cref="IClass">Thing</see> attributes
        /// (<c>createdAt</c>, <c>modifiedAt</c>) - one index per attribute, covering every class, rather than one
        /// per class, since these attributes are present on every entity regardless of its concrete type.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteUniversalAttributeIndexes(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (context.Value is not IEnumerable<IClass> classes)
            {
                throw new ArgumentException("Forge.SQL.WriteUniversalAttributeIndexes - context is supposed to be IEnumerable<IClass>");
            }

            var thingClass = classes.SingleOrDefault(x => x.IsThingClass());

            if (thingClass == null)
            {
                return;
            }

            var stringBuilder = new StringBuilder();

            foreach (var property in thingClass.QuerySqlIndexableOwnAttributes().OrderBy(x => x.Name))
            {
                var attributeName = property.QuerySqlAttributeName();

                stringBuilder.AppendLine(
                    $"CREATE INDEX \"idx_Thing_classKind_{attributeName}\" ON \"Forge\".\"Thing\" (\"classKind\", {property.QueryJsonbDataExpression()});");
            }

            writer.WriteSafeString(stringBuilder);
        }

        /// <summary>
        /// Writes one partial expression index per own-or-inherited indexable scalar attribute for an
        /// <see cref="IClass" />, scoped to that class's own <c>classKind</c> value. Skips <c>Thing</c> itself
        /// (its own attributes get the shared indexes from <see cref="WriteUniversalAttributeIndexes" /> instead)
        /// and abstract classes (no row's <c>classKind</c> is ever an abstract class's name).
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteClassAttributeIndexes(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException("Forge.SQL.WriteClassAttributeIndexes - context is supposed to be IClass");
            }

            if (@class.IsThingClass() || @class.IsAbstract)
            {
                return;
            }

            var stringBuilder = new StringBuilder();

            foreach (var property in @class.QuerySqlIndexableAttributes())
            {
                var attributeName = property.QuerySqlAttributeName();

                stringBuilder.AppendLine(
                    $"CREATE INDEX \"idx_Thing_{@class.QuerySqlTableName()}_{attributeName}\" ON \"Forge\".\"Thing\" ({property.QueryJsonbDataExpression()}) WHERE \"classKind\" = '{@class.QuerySqlTableName()}';");
            }

            writer.WriteSafeString(stringBuilder);
        }

        /// <summary>
        /// Builds the parenthesized, type-cast JSONB expression identifying an indexable scalar attribute's value
        /// inside <c>Thing.data</c>, e.g. <c>("data"->>'status')</c> or <c>(("data"->>'createdAt')::timestamp)</c>.
        /// Reuses <see cref="PropertyExtension.QuerySqlTypeName" /> for the cast target, since a text-typed value
        /// (the JSONB <c>-&gt;&gt;</c> operator's own return type) needs no cast at all.
        /// </summary>
        /// <param name="property">The scalar attribute to build the expression for.</param>
        /// <returns>The parenthesized expression, ready to embed as a single index element.</returns>
        private static string QueryJsonbDataExpression(this IProperty property)
        {
            var jsonAccess = $"\"data\"->>'{property.QuerySqlAttributeName()}'";
            var sqlType = property.QuerySqlTypeName();

            // text::timestamp/date parsing is only ever STABLE in Postgres (it can depend on the session's
            // DateStyle/timezone), never IMMUTABLE, so it cannot be used directly in an index expression -
            // confirmed against a live database (SQLSTATE 42P17). Route those two through a small wrapper
            // function explicitly marked IMMUTABLE instead; every other cast target (integer, boolean, ...)
            // already has an IMMUTABLE input function and needs no wrapper.
            return sqlType switch
            {
                "text" => $"({jsonAccess})",
                "timestamp" => $"(\"Forge\".jsonb_to_timestamp({jsonAccess}))",
                "date" => $"(\"Forge\".jsonb_to_date({jsonAccess}))",
                _ => $"(({jsonAccess})::{sqlType})"
            };
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
