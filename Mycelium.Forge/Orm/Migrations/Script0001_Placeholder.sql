-- Placeholder migration (F-07): proves the DbUp pipeline moves a real script through the journal
-- end-to-end, from a clean database to a running application. This is NOT the real Forge schema -
-- that is generated from the Enterprise Architect model per DD-18 and becomes migration 0001 once
-- F-10 (DAO/schema generation) and A-01 (the real baseline migration) land. This script, and this
-- comment, are removed entirely when that happens - it is not meant to survive alongside the real
-- schema.
CREATE TABLE IF NOT EXISTS f07_placeholder
(
    id         serial PRIMARY KEY,
    created_at timestamptz NOT NULL DEFAULT now()
);
