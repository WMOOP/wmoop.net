using Wmoop.Core;
using Wmoop.Nature;
using Wmoop.Transport;
using Wmoop.Geography;
using Wmoop.Patterns.Visitors;
using Wmoop.Patterns.Specifications;
using Wmoop.Patterns.Builders;
using Wmoop.Patterns.Strategies;
using Wmoop.Patterns.Registries;

// =====================================================================================
// WMOOP design-patterns showcase — each section demonstrates one named pattern on the
// generated WMOOP model and self-checks with a tiny assert helper (non-zero exit on
// failure, so CI can gate on it). Deterministic: fixed dates, no DateTime.Now / Random.
// =====================================================================================

int failures = 0;
void Check(string label, bool ok)
{
    Console.WriteLine($"  [{(ok ? "PASS" : "FAIL")}] {label}");
    if (!ok) failures++;
}

Console.WriteLine("=== WMOOP design-patterns showcase ===\n");

// -------------------------------------------------------------------------------------
// Composite (part_of): Country -> County -> City, a generated relation, not inheritance.
// -------------------------------------------------------------------------------------
Console.WriteLine("-- Composite: administrative containment (part_of, NOT inheritance) --");
var cluj = new City { Id = "wmoop.geography/city/cluj-napoca", Name = "Cluj-Napoca", ResidentPopulation = 286_598 };
var turda = new City { Id = "wmoop.geography/city/turda", Name = "Turda", ResidentPopulation = 43_319 };
var bucharest = new City { Id = "wmoop.geography/city/bucharest", Name = "Bucharest", ResidentPopulation = 1_716_961 };

var clujCounty = new County { Id = "wmoop.geography/county/cluj", Name = "Cluj" };
clujCounty.Contains.Add(cluj);
turda.PartOf = clujCounty; // either end of the relation can be used

var romania = new Country { Id = "wmoop.geography/country/romania", Name = "Romania" };
romania.Contains.Add(clujCounty);
romania.Contains.Add(bucharest); // a municipality at county level, not part of a county

Check("The inverse end follows: a city knows its county (partOf)", ReferenceEquals(cluj.PartOf, clujCounty));
Check("Composite sums population recursively", romania.TotalPopulation == 286_598L + 43_319 + 1_716_961);

// -------------------------------------------------------------------------------------
// Visitor (double dispatch): behaviour lives OUTSIDE the data model.
// -------------------------------------------------------------------------------------
Console.WriteLine("\n-- Visitor: operations over the model (double dispatch) --");
var printer = new PrettyPrintVisitor();
romania.Accept(printer);
Console.Write(printer.Result);

var census = new PopulationSumVisitor();
romania.Accept(census);
Check("Visitor computes the same total, independently", census.Total == romania.TotalPopulation);
Check("Visitor counts 3 cities", census.CityCount == 3);

// -------------------------------------------------------------------------------------
// Builder + Fluent Interface: construct a Person together with the roles they play.
// -------------------------------------------------------------------------------------
Console.WriteLine("\n-- Builder: fluent construction of a Person with roles --");
var moped = new Moped
{
    Id = "wmoop.transport/moped/ciao",
    Name = "Piaggio Ciao",
    WheelCount = 2,
    MaxSpeed = new Quantity(45, "km/h"),
    EngineDisplacement = new Quantity(49, "cm3"),
};
var ada = new PersonBuilder()
    .WithId("wmoop.nature/person/ada")
    .Named("Ada", "Ada Lovelace")
    .BornOn(new DateOnly(1990, 1, 1))
    .WithDriverLicence("RO-AB-123456", new[] { LicenceCategory.AM, LicenceCategory.A1, LicenceCategory.B }, new DateOnly(2028, 6, 1))
    .WithOwnership(new DateOnly(2022, 3, 15), moped)
    .Build();
Check("Builder attached 2 roles", ada.Roles.Count == 2);
Check("Builder's person is a driver (has_role)", ada.HasRole<Driver>());
Check("The moped knows its owner (ownedBy)", moped.OwnedBy.Single().Subject == ada);

// -------------------------------------------------------------------------------------
// Specification: composable boolean rules (And / Or / Not), usable as a query predicate.
// -------------------------------------------------------------------------------------
Console.WriteLine("\n-- Specification: composable rules (And / Or / Not) --");
var on = new DateOnly(2026, 10, 16);
ISpecification<Person> canRideMoped = new ActiveDriverSpecification(LicenceCategory.AM, on);
ISpecification<Person> canRideAnyMotorcycle = new ActiveDriverSpecification(LicenceCategory.A, on);

Check("Ada satisfies 'can ride a moped (AM)'", canRideMoped.IsSatisfiedBy(ada));
Check("Ada does NOT satisfy 'can ride an unrestricted motorcycle (A)'", !canRideAnyMotorcycle.IsSatisfiedBy(ada));
Check("AM AND NOT A holds", canRideMoped.And(canRideAnyMotorcycle.Not()).IsSatisfiedBy(ada));
Check("A OR AM holds", canRideAnyMotorcycle.Or(canRideMoped).IsSatisfiedBy(ada));

// -------------------------------------------------------------------------------------
// Strategy: swappable algorithms, decoupled from the data.
// -------------------------------------------------------------------------------------
Console.WriteLine("\n-- Strategy: swappable area formatting --");
IAreaFormatStrategy compact = new CompactAreaFormat();
IAreaFormatStrategy detailed = new CensusAreaFormat();
Console.WriteLine("  compact:  " + compact.Format(romania));
Console.WriteLine("  detailed: " + detailed.Format(romania));
Check("Two strategies produce different output", compact.Format(romania) != detailed.Format(romania));

// -------------------------------------------------------------------------------------
// Registry + Flyweight: canonical ids and an L2 instance store.
// -------------------------------------------------------------------------------------
Console.WriteLine("\n-- Registry + Flyweight: canonical ids and instance lookup --");
var registry = new ConceptRegistry();
var id1 = registry.Intern("wmoop.nature/Person");
var id2 = registry.Intern("wmoop.nature/Person");
Check("Interned ids compare equal", id1 == id2);
registry.Register(ada);
registry.Register(romania);
Check("Registry resolves a registered entity", registry.Resolve("wmoop.nature/person/ada") is Person);
Check("Registry holds 2 entities", registry.Count == 2);

Console.WriteLine($"\n=== {(failures == 0 ? "all checks passed" : failures + " check(s) FAILED")} ===");
return failures == 0 ? 0 : 1;
