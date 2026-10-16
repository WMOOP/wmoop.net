# wmoop.net

**The C# / .NET binding and reference implementation of [WMOOP](https://github.com/WMOOP) —
*World Map Object-Oriented Programming*.**

WMOOP models the real world as one language-neutral, object-oriented knowledge model (the *World
Model*, authored in the **WML** DSL in [`wmoop.doc`](https://github.com/WMOOP/wmoop.doc)). This
repo is the **C# projection** of that model: idiomatic .NET types for the meta-model and a few seed
domains, demonstrating the architecture end to end.

> The canonical model and all design documents live in **[`wmoop.doc`](https://github.com/WMOOP/wmoop.doc)**.
> Read the [whitepaper](https://github.com/WMOOP/wmoop.doc/blob/main/docs/whitepaper.md),
> the [meta-model](https://github.com/WMOOP/wmoop.doc/blob/main/docs/02-meta-model.md), and
> [PATTERNS.md](https://github.com/WMOOP/wmoop.doc/blob/main/PATTERNS.md) first.

## What this shows

- **A clean meta-model kernel** (`Wmoop.Core`): `Entity`, `ConceptId`, `Quantity` (unit-bearing),
  and the role machinery `IRole` / `Role` / `IHasRoles` / `RoleContainer`.
- **A genuine, shallow is-a hierarchy** (`Wmoop.Transport`): `Vehicle → RoadVehicle → Motorcycle →
  Moped`, with polymorphism.
- **The cross-domain crux done right** — a `Person` (`Wmoop.Nature`) acquires a **`Driver`** and an
  **`Owner`** role by **composition**, never `Driver : Person`. A person holds several roles at
  once; each carries its own meta (licence, ownership).
- **Design patterns on the model** (`Wmoop.Patterns`): **Composite** (geography `Country ⊃ County ⊃
  City`, `part_of`), **Visitor** (where behaviour lives when data is generated), **Specification**
  (composable rules / queries), **Builder**, **Strategy**, **Registry/Flyweight**.
- **Dependency-free conformance tests** and two runnable demos.

## Layout

```
Wmoop.Net/
├── Wmoop.Net.sln
├── src/
│   ├── Wmoop.Core/            # L0 meta-model kernel
│   ├── Wmoop.Nature/          # Animal → Person (biology)
│   ├── Wmoop.Transport/       # Vehicle hierarchy + Driver/Owner roles
│   ├── Wmoop.Geography/       # AdministrativeArea Composite (part_of)
│   ├── Wmoop.Patterns/        # Visitors, Specifications, Builders, Strategies, Registry
│   ├── Wmoop.Demo/            # cross-domain roles + polymorphism walkthrough
│   └── Wmoop.Patterns.Demo/   # one runnable section per design pattern
└── test/
    └── Wmoop.Tests/           # dependency-free conformance checks (non-zero exit on failure)
```

Each type's XML-doc names the WM meta-model concept (or design pattern) it realises.

## Build & run

Requires the **.NET 8 SDK** (the projects target `net8.0`). With a newer runtime only, enable
roll-forward as shown.

```bash
cd Wmoop.Net
dotnet build Wmoop.Net.sln

# run the walkthroughs
dotnet run --project src/Wmoop.Demo
dotnet run --project src/Wmoop.Patterns.Demo

# run the conformance tests (exit code is non-zero on failure, so CI can gate on it)
dotnet run --project test/Wmoop.Tests
```

If you only have .NET 9/10/11 installed, prefix with `DOTNET_ROLL_FORWARD=LatestMajor` (bash) or
`$env:DOTNET_ROLL_FORWARD='LatestMajor'` (PowerShell).

## Relation to the canonical model

These C# types mirror the WML in `wmoop.doc/model/` (`transport`, `nature`, `geography`). In the
target architecture the data classes are **generated** from that WML; this repo is the hand-written
reference that the generator's output will match (conformance-tested). Behaviour (visitors,
strategies, specifications) is always hand-written and lives *outside* the generated data model —
see [PATTERNS.md](https://github.com/WMOOP/wmoop.doc/blob/main/PATTERNS.md).

## License

**Apache-2.0** — see [`LICENSE`](LICENSE).
