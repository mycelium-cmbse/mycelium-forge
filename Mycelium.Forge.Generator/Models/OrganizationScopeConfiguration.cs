// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationScopeConfiguration.cs" company="Starion Group S.A.">
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
    /// Configuration class holding resolved values for organization scope permission behavior.
    /// </summary>
    public class OrganizationScopeConfiguration : BehaviorConfigurationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationScopeConfiguration" /> class.
        /// </summary>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The entity behavior definition.</param>
        public OrganizationScopeConfiguration(EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
            : base(definition, behavior)
        {
            var config = behavior.Configuration;

            var scopeEntity = GetRequiredValue(config, this.EntityName, "OrganizationScope", ConfigurationKeys.ScopeEntity);
            var ocp = GetRequiredValue(config, this.EntityName, "OrganizationScope", ConfigurationKeys.OrgCreatePermission);

            var visProp = GetRequiredValueWithFallback(config, ConfigurationKeys.VisibilityProperty, definition.VisibilityProperty,
                $"Entity '{this.EntityName}' with OrganizationScope behavior must configure '{ConfigurationKeys.VisibilityProperty}' or have a VisibilityProperty in entity permissions.");

            var ownerProp = GetRequiredValueWithFallback(config, ConfigurationKeys.OwnerProperty, definition.OwnerProperty,
                $"Entity '{this.EntityName}' with OrganizationScope behavior must configure '{ConfigurationKeys.OwnerProperty}' or have an OwnerProperty in entity permissions.");

            var personalCreatePerm = GetRequiredValueWithFallback(config, ConfigurationKeys.PersonalCreatePermission, definition.CreatePermission,
                $"Entity '{this.EntityName}' with OrganizationScope behavior must configure '{ConfigurationKeys.PersonalCreatePermission}' or have a '{ConfigurationKeys.CreatePermission}'.");

            var scopeMembers = GetOptionalValue(config, ConfigurationKeys.ScopeMemberProperties, "Member,Administrator");
            var bypassPerms = GetOptionalValue(config, ConfigurationKeys.BypassPermissions, "ManageOrganizations");

            this.ScopeEntity = scopeEntity;
            this.ScopeServiceField = FormatServiceField(scopeEntity);
            this.VisibilityProperty = visProp;
            this.VisibilityPropertyLower = visProp.ToLowerInvariant();
            this.OwnerProperty = ownerProp;
            this.PersonalCreatePermission = personalCreatePerm;
            this.OrgCreatePermission = ocp;
            this.ScopeMemberProperties = SplitValues(scopeMembers);
            this.BypassPermissions = SplitValues(bypassPerms);
        }

        /// <summary>
        /// Gets the scope entity name.
        /// </summary>
        public string ScopeEntity { get; }

        /// <summary>
        /// Gets the injected domain service field name for the scope entity.
        /// </summary>
        public string ScopeServiceField { get; }

        /// <summary>
        /// Gets the visibility property name on the entity.
        /// </summary>
        public string VisibilityProperty { get; }

        /// <summary>
        /// Gets the lower-case name of the visibility property.
        /// </summary>
        public string VisibilityPropertyLower { get; }

        /// <summary>
        /// Gets the owner property name on the entity.
        /// </summary>
        public string OwnerProperty { get; }

        /// <summary>
        /// Gets the permission required to create an entity in personal scope.
        /// </summary>
        public string PersonalCreatePermission { get; }

        /// <summary>
        /// Gets the permission required to create an entity in organization scope.
        /// </summary>
        public string OrgCreatePermission { get; }

        /// <summary>
        /// Gets the property names on the organization entity representing membership.
        /// </summary>
        public string[] ScopeMemberProperties { get; }

        /// <summary>
        /// Gets the permissions that bypass membership checks for internal visibility.
        /// </summary>
        public string[] BypassPermissions { get; }
    }
}
