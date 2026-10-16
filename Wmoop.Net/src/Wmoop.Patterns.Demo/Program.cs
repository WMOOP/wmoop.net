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
// WMOOP model and self-checks with a tiny assert helper (non-zero exit on failure, so
// CI can gate on it). Deterministic: fixed dates, no DateTime.Now / Random.
// =====================================================================================

int failures = 0;
void Check(string label, bool ok)
{
    Console.WriteLine($"  [{(ok ? "PASS" : "FAIL")}] {label}");
    if (!ok) failures++;
}

Console.WriteLine("=== WMOOP design-patterns showcase ===\n");

// -------------------------------------------------------------------------------------
// Composite (part_of): Country -> County -> City, built by composition, not inheritance.
// -------------------------------------------------------------------------------------
Console.WriteLine("-- Composite: administrative containment (part_of, NOT inheritance) --");
var cluj = new City("wm:geography/city/cluj-napoca", "Cluj-Napoca", 286_598);
var turda = new City("wm:geography/city/turda", "Turda", 43_319);
var bucharest = new City("wm:geography/city/bucharest", "Bucharest", 1_716_961);

var clujCounty = new County("wm:geography/county/cluj", "Cluj");
clujCounty.Add(cluj);
clujCounty.Add(turda);

var ilfovCounty = new County("wm:geography/county/ilfov", "Ilfov");
ilfovCounty.Add(bucharest);

var romania = new Country("wm:geography/country/romania", "Romania");
romania.Add(clujCounty);
romania.Add(ilfovCounty);

Check("A city is a leaf (Add is rejected)", Throws<InvalidOperationException>(() => cluj.Add(turda)));
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
var moped = new Moped("wm:transport/moped/ciao", "Piaggio Ciao", new Quantity(45, "km/h"), new Quantity(49, "cc"));
var ada = new PersonBuilder()
    .WithId("wm:nature/person/ada")
    .Named("Ada")
    .BornIn(1990)
    .WithDriverLicence("RO-AB-123456", new[] { "AM", "A1", "B" }, new DateOnly(2018, 6, 1), new DateOnly(2028, 6, 1))
    .WithOwnership(moped, new DateOnly(2022, 3, 15))
    .Build();
Check("Builder attached 2 roles", ada.Roles.Count == 2);
Check("Builder's person is a driver (has_role)", ada.HasRole<DriverRole>());

// -------------------------------------------------------------------------------------
// Specification: composable boolean rules (And / Or / Not), usable as a query predicate.
// -------------------------------------------------------------------------------------
Console.WriteLine("\n-- Specification: composable rules (And / Or / Not) --");
var on = new DateOnly(2026, 10, 16);
ISpecification<Person> canRideMoped = new ActiveDriverSpecification("AM", on);
ISpecification<Person> canDriveTruck = new ActiveDriverSpecification("C", on);
Check("Ada satisfies 'can ride a moped (AM)'", canRideMoped.IsSatisfiedBy(ada));
Check("Ada does NOT satisfy 'can drive a truck (C)'", !canDriveTruck.IsSatisfiedBy(ada));
Check("AM AND NOT C holds", canRideMoped.And(canDriveTruck.Not()).IsSatisfiedBy(ada));
Check("C OR AM holds", canDriveTruck.Or(canRideMoped).IsSatisfiedBy(ada));

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
var id1 = registry.Intern("wm:nature/Person");
var id2 = registry.Intern("wm:nature/Person");
Check("Interned ids compare equal", id1 == id2);
registry.Register(ada);
registry.Register(romania);
Check("Registry resolves a registered entity", registry.Resolve("wm:nature/person/ada") is Person);
Check("Registry holds 2 entities", registry.Count == 2);

Console.WriteLine($"\n=== {(failures == 0 ? "all checks passed" : failures + " check(s) FAILED")} ===");
return failures == 0 ? 0 : 1;

static bool Throws<TException>(Action action) where TException : Exception
{
    try { action(); return false; }
    catch (TException) { return true; }
}
