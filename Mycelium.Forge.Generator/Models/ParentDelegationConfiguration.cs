// ------------------------------------------------------------------------------------------------
// <copyright file="ParentDelegationConfiguration.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Models
{
    using Mycelium.Forge.Generator.Constants;
    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;

    /// <summary>
    /// Configuration class holding resolved values for parent delegation permission behavior.
    /// </summary>
    public class ParentDelegationConfiguration : BehaviorConfigurationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParentDelegationConfiguration" /> class.
        /// </summary>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The entity behavior definition.</param>
        public ParentDelegationConfiguration(EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
            : base(definition, behavior)
        {
            var config = behavior.Configuration;

            var parentEntity = GetValue(config, ConfigurationKeys.ParentEntity, errorMessage: $"Entity '{this.EntityName}' with ParentDelegation behavior must configure '{ConfigurationKeys.ParentEntity}'.");

            var createPerm = GetValue(config, ConfigurationKeys.CreatePermission, definition.CreatePermission,
                errorMessage: $"Entity '{this.EntityName}' with ParentDelegation behavior must configure '{ConfigurationKeys.CreatePermission}' or have a CreatePermission in entity permissions.");

            var deletePerm = GetValue(config, ConfigurationKeys.DeletePermission, definition.DeletePermission,
                errorMessage: $"Entity '{this.EntityName}' with ParentDelegation behavior must configure '{ConfigurationKeys.DeletePermission}' or have a DeletePermission in entity permissions.");

            var stateActivePerm = GetValue(config, ConfigurationKeys.StateActivePermission, errorMessage: $"Entity '{this.EntityName}' with ParentDelegation behavior must configure '{ConfigurationKeys.StateActivePermission}'.");
            var stateInactivePerm = GetValue(config, ConfigurationKeys.StateInactivePermission, errorMessage: $"Entity '{this.EntityName}' with ParentDelegation behavior must configure '{ConfigurationKeys.StateInactivePermission}'.");

            var parentKey = GetValue(config, ConfigurationKeys.ParentKey, parentEntity, isOptional: true);
            var parentOwnerProps = GetValue(config, ConfigurationKeys.ParentOwnerProperties, "Owner,Maintainer", isOptional: true);
            var stateProp = GetValue(config, ConfigurationKeys.StateProperty, PropertyNames.IsActive, isOptional: true);

            this.ParentEntity = parentEntity;
            this.ParentKey = parentKey;
            this.ParentKeyColumn = parentKey.ToLowerInvariant();
            this.ParentServiceType = $"I{parentEntity}Service";
            this.ParentServiceField = FormatServiceField(parentEntity);
            this.ParentPermServiceType = $"I{parentEntity}PermissionService";
            this.ParentPermServiceField = $"{char.ToLowerInvariant(parentEntity[0])}{parentEntity[1..]}PermissionService";
            this.ParentVar = FormatVariableName(parentEntity);
            this.CreatePermission = createPerm;
            this.DeletePermission = deletePerm;
            this.ParentOwnerProperties = SplitValues(parentOwnerProps);
            this.StateProperty = stateProp;
            this.StateActivePermission = stateActivePerm;
            this.StateInactivePermission = stateInactivePerm;
        }

        /// <summary>
        /// Gets the parent entity name.
        /// </summary>
        public string ParentEntity { get; }

        /// <summary>
        /// Gets the foreign key property name pointing to the parent entity.
        /// </summary>
        public string ParentKey { get; }

        /// <summary>
        /// Gets the lower-case database column name for the foreign key.
        /// </summary>
        public string ParentKeyColumn { get; }

        /// <summary>
        /// Gets the parent entity domain service interface type name.
        /// </summary>
        public string ParentServiceType { get; }

        /// <summary>
        /// Gets the parent entity domain service field name.
        /// </summary>
        public string ParentServiceField { get; }

        /// <summary>
        /// Gets the parent entity permission service interface type name.
        /// </summary>
        public string ParentPermServiceType { get; }

        /// <summary>
        /// Gets the parent entity permission service field name.
        /// </summary>
        public string ParentPermServiceField { get; }

        /// <summary>
        /// Gets the local variable name used for the parent entity instance.
        /// </summary>
        public string ParentVar { get; }

        /// <summary>
        /// Gets the permission required to create a child entity.
        /// </summary>
        public string CreatePermission { get; }

        /// <summary>
        /// Gets the property names on the parent entity checked for ownership or maintenance.
        /// </summary>
        public string[] ParentOwnerProperties { get; }

        /// <summary>
        /// Gets the mutable state property name on the child entity.
        /// </summary>
        public string StateProperty { get; }

        /// <summary>
        /// Gets the permission required to activate the child entity state.
        /// </summary>
        public string StateActivePermission { get; }

        /// <summary>
        /// Gets the permission required to deactivate the child entity state.
        /// </summary>
        public string StateInactivePermission { get; }

        /// <summary>
        /// Gets the permission required to delete a child entity.
        /// </summary>
        public string DeletePermission { get; }
    }
}
