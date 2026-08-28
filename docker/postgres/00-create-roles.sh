#!/bin/sh
# Runs once via docker-entrypoint-initdb.d, on first init of an empty data directory. Creates the
# least-privileged runtime role (forge_runtime only ever gets DELETE on Forge.Thing, nothing else)
# alongside the admin/migrator role the image itself bootstraps from POSTGRES_USER. A shell script
# rather than a plain .sql file so the password can come from the container's own environment
# (FORGE_RUNTIME_PASSWORD) instead of being hardcoded here - the same script runs unmodified in
# dev (a fixed, checked-in value) and production (a real generated secret).
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<EOSQL
CREATE ROLE forge_runtime WITH LOGIN PASSWORD '$FORGE_RUNTIME_PASSWORD';
EOSQL
