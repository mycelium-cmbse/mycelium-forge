# Local development environment

This is the seed path for F-07: everything needed to reach a working local Forge instance from a
clean clone. See `docs/design.md` DD-09, DD-18, DD-20, DD-21, DD-23 and §12.2 for the reasoning
behind the choices this document only records.

## Clean-clone path

```
docker compose up
```

This builds the app image and brings up PostgreSQL 18, Garage (S3-compatible object storage), Forge's
own Keycloak, a one-shot migrator, and the app itself, in that order - the migrator must complete
successfully before the app starts (DD-18). Once it's up, the app is reachable at
`http://localhost:8080`.

Every credential in `docker-compose.yml` and `docker/garage.toml` is a fixed, checked-in, dev-only
value. None of it is meant to be reused anywhere real.

## Devcontainer path

Open the repository in an editor with Dev Containers support - VS Code's
[Dev Containers extension](https://containers.dev/) or JetBrains Gateway/Rider both implement the same
open specification, so there's no per-editor setup - and reopen it in the container. This brings up
PostgreSQL, Garage, Keycloak and a completed migrator run alongside the workspace, without also
starting a competing `app` container: the app is meant to be run and debugged interactively
(`dotnet run`, F5, or your editor's equivalent) from inside the workspace itself.

## The identity provider

Forge ships and runs its **own** Keycloak in every environment, including production (DD-20) - it is
not a stand-in for anything. Interactive authentication is OIDC against it. Where an upstream identity
provider exists (Fabric's, or an enterprise one), Forge's Keycloak can federate to it, but that is
optional configuration, never a requirement: **production Forge does not use Fabric's Keycloak**, and
a standalone Forge installation with no upstream IdP configured works unchanged. The development
environment therefore runs the exact same component production does, not a substitute for it.

The `docker-compose.yml` `keycloak` service imports a minimal realm
(`docker/keycloak/forge-realm.json`) - just enough for the container to exist and be reachable. The
account and organization administration surface (registration, membership, invitations) is separate,
later work and isn't part of this environment yet.

## What's intentionally left out

- **No PgBouncer.** §12.2 records it as a contingency for SaaS-scale connection pressure, not a
  standard component - a single-replica `docker compose`/on-premise/air-gapped environment (this one)
  doesn't need it.
- **No real domain schema yet.** `Orm/Migrations/Script0001_Placeholder.sql` is an explicitly
  temporary migration that proves the DbUp pipeline moves a script through the journal end-to-end. The
  real schema is generated from the Enterprise Architect model (DD-18) once F-10 (DAO/schema
  generation) and A-01 (the real baseline migration) land - at which point this placeholder is removed
  entirely, not built upon.
- **No `/ready` schema-journal gate yet.** DD-18 calls for every replica to verify at startup that the
  migration journal holds every embedded script, and fail `/ready` (not `/healthz`) if it doesn't.
  That's deferred out of this environment for now; `/healthz` and `/ready` currently report the same
  (empty) check set.
