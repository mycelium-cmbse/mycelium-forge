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
# can compute its static web assets manifest before BuildTailwind (Directory.Build.targets) has
# written the file - the image then serves the app unstyled. This reproduced consistently in CI even
# though it built successfully; running `dotnet build` first (rather than just the narrow
# BuildTailwind target) still let the SDK's own static-web-assets discovery run in that same
# invocation. Targeting BuildTailwind directly writes the file with no `Build`-target machinery
# involved at all, so the later `dotnet publish` is the first, and only, evaluation that ever needs
# to discover it.
RUN dotnet msbuild Mycelium.Forge/Mycelium.Forge.csproj -t:BuildTailwind -p:Configuration=Release

RUN dotnet publish Mycelium.Forge/Mycelium.Forge.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

# GH113: fail the build loudly rather than silently ship an unstyled image again if the static web
# assets manifest ever ends up without app.css.
RUN test -s /app/publish/wwwroot/css/app.css.gz \
    && grep -q "css/app.css" /app/publish/Mycelium.Forge.staticwebassets.endpoints.json \
    || (echo "app.css is missing from the published static web assets - see GH113" && exit 1)


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
