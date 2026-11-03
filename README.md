<p align="center">
  <img alt="wmoop.net" src="brand/wmoop.net.svg" width="340">
</p>

# wmoop.net

**The C# / .NET binding and reference implementation of [WMOOP](https://github.com/WMOOP) —
*World Map Object-Oriented Programming*.**

WMOOP models the real world as one language-neutral, object-oriented knowledge model (the *World
Model*, authored in the **WML** DSL in [`wmoop.doc`](https://github.com/WMOOP/wmoop.doc)). This
repo is the **C# projection** of that model: .NET types generated from it for every domain, plus
hand-written behaviour, design patterns and demos on a few seed domains, demonstrating the
architecture end to end.

> The canonical model and all design documents live in **[`wmoop.doc`](https://github.com/WMOOP/wmoop.doc)**.
> Read the [whitepaper](https://github.com/WMOOP/wmoop.doc/blob/main/docs/whitepaper.md),
> the [meta-model](https://github.com/WMOOP/wmoop.doc/blob/main/docs/02-meta-model.md), and
> [PATTERNS.md](https://github.com/WMOOP/wmoop.doc/blob/main/PATTERNS.md) first.

## What this shows

- **The data classes are generated** from the WML model by [`wmoop.tools`](https://github.com/WMOOP/wmoop.tools):
  - one project per domain (`Wmoop.Core`, `Wmoop.Nature`, `Wmoop.Transport`, `Wmoop.Geography`, …), in `generated/`;
  - the kernel `Entity`, `ConceptId`, `Quantity` (unit-bearing), roles and `Links<T>` live in `Wmoop.Core`.
- **Behaviour is hand-written**, in the other half of each `partial` class (the generation gap) under `Wmoop.Net/src/`:
  - `Vehicle.Describe`;
  - `Driver.CanDrive`;
  - `Person.AgeOn`;
  - `AdministrativeArea.TotalPopulation` / `Accept`.
- **A genuine, shallow is-a hierarchy** (`Wmoop.Transport`): `Vehicle → RoadVehicle → Motorcycle →
  Moped`, with polymorphism.
- **The cross-domain crux done right**:
  - a `Person` (`Wmoop.Nature`) acquires a **`Driver`** and an **`Owner`** role by **composition**, never `Driver : Person`;
  - a role refuses a subject of the wrong type;
  - a person holds several roles at once, each carrying its own meta (licence, ownership).
- **Relations are generated with their inverse**:
  - `county.Contains` / `city.PartOf`;
  - `owner.Owns` / `vehicle.OwnedBy`.
- **Design patterns on the model** (`Wmoop.Patterns`):
  - **Composite** (geography `Country ⊃ County ⊃ City`, `part_of`);
  - **Visitor** (where behaviour lives when data is generated);
  - **Specification** (composable rules / queries);
  - **Builder**, **Strategy**, **Registry/Flyweight**.
- **The shared conformance vectors** of `wmoop.doc`, run by `Wmoop.Tests`: the same vectors
  `wmoop.py` runs, so the two bindings provably agree. Plus two runnable demos.

## Layout

```
generated/                     # GENERATED from the pinned model; never edited by hand
├── Wmoop.Generated.sln        # every generated project
├── Wmoop.Core/                # kernel + the wmoop.core module
├── Wmoop.Nature/  Wmoop.Transport/  Wmoop.Geography/  …   # one project per domain, *.Gen.cs
└── WMOOP_SOURCE.json          # the wmoop.doc / wmoop.json / wmoop.tools commits it came from
external/                      # git submodules: wmoop.doc (the model), wmoop.tools (the generator)
Directory.Build.props          # WmoopSourceRoot: where the generated projects find their hand-written halves
Wmoop.Net/
├── Wmoop.Net.sln              # the generated projects used here + the projects below
├── src/
│   ├── Wmoop.Nature/          # hand-written halves (no project of their own): Person.cs
│   ├── Wmoop.Transport/       # Vehicle.cs, Motorcycle.cs, Moped.cs, Driver.cs
│   ├── Wmoop.Geography/       # AdministrativeArea.cs, Country/County/City.cs, IAreaVisitor.cs
│   ├── Wmoop.Patterns/        # Visitors, Specifications, Builders, Strategies, Registry
│   ├── Wmoop.Demo/            # cross-domain roles + polymorphism walkthrough
│   └── Wmoop.Patterns.Demo/   # one runnable section per design pattern, self-checking
└── test/
    └── Wmoop.Tests/           # runs the shared conformance vectors (non-zero exit on failure)
```

## Build & run

Requires the **.NET 8 SDK** (the projects target `net8.0`). With a newer runtime only, enable
roll-forward as shown.

```bash
dotnet build generated/Wmoop.Generated.sln     # every domain
dotnet build Wmoop.Net/Wmoop.Net.sln           # patterns, demos, tests

# run the walkthroughs
dotnet run --project Wmoop.Net/src/Wmoop.Demo
dotnet run --project Wmoop.Net/src/Wmoop.Patterns.Demo

# run the conformance vectors (exit code is non-zero on failure, so CI can gate on it)
dotnet run --project Wmoop.Net/test/Wmoop.Tests
```

If you only have .NET 9/10/11 installed, prefix with `DOTNET_ROLL_FORWARD=LatestMajor` (bash) or
`$env:DOTNET_ROLL_FORWARD='LatestMajor'` (PowerShell).

## Conformance

The checks are not written here. They are the shared conformance vectors of
[`wmoop.doc/conformance`](https://github.com/WMOOP/wmoop.doc/tree/main/conformance), written once
as data and run by every binding. `Wmoop.Tests` reads the bundle
`external/wmoop.doc/conformance/suite.json` (clone with `--recurse-submodules`), and works by
reflection:

- A model type resolves through the generated `WmId` constant.
- A model name such as `wheelCount` matches the member `WheelCount`.
- An error matches by its registry code (`IWmoopError.Code`).
- C# rejects a missing `required` member or an abstract class at compile time. The runner reports
  those from the declarations as the model's `WM-REQUIRED-MISSING` and `WM-ABSTRACT-INSTANTIATION`.

A `pending` vector describes a rule the model has and no binding enforces yet. It is reported but
does not fail the run. Because the vectors are pinned with the model, a regeneration that brings new
vectors also runs them. To add or change a check, add a vector in `wmoop.doc`, not a test here.

## Generated binding

`generated/` is generated by [`wmoop.tools`](https://github.com/WMOOP/wmoop.tools) from the
canonical model in [`wmoop.doc`](https://github.com/WMOOP/wmoop.doc). Both are pinned as git
submodules, `external/wmoop.doc` and `external/wmoop.tools`, and `generated/WMOOP_SOURCE.json`
records the exact commits it was generated from.

- **Never edit `generated/` by hand.** CI regenerates it from the pins and fails on any difference.
  To change data, change the model in `wmoop.doc` (or the generator in `wmoop.tools`). To add
  behaviour, add a `partial` class under `Wmoop.Net/src/<Project>/`. The generated project compiles it.
- **Regeneration is automatic.** When `wmoop.doc` or `wmoop.tools` changes, and daily,
  [`regen.yml`](.github/workflows/regen.yml) moves both pins to upstream `main`, regenerates, builds,
  runs the tests and opens (or updates) one `chore(regen): …` pull request.
- **To regenerate by hand** after moving a pin, you need Python 3.10+ and
  `pip install -r external/wmoop.doc/schema/requirements.txt`:

  ```bash
  git submodule update --init                        # external/wmoop.doc, external/wmoop.tools
  git -C external/wmoop.doc submodule update --init  # the grammar the model validator reads
  python external/wmoop.tools/tools/regen_binding.py --lang csharp
  ```

Sometimes the model changes in a way the hand-written half relies on, for example a new
`AdministrativeArea` subclass that needs its `Accept`. The regeneration run then fails at build and
opens no pull request. A human moves the pins on a branch, regenerates and adds the behaviour half.
Generated code never needs a manual edit
([docs/03-architecture.md §4.4](https://github.com/WMOOP/wmoop.doc/blob/main/docs/03-architecture.md)).

## License

**Apache-2.0** — see [`LICENSE`](LICENSE).
