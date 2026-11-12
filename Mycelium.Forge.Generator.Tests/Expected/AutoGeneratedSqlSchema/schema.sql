------------------------------------------------------------------------------------------------
-- <copyright file="schema.sql" company="Starion Group S.A.">
--
--   Copyright 2026 Starion Group S.A.
--   SPDX-License-Identifier: Apache-2.0
--
-- </copyright>
------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------
--------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
------------------------------------------------------------------------------------------------

-- Initialize database
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE SCHEMA IF NOT EXISTS forge;

-- Root Thing table
CREATE TABLE forge.thing (
    id uuid NOT NULL,
    class_kind text,
    data JSONB,
    PRIMARY KEY (id)
);

ALTER TABLE forge.thing SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.thing SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.thing SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.thing SET (autovacuum_analyze_threshold = 2500);

-- Table definitions
CREATE TABLE forge.account (
    id uuid NOT NULL,
    owner uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.account SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.account SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.account SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.account SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.address (
    id uuid NOT NULL,
    country uuid NOT NULL,
    owner uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.address SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.address SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.address SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.address SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.api_key (
    id uuid NOT NULL,
    owner uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.api_key SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.api_key SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.api_key SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.api_key SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.country (
    id uuid NOT NULL,
    owner uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.country SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.country SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.country SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.country SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.forge (
    id uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.forge SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.forge SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.forge SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.forge SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.invitation (
    id uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.invitation SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.invitation SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.invitation SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.invitation SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.namespace (
    id uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.namespace SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.namespace SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.namespace SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.namespace SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.organization (
    id uuid NOT NULL,
    owner uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.organization SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.organization SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.organization SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.organization SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.organization_invitation (
    id uuid NOT NULL,
    organization uuid NOT NULL,
    owner uuid NOT NULL,
    target uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.organization_invitation SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.organization_invitation SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.organization_invitation SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.organization_invitation SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.package (
    id uuid NOT NULL,
    owner uuid NOT NULL,
    package_type uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.package SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.package SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.package SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.package SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.package_invitation (
    id uuid NOT NULL,
    owner uuid NOT NULL,
    package uuid NOT NULL,
    target uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.package_invitation SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.package_invitation SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.package_invitation SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.package_invitation SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.package_meta_data (
    id uuid NOT NULL,
    owner uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.package_meta_data SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.package_meta_data SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.package_meta_data SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.package_meta_data SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.package_type (
    id uuid NOT NULL,
    owner uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.package_type SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.package_type SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.package_type SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.package_type SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.package_version (
    id uuid NOT NULL,
    owner uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.package_version SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.package_version SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.package_version SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.package_version SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.profile_link (
    id uuid NOT NULL,
    owner uuid NOT NULL,
    profile_type uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.profile_link SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.profile_link SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.profile_link SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.profile_link SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.profile_type (
    id uuid NOT NULL,
    owner uuid NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE forge.profile_type SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.profile_type SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.profile_type SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.profile_type SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE forge.scope (
    id uuid NOT NULL,
    primary_address uuid,
    PRIMARY KEY (id)
);

ALTER TABLE forge.scope SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.scope SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.scope SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.scope SET (autovacuum_analyze_threshold = 2500);


-- Thing foreign key constraints
ALTER TABLE forge.account ADD CONSTRAINT account_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.address ADD CONSTRAINT address_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.api_key ADD CONSTRAINT api_key_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.country ADD CONSTRAINT country_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.forge ADD CONSTRAINT forge_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.invitation ADD CONSTRAINT invitation_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.namespace ADD CONSTRAINT namespace_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.organization ADD CONSTRAINT organization_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.organization_invitation ADD CONSTRAINT organization_invitation_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.package ADD CONSTRAINT package_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.package_invitation ADD CONSTRAINT package_invitation_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.package_meta_data ADD CONSTRAINT package_meta_data_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.package_type ADD CONSTRAINT package_type_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.package_version ADD CONSTRAINT package_version_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.profile_link ADD CONSTRAINT profile_link_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.profile_type ADD CONSTRAINT profile_type_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE forge.scope ADD CONSTRAINT scope_thing_fk_source FOREIGN KEY (id) REFERENCES forge.thing (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;

-- Many to Many link tables including Foreign Key Constraints
CREATE TABLE forge.forge_administrator__account (
    source_forge uuid NOT NULL,
    target_account uuid NOT NULL,
    PRIMARY KEY (source_forge, target_account)
);

ALTER TABLE forge.forge_administrator__account SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.forge_administrator__account SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.forge_administrator__account SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.forge_administrator__account SET (autovacuum_analyze_threshold = 2500);

ALTER TABLE forge.forge_administrator__account ADD CONSTRAINT forge_fk_source FOREIGN KEY (source_forge) REFERENCES forge.forge (id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX idx_forge_administrator_source_forge ON forge.forge_administrator__account (source_forge);
ALTER TABLE forge.forge_administrator__account ADD CONSTRAINT account_fk_target FOREIGN KEY (target_account) REFERENCES forge.account (id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX idx_forge_administrator_target_account ON forge.forge_administrator__account (target_account);

CREATE TABLE forge.organization_administrator__account (
    source_organization uuid NOT NULL,
    target_account uuid NOT NULL,
    PRIMARY KEY (source_organization, target_account)
);

ALTER TABLE forge.organization_administrator__account SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.organization_administrator__account SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.organization_administrator__account SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.organization_administrator__account SET (autovacuum_analyze_threshold = 2500);

ALTER TABLE forge.organization_administrator__account ADD CONSTRAINT organization_fk_source FOREIGN KEY (source_organization) REFERENCES forge.organization (id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX idx_organization_administrator_source_organization ON forge.organization_administrator__account (source_organization);
ALTER TABLE forge.organization_administrator__account ADD CONSTRAINT account_fk_target FOREIGN KEY (target_account) REFERENCES forge.account (id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX idx_organization_administrator_target_account ON forge.organization_administrator__account (target_account);

CREATE TABLE forge.organization_member__account (
    source_organization uuid NOT NULL,
    target_account uuid NOT NULL,
    PRIMARY KEY (source_organization, target_account)
);

ALTER TABLE forge.organization_member__account SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.organization_member__account SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.organization_member__account SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.organization_member__account SET (autovacuum_analyze_threshold = 2500);

ALTER TABLE forge.organization_member__account ADD CONSTRAINT organization_fk_source FOREIGN KEY (source_organization) REFERENCES forge.organization (id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX idx_organization_member_source_organization ON forge.organization_member__account (source_organization);
ALTER TABLE forge.organization_member__account ADD CONSTRAINT account_fk_target FOREIGN KEY (target_account) REFERENCES forge.account (id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX idx_organization_member_target_account ON forge.organization_member__account (target_account);

CREATE TABLE forge.package_package_maintainer__account (
    source_package uuid NOT NULL,
    target_account uuid NOT NULL,
    PRIMARY KEY (source_package, target_account)
);

ALTER TABLE forge.package_package_maintainer__account SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.package_package_maintainer__account SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.package_package_maintainer__account SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.package_package_maintainer__account SET (autovacuum_analyze_threshold = 2500);

ALTER TABLE forge.package_package_maintainer__account ADD CONSTRAINT package_fk_source FOREIGN KEY (source_package) REFERENCES forge.package (id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX idx_package_package_maintainer_source_package ON forge.package_package_maintainer__account (source_package);
ALTER TABLE forge.package_package_maintainer__account ADD CONSTRAINT account_fk_target FOREIGN KEY (target_account) REFERENCES forge.account (id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX idx_package_package_maintainer_target_account ON forge.package_package_maintainer__account (target_account);

CREATE TABLE forge.package_package_owner__account (
    source_package uuid NOT NULL,
    target_account uuid NOT NULL,
    PRIMARY KEY (source_package, target_account)
);

ALTER TABLE forge.package_package_owner__account SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.package_package_owner__account SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.package_package_owner__account SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.package_package_owner__account SET (autovacuum_analyze_threshold = 2500);

ALTER TABLE forge.package_package_owner__account ADD CONSTRAINT package_fk_source FOREIGN KEY (source_package) REFERENCES forge.package (id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX idx_package_package_owner_source_package ON forge.package_package_owner__account (source_package);
ALTER TABLE forge.package_package_owner__account ADD CONSTRAINT account_fk_target FOREIGN KEY (target_account) REFERENCES forge.account (id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX idx_package_package_owner_target_account ON forge.package_package_owner__account (target_account);


-- Reference Properties that are not Many-to-Many
ALTER TABLE forge.account ADD CONSTRAINT account_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.forge (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_account_owner ON forge.account (owner);
ALTER TABLE forge.address ADD CONSTRAINT address_country_fk_source FOREIGN KEY (country) REFERENCES forge.country (id) ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_address_country ON forge.address (country);
ALTER TABLE forge.address ADD CONSTRAINT address_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.scope (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_address_owner ON forge.address (owner);
ALTER TABLE forge.api_key ADD CONSTRAINT api_key_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.account (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_api_key_owner ON forge.api_key (owner);
ALTER TABLE forge.country ADD CONSTRAINT country_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.forge (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_country_owner ON forge.country (owner);
ALTER TABLE forge.organization ADD CONSTRAINT organization_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.forge (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_organization_owner ON forge.organization (owner);
ALTER TABLE forge.organization_invitation ADD CONSTRAINT organization_invitation_organization_fk_source FOREIGN KEY (organization) REFERENCES forge.organization (id) ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_organization_invitation_organization ON forge.organization_invitation (organization);
ALTER TABLE forge.organization_invitation ADD CONSTRAINT organization_invitation_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.account (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_organization_invitation_owner ON forge.organization_invitation (owner);
ALTER TABLE forge.organization_invitation ADD CONSTRAINT organization_invitation_target_fk_source FOREIGN KEY (target) REFERENCES forge.account (id) ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_organization_invitation_target ON forge.organization_invitation (target);
ALTER TABLE forge.package ADD CONSTRAINT package_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.scope (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_package_owner ON forge.package (owner);
ALTER TABLE forge.package ADD CONSTRAINT package_package_type_fk_source FOREIGN KEY (package_type) REFERENCES forge.package_type (id) ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_package_package_type ON forge.package (package_type);
ALTER TABLE forge.package_invitation ADD CONSTRAINT package_invitation_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.account (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_package_invitation_owner ON forge.package_invitation (owner);
ALTER TABLE forge.package_invitation ADD CONSTRAINT package_invitation_package_fk_source FOREIGN KEY (package) REFERENCES forge.package (id) ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_package_invitation_package ON forge.package_invitation (package);
ALTER TABLE forge.package_invitation ADD CONSTRAINT package_invitation_target_fk_source FOREIGN KEY (target) REFERENCES forge.account (id) ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_package_invitation_target ON forge.package_invitation (target);
ALTER TABLE forge.package_meta_data ADD CONSTRAINT package_meta_data_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.package_version (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_package_meta_data_owner ON forge.package_meta_data (owner);
ALTER TABLE forge.package_type ADD CONSTRAINT package_type_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.forge (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_package_type_owner ON forge.package_type (owner);
ALTER TABLE forge.package_version ADD CONSTRAINT package_version_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.package (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_package_version_owner ON forge.package_version (owner);
ALTER TABLE forge.profile_link ADD CONSTRAINT profile_link_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.scope (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_profile_link_owner ON forge.profile_link (owner);
ALTER TABLE forge.profile_link ADD CONSTRAINT profile_link_profile_type_fk_source FOREIGN KEY (profile_type) REFERENCES forge.profile_type (id) ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_profile_link_profile_type ON forge.profile_link (profile_type);
ALTER TABLE forge.profile_type ADD CONSTRAINT profile_type_owner_fk_source FOREIGN KEY (owner) REFERENCES forge.forge (id) ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_profile_type_owner ON forge.profile_type (owner);
ALTER TABLE forge.scope ADD CONSTRAINT scope_primary_address_fk_source FOREIGN KEY (primary_address) REFERENCES forge.address (id) ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX idx_scope_primary_address ON forge.scope (primary_address);

-- Indexable Value Properties
CREATE INDEX idx_api_key_expires_at ON forge.thing ((data ->> 'expiresAt')) WHERE class_kind = 'APIKey';
CREATE INDEX idx_api_key_last_used_at ON forge.thing ((data ->> 'lastUsedAt')) WHERE class_kind = 'APIKey';
CREATE INDEX idx_api_key_name ON forge.thing ((data ->> 'name')) WHERE class_kind = 'APIKey';
CREATE INDEX idx_country_name ON forge.thing ((data ->> 'name')) WHERE class_kind = 'Country';
CREATE INDEX idx_namespace_name ON forge.thing ((data ->> 'name')) WHERE class_kind IN ('Account', 'Forge', 'Organization', 'Package');
CREATE INDEX idx_namespace_short_name ON forge.thing ((data ->> 'shortName')) WHERE class_kind IN ('Account', 'Forge', 'Organization', 'Package');
CREATE INDEX idx_package_description ON forge.thing ((data ->> 'description')) WHERE class_kind = 'Package';
CREATE INDEX idx_package_type_name ON forge.thing ((data ->> 'name')) WHERE class_kind = 'PackageType';
CREATE INDEX idx_scope_email ON forge.thing ((data ->> 'email')) WHERE class_kind IN ('Account', 'Organization');
CREATE INDEX idx_scope_origin ON forge.thing ((data ->> 'origin')) WHERE class_kind IN ('Account', 'Organization');
CREATE INDEX idx_scope_status ON forge.thing ((data ->> 'status')) WHERE class_kind IN ('Account', 'Organization');

-- Timestamp Trigger Functions for Thing Table
CREATE OR REPLACE FUNCTION forge.thing_timestamps_insert()
RETURNS trigger
LANGUAGE plpgsql
AS $$
    DECLARE
        ts text := to_char(transaction_timestamp() AT TIME ZONE 'UTC', 'YYYY-MM-DD"T"HH24:MI:SS.US"Z"');
    BEGIN

        NEW.data = COALESCE(NEW.data, '{}'::jsonb)
                || jsonb_build_object('createdAt', ts, 'modifiedAt', ts);

        RETURN NEW;
    END;
$$;

CREATE OR REPLACE FUNCTION forge.thing_timestamps_update()
RETURNS trigger
LANGUAGE plpgsql
AS $$
    DECLARE
        ts text := to_char(transaction_timestamp() AT TIME ZONE 'UTC', 'YYYY-MM-DD"T"HH24:MI:SS.US"Z"');
    BEGIN

        -- createdAt is immutable: always taken from the existing row (fallback: now)
        NEW.data = COALESCE(NEW.data, '{}'::jsonb)
                || jsonb_build_object(
                       'createdAt',  COALESCE(OLD.data ->> 'createdAt', ts),
                       'modifiedAt', ts
                   );

        RETURN NEW;
    END;
$$;

-- Timestamp Triggers for Thing Table
CREATE OR REPLACE TRIGGER trg_thing_timestamps_insert
    BEFORE INSERT ON forge.thing
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_timestamps_insert();

-- WHEN: skip the function entirely for no-op updates (modifiedAt is not bumped)
CREATE OR REPLACE TRIGGER trg_thing_timestamps_update
    BEFORE UPDATE ON forge.thing
    FOR EACH ROW
    WHEN (OLD.* IS DISTINCT FROM NEW.*)
        EXECUTE FUNCTION forge.thing_timestamps_update();

CREATE OR REPLACE FUNCTION forge.thing_delete()
RETURNS trigger
LANGUAGE plpgsql
AS $$
    BEGIN

        EXECUTE 'DELETE FROM forge.thing WHERE id = $1' USING OLD.id;

        RETURN OLD;
    END;
$$;

-- Delete Trigger Functions for Base Tables
CREATE OR REPLACE FUNCTION forge.invitation_delete()
    RETURNS trigger
    LANGUAGE plpgsql
    AS $$
    BEGIN
        EXECUTE 'DELETE FROM forge.invitation WHERE id = $1' USING OLD.id;
        RETURN OLD;
    END;
$$;

CREATE OR REPLACE FUNCTION forge.namespace_delete()
    RETURNS trigger
    LANGUAGE plpgsql
    AS $$
    BEGIN
        EXECUTE 'DELETE FROM forge.namespace WHERE id = $1' USING OLD.id;
        RETURN OLD;
    END;
$$;

CREATE OR REPLACE FUNCTION forge.scope_delete()
    RETURNS trigger
    LANGUAGE plpgsql
    AS $$
    BEGIN
        EXECUTE 'DELETE FROM forge.scope WHERE id = $1' USING OLD.id;
        RETURN OLD;
    END;
$$;


-- Delete Triggers to the Thing Table
CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.account
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.address
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.api_key
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.country
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.forge
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.invitation
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.namespace
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.organization
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.organization_invitation
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.package
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.package_invitation
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.package_meta_data
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.package_type
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.package_version
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.profile_link
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.profile_type
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();

CREATE OR REPLACE TRIGGER trg_thing_delete
    AFTER DELETE ON forge.scope
    FOR EACH ROW
        EXECUTE FUNCTION forge.thing_delete();


-- Delete Triggers to the Base Table Table
CREATE OR REPLACE TRIGGER trg_scope_on_account_delete
    AFTER DELETE ON forge.account
    FOR EACH ROW
        EXECUTE FUNCTION forge.scope_delete();

CREATE OR REPLACE TRIGGER trg_namespace_on_forge_delete
    AFTER DELETE ON forge.forge
    FOR EACH ROW
        EXECUTE FUNCTION forge.namespace_delete();

CREATE OR REPLACE TRIGGER trg_scope_on_organization_delete
    AFTER DELETE ON forge.organization
    FOR EACH ROW
        EXECUTE FUNCTION forge.scope_delete();

CREATE OR REPLACE TRIGGER trg_invitation_on_organization_invitation_delete
    AFTER DELETE ON forge.organization_invitation
    FOR EACH ROW
        EXECUTE FUNCTION forge.invitation_delete();

CREATE OR REPLACE TRIGGER trg_namespace_on_package_delete
    AFTER DELETE ON forge.package
    FOR EACH ROW
        EXECUTE FUNCTION forge.namespace_delete();

CREATE OR REPLACE TRIGGER trg_invitation_on_package_invitation_delete
    AFTER DELETE ON forge.package_invitation
    FOR EACH ROW
        EXECUTE FUNCTION forge.invitation_delete();

CREATE OR REPLACE TRIGGER trg_namespace_on_scope_delete
    AFTER DELETE ON forge.scope
    FOR EACH ROW
        EXECUTE FUNCTION forge.namespace_delete();


-- ModelVersion
CREATE OR REPLACE FUNCTION forge.query_model_version()
RETURNS text AS $$
BEGIN
    RETURN '0.5.0';
END;
$$ LANGUAGE plpgsql;

------------------------------------------------------------------------------------------------
--------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
------------------------------------------------------------------------------------------------
