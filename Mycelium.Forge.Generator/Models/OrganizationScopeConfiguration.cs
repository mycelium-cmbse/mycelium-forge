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

            if (!config.TryGetValue(ConfigurationKeys.ScopeEntity, out var scopeEntity) || string.IsNullOrWhiteSpace(scopeEntity))
            {
                throw new InvalidOperationException($"Entity '{this.EntityName}' with OrganizationScope behavior must configure '{ConfigurationKeys.ScopeEntity}'.");
            }

            if (!config.TryGetValue(ConfigurationKeys.OrgCreatePermission, out var ocp) || string.IsNullOrWhiteSpace(ocp))
            {
                throw new InvalidOperationException($"Entity '{this.EntityName}' with OrganizationScope behavior must configure '{ConfigurationKeys.OrgCreatePermission}'.");
            }

            var visProp = config.TryGetValue(ConfigurationKeys.VisibilityProperty, out var vp) && !string.IsNullOrWhiteSpace(vp) ? vp : definition.VisibilityProperty;

            if (string.IsNullOrWhiteSpace(visProp))
            {
                throw new InvalidOperationException($"Entity '{this.EntityName}' with OrganizationScope behavior must configure '{ConfigurationKeys.VisibilityProperty}' or have a VisibilityProperty in entity permissions.");
            }

            var ownerProp = config.TryGetValue(ConfigurationKeys.OwnerProperty, out var op) && !string.IsNullOrWhiteSpace(op) ? op : definition.OwnerProperty;

            if (string.IsNullOrWhiteSpace(ownerProp))
            {
                throw new InvalidOperationException($"Entity '{this.EntityName}' with OrganizationScope behavior must configure '{ConfigurationKeys.OwnerProperty}' or have an OwnerProperty in entity permissions.");
            }

            var personalCreatePerm = config.TryGetValue(ConfigurationKeys.PersonalCreatePermission, out var pcp) && !string.IsNullOrWhiteSpace(pcp) ? pcp : definition.CreatePermission;

            if (string.IsNullOrWhiteSpace(personalCreatePerm))
            {
                throw new InvalidOperationException($"Entity '{this.EntityName}' with OrganizationScope behavior must configure '{ConfigurationKeys.PersonalCreatePermission}' or have a '{ConfigurationKeys.CreatePermission}'.");
            }

            var scopeMembers = config.TryGetValue(ConfigurationKeys.ScopeMemberProperties, out var sm) && !string.IsNullOrWhiteSpace(sm) ? sm : "Member,Administrator";
            var bypassPerms = config.TryGetValue(ConfigurationKeys.BypassPermissions, out var bp) && !string.IsNullOrWhiteSpace(bp) ? bp : "ManageOrganizations";

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
