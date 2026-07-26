# Generated data transfer objects

Everything in this folder is **generated output** and is not edited by hand.

The Mycelium Forge DTOs are produced from an Enterprise Architect model exported as an XMI
document, using the `uml4net` toolchain. The same pipeline also emits the **serialisers** for those
types, so a model change regenerates the type and the code that reads and writes it in a single pass.
Serialisation is therefore neither reflection-based nor dependent on third-party source generators —
see `docs/design.md` DD-05.

JSON is the only wire format for Forge metadata (DD-04), so only JSON serialisers are emitted.

Hand-written code that extends a generated type belongs **outside** this folder, as a `partial`
declaration in the project root, so that regeneration never overwrites it.

The XMI source, the generator, and the regeneration command are specified in `docs/design.md`.
