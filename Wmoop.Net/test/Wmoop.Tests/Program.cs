using Wmoop.Core;
using Wmoop.Geography;
using Wmoop.Nature;
using Wmoop.Transport;

// A minimal, dependency-free test runner. Each Check records a pass/fail; the process
// exits non-zero if anything failed, so CI can treat it as a gate (invariant #6).
// The classes under test are generated from the WML model; the behaviour (Classification,
// CanDrive, TotalPopulation) is the hand-written half of their partial classes.

var t = new TestRunner();
var on = new DateOnly(2026, 1, 1);

Moped NewMoped() => new()
{
    Id = "wmoop.transport/moped/x",
    Name = "X",
    WheelCount = 2,
    MaxSpeed = new Quantity(45, "km/h"),
    EngineDisplacement = new Quantity(49, "cm3"),
};
Person NewPerson(string id) => new() { Id = id, Name = id, FullName = id };
Driver NewDriver(params LicenceCategory[] categories) => new()
{
    LicenceNumber = "L1",
    LicenceCategories = categories,
    LicenceExpiry = new DateOnly(2030, 1, 1),
};

// ---- Kernel value types -------------------------------------------------------------
t.Check("ConceptId rejects blank", () =>
{
    try { _ = new ConceptId("  "); return false; }
    catch (ArgumentException) { return true; }
});
t.Check("ConceptId equality is by value", () =>
    new ConceptId("wmoop.x/y") == new ConceptId("wmoop.x/y"));
t.Check("Quantity requires a unit", () =>
{
    try { _ = new Quantity(1, ""); return false; }
    catch (ArgumentException) { return true; }
});
t.Check("Quantity renders value and unit", () =>
    new Quantity(49, "cm3").ToString() == "49 cm3");

// ---- Inheritance (shallow, honest) --------------------------------------------------
t.Check("Moped is-a Vehicle (polymorphism)", () =>
{
    Vehicle v = NewMoped();
    return v is Motorcycle and RoadVehicle;
});
t.Check("Classification overrides down the chain", () =>
    NewMoped().Classification == "moped");
t.Check("Person is-a Animal with 2 legs", () =>
{
    Animal a = new Person { Id = "wmoop.nature/person/p", Name = "P", FullName = "P", LegCount = 2 };
    return a.LegCount == 2 && a.IsA("wmoop.nature/Human");
});

// ---- Roles over composition (the cross-domain crux) ---------------------------------
t.Check("Person starts with no roles", () =>
    NewPerson("p").Roles.Count == 0);
t.Check("Person can hold Driver and Owner at once", () =>
{
    var p = NewPerson("p");
    p.AddRole(NewDriver(LicenceCategory.AM));
    p.AddRole(new Owner { OwnedSince = new DateOnly(2021, 1, 1) });
    return p.HasRole<Driver>() && p.HasRole<Owner>() && p.Roles.Count == 2;
});
t.Check("AddRole binds the role's Subject back to the person", () =>
{
    var p = NewPerson("p");
    var role = p.AddRole(NewDriver(LicenceCategory.B));
    return ReferenceEquals(role.Subject, p);
});
t.Check("GetRole returns null when absent", () =>
    NewPerson("p").GetRole<Driver>() is null);
t.Check("Driver categories and expiry are honoured", () =>
{
    var d = NewDriver(LicenceCategory.AM, LicenceCategory.B);
    return d.CanDrive(LicenceCategory.AM, on)                 // category held, in date
        && !d.CanDrive(LicenceCategory.A, on)                 // category not held
        && !d.CanDrive(LicenceCategory.B, new DateOnly(2031, 1, 1)); // expired
});
t.Check("A role cannot be attached to two subjects", () =>
{
    var role = NewDriver(LicenceCategory.B);
    NewPerson("p1").AddRole(role);
    try { NewPerson("p2").AddRole(role); return false; }
    catch (InvalidOperationException) { return true; }
});
t.Check("A role refuses a subject that is not its played_by type (Driver on a Moped)", () =>
{
    try { NewMoped().AddRole(NewDriver(LicenceCategory.B)); return false; }
    catch (InvalidOperationException) { return true; }
});

// ---- Generated relations ------------------------------------------------------------
t.Check("Owner.Owns and Vehicle.OwnedBy stay in step", () =>
{
    var owner = NewPerson("p").AddRole(new Owner());
    var moped = NewMoped();
    owner.Owns.Add(moped);
    var linked = moped.OwnedBy.Contains(owner);
    moped.OwnedBy.Remove(owner);
    return linked && owner.Owns.Count == 0;
});
t.Check("Composite: moving a city updates both parents", () =>
{
    var country = new Country { Id = "wmoop.geography/c", Name = "C" };
    var county = new County { Id = "wmoop.geography/k", Name = "K", PartOf = country };
    var city = new City { Id = "wmoop.geography/x", Name = "X", ResidentPopulation = 10, PartOf = country };
    city.PartOf = county;
    return country.Contains.Count == 1 && county.Contains.Contains(city) && country.TotalPopulation == 10;
});

return t.Summarize();

/// <summary>Tiny pass/fail recorder; no external test framework.</summary>
internal sealed class TestRunner
{
    private int _passed;
    private int _failed;

    public void Check(string name, Func<bool> body)
    {
        bool ok;
        try { ok = body(); }
        catch (Exception ex) { ok = false; Console.WriteLine($"[THREW] {name}: {ex.GetType().Name}: {ex.Message}"); }

        if (ok) { _passed++; Console.WriteLine($"[PASS] {name}"); }
        else { _failed++; Console.WriteLine($"[FAIL] {name}"); }
    }

    public int Summarize()
    {
        Console.WriteLine($"\n{_passed} passed, {_failed} failed.");
        return _failed == 0 ? 0 : 1;
    }
}
