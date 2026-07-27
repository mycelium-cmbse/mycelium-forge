# Mycelium Forge — implementation plan

**Status:** draft for review, 2026-07-27
**Companion to:** [`design.md`](design.md)

---

## 1. What this document is, and what it is not

`design.md` answers *why Forge is built this way*. It is durable: every decision in it stays true
after the work ships, and someone joining in year two reads it to understand the system.

This document answers *what we do, in what order, and who can work in parallel*. It is **perishable**.
The moment the GitHub issues exist, GitHub owns status, and this file becomes a snapshot of how the
work was decomposed rather than a live tracker.

That difference is why the plan is a separate file. A plan section inside `design.md` would start
rotting the day the first issue is closed — the same failure the decision log had before it was
removed. Keeping them apart means neither document has to lie.

The boundary between the two:

| | Lives in `design.md` | Lives here |
|---|---|---|
| Phases, seams, what parallelises | **§19** — these derive from architecture and remain true afterwards | Referenced, not restated |
| Why a decision was taken | **§6, DD-01…DD-17** | Referenced by number |
| Decomposition into assignable units | — | §4–§8 |
| Acceptance criteria and dependencies | — | §4–§8 |
| Sequencing risk and critical path | — | §9 |

**§19 stays where it is.** The three seams exist for architectural reasons, not scheduling ones, and
they are still worth understanding once phase 4 has shipped.

---

## 2. Decisions still required

Three things are not yet decided, and two of them block work. Each becomes a design decision in
`design.md` once settled — this list is where they are visible until then.

**D-1, data access and migration tooling, is settled** and recorded as **DD-18**: DAOs generated
from the Enterprise Architect model over raw Npgsql, with numbered forward-only SQL migrations
applied by DbUp.

**D-4, the development OIDC provider, is settled** and recorded as **DD-20** — dissolved rather than
answered. Forge ships its own Keycloak, so the development provider *is* the production one and there
is no longer a stand-in to choose. Neither is listed below.

| | Decision | Blocks | Recommendation |
|---|---|---|---|
| **D-2** | **Object storage client, and whether S3 is mandatory on-premise** | Epic A, the `IArtifactStore` seam | See below |
| **D-3** | **The Enterprise Architect model.** DD-07 generates all DTOs from it, DD-18 now generates the DAOs and the schema from it too, and DD-20 adds `Account`, `Organization` and `Membership` to what it must cover. Does it exist, and who authors it? | `Mycelium.Forge.Common` and the persistence layer, therefore every project | Treated as F-03 below; **this is the most urgent item in the document** and needs an owner named |
| **D-5** | **Requirements coverage for §3.3 items.** The CLI, mirroring, verified publishers, the docs site, `/api/v1/elements` and the two popularity metrics all lack SSS requirements. The second metric is now specified in DD-19 as a **dependents** count derived from the dependency graph, not the "imports" event it was previously described as, so the requirement to be written differs materially from what §3.3 originally implied | Nothing technically; it is a traceability gap that grows the longer it is open | One tracking issue against the requirements repository |

**On D-2.** `AWSSDK.S3` against MinIO locally is the conventional answer and needs little discussion.
The question worth deciding deliberately is whether an on-premise or air-gapped customer (§5.1) must
run object storage at all, or whether `IArtifactStore` also gets a filesystem implementation. The seam
already exists, so this is a matter of scope rather than architecture — but it should be answered
before the seam is implemented, not after.

---

## 3. How this maps onto GitHub

- **Milestone per phase** — `Phase 0 — Foundations`, `Phase 1 — Registry core`, and so on.
- **Label per epic** — `epic:persistence`, `epic:publish`, `epic:web`, … so an epic's issues are one
  filter away without needing a tracking issue per epic.
- **Label per kind** — `decision`, `seam`, `spike`, `chore`.
- **`seam` is worth its own label.** §19.1 warns the write-authority check is the one most likely to be
  skipped because in phase 1 it always returns `true`. A reviewer needs to be able to find all three.
- **Every issue traces.** Title, one-paragraph context, acceptance criteria as checkboxes, and the
  `design.md` section or `SSS-…` requirement it satisfies. An issue that traces to neither is either
  scope creep or a missing requirement, and both are worth catching at issue-creation time.

Sizes below are **S** (≤1 day), **M** (2–4 days), **L** (a week or more — a candidate for splitting
once someone picks it up).

### Issues as created

**78 issues, #3 to #80**, each titled with its plan reference so the two can be read against each
other. Seven were raised later, all from decisions that did not exist when that pass was made:

| From | Issues |
|---|---|
| DD-18 | **A-07** is #85, **F-10** is #88 |
| DD-19 | **D-07** is #86 |
| DD-20 | **F1-05** is #89, **F1-06** is #90, **F1-07** is #91, **F1-08** is #92 |

Two pre-existing issues were left alone:

- **#2 CI/CD pipeline** already covers F-08, so no duplicate was created. It predates §15.1 and should
  gain the `--sbom=true --provenance=true` requirement and the standalone SBOM release file.
- **#1 Solution scaffolding** specified `Mycelium.Forge.Server` and `Mycelium.Forge.Server.Tests`.
  That issue was stale: **`Mycelium.Forge` and `Mycelium.Forge.Tests` are correct**, and §16 remains
  the authoritative solution structure. #1 has been corrected in place. Its file-header requirement is
  satisfied for source files — all seven `.cs` files carry the Starion Apache-2.0 header — but the
  **Rider Team-scope File Header Template is still outstanding**, so the header is a convention rather
  than something the IDE applies to new files. No `Mycelium.Forge.sln.DotSettings` is committed.

---

## 4. Phase 0 — Foundations

Blocks everything else. Small, and worth doing properly.

| Id | Issue | Size | Depends on |
|---|---|---|---|
| F-01 | **Decide the data-access and migration stack** (D-1); record as a DD — settled as DD-18 | S | — |
| F-02 | **Decide the object-storage client and the on-premise storage question** (D-2); record as a DD | S | — |
| F-03 | **Author the Forge domain model in Enterprise Architect and export XMI.** §8's class diagram is the specification, **including `Account`, `Organization` and `Membership`** — DD-20 makes those Forge's own records rather than an external directory's | L | — |
| F-04 | **uml4net DTO generation**: templates, MSBuild target, output into `Common/Generated/` (DD-07) | M | F-03 |
| F-05 | **uml4net JSON serialiser generation** (DD-05), including DD-13's abbreviated projection | M | F-04 |
| F-06 | **Contract-test harness** for generated serialisers (§17) — a template defect is systematic, so this is the test that matters most | S | F-05 |
| F-07 | **Local environment**: compose with PostgreSQL, MinIO and the Forge Keycloak — which since DD-20 is the production component rather than a stand-in — plus a one-shot migrator service, since migrations are an explicit invocation and without it the local database never gets a schema (DD-18); devcontainer wiring (DD-09) | M | F-01, F-02 |
| F-08 | **CI pipeline**: build, test, `docker buildx --sbom=true --provenance=true`, SBOM published as a release file (§15.1). *Already tracked as #2 — no separate issue was created; the SBOM and provenance requirements should be added to it* | M | — |
| F-09 | **Make the end-to-end suite self-hosting.** It currently requires a host already listening on `:5000` and fails with connection-refused otherwise, so it cannot run in CI as it stands | S | — |
| F-10 | **uml4net DAO and schema generation** (DD-18): DAO templates emitting raw Npgsql over the §8 entities, a DDL template emitting the schema that becomes migration `0001`, and golden-file coverage of both | L | F-04 |

F-03 is the critical path and the one with schedule risk — it is upstream of every project in the
solution and it is not a coding task. If it slips, F-04, F-05 and F-10 slip with it and phase 1 cannot
start cleanly. Worth naming an owner before anything else on this list.

**F-10 was missed when DD-18 was written.** That decision generates the data-access layer and the
schema from the model, but the plan had generation issues only for DTOs (F-04) and serialisers
(F-05), while A-01 spoke of "the generated baseline" as though a generator already existed. Nothing
produced it. The critical path is therefore **F-03 → F-04 → F-10 → A-01 → everything**, with F-05 and
F-06 running alongside F-10 rather than ahead of it.

---

## 5. Phase 1 — Registry core

The bulk of the work, and the only phase on the critical path (§19.3).

### Epic A — Persistence and the three seams

| Id | Issue | Size | Depends on |
|---|---|---|---|
| A-01 | Schema and migrations for `Scope`, `Package`, `PackageVersion`, `Maintainer`, `ApiKey`, `AuditEntry` (§8): the generated baseline, the DbUp runner, the transaction-scoped advisory lock that stops concurrent migrators racing (DD-18), and an explicitly sized connection pool rather than Npgsql's default (§12.2) | M | F-01, F-10 |
| A-02 | **Seam:** `Scope.Origin`, always `Local` in this phase (§19.1) | S | A-01 |
| A-03 | **Seam:** `IArtifactStore` resolving by content hash over content-addressed blob storage (§19.1, §12) | M | F-02, A-01 |
| A-04 | **Seam:** write-authority check on every publish, unlist, maintainer and ownership path; always `true` in this phase (§19.1) | S | A-01 |
| A-05 | §8.1 invariants enforced in the **domain layer**, not at the API boundary: immutability of `{package, version}`, strictly increasing SemVer, release notes required on a major change, at least one individual-Account Owner | M | A-01 |
| A-06 | Append-only tamper-evident audit entries on every privileged operation (`SSS-FG-AUTH-R9J`) | M | A-01 |
| A-07 | **Schema drift check in CI**: build one database by running every migration in order and another from the generated schema, then fail the build if the migrations did not produce every object the model implies (DD-18) | M | A-01 |

A-07 is not optional polish, for the same reason E-04 is not. DD-18 keeps the Enterprise Architect
model authoritative over the schema by generating the baseline and then checking that the
hand-written deltas still add up to what the model implies. Without that check the two drift
silently, the generated schema becomes decoration, and schema correctness falls back onto review —
which is precisely the objection DD-18 exists to answer.

It is a separate issue rather than a clause on A-01 because a verification harness attached to a
feature issue is the first thing dropped when the feature runs long.

Two details for whoever picks it up. The comparison is **one-directional**: every object the
generated schema declares must exist in the migrated database, but the reverse does not hold, because
the job table (DD-17), the counter events and their watermark (DD-15), and the search projection
(E-01) are hand-written and have no model counterpart. That asymmetry is what lets the check work
without an exclusion list to maintain. And the diff must be normalised before comparison — `pg_dump
--schema-only` does not guarantee a stable ordering of constraints and indexes between two databases
built by different routes, so an unsorted diff will report drift that is not there.

### Epic B — Publish

| Id | Issue | Size | Depends on |
|---|---|---|---|
| B-01 | `IArtifactManifestExtractor` and Autofac registration; an unregistered kind is rejected at the API boundary rather than stored unvalidated (§8.3) | S | A-01 |
| B-02 | kpar extractor over `SysML2.NET.Kpar`, implementing the ten checks in §9.1 | L | B-01 |
| B-03 | Scope declared at publish and authorised against the credential; name equality against the manifest; mismatch **rejected**, not warned (§8.2) | M | B-01, F-03 |
| B-04 | Atomic publish across two stores: blob first under a content-addressed key, then the metadata transaction; concurrent publishes serialised by a unique constraint (§12, `SSS-FG-REG-A5E`, `-I3C`) | M | A-03, A-05 |
| B-05 | `PUT /api/v1/packages` | M | B-02, B-04 |
| B-06 | Publish page — static SSR multipart form, validation failures re-rendered on the form (§7.4) | M | B-05, G-01 |

### Epic C — Download and unlist

| Id | Issue | Size | Depends on |
|---|---|---|---|
| C-01 | `GET …/artifact` (latest listed, non-prerelease) and `…/{version}/artifact` (`SSS-FG-REG-D6F`) | M | A-03 |
| C-02 | Unlist: hidden from search and resolution, still served on direct download (`SSS-FG-REG-U4D`) | M | A-04, A-05 |
| C-03 | Append-only download event recording — never a synchronous counter increment (DD-15). Downloads only; the dependents count is D-07 and is not an event | S | A-01 |

### Epic D — Metadata and the API surface

| Id | Issue | Size | Depends on |
|---|---|---|---|
| D-01 | Metadata projection from manifest to registry model, frozen at publish (§8.1) | M | B-02 |
| D-02 | `GET /api/v1/packages/{scope}/{name}` — manifest, versions, dependency graph, release notes (`SSS-FG-REG-M8H`) | M | D-01 |
| D-03 | DD-13 abbreviated representation, `MetadataSource` discriminator, and the absent-versus-empty dependency field so a resolver can tell "no dependencies" from "not expressible" (§9.2.1) | M | D-02, F-05 |
| D-04 | Content negotiation: media types, `Vary: Accept`, `406` with an RFC 9457 body listing supported types, no silent downgrade (§10.3, DD-12) | M | D-02 |
| D-05 | RFC 9457 problem details carrying the correlation identifier across the whole API (§10.1) | S | — |
| D-06 | Maintainer endpoints, with the §8.1 owner invariants and explicit-acceptance transfer (`SSS-FG-AUTH-M3C`, `-T5E`) | M | A-05, A-06 |
| D-07 | Dependents count derived from the `usage[]` graph and maintained in the publish and unlist transactions — distinct packages, latest listed version, direct dependencies only, and **decrementing** when a new version drops a dependency (DD-19) | M | D-01, B-04, C-02 |

### Epic E — Search and resolution

| Id | Issue | Size | Depends on |
|---|---|---|---|
| E-01 | Search projection in PostgreSQL behind an interface, so §12.1's contingency stays open (DD-14) | M | D-01 |
| E-02 | `GET /api/v1/packages` — free text over metadata, facets, sort, pagination (§3.4, `SSS-FG-REG-Q7G`) | L | E-01 |
| E-03 | `GET /api/v1/elements` — qualified-name resolution, exact and prefix, **unranked**, returning all matches without choosing one (§3.4, §8.2) | M | E-01 |
| E-04 | Latency benchmark against the p95 500 ms budget at the target corpus, with facets enabled (§12.1) | M | E-02 |

E-04 is not optional polish. §12.1 sets 500 ms as the trigger for leaving PostgreSQL; without a
harness that measures it, the trigger cannot fire and the decision becomes unfalsifiable.

### Epic F — Authentication and authorisation

| Id | Issue | Size | Depends on |
|---|---|---|---|
| F1-01 | OIDC interactive login against **Forge's own** identity provider, with federation to an external one as optional configuration (DD-20) | M | F-07 |
| F1-02 | API keys: issuance, revocation, hashed storage, one-time reveal in the `POST` response with an idempotency token (§7.4, `SSS-FG-REG-Y2L`) | M | A-01, A-06 |
| F1-03 | Owner and Maintainer authority model wired into every privileged path (`SSS-FG-AUTH-M3C`) | M | A-05 |
| F1-04 | Anonymous read access to public packages (`SSS-FG-REG-W9J`, `-Y2L`) | S | — |
| F1-05 | **Account provisioning on first login**, and the seeded-administrator bootstrap from configuration (DD-20) | M | F1-01, A-01 |
| F1-06 | **Organization creation and slug allocation**, with §5.1.2's rejection of a slug already in the proxied set (DD-16, DD-20) | M | F1-05, A-02 |
| F1-07 | **Membership and organization roles** — the relation `SSS-FG-AUTH-G6F` needs to authorise publishing on behalf of an Organization (RD-01, DD-20) | M | F1-06 |
| F1-08 | **Invitations and deprovisioning**, with audit entries on both (`SSS-FG-AUTH-R9J`, DD-20) | M | F1-07, A-06 |

**F1-05 to F1-08 are new, and they are not a small addition.** Until DD-20 the design inherited
Accounts, Organizations and membership from Fabric's directory, and §13's sentence "no Forge-specific
registration exists" was carrying all of it. Standalone deployment makes that surface Forge's own.

They are phase 1 rather than deferrable: publish is authorised against scope (§8.2, B-03) and §8.1's
"at least one individual-Account Owner" invariant is enforced in the domain layer (A-05). Neither can
be built against an account model that does not exist.

### Epic G — Web interface

All static SSR (DD-01, DD-02). No component runtime anywhere in this epic.

| Id | Issue | Size | Depends on |
|---|---|---|---|
| G-01 | Layout, header, navigation, Tailwind and BlazorBlueprint wiring (DD-08) | M | — |
| G-02 | Discover | M | E-02, G-01 |
| G-03 | Search results, facets, empty state — results as addressable URLs | M | E-02, G-01 |
| G-04 | Package detail and its five tabs, each an addressable route | L | D-02, G-01 |
| G-05 | Publisher page | M | G-01 |
| G-06 | My packages, and its empty state | M | F1-01, G-01 |
| G-07 | Package settings, with destructive actions as their own confirmation pages requiring the package name (§7.4) | M | C-02, F1-03 |
| G-08 | API keys, including the one-time secret page | M | F1-02 |
| G-09 | Docs site — Home, Concept, Howto, CLI, HTTP API (§3.3) | L | G-01 |
| G-10 | Header search: plain `GET` form plus the `Ctrl K` binding; **no live dropdown** (§7.3) | S | E-02, G-01 |
| G-11 | Both popularity metrics shown distinctly enough that they are not read as one number, and labelled **downloads** and **dependents** rather than "imports" (§3.3, DD-19) | S | H-02, D-07 |

### Epic H — Background jobs (DD-17)

| Id | Issue | Size | Depends on |
|---|---|---|---|
| H-01 | Job table, `FOR UPDATE SKIP LOCKED` claim, lease renewal, expiry reclaim, progress on the row | L | A-01 |
| H-02 | Download-count aggregation advancing a watermark in the same transaction as the aggregate (DD-15, DD-17). Downloads only — DD-19's dependents count needs no job | M | H-01, C-03 |
| H-03 | Orphaned blob collection (§12) | M | H-01, A-03 |
| H-04 | `Forge__Roles` role switch, role-aware startup and probes (DD-03), including the `/ready` schema-version gate and the advisory-locked migrator invocation (DD-18) | M | H-01 |

### Epic I — Observability

| Id | Issue | Size | Depends on |
|---|---|---|---|
| I-01 | Serilog structured JSON with trace, span and correlation identifiers (`SSS-FB-OBS-S1A`) | S | — |
| I-02 | OpenTelemetry traces over OTLP (`SSS-FB-OBS-D2B`) | S | — |
| I-03 | Prometheus `/metrics`, including job duration, outcome and queue lag (`SSS-FB-OBS-M3C`, DD-17) | M | H-01 |
| I-04 | Credential and PII scrubbing, bounded retention (`SSS-FB-OBS-R8H`) | M | I-01 |

---

## 6. Phase 2 — Client surfaces

Depends only on phase 1's `/api/v1`.

| Id | Issue | Size |
|---|---|---|
| P2-01 | `Mycelium.Forge.Client` over the seven `SSS-FG-REG-C3M` operations: base-URL configuration, `IHttpClientFactory`, `FluentResults` rather than exceptions | L |
| P2-02 | Client tests against a running host, over the real transport (§17) | M |
| P2-03 | CLI: `System.CommandLine` shell over the client — `search`, `info`, `versions`, `download`, `publish`, `unlist`, `key` (§11.2) | L |
| P2-04 | `forge login` and local credential storage — the one command with no library counterpart | M |
| P2-05 | NativeAOT self-contained binaries per platform, not a `dotnet tool` (§11.2) | M |
| P2-06 | CLI SBOM generated from the restore graph (§15.1) | S |

---

## 7. Phase 3 — Multi-format

Extends phase 1's extractor interface. Independent of phases 2 and 4.

| Id | Issue | Size |
|---|---|---|
| P3-01 | Publisher-supplied metadata path, frozen at publish exactly as manifest-sourced metadata is (§9.2.1) | M |
| P3-02 | Capella extractor over `Auriga`, pre-filled from `ModelVersion` where available; `MetadataSource` stays publisher-supplied (§9.2.1) | L |
| P3-03 | ECSS-E-TM-10-25 Annex C.3 extractor over `CDP4JsonFileDal-CE`; `Header.json` is provenance, not package identity | L |
| P3-04 | Publish flow for publisher-supplied metadata, with pre-filling where the reader offers a value | M |
| P3-05 | Record COMET-SDK's LGPL-3.0 as a known SBOM entry rather than a surprise (§9.2) | S |

`Auriga` is only partially published — the object model is at 1.0.0 and the reader layers are in
progress. P3-02 depends on work outside this repository and should not be scheduled as though it does
not.

---

## 8. Phase 4 — Mirroring

Additive if and only if the three seams exist. Independent of phases 2 and 3.

| Id | Issue | Size | Depends on |
|---|---|---|---|
| P4-01 | Scope routing configuration, with DD-16's configuration-time rejection of overlapping or package-level rules | M | A-02 |
| P4-02 | Proxied scope origin and read-only enforcement on every write path (§5.1.3) | M | A-02, A-04 |
| P4-03 | Fetch-on-miss from upstream, then permanent artefact cache — artefacts are immutable so they never need invalidation (§5.1.4) | L | A-03 |
| P4-04 | Metadata TTL for proxied version lists — the one place immutability does not help (§5.1.4) | M | P4-03 |
| P4-05 | Metadata index replication: snapshot, incremental deltas, resumable position marker (§5.1.6) | L | E-01 |
| P4-06 | Availability-aware search — every result carries cached-now versus available-on-demand, and is filterable on it (§5.1.6) | M | P4-05 |
| P4-07 | Bulk pre-warm as a claimed job with progress on the row (§5.1.5, DD-17) | L | H-01, P4-03 |
| P4-08 | Air-gapped bundle export | L | P4-07 |
| P4-09 | Air-gapped bundle import, with index staleness and last-import date surfaced to the operator | M | P4-08 |
| P4-10 | Read-only upstream credential per upstream (§5.1.7) | S | P4-01 |

---

## 9. Critical path and risk

**The critical path is F-03 → F-04 → F-10 → Epic A → everything.** The Enterprise Architect model is
upstream of the generated DTOs, which are upstream of every project in the solution — and since
DD-18 it is upstream of the data-access layer and the database schema as well, which is what puts
F-10 on the path rather than F-05. It is also the only item on that path that is not a coding task,
which makes it the likeliest to slip quietly.

**Two decisions have enlarged F-03 since it was written**, and both landed on the one item least able
to absorb them. DD-18 made the model the source of the schema, so an error in it is now a migration
rather than an edit. DD-20 added `Account`, `Organization` and `Membership` to what it must cover.
F-03 is sized L rather than M as a result, and naming its owner is the single most urgent action in
this document.

Three risks worth stating explicitly:

**The seams are cheap now and expensive later.** §19.1 already says this; the scheduling consequence is
that A-02, A-03 and A-04 must not be deferred out of phase 1 for being small and apparently pointless.
A-04 in particular is a function returning `true` — trivial to write, and expensive to retrofit across
every write path once there are several.

**`Auriga` is not fully published.** P3-02 depends on another team's release schedule. Phase 3 should
be sequenced with that visibility rather than assuming availability.

**Traceability gaps compound.** Six capabilities in §3.3 have no SSS requirement. Each is confirmed in
scope, so the risk is not that they are built wrongly but that the requirements baseline drifts from
the product while the work is in flight — and reconstructing intent afterwards is far more expensive
than recording it now.

### What parallelises

Once phase 0 and Epic A are complete, the phase 1 epics fan out with limited contention:

- **B, C, D** share the domain and persistence layer, so they are best held by one team.
- **E** depends only on the metadata projection.
- **G** depends on the endpoints but not on each other; G-01 unblocks all the rest.
- **H** and **I** touch almost nothing else and can run alongside from the start.

After phase 1, §19.3 applies: phases 2, 3 and 4 are mutually independent and three teams can run them
concurrently without contending on the same code.
