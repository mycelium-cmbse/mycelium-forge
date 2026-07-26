# Generated data transfer objects

Everything in this folder is **generated output** and is not edited by hand.

The Mycelium Forge DTOs are produced from an Enterprise Architect model exported as an XMI
document. The generator emits one file per type, carrying both `System.Text.Json` and
`MessagePack` annotations so the same DTO serves the JSON and MessagePack representations of the
Forge HTTP API.

Hand-written code that extends a generated type belongs **outside** this folder, as a `partial`
declaration in the project root, so that regeneration never overwrites it.

The XMI source, the generator, and the regeneration command are specified in `docs/design.md`.
