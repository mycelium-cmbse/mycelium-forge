---
name: mycelium-forge
description: Operational and development workflow guidelines for the Mycelium Forge project, including database execution modes (seed and migrate) and codegen regeneration.
---

# Mycelium Forge Project Guidelines

## Running Project Modes (Seed / Migrate)

When asked to run the project in seed mode or migrate mode, use the explicit project switch and CLI arguments:

### Seed Mode
```powershell
dotnet run --project Mycelium.Forge/Mycelium.Forge.csproj -- seed
```
> [!NOTE]
> Ensure the PostgreSQL container is running and healthy prior to executing seeding:
> ```powershell
> docker compose up -d postgres
> ```

### Migrate Mode
```powershell
dotnet run --project Mycelium.Forge/Mycelium.Forge.csproj -- migrate
```

## Code Generation (Regeneration)

To regenerate code artifacts from the XMI model:

1. Run the regeneration tests (or for a specific target, e.g., DTOs):
   ```powershell
   dotnet test Mycelium.Forge.Generator.Tests/Mycelium.Forge.Generator.Tests.csproj --filter "FullyQualifiedName~RegenerationTests"
   ```
   *Example for DTOs only:*
   ```powershell
   dotnet test Mycelium.Forge.Generator.Tests/Mycelium.Forge.Generator.Tests.csproj --filter "FullyQualifiedName~AutoGenDtoRegenerationTests"
   ```

2. The output is placed in:
   `Mycelium.Forge.Generator.Tests/bin/Debug/net10.0/_Forge.*/`

3. Copy the verified output files over the committed project folders (e.g., `_Forge.Common.AutoGenDto/*` to `Mycelium.Forge.Common/AutoGenDto/`).
