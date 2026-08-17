# Getting started

How to go from a clean clone of `mycelium-forge` to a running, debuggable instance of Forge on your
own machine. This walks through **JetBrains Rider** as the reference IDE, with a plain-`docker compose`
path alongside it for anyone not using Rider's Dev Containers support.

If you just want to see the app running without opening an editor at all, skip to
[Just run it](#just-run-it).

## Prerequisites

- **Docker Desktop**, running, before you do anything else below.
- **JetBrains Rider**, a reasonably current version — Dev Containers support ships bundled; if you
  don't see the options described below, update Rider first.
- Git, obviously, and an SSH agent running locally if you plan to open the project as a Dev Container
  directly from its GitHub URL rather than from an already-cloned folder (see
  [Option A2](#a2-from-the-welcome-screen-no-clone-needed) below).

You do **not** need the .NET SDK installed locally for the Dev Container path — it's already inside
the container. You do need it if you choose [Option B](#option-b-run-the-supporting-services-only)
instead.

## Option A: Dev Container in Rider (recommended)

This gets you PostgreSQL, Garage (object storage), Keycloak (identity provider) and a completed
database migration running alongside a full .NET SDK, with the repository's source live-mounted —
edit on your host, the container sees it immediately.

### A1. From an already-cloned repo

1. Clone the repo and open it in Rider as a normal local project.
2. Open the **Services** tool window (bottom toolbar, or <kbd>Alt+8</kbd>).
3. Find **Dev Containers** in the list, right-click it (or use the **+** button) and choose
   **Create Dev Container from local project**.

   (There's also a gutter icon in `devcontainer.json` itself that does the same thing — **Create Dev
   Container and Mount Sources…** — but it only appears once Rider has a working Docker connection
   configured under `Settings/Preferences → Build, Execution, Deployment → Docker`; the Services tool
   window route above works regardless and is easier to find.)
4. Pick **Rider** as the backend IDE when prompted.
5. Watch progress in the **Services** tool window — this is Rider building the workspace image,
   starting the supporting containers (Postgres, Garage, Keycloak, and a one-shot migration run), and
   **downloading the Rider backend into the container**. That backend download only happens once and
   can take a while (several hundred MB+) — subsequent connects reuse it and are fast.
6. Once it's ready, click **Connect**. Rider reopens the project from inside the container.

### A2. From the Welcome screen (no clone needed)

1. On Rider's Welcome screen, click **Remote Development**, then **Create Dev Container**.
2. Rider auto-detects your local Docker connection. If it doesn't, make sure Docker Desktop is
   running.
3. Choose **Rider** as the backend IDE.
4. Under project source, choose **Git Repository** and give it this repo's URL. Let it auto-detect
   `.devcontainer/devcontainer.json`.
5. Click **Build Container and Continue**.
6. Once built, the project opens inside the container automatically.

### Once you're connected

You're now working inside the `workspace` container, with the repository bind-mounted at
`/workspaces/mycelium-forge` and the .NET SDK already there. Postgres, Garage and Keycloak are running
alongside you as separate containers on the same network, and the placeholder database migration has
already run.

- **Run/debug the app**: open `Mycelium.Forge.sln`, make sure `Mycelium.Forge` is the startup project,
  and Run or Debug as usual (▶ / 🐞 in the toolbar, or <kbd>Shift+F10</kbd> / <kbd>Shift+F9</kbd>). The
  connection string is already set via environment variable inside the container, so this just works —
  no local `appsettings.json` to create.
- **Run tests**: use Rider's built-in test runner (right-click a test project or class → **Run
  Unit Tests**), or `dotnet test` from the integrated terminal.
- The `app` service defined in `docker-compose.yml` is **deliberately not started** by the dev
  container — you're running the app yourself, interactively, instead of it running as its own
  container. If you also want to see the fully containerized app running side-by-side, see
  [Just run it](#just-run-it) in a separate terminal.

### Claude Code inside the Dev Container

The Dev Container includes the Claude Code CLI and runs as a non-root user (`forge`), so
`claude --dangerously-skip-permissions` is available from the integrated terminal if you want it — the CLI
refuses that flag when launched as root, which is why the container doesn't run as root in the first place.
Login state persists across rebuilds (it lives in a named Docker volume), so you only need to `claude login`
once. Skipping permission prompts is still a real trust decision, not just a root-vs-non-root technicality —
only use it in a container you're comfortable giving broad, prompt-free access to.

## Option B: run the supporting services only

If you'd rather work outside a Dev Container — plain Rider on your host, .NET SDK installed locally —
you can still get Postgres/Garage/Keycloak from Compose and just point Rider at them directly.

1. From the repo root, start only the supporting services (skip `app`, which you're about to run
   yourself):
   ```
   docker compose up postgres garage keycloak migrator
   ```
   Wait for the migrator to log `Upgrade successful` and exit — that's Postgres ready and migrated.
2. Open `Mycelium.Forge.sln` in Rider normally (no Dev Container).
3. Edit the `Mycelium.Forge` run configuration and add an environment variable:
   ```
   ConnectionStrings__Default=Host=localhost;Port=5432;Database=forge;Username=forge;Password=forge-dev-password
   ```
4. Run or debug as usual.

## Just run it

No editor, no interactive development — just see a working Forge instance:

```
docker compose up
```

This builds the app image and brings up everything: Postgres, Garage, Keycloak, the one-shot migrator,
and the app itself, in dependency order (the app doesn't start until the migrator finishes
successfully). Once it settles, browse to **http://localhost:8080**.

Run it a second time and it's a clean no-op — the migrator sees nothing new to apply and the app
starts immediately.

## Resetting local state

```
docker compose down -v
```

This removes the containers **and** their volumes — Postgres data, Garage's buckets/keys, Keycloak's
realm state. The next `docker compose up` starts from scratch. Use this if something's in a state you
don't trust, or after pulling changes that touch the database schema.

## Troubleshooting

- **"Cannot connect to the Docker daemon"** — Docker Desktop isn't running. Start it and wait for it to
  fully initialize before retrying.
- **Port already in use** (5432, 8080, 8081, 3900, 3903) — something else on your machine is already
  using it. Stop that, or edit the port mapping in `docker-compose.yml`.
- **A service won't go "healthy"** — check its logs: `docker compose logs <service>`. `docker compose
  ps` shows the current status of everything.
- **Stale/weird state after switching branches** — `docker compose down -v`, then bring it back up.

## What's here, and what isn't yet

- Every credential in `docker-compose.yml` / `docker/garage.toml` is a fixed, checked-in, dev-only
  value. Never reuse any of it anywhere real.
- The database migration that runs is a small **placeholder**, not the real Forge schema — that
  lands once the model-driven schema generator ships. Don't build on it.
- Forge's Keycloak here runs a minimal realm, just enough to exist and be reachable. Account/
  organization sign-up isn't wired up yet.
- There's no PgBouncer, and none is needed for local development — it's a scaling component for
  much larger deployments.

For the reasoning behind these choices, see `docs/design.md` — DD-09 (devcontainer), DD-18
(migrations), DD-20 (identity provider), DD-21 (object storage), DD-23 (PostgreSQL version) and §12.2
(why there's no PgBouncer).
