------------------------------------------------------------------------------------------------
-- <copyright file="Script0002_Outbox.sql" company="Starion Group S.A.">
--
--   Copyright 2026 Starion Group S.A.
--   SPDX-License-Identifier: Apache-2.0
--
-- </copyright>
------------------------------------------------------------------------------------------------

CREATE TABLE forge.outbox_message (
    id uuid PRIMARY KEY DEFAULT uuidv7(),
    aggregate_id uuid NOT NULL,
    event_type text NOT NULL,
    payload jsonb NOT NULL,
    status text NOT NULL DEFAULT 'PENDING' CHECK (status IN ('PENDING', 'FAILED')),
    attempts integer NOT NULL DEFAULT 0 CHECK (attempts >= 0),
    next_attempt_at timestamptz NOT NULL DEFAULT now(),
    created_at timestamptz NOT NULL DEFAULT now(),
    last_error text NULL,
    claim_token uuid NULL
);

ALTER TABLE forge.outbox_message SET (autovacuum_vacuum_scale_factor = 0.0);
ALTER TABLE forge.outbox_message SET (autovacuum_vacuum_threshold = 2500);
ALTER TABLE forge.outbox_message SET (autovacuum_analyze_scale_factor = 0.0);
ALTER TABLE forge.outbox_message SET (autovacuum_analyze_threshold = 2500);

CREATE INDEX idx_outbox_message_pending_next_attempt
    ON forge.outbox_message (next_attempt_at, id)
    WHERE status = 'PENDING';

CREATE INDEX idx_outbox_message_pending_aggregate
    ON forge.outbox_message (aggregate_id, id)
    WHERE status = 'PENDING';
