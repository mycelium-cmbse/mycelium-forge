# Getting started

How to go from a clean clone of `mycelium-forge` to a running, debuggable instance of Forge on your
own machine. This walks through **JetBrains Rider** as the reference IDE, with a plain `dotnet run`
path alongside it for anyone not using Rider's Dev Containers support. Local services are orchestrated by the
.NET Aspire AppHost (`Mycelium.Forge.AppHost`).

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
the container. You do need it if you choose [Option B](#option-b-run-the-apphost-on-your-machine)
instead.

## Option A: Dev Container in Rider (recommended)

This gets you a full .NET SDK plus a Docker daemon of its own, so the Aspire AppHost can start PostgreSQL,
Garage (object storage), Keycloak (identity provider), the migrator and seeder, and Forge itself. The
repository lives in a container volume (see [A2](#a2-from-the-welcome-screen-no-clone-needed)).

### A1. From an already-cloned repo (not recommended)

> **Heads up:** this bind-mounts your local folder. On Windows/macOS the files are not owned by the container's
> non-root `forge` user, so builds fail (`MSB3374 ... cannot be set`). Prefer A2 below, which clones into a
> container volume.

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
   installing the Docker-in-Docker feature, and
   **downloading the Rider backend into the container**. That backend download only happens once and
   can take a while (several hundred MB+) — subsequent connects reuse it and are fast.
6. Once it's ready, click **Connect**. Rider reopens the project from inside the container.

### A2. From the Welcome screen (no clone needed)

This is the supported route: the repo is cloned into a container volume, so the non-root `forge` user owns
every file.

1. On Rider's Welcome screen, click **Remote Development**, then **Create Dev Container**.
2. Rider auto-detects your local Docker connection. If it doesn't, make sure Docker Desktop is
   running.
3. Choose **Rider** as the backend IDE.
4. Under project source, choose **Git Repository** and give it this repo's URL. Let it auto-detect
   `.devcontainer/devcontainer.json`.
5. Click **Build Container and Continue**.
6. Once built, the project opens inside the container automatically.

### Once you're connected

You're now working inside the `workspace` container, with the repository in a container volume at
`/workspaces/mycelium-forge` and the .NET SDK already there. Start the Aspire AppHost from the terminal
(`dotnet run --project Mycelium.Forge.AppHost`): Postgres, Garage, Keycloak, the migrator, the seeder and Forge
run as containers/processes inside this container.

- **Run/debug the app**: open `Mycelium.Forge.sln`, make sure `Mycelium.Forge.AppHost` is the startup project,
  and Run or Debug as usual (▶ / 🐞 in the toolbar, or <kbd>Shift+F10</kbd> / <kbd>Shift+F9</kbd>). The
  database, storage and authentication settings are injected by the AppHost, so this just works -
  no local `appsettings.json` to create.
- **Run tests**: use Rider's built-in test runner (right-click a test project or class → **Run
  Unit Tests**), or `dotnet test` from the integrated terminal.
- The Aspire dashboard port is random: use the URL (with its login token) printed by the AppHost; the IDE
  forwards it automatically, as it does Forge (5000) and Keycloak (8081).
- **Getting a shell**: Rider's integrated terminal already opens inside the container — no extra step
  needed. From a plain host terminal instead (Rider not open, or not connected right now), with the
  container already running:
  ```
  docker exec -it -u forge <container-name> bash
  ```

### Claude Code inside the Dev Container

The Dev Container includes the Claude Code CLI and runs as a non-root user (`forge`), so
`claude --dangerously-skip-permissions` is available from the integrated terminal if you want it — the CLI
refuses that flag when launched as root, which is why the container doesn't run as root in the first place.
Login state persists across rebuilds (it lives in a named Docker volume), so you only need to `claude login`
once. Skipping permission prompts is still a real trust decision, not just a root-vs-non-root technicality —
only use it in a container you're comfortable giving broad, prompt-free access to.

## Option B: run the AppHost on your machine

If you'd rather work outside a Dev Container - plain Rider on your host, .NET SDK installed locally -
the Aspire AppHost still gives you Postgres/Garage/Keycloak, the migrator and seeder, and Forge.

1. Open `Mycelium.Forge.sln` in Rider normally (no Dev Container) and run or debug the
   `Mycelium.Forge.AppHost` project, or from the repo root:
   ```
   dotnet run --project Mycelium.Forge.AppHost
   ```
2. Open the Aspire dashboard URL printed in the console (it carries a login token). Wait for the migrator and
   seeder to finish and for `forge` to be running.
3. Browse to **http://localhost:5000**.

Forge runs as a normal host process started by the AppHost, so debugging the AppHost debugs Forge. Always start
Forge through the AppHost: the database, storage and authentication settings are injected by it as environment
variables.

## Just run it

No editor, no interactive development — just see a working Forge instance:

```
dotnet run --project Mycelium.Forge.AppHost
```

This starts the Aspire AppHost, which brings up Postgres, Garage, Keycloak, the one-shot migrator,
the database seeder, and the app itself, in dependency order (the app doesn't start until the seeder finishes
successfully). Once it settles, browse to **http://localhost:5000**.

Run it a second time and it's a clean no-op — the migrator sees nothing new to apply and the app
starts immediately.

## Resetting local state

```
docker volume rm forge-postgres-data forge-keycloak-postgres-data forge-garage-meta forge-garage-data
```

This removes Postgres data, Garage's buckets/keys and Keycloak's realm state. Stop the AppHost first. The next
AppHost start begins from scratch. Use this if something's in a state you
don't trust, or after pulling changes that touch the database schema.

## Troubleshooting

- **"Cannot connect to the Docker daemon"** — Docker Desktop isn't running. Start it and wait for it to
  fully initialize before retrying.
- **Port already in use** (5000, 5432, 5433, 8081, 3900, 3903) - something else on your machine is already
  using it. Stop that; these ports are fixed in `Mycelium.Forge.AppHost/Program.cs` and the Forge launch profile.
- **A resource won't go "healthy"** - open it in the Aspire dashboard and read its console logs.
- **Stale/weird state after switching branches** - reset the local state as above, then start the AppHost again.

## What's here, and what isn't yet

- Every credential in `Mycelium.Forge.AppHost/Program.cs` / `docker/garage.toml` is a fixed, checked-in, dev-only
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
