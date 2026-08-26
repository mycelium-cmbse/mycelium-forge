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

# Run BuildTailwind standalone so wwwroot/css/app.css exists on disk before any MSBuild invocation
# that discovers static web assets - discovering the file and generating it in the same `dotnet
# build`/`publish` can race, silently shipping the image without it.
RUN dotnet msbuild Mycelium.Forge/Mycelium.Forge.csproj -t:BuildTailwind -p:Configuration=Release

RUN dotnet publish Mycelium.Forge/Mycelium.Forge.csproj -c Release -o /app/publish /p:UseAppHost=false

RUN test -s /app/publish/wwwroot/css/app.css.gz \
    && grep -q "css/app.css" /app/publish/Mycelium.Forge.staticwebassets.endpoints.json \
    || (echo "app.css is missing from the published static web assets" && exit 1)

RUN test -s /app/publish/wwwroot/_framework/blazor.web.js \
    || (echo "blazor.web.js is missing from the publish output" && exit 1)


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
