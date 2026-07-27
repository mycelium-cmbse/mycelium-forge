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

RUN dotnet publish Mycelium.Forge/Mycelium.Forge.csproj -c Release -o /app/publish /p:UseAppHost=false


# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Mycelium.Forge.dll"]
