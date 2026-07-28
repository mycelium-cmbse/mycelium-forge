# Mycelium Forge Roles and Permissions

This document defines the role and permission model for **Mycelium Forge**, the MBSE artefact sharing platform. It is a working document: it is the authority on the intended model while it is being agreed, after which the settled parts move into `design.md` §13 and into SSS requirements.

Forge is a **registry**: a scope holds packages, and a package holds immutable versions (`design.md` §8).

| Concept | Definition | Identity |
|---|---|---|
| **Scope** | Resolves to an Account or Organization slug | `@starion` |
| **Package** | The container for the versions of one artefact | `@starion/ECSS-MM-THE` |
| **PackageVersion** | Immutable once published | `@starion/ECSS-MM-THE 1.4.2` |

There is no container between Scope and Package. `SSS-FG-AUTH-S2B` fixes the identifier at `@<scope>/<package-name>`, which leaves no third segment, and every `usage[]` IRI in an already-published kpar depends on that shape.

---

## Core principles

1. **Forge owns its identity registry.** Per DD-20 Forge ships its own OIDC provider (Keycloak) and does not depend on Fabric's authentication or authorization. An external identity provider may be federated as configuration, but Forge is deployable without one.
2. **Roles define capabilities; visibility defines reach.** A role determines which operations a principal may perform on a package. Whether a principal can *see* the package at all is governed by the package's visibility, not by a role assignment.
3. **Self-service by default.** Authenticated users create Organizations and publish packages without administrator intervention.
4. **Principals are not only people.** An API key is a first-class principal with its own authority, because CI/CD pipelines may publish (`SSS-FG-REG-Y2L`).
5. **Immutability is not a permission.** No role can edit a published version. `SSS-FG-REG-I3C` freezes `{package, version}`, and `SSS-FG-AUTH-M3C` freezes metadata at publish time. Correction means publishing a new version.

---

## Principals

| Principal | Description | Authentication |
|---|---|---|
| **Anonymous** | An unauthenticated visitor or crawler | None. `SSS-FG-REG-W9J` requires the web interface to be reachable unauthenticated, and DD-01 depends on public pages being crawlable and CDN-cacheable |
| **Account** | A registered person. Exists at installation level, independently of any Organization | OIDC against Forge's own provider (DD-20) |
| **API key** | A machine credential issued by an Account, acting with that Account's authority narrowed to an explicit set of operations | Bearer credential, hashed at rest, revealed once at issuance (`SSS-FG-REG-Y2L`) |

An API key never exceeds the authority of the Account that issued it. Where the two differ, the narrower applies. Revoking an Account's access revokes every key it issued.

---

## Scope hierarchy

```
Installation
  ├── Installation Administrator (super-admin; seeded from configuration)
  ├── Platform Operator (SaaS only; infrastructure, no package content)
  └── Account (any authenticated user)
       ├── owns a personal Scope           →  @alice/…
       ├── can create Organizations
       └── Organization (tenant boundary)
            ├── Organization Administrator
            ├── Organization Member
            └── owns an organization Scope →  @starion/…

Scope (@alice or @starion)
  └── Package  (private | organization-visible | public)
       ├── Owner
       ├── Maintainer
       ├── Reader        (meaningful only where visibility restricts)
       └── PackageVersion (immutable)
```

**An Account is a namespace in its own right.** `design.md` §8.2 resolves a scope to an Account *or* an Organization slug, and §8's model has `Account "1" --> "0..1" Scope`. An individual publishes to `@alice/…` without belonging to any Organization.

---

## Installation scope

### Installation Administrator

The Installation Administrator is a super-admin over the whole installation. It exists in both SaaS and on-premise deployments.

**Bootstrap is from configuration, not from whoever arrives first.** Per DD-20 and `F1-05`, the seeded administrator is supplied as deployment configuration.

| Capability | Description |
|---|---|
| View all organizations | Name, creation date, member count, package count, status |
| Manage organizations | Create, suspend, reactivate and delete organizations |
| View all accounts | Username, email, memberships, roles, status |
| Manage accounts | Deactivate and delete accounts; grant and revoke the Installation Administrator role |
| Assign organization memberships | Add and remove accounts to and from any organization with a specified role |
| Reserve and release scope slugs | Including refusing a slug that collides with a proxied upstream scope (§5.1.2, `F1-06`) |
| Configure mirroring | Scope routing to an upstream, upstream credentials, bulk pre-warm, air-gapped bundle import and export (§5.1, DD-16) |
| View installation metrics | Accounts, organizations, packages, storage usage, active sessions |
| View the audit log | The append-only, tamper-evident record of privileged operations (`SSS-FG-AUTH-R9J`) |

**The Installation Administrator does not gain read access to private packages by virtue of the role.** Administration is over accounts, organizations and the installation, not over package content. Where an operator genuinely needs content access — a legal hold, an incident — it is an explicit, audited grant rather than an ambient capability.

### Platform Operator — SaaS only

The Platform Operator is held by the team operating the SaaS infrastructure. It is not available to customers, and the SaaS deployment is not offered to organisations outside Starion.

| Capability | Description |
|---|---|
| Monitor platform health | Infrastructure metrics, logs and alerts across all tenants |
| Perform platform maintenance | Backups, upgrades, schema migrations (DD-18) |
| Configure platform defaults | Authentication policy, retention, compliance settings |
| Manage billing and quotas | Storage, account and package limits per organization |
| Suspend organizations | For policy violation or non-payment |

**Platform Operator and Installation Administrator are distinct roles, not two names for one.** The Platform Operator acts on infrastructure and never on package content or account records; the Installation Administrator acts on accounts, organizations and scopes and never on infrastructure.

### On-premise deployment

The Platform scope does not exist inside the application on-premise. Its responsibilities are the customer IT function's:

| SaaS Platform Operator responsibility | On-premise equivalent |
|---|---|
| Monitor platform health | Container orchestration dashboards, log aggregation, APM |
| Perform platform maintenance | Backups and migrations via deployment pipelines; the migrator is an explicit invocation (DD-18) |
| Configure platform defaults | Environment variables, configuration files or Helm values |
| Manage billing and quotas | Not applicable — the customer manages its own capacity |
| Suspend organizations | Installation Administrator, in the application |

---

## Organization scope

An Organization is the tenant boundary and owns a Scope. On SaaS each paying customer is an Organization.

### Organization Administrator

The Account that creates an Organization becomes its Administrator. Multiple Accounts may hold the role, and **at least one must exist at all times**.

| Capability | Description |
|---|---|
| Manage organization settings | Display name, description, profile |
| Invite and remove members | Invitations are accepted, not imposed |
| Manage organization roles | Assign and revoke Administrator and Member |
| Configure publishing policy | Whether Members may create new packages in the organization scope (enabled by default) |
| Transfer administration | Transfer the role to another member, on that member's acceptance |
| Delete any package in the scope | Subject to the deletion policy below |
| Configure default package visibility | The visibility new packages receive unless overridden |

**The Organization Administrator does not automatically hold a package role.** Package access is granted per package. The role may optionally be configured to carry implicit *read* access across the organization's packages for audit purposes; it never carries implicit write.

**An Organization does not own its members' Accounts.** An Account exists at installation level and is provisioned on first login (DD-20, `F1-05`). An Organization controls membership — who belongs and with what role — not existence. Removing a member from an Organization does not deactivate their Account, and no Organization Administrator can create or delete one.

### Organization Member

| Capability | Description |
|---|---|
| Publish to the organization scope | Creating a new package where policy permits, becoming its Owner |
| View the organization package list | Public, organization-visible, and private packages the member holds a role on |
| View the member list | Other members of the organization |
| Accept package invitations | Take up an Owner, Maintainer or Reader role when granted |
| Leave the organization | Subject to the Owner invariant below |

Members cannot manage roles or memberships, delete packages they do not own, or read private packages they hold no role on.

---

## Package scope

A Package is the container for versions of a kpar or other MBSE artefact (§9). It is the unit of visibility, ownership and collaboration.

### Visibility

Visibility is an attribute of the Package, set by an Owner.

| Visibility | Who may read | Use |
|---|---|---|
| **Private** | Only principals holding an explicit role on the package | Default. Most MBSE artefacts are confidential |
| **Organization-visible** | All members of the owning Organization, read-only; write requires an explicit role | Sharing within the organisation |
| **Public** | **Anyone, including unauthenticated visitors and crawlers** | Publishing to the community |

**Public means anonymous.**  `SSS-FG-REG-W9J` requires unauthenticated reach, and DD-01 and §7.2 rest on public pages being linkable, crawlable and cacheable at a CDN.

**Visibility ships in the first release.** It is not a later addition: search (`E-02`), qualified-name resolution (`E-03`), artefact serving (`C-01`) and mirror replication (§5.1.6) each carry an authorisation dimension from the outset, and `A-01`'s baseline schema carries the attribute.

**New packages are private by default.** An Organization Administrator may set a different default for their organisation; the installation default is private. The two failure modes are not symmetric — an accidental publication cannot be recalled once crawlers, CDN edges, mirrors and downstream copies have taken it, whereas an accidentally private package is corrected in one action.

**Private and organization-visible artefacts are not cached at a CDN.** DD-22 sets `Cache-Control: public, max-age=31536000, immutable` on artefact responses, which is correct only for public packages: the artefact URL is `@scope/name/version/artifact` and therefore guessable, so a shared edge would serve private bytes to anyone who asked for them. Non-public artefacts are served from origin under `Cache-Control: private, no-store`. DD-22's economics are unaffected — its argument rests on *popular* artefacts absorbing origin load, and a non-public package has a small, known audience by construction. The content hash remains the `ETag` on both paths.

**Visibility and unlisting are orthogonal, not two points on one scale.** `SSS-FG-REG-U4D` unlisting hides a version from search and resolution while *still serving direct downloads*; it is a deprecation signal. A package may be public-and-unlisted or private-and-listed. Conflating the two is the likeliest implementation error in this area.

### Owner

The Account that first publishes a package name becomes its Owner. Multiple Owners may exist.

| Capability | Description |
|---|---|
| Publish a version | Subject to `SSS-FG-REG-S2B` monotonic SemVer and `I3C` immutability |
| Unlist and relist a version | `SSS-FG-REG-U4D` |
| Set visibility | Private, organization-visible or public |
| Manage the package team | Grant and revoke Owner, Maintainer and Reader |
| Transfer ownership | Effective only on the recipient's explicit acceptance (`SSS-FG-AUTH-T5E`) |
| Manage package settings | Description, licence, links — within the limits of frozen metadata (`M3C`) |
| Delete the package | Subject to the deletion policy below |

**A package always retains at least one individual-Account Owner** (`SSS-FG-AUTH-O4D`). An Organization may hold ownership, but an Organization Owner alone does not satisfy the invariant (`P7G`). Any operation that would leave a package without an individual Owner — the last Owner leaving, being removed, or the Organization being deleted — is refused, not silently repaired.

### Maintainer

| Capability | Description |
|---|---|
| Publish a version | As Owner |
| Unlist and relist a version | As Owner |
| Read the package | Regardless of visibility |

A Maintainer cannot change visibility, alter the team, transfer ownership or delete the package.

### Reader

An explicit read grant on a package whose visibility would otherwise exclude the principal. **A Reader role on a public package is meaningless and should not be assignable** — public packages are readable by everyone, including anonymous visitors, so the grant would express nothing.

| Capability | Description |
|---|---|
| Read package metadata and versions | Manifest, version list, dependency graph (`SSS-FG-REG-M8H`) |
| Download artefacts | `SSS-FG-REG-D6F` |

### Anonymous and unauthenticated access

An anonymous visitor may read metadata for, search, resolve names within, and download artefacts from **public packages only** (`SSS-FG-REG-W9J`, `F1-04`). Private and organization-visible packages are absent from search results and from qualified-name resolution, and are indistinguishable from packages that do not exist — see *How visibility propagates*.

### Publishing authority

Publishing is authorised against the **scope**, not inherited from the Organization role (§8.2, `B-03`, `SSS-FG-AUTH-G6F`):

| Case | Who may publish |
|---|---|
| A new package in a personal scope `@alice/…` | The Account owning that scope, or one of its API keys |
| A new package in an organization scope `@starion/…` | An Organization Administrator, or a Member where the organization's publishing policy permits |
| A new version of an existing package | An Owner or Maintainer of that package |

The scope is **declared at publish time and authorised**, never derived from the credential, because an Account may hold publishing rights in several scopes and must be able to say which one a publication targets.

### Deletion and erasure

**A published version is never hard-deleted by a user.** `SSS-FG-REG-U4D` unlisting is the only withdrawal available: the version leaves search and resolution and continues to serve direct downloads. This follows from `I3C` immutability and from the `usage[]` IRIs that point at published versions — a hard delete breaks resolution permanently and silently for every dependant, which is precisely the failure §8.2's hash fallback exists to survive.

| Action | Who | Condition |
|---|---|---|
| Unlist or relist a version | Owner, Maintainer | Always available |
| Delete a package | Owner; Organization Administrator of the owning scope | Only while no version has been downloaded and no dependants exist — DD-19's `usage[]` graph supplies the check. Otherwise the operation degrades to unlisting every version, and says so rather than failing silently |
| Erase a package or version | Installation Administrator | Audited. Reserved for accidental disclosure of confidential material, or a lawful erasure request |

Erasure is deliberately an administrator operation rather than a self-service one. It is the escape hatch every registry eventually needs — a credential committed into an artefact, a model published from the wrong scope — and making it self-service turns a rare, considered act into an ordinary button.

Destructive actions are confirmed on their own page requiring the package name to be typed (§7.4, `G-07`) and are recorded in the audit log (`SSS-FG-AUTH-R9J`).

---

## API keys

| Capability | Held by |
|---|---|
| Issue an API key | Any Account, for itself |
| Scope a key to operations | The issuing Account, at issuance — publish, unlist, read |
| Revoke a key | The issuing Account; the Installation Administrator for any key |
| List own keys | The issuing Account — metadata and prefix only, never the secret |

A key's secret is displayed once, at issuance, and is stored only as a hash (`SSS-FG-REG-Y2L`, `F1-02`). A key is not a principal that can be granted package roles in its own right; it derives every permission from its issuing Account at the time of use.

---

## Role assignment rules

1. Every authenticated user has an Account, in both SaaS and on-premise deployments.
2. An Account exists at installation level and is not owned by any Organization.
3. An Account may belong to multiple Organizations and hold a different role in each.
4. An Account holds at most one role per package: Owner, Maintainer or Reader.
5. The Organization Administrator role implies no package role. Package access is granted per package.
6. Every package retains at least one individual-Account Owner at all times (`SSS-FG-AUTH-O4D`, `P7G`).
7. Every Organization retains at least one Organization Administrator at all times.
8. The Account creating an Organization becomes its Administrator; the Account first publishing a package name becomes its Owner.
9. Ownership transfer, organization invitations and package role grants take effect only on the recipient's explicit acceptance (`SSS-FG-AUTH-T5E`).
10. The Platform Operator role grants no access to any package content or account record.
11. An API key never exceeds the authority of its issuing Account.

---

## How visibility propagates

Visibility is an attribute of the Package, but four mechanisms elsewhere in the design read package content and must honour it. Each is recorded here because each was specified before visibility existed, and each would otherwise default to the pre-visibility behaviour.

### Search and qualified-name resolution

Both filter to what the requester may read. **A requester cannot distinguish "does not exist" from "exists but is not yours"** — package lookup (`D-02`) and qualified-name resolution (`E-03`) return the same response in either case, because the existence of a private package name may itself be sensitive for a defence or space programme.

One residual oracle is accepted rather than concealed: publishing to a name already taken privately within the same scope must fail, and that failure reveals the name is taken. It is bounded — names are per-scope, so `@alice/foo` does not block `@bob/foo`, and a principal publishing into a scope generally holds a role in it.

### The content-hash fallback

§8.2 permits serving a byte-identical copy from another scope when the declared version cannot be served. That candidate set is **restricted to artefacts the requester is authorised to read**. Where the only byte-identical copies are invisible to the requester, resolution fails exactly as if none existed.

The filter is on the **requester's** authorisation, not on the artefact's visibility. Filtering the other way would make identical content an oracle for the existence of private packages. §8.2 also has Forge report the substitution to the caller; that report names only a scope the caller can already see.

This degrades availability, which §8.2 anticipates when it calls the mechanism "an availability fallback, not a resolution rule".

### Mirror replication

A mirror replicates exactly what its upstream credential (§5.1.7) is entitled to read. §5.1.6's promise that a mirror searches the whole upstream catalogue therefore reads: **the whole upstream catalogue visible to this installation's credential**. A mirror configured with an anonymous or public-scope credential replicates public packages only; an organisation mirroring its own private packages to an on-premise instance supplies a credential that can see them. Proxied scopes remain read-only (`P4-02`).

### Artefact caching

Non-public artefacts bypass the CDN — see *Visibility*, above.

---

## Verified publishers — deferred

`design.md` §13.1 defers verified publishers beyond the first release, scoped to publisher identity. Two things are settled in advance so the deferral does not become a design gap:

- **It is an attribute of the Scope, not a role.** Verification asserts that a namespace is who it claims to be — that `@esa` is the European Space Agency. It grants no capability, so modelling it as a role would misrepresent it.
- **It needs a granting authority.** The Installation Administrator on-premise; Starion on SaaS. DD-20 notes there is no external authority to vouch for a scope, so the grant is an operational act with an audit entry, not an automated check.

---