# Mycelium Forge

Mycelium Forge is the package registry of the [Mycelium](https://github.com/mycelium-cmbse) ecosystem for
Model-Based Systems Engineering (MBSE) artefacts. Forge provides a public web interface, an HTTP API, and first-party client libraries for publishing, discovering and consuming SysML v2, KerML, ECSS-E-TM-10-25 and Capella artefacts.

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mycelium-cmbse_mycelium-forge&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=mycelium-cmbse_mycelium-forge)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=mycelium-cmbse_mycelium-forge&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=mycelium-cmbse_mycelium-forge)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=mycelium-cmbse_mycelium-forge&metric=coverage)](https://sonarcloud.io/summary/new_code?id=mycelium-cmbse_mycelium-forge)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=mycelium-cmbse_mycelium-forge&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=mycelium-cmbse_mycelium-forge)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=mycelium-cmbse_mycelium-forge&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=mycelium-cmbse_mycelium-forge)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=mycelium-cmbse_mycelium-forge&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=mycelium-cmbse_mycelium-forge)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=mycelium-cmbse_mycelium-forge&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=mycelium-cmbse_mycelium-forge)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=mycelium-cmbse_mycelium-forge&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=mycelium-cmbse_mycelium-forge)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=mycelium-cmbse_mycelium-forge&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=mycelium-cmbse_mycelium-forge)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=mycelium-cmbse_mycelium-forge&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=mycelium-cmbse_mycelium-forge)

## Getting started

To go from a clean clone to a running, debuggable instance of Forge on your own machine, including the Dev Container and plain `docker compose` paths — see [`docs/getting-started.md`](docs/getting-started.md).

The short version:

```
docker compose up
```
brings up Postgres, Garage (object storage), Keycloak, the one-shot migrator and the app itself. Once it settles, browse to **http://localhost:8080**.

## Installation

The packages are available on NuGet at:

project                                                                                                     | Nuget
------------------------------------------------------------------------------------------------------------ | ------------
[Mycelium.Forge.Common](https://www.nuget.org/packages/Mycelium.Forge.Common)                                 | ![NuGet Version](https://img.shields.io/nuget/v/Mycelium.Forge.Common)
[Mycelium.Forge.Client](https://www.nuget.org/packages/Mycelium.Forge.Client)                                 | ![NuGet Version](https://img.shields.io/nuget/v/Mycelium.Forge.Client)
[Mycelium.Forge.Serializer.Json](https://www.nuget.org/packages/Mycelium.Forge.Serializer.Json)               | ![NuGet Version](https://img.shields.io/nuget/v/Mycelium.Forge.Serializer.Json)

The container image is published to [GitHub Container Registry](https://github.com/mycelium-cmbse/mycelium-forge/pkgs/container/mycelium-forge)
on every release, as `ghcr.io/mycelium-cmbse/mycelium-forge`.

## Solution layout

project                                | Purpose
--------------------------------------- | -------
`Mycelium.Forge`                        | Public web interface and Forge HTTP API host.
`Mycelium.Forge.Common`                 | Shared DTOs consumed by the Forge host and the Forge client library.
`Mycelium.Forge.Client`                 | First-party client library for the Forge HTTP API, consumable by Mycelium Bloom, CI/CD pipelines and third-party tooling.
`Mycelium.Forge.Serializer.Json`        | Forge JSON serializer.
`Mycelium.Forge.Generator`              | uml4net/Handlebars code-generation pipeline: DTOs and enums generated from the Enterprise Architect model.

Each of the above has a matching `*.Tests` project; `Mycelium.Forge.EndToEndTests` drives a running host over the network.


## Build Status

GitHub Actions are used to build, test and analyze the solution.

Branch | Build Status
------- | :------------
Main | ![Build Status](https://github.com/mycelium-cmbse/mycelium-forge/actions/workflows/CodeQuality.yml/badge.svg?branch=main)
Development | ![Build Status](https://github.com/mycelium-cmbse/mycelium-forge/actions/workflows/CodeQuality.yml/badge.svg?branch=development)

# Software Bill of Materials (SBOM) and Provenance

As part of our commitment to security, transparency, and traceability, the container image is built with `--sbom=true --provenance=true`. On every release, the SBOM (SPDX) and provenance attestations travel with the image and the SPDX document is additionally attached to the GitHub release as a standalone file, for air-gapped delivery. This provides:

- A comprehensive list of all open-source and third-party components in the image, tracking dependencies, licenses and versions, to support vulnerability management.
- A record of the image's origin and build process, so its integrity and authenticity can be verified.

# License

The Mycelium Forge libraries and reference web application are provided to the community under the [Apache License 2.0](LICENSE).

# Contributions

Contributions to the code base are welcome — see [`.github/CONTRIBUTING.md`](.github/CONTRIBUTING.md) for the workflow.
