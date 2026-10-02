-- ------------------------------------------------------------------------------------------------
-- <copyright file="Script0002_ScopeShortNameIndex.sql" company="Starion Group S.A.">
--
--   Copyright 2026 Starion Group S.A.
--   SPDX-License-Identifier: Apache-2.0
--
-- </copyright>
-- ------------------------------------------------------------------------------------------------

-- Creates a unique index on Scope (Account and Organization) shortName to enforce uniqueness
-- across both account and organization namespaces, making read operations more efficient.
CREATE UNIQUE INDEX IF NOT EXISTS "idx_scope_shortname_unique" 
ON "Forge"."Thing" (LOWER("data"->>'shortName')) 
WHERE "classKind" IN ('Account', 'Organization');
