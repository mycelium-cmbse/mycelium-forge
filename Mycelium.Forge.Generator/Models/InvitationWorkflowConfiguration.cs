// ------------------------------------------------------------------------------------------------
// <copyright file="InvitationWorkflowConfiguration.cs" company="Starion Group S.A.">
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
    /// Configuration class holding resolved values for invitation workflow permission behavior.
    /// </summary>
    public class InvitationWorkflowConfiguration : BehaviorConfigurationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvitationWorkflowConfiguration" /> class.
        /// </summary>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The entity behavior definition.</param>
        public InvitationWorkflowConfiguration(EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
            : base(definition, behavior)
        {
            var entityName = behavior.EntityName;
            var config = behavior.Configuration;

            if (!config.TryGetValue(ConfigurationKeys.ScopeEntity, out var scopeEntity) || string.IsNullOrWhiteSpace(scopeEntity))
            {
                throw new InvalidOperationException($"Entity '{entityName}' with InvitationWorkflow behavior must configure '{ConfigurationKeys.ScopeEntity}'.");
            }

            if (!config.TryGetValue(ConfigurationKeys.InviteeProperty, out var inviteeProp) || string.IsNullOrWhiteSpace(inviteeProp))
            {
                throw new InvalidOperationException($"Entity '{entityName}' with InvitationWorkflow behavior must configure '{ConfigurationKeys.InviteeProperty}'.");
            }

            if (!config.TryGetValue(ConfigurationKeys.AcceptPermission, out var acceptPerm) || string.IsNullOrWhiteSpace(acceptPerm))
            {
                throw new InvalidOperationException($"Entity '{entityName}' with InvitationWorkflow behavior must configure '{ConfigurationKeys.AcceptPermission}'.");
            }

            if (!config.TryGetValue(ConfigurationKeys.RevokePermission, out var revokePerm) || string.IsNullOrWhiteSpace(revokePerm))
            {
                throw new InvalidOperationException($"Entity '{entityName}' with InvitationWorkflow behavior must configure '{ConfigurationKeys.RevokePermission}'.");
            }

            if (!config.TryGetValue(ConfigurationKeys.ScopeRoles, out var sr) || string.IsNullOrWhiteSpace(sr))
            {
                throw new InvalidOperationException($"Entity '{entityName}' with InvitationWorkflow behavior must configure '{ConfigurationKeys.ScopeRoles}'.");
            }

            var ownerProp = config.TryGetValue(ConfigurationKeys.OwnerProperty, out var op) && !string.IsNullOrWhiteSpace(op) ? op : definition.OwnerProperty;

            if (string.IsNullOrWhiteSpace(ownerProp))
            {
                throw new InvalidOperationException($"Entity '{entityName}' with InvitationWorkflow behavior must configure '{ConfigurationKeys.OwnerProperty}' or have an OwnerProperty in entity permissions.");
            }

            var createPerm = config.TryGetValue(ConfigurationKeys.CreatePermission, out var cp) && !string.IsNullOrWhiteSpace(cp) ? cp : definition.CreatePermission;

            if (string.IsNullOrWhiteSpace(createPerm))
            {
                throw new InvalidOperationException($"Entity '{entityName}' with InvitationWorkflow behavior must configure '{ConfigurationKeys.CreatePermission}' or have a CreatePermission in entity permissions.");
            }

            var readPerm = config.TryGetValue(ConfigurationKeys.ReadPermission, out var rp) && !string.IsNullOrWhiteSpace(rp) ? rp : definition.ReadPermission;

            if (string.IsNullOrWhiteSpace(readPerm))
            {
                throw new InvalidOperationException($"Entity '{entityName}' with InvitationWorkflow behavior must configure '{ConfigurationKeys.ReadPermission}' or have a ReadPermission in entity permissions.");
            }

            var adminPerm = config.TryGetValue(ConfigurationKeys.AdminPermission, out var ap) && !string.IsNullOrWhiteSpace(ap) ? ap : entityName == "PackageInvitation" ? "ManagePackageTeam" : "ManageOrganizations";

            this.ScopeEntity = scopeEntity;
            this.ScopeProperty = config.TryGetValue(ConfigurationKeys.ScopeProperty, out var sp) && !string.IsNullOrWhiteSpace(sp) ? sp : scopeEntity;
            this.ScopeServiceField = FormatServiceField(scopeEntity);
            this.ScopeVar = FormatVariableName(scopeEntity);
            this.InviteeProperty = inviteeProp;
            this.InviteeColumn = char.ToLowerInvariant(inviteeProp[0]) + inviteeProp[1..];
            this.OwnerProperty = ownerProp;
            this.OwnerColumn = ownerProp.ToLowerInvariant();
            this.CreatePermission = createPerm;
            this.ReadPermission = readPerm;
            this.AcceptPermission = acceptPerm;
            this.RevokePermission = revokePerm;
            this.ScopeRoles = SplitValues(sr);
            this.ScopeRoleDescription = string.Join(" or ", this.ScopeRoles.Select(r => char.ToLowerInvariant(r[0]) + r[1..]));
            this.AdminPermission = adminPerm;
        }

        /// <summary>
        /// Gets the scope entity name.
        /// </summary>
        public string ScopeEntity { get; }

        /// <summary>
        /// Gets the scope property name on the invitation entity.
        /// </summary>
        public string ScopeProperty { get; }

        /// <summary>
        /// Gets the injected domain service field name for the scope entity.
        /// </summary>
        public string ScopeServiceField { get; }

        /// <summary>
        /// Gets the local variable name used for the scope entity instance.
        /// </summary>
        public string ScopeVar { get; }

        /// <summary>
        /// Gets the invitee property name on the invitation entity.
        /// </summary>
        public string InviteeProperty { get; }

        /// <summary>
        /// Gets the invitee database column name.
        /// </summary>
        public string InviteeColumn { get; }

        /// <summary>
        /// Gets the owner property name on the invitation entity.
        /// </summary>
        public string OwnerProperty { get; }

        /// <summary>
        /// Gets the owner database column name.
        /// </summary>
        public string OwnerColumn { get; }

        /// <summary>
        /// Gets the permission required to create an invitation.
        /// </summary>
        public string CreatePermission { get; }

        /// <summary>
        /// Gets the permission required to read an invitation.
        /// </summary>
        public string ReadPermission { get; }

        /// <summary>
        /// Gets the permission required to accept an invitation.
        /// </summary>
        public string AcceptPermission { get; }

        /// <summary>
        /// Gets the permission required to revoke an invitation.
        /// </summary>
        public string RevokePermission { get; }

        /// <summary>
        /// Gets the roles on the scope entity that can invite or revoke members.
        /// </summary>
        public string[] ScopeRoles { get; }

        /// <summary>
        /// Gets the human-readable description of the scope roles.
        /// </summary>
        public string ScopeRoleDescription { get; }

        /// <summary>
        /// Gets the administrative permission name that bypasses role checks.
        /// </summary>
        public string AdminPermission { get; }
    }
}
