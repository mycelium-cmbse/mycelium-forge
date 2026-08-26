# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project files first so restore is cached independently of source changes.
# Directory.Build.targets carries the Tailwind build (DD-08) and is imported by every project, so it
# has to be present for the restore as well as for the publish; without it wwwroot/css/app.css is
# never generated and the image serves the interface unstyled.
COPY Mycelium.Forge/Mycelium.Forge.csproj ./Mycelium.Forge/
COPY Mycelium.Forge.Common/Mycelium.Forge.Common.csproj ./Mycelium.Forge.Common/
COPY Directory.Build.targets Nuget.Config ./

RUN dotnet restore Mycelium.Forge/Mycelium.Forge.csproj

# Copy the rest of the source
COPY Mycelium.Forge/ ./Mycelium.Forge/
COPY Mycelium.Forge.Common/ ./Mycelium.Forge.Common/

# GH113: on a fresh checkout, wwwroot/css/app.css doesn't exist yet, so a single `dotnet publish`
# computes its static web assets manifest before BuildTailwind (Directory.Build.targets) has written
# the file - the image then serves the app unstyled. `dotnet build` first gets the file onto disk, so
# the separate `dotnet publish` afterwards is a fresh MSBuild evaluation that picks it up.
RUN dotnet build Mycelium.Forge/Mycelium.Forge.csproj -c Release --no-restore

RUN dotnet publish Mycelium.Forge/Mycelium.Forge.csproj -c Release -o /app/publish --no-restore --no-build /p:UseAppHost=false


# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

# Npgsql (F-07, DD-18) probes for libgssapi_krb5 at startup to support GSSAPI authentication, which
# this image otherwise lacks - harmless (password auth still works) but logs a scary-looking warning
# on every start without it.
RUN apt-get update && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Mycelium.Forge.dll"]
