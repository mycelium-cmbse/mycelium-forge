# Mycelium.Forge.Common

Shared data transfer objects for the Mycelium Forge package registry.

## Generated code

`AutoGenDto/` (DTO interfaces and classes) and `AutoGenEnum/` (enumerations) are **generated
output** and are not edited by hand.

The Mycelium Forge DTOs and enums are produced from an Enterprise Architect model exported as an
XMI document — shipped as the NuGet package
[`Mycelium.Model.Forge`](https://www.nuget.org/packages/Mycelium.Model.Forge) — using the
`uml4net` toolchain (`uml4net.xmi` to read, `uml4net.HandleBars` to emit). The same pipeline will
also emit the **serialisers** for those types (see `docs/design.md` DD-05), so a model change
regenerates the type and the code that reads and writes it in a single pass. Serialisation is
therefore neither reflection-based nor dependent on third-party source generators.

JSON is the only wire format for Forge metadata (DD-04), so only JSON serialisers are emitted.

Hand-written code that extends a generated type belongs **outside** `AutoGenDto/`/`AutoGenEnum/`,
as a `partial` declaration in this project's root. **The hand-written file's namespace must match
the generated type's namespace** — `Mycelium.Forge.Common`, the project's own root namespace, not
a namespace named after the `AutoGenDto`/`AutoGenEnum` folder — otherwise the two halves compile as
two unrelated types instead of merging into one `partial` type. The `AutoGenDto`/`AutoGenEnum`
split is a folder-level, on-disk organisation by artefact kind; it is not reflected in the C#
namespace.

Generation is performed by `Mycelium.Forge.Generator` (a plain class library, no build-time
integration) at design-time, not at run-time — it is driven by the `Mycelium.Forge.Generator.Tests`
NUnit project, which reads the XMI once and runs each generator as a test fixture. Matching the
uml4net/SysML2.NET code generation tutorial's own workflow, there is no automated test that
regenerates the *entire* model and asserts it matches what's committed — that review is a manual,
visual-inspection step by design:

- **`ExpectedOutputTestFixture`** (run on every `dotnet test`) renders a small, hand-picked sample of
  "interesting" classes (covering every type/multiplicity/subsetting variation in the model, per
  `uml4net`'s `ModelInspector`) and diffs it against `Expected/` golden files. This is what catches
  template regressions on every run — it does not cover every class in the model.
- **`*RegenerationTests`** (marked `[Explicit]` so they never run by default) render the *full* set of
  DTOs/enums to this test project's own build output — `_Forge.Common.AutoGenDto/` and
  `_Forge.Common.AutoGenEnum/` (a future generator targeting a different project, e.g. a JSON
  serialiser, would use its own prefixed folder, such as `_Forge.Serializer.Json.AutoGenSerializer/`).
  After a model change: run these deliberately, review the output there by eye (or diff it against
  `AutoGenDto/`/`AutoGenEnum/`), and manually copy over whatever you accept.
