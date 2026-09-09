// ------------------------------------------------------------------------------------------------
// <copyright file="ScopedBehaviorTypeHelperBase.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers.BehaviorTypeHelpers
{
    using System.Text;

    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;
    using Mycelium.Forge.Generator.Models;

    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Base class for entity behavior type helpers that depend on a scope entity domain service.
    /// </summary>
    /// <typeparam name="TConfiguration">The resolved configuration type implementing <see cref="IScopeConfiguration" />.</typeparam>
    public abstract class ScopedBehaviorTypeHelperBase<TConfiguration> : BehaviorTypeHelperBase<TConfiguration>
        where TConfiguration : class, IScopeConfiguration
    {
        /// <summary>
        /// Writes fields, constructors, and dependency injection parameters for the entity class.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        public override void WriteFieldsAndConstructors(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            var config = this.GetConfiguration(definition, behavior);
            var scopeService = $"I{config.ScopeEntity}Service";

            stringBuilder.AppendLine($$"""
                                               /// <summary>
                                               /// The (injected) <see cref="{{scopeService}}" /> domain service.
                                               /// </summary>
                                               private readonly {{scopeService}} {{config.ScopeServiceField}};

                                               /// <summary>
                                               /// Initializes a new instance of the <see cref="{{@class.Name}}PermissionService"/> class.
                                               /// </summary>
                                               public {{@class.Name}}PermissionService()
                                               {
                                               }

                                               /// <summary>
                                               /// Initializes a new instance of the <see cref="{{@class.Name}}PermissionService"/> class.
                                               /// </summary>
                                               /// <param name="{{config.ScopeServiceField}}">The (injected) <see cref="{{scopeService}}" /> domain service.</param>
                                               public {{@class.Name}}PermissionService({{scopeService}} {{config.ScopeServiceField}})
                                               {
                                                   this.{{config.ScopeServiceField}} = {{config.ScopeServiceField}};
                                               }
                                       """);
        }
    }
}
