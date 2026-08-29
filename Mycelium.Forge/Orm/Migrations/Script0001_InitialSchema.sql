------------------------------------------------------------------------------------------------
-- <copyright file="Script0001_InitialSchema.sql" company="Starion Group S.A.">
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
CREATE SCHEMA IF NOT EXISTS "Forge";

-- Root Thing table
CREATE TABLE "Forge"."Thing" (
    "id" uuid NOT NULL,
    "classKind" text,
    "data" JSONB,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."Thing" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Thing" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Thing" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Thing" SET (autovacuum_analyze_threshold = 2500);

-- Table definitions
CREATE TABLE "Forge"."Account" (
    "id" uuid NOT NULL,
    "forge" uuid NOT NULL,
    "owner" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."Account" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Account" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Account" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Account" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."Address" (
    "id" uuid NOT NULL,
    "country" uuid NOT NULL,
    "owner" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."Address" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Address" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Address" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Address" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."APIKey" (
    "id" uuid NOT NULL,
    "owner" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."APIKey" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."APIKey" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."APIKey" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."APIKey" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."Country" (
    "id" uuid NOT NULL,
    "owner" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."Country" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Country" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Country" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Country" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."Forge" (
    "id" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."Forge" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Forge" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Forge" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Forge" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."Invitation" (
    "id" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."Invitation" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Invitation" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Invitation" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Invitation" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."Namespace" (
    "id" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."Namespace" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Namespace" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Namespace" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Namespace" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."Organization" (
    "id" uuid NOT NULL,
    "owner" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."Organization" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Organization" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Organization" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Organization" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."OrganizationInvitation" (
    "id" uuid NOT NULL,
    "organization" uuid NOT NULL,
    "owner" uuid NOT NULL,
    "target" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."OrganizationInvitation" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."OrganizationInvitation" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."OrganizationInvitation" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."OrganizationInvitation" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."Package" (
    "id" uuid NOT NULL,
    "owner" uuid NOT NULL,
    "packageType" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."Package" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Package" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Package" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Package" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."PackageInvitation" (
    "id" uuid NOT NULL,
    "owner" uuid NOT NULL,
    "package" uuid NOT NULL,
    "target" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."PackageInvitation" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."PackageInvitation" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."PackageInvitation" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."PackageInvitation" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."PackageMetaData" (
    "id" uuid NOT NULL,
    "owner" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."PackageMetaData" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."PackageMetaData" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."PackageMetaData" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."PackageMetaData" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."PackageType" (
    "id" uuid NOT NULL,
    "owner" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."PackageType" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."PackageType" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."PackageType" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."PackageType" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."PackageVersion" (
    "id" uuid NOT NULL,
    "owner" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."PackageVersion" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."PackageVersion" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."PackageVersion" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."PackageVersion" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."ProfileLink" (
    "id" uuid NOT NULL,
    "owner" uuid NOT NULL,
    "profileType" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."ProfileLink" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."ProfileLink" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."ProfileLink" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."ProfileLink" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."ProfileType" (
    "id" uuid NOT NULL,
    "owner" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."ProfileType" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."ProfileType" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."ProfileType" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."ProfileType" SET (autovacuum_analyze_threshold = 2500);

CREATE TABLE "Forge"."Scope" (
    "id" uuid NOT NULL,
    "primaryAddress" uuid NOT NULL,
    PRIMARY KEY ("id")
);

ALTER TABLE "Forge"."Scope" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Scope" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Scope" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Scope" SET (autovacuum_analyze_threshold = 2500);


-- Thing foreign key constraints
ALTER TABLE "Forge"."Account" ADD CONSTRAINT "Account_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."Address" ADD CONSTRAINT "Address_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."APIKey" ADD CONSTRAINT "APIKey_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."Country" ADD CONSTRAINT "Country_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."Forge" ADD CONSTRAINT "Forge_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."Invitation" ADD CONSTRAINT "Invitation_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."Namespace" ADD CONSTRAINT "Namespace_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."Organization" ADD CONSTRAINT "Organization_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."OrganizationInvitation" ADD CONSTRAINT "OrganizationInvitation_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."Package" ADD CONSTRAINT "Package_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."PackageInvitation" ADD CONSTRAINT "PackageInvitation_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."PackageMetaData" ADD CONSTRAINT "PackageMetaData_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."PackageType" ADD CONSTRAINT "PackageType_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."PackageVersion" ADD CONSTRAINT "PackageVersion_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."ProfileLink" ADD CONSTRAINT "ProfileLink_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."ProfileType" ADD CONSTRAINT "ProfileType_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
ALTER TABLE "Forge"."Scope" ADD CONSTRAINT "Scope_Thing_FK_Source" FOREIGN KEY ("id") REFERENCES "Forge"."Thing" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;

-- Many to Many link tables including Foreign Key Constraints
CREATE TABLE "Forge"."Organization_administrator__Account" (
    "sourceOrganization" uuid NOT NULL,
    "targetAccount" uuid NOT NULL,
    PRIMARY KEY ("sourceOrganization", "targetAccount")
);

ALTER TABLE "Forge"."Organization_administrator__Account" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Organization_administrator__Account" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Organization_administrator__Account" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Organization_administrator__Account" SET (autovacuum_analyze_threshold = 2500);

ALTER TABLE "Forge"."Organization_administrator__Account" ADD CONSTRAINT "Organization_FK_Source" FOREIGN KEY ("sourceOrganization") REFERENCES "Forge"."Organization" ("id") ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX "idx_Organization_administrator_sourceOrganization" ON "Forge"."Organization_administrator__Account" ("sourceOrganization");
ALTER TABLE "Forge"."Organization_administrator__Account" ADD CONSTRAINT "Account_FK_Target" FOREIGN KEY ("targetAccount") REFERENCES "Forge"."Account" ("id") ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX "idx_Organization_administrator_targetAccount" ON "Forge"."Organization_administrator__Account" ("targetAccount");

CREATE TABLE "Forge"."Organization_member__Account" (
    "sourceOrganization" uuid NOT NULL,
    "targetAccount" uuid NOT NULL,
    PRIMARY KEY ("sourceOrganization", "targetAccount")
);

ALTER TABLE "Forge"."Organization_member__Account" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Organization_member__Account" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Organization_member__Account" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Organization_member__Account" SET (autovacuum_analyze_threshold = 2500);

ALTER TABLE "Forge"."Organization_member__Account" ADD CONSTRAINT "Organization_FK_Source" FOREIGN KEY ("sourceOrganization") REFERENCES "Forge"."Organization" ("id") ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX "idx_Organization_member_sourceOrganization" ON "Forge"."Organization_member__Account" ("sourceOrganization");
ALTER TABLE "Forge"."Organization_member__Account" ADD CONSTRAINT "Account_FK_Target" FOREIGN KEY ("targetAccount") REFERENCES "Forge"."Account" ("id") ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX "idx_Organization_member_targetAccount" ON "Forge"."Organization_member__Account" ("targetAccount");

CREATE TABLE "Forge"."Package_packageMaintainer__Account" (
    "sourcePackage" uuid NOT NULL,
    "targetAccount" uuid NOT NULL,
    PRIMARY KEY ("sourcePackage", "targetAccount")
);

ALTER TABLE "Forge"."Package_packageMaintainer__Account" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Package_packageMaintainer__Account" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Package_packageMaintainer__Account" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Package_packageMaintainer__Account" SET (autovacuum_analyze_threshold = 2500);

ALTER TABLE "Forge"."Package_packageMaintainer__Account" ADD CONSTRAINT "Package_FK_Source" FOREIGN KEY ("sourcePackage") REFERENCES "Forge"."Package" ("id") ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX "idx_Package_packageMaintainer_sourcePackage" ON "Forge"."Package_packageMaintainer__Account" ("sourcePackage");
ALTER TABLE "Forge"."Package_packageMaintainer__Account" ADD CONSTRAINT "Account_FK_Target" FOREIGN KEY ("targetAccount") REFERENCES "Forge"."Account" ("id") ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX "idx_Package_packageMaintainer_targetAccount" ON "Forge"."Package_packageMaintainer__Account" ("targetAccount");

CREATE TABLE "Forge"."Package_packageOwner__Account" (
    "sourcePackage" uuid NOT NULL,
    "targetAccount" uuid NOT NULL,
    PRIMARY KEY ("sourcePackage", "targetAccount")
);

ALTER TABLE "Forge"."Package_packageOwner__Account" SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE "Forge"."Package_packageOwner__Account" SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE "Forge"."Package_packageOwner__Account" SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE "Forge"."Package_packageOwner__Account" SET (autovacuum_analyze_threshold = 2500);

ALTER TABLE "Forge"."Package_packageOwner__Account" ADD CONSTRAINT "Package_FK_Source" FOREIGN KEY ("sourcePackage") REFERENCES "Forge"."Package" ("id") ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX "idx_Package_packageOwner_sourcePackage" ON "Forge"."Package_packageOwner__Account" ("sourcePackage");
ALTER TABLE "Forge"."Package_packageOwner__Account" ADD CONSTRAINT "Account_FK_Target" FOREIGN KEY ("targetAccount") REFERENCES "Forge"."Account" ("id") ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;
CREATE INDEX "idx_Package_packageOwner_targetAccount" ON "Forge"."Package_packageOwner__Account" ("targetAccount");


-- Reference Properties that are not Many-to-Many
ALTER TABLE "Forge"."Account" ADD CONSTRAINT "Account_forge_FK_Source" FOREIGN KEY ("forge") REFERENCES "Forge"."Forge" ("id") ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_Account_forge" ON "Forge"."Account" ("forge");
ALTER TABLE "Forge"."Account" ADD CONSTRAINT "Account_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."Forge" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_Account_owner" ON "Forge"."Account" ("owner");
ALTER TABLE "Forge"."Address" ADD CONSTRAINT "Address_country_FK_Source" FOREIGN KEY ("country") REFERENCES "Forge"."Country" ("id") ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_Address_country" ON "Forge"."Address" ("country");
ALTER TABLE "Forge"."Address" ADD CONSTRAINT "Address_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."Scope" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_Address_owner" ON "Forge"."Address" ("owner");
ALTER TABLE "Forge"."APIKey" ADD CONSTRAINT "APIKey_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."Account" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_APIKey_owner" ON "Forge"."APIKey" ("owner");
ALTER TABLE "Forge"."Country" ADD CONSTRAINT "Country_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."Forge" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_Country_owner" ON "Forge"."Country" ("owner");
ALTER TABLE "Forge"."Organization" ADD CONSTRAINT "Organization_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."Forge" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_Organization_owner" ON "Forge"."Organization" ("owner");
ALTER TABLE "Forge"."OrganizationInvitation" ADD CONSTRAINT "OrganizationInvitation_organization_FK_Source" FOREIGN KEY ("organization") REFERENCES "Forge"."Organization" ("id") ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_OrganizationInvitation_organization" ON "Forge"."OrganizationInvitation" ("organization");
ALTER TABLE "Forge"."OrganizationInvitation" ADD CONSTRAINT "OrganizationInvitation_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."Account" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_OrganizationInvitation_owner" ON "Forge"."OrganizationInvitation" ("owner");
ALTER TABLE "Forge"."OrganizationInvitation" ADD CONSTRAINT "OrganizationInvitation_target_FK_Source" FOREIGN KEY ("target") REFERENCES "Forge"."Account" ("id") ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_OrganizationInvitation_target" ON "Forge"."OrganizationInvitation" ("target");
ALTER TABLE "Forge"."Package" ADD CONSTRAINT "Package_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."Scope" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_Package_owner" ON "Forge"."Package" ("owner");
ALTER TABLE "Forge"."Package" ADD CONSTRAINT "Package_packageType_FK_Source" FOREIGN KEY ("packageType") REFERENCES "Forge"."PackageType" ("id") ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_Package_packageType" ON "Forge"."Package" ("packageType");
ALTER TABLE "Forge"."PackageInvitation" ADD CONSTRAINT "PackageInvitation_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."Account" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_PackageInvitation_owner" ON "Forge"."PackageInvitation" ("owner");
ALTER TABLE "Forge"."PackageInvitation" ADD CONSTRAINT "PackageInvitation_package_FK_Source" FOREIGN KEY ("package") REFERENCES "Forge"."Package" ("id") ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_PackageInvitation_package" ON "Forge"."PackageInvitation" ("package");
ALTER TABLE "Forge"."PackageInvitation" ADD CONSTRAINT "PackageInvitation_target_FK_Source" FOREIGN KEY ("target") REFERENCES "Forge"."Account" ("id") ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_PackageInvitation_target" ON "Forge"."PackageInvitation" ("target");
ALTER TABLE "Forge"."PackageMetaData" ADD CONSTRAINT "PackageMetaData_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."PackageVersion" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_PackageMetaData_owner" ON "Forge"."PackageMetaData" ("owner");
ALTER TABLE "Forge"."PackageType" ADD CONSTRAINT "PackageType_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."Forge" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_PackageType_owner" ON "Forge"."PackageType" ("owner");
ALTER TABLE "Forge"."PackageVersion" ADD CONSTRAINT "PackageVersion_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."Package" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_PackageVersion_owner" ON "Forge"."PackageVersion" ("owner");
ALTER TABLE "Forge"."ProfileLink" ADD CONSTRAINT "ProfileLink_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."Scope" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_ProfileLink_owner" ON "Forge"."ProfileLink" ("owner");
ALTER TABLE "Forge"."ProfileLink" ADD CONSTRAINT "ProfileLink_profileType_FK_Source" FOREIGN KEY ("profileType") REFERENCES "Forge"."ProfileType" ("id") ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_ProfileLink_profileType" ON "Forge"."ProfileLink" ("profileType");
ALTER TABLE "Forge"."ProfileType" ADD CONSTRAINT "ProfileType_owner_FK_Source" FOREIGN KEY ("owner") REFERENCES "Forge"."Forge" ("id") ON DELETE CASCADE ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_ProfileType_owner" ON "Forge"."ProfileType" ("owner");
ALTER TABLE "Forge"."Scope" ADD CONSTRAINT "Scope_primaryAddress_FK_Source" FOREIGN KEY ("primaryAddress") REFERENCES "Forge"."Address" ("id") ON UPDATE CASCADE DEFERRABLE;
CREATE INDEX "idx_Scope_primaryAddress" ON "Forge"."Scope" ("primaryAddress");

-- Runtime role delete privileges: every deletion goes through Thing, whose downward
-- ..._Thing_FK_Source ON DELETE CASCADE constraints already clean up every table sharing that id.
-- USAGE is required just to reach any object in the schema; SELECT ("id") is required because a
-- DELETE ... WHERE id = $1 statement reads the id column to evaluate the WHERE clause.
GRANT USAGE ON SCHEMA "Forge" TO forge_runtime;
REVOKE DELETE ON ALL TABLES IN SCHEMA "Forge" FROM forge_runtime;
GRANT DELETE ON "Forge"."Thing" TO forge_runtime;
GRANT SELECT ("id") ON "Forge"."Thing" TO forge_runtime;

-- ModelVersion
CREATE OR REPLACE FUNCTION "Forge".query_model_version()
RETURNS text AS $$
BEGIN
    RETURN '0.1.0';
END;
$$ LANGUAGE plpgsql;

------------------------------------------------------------------------------------------------
--------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
------------------------------------------------------------------------------------------------
