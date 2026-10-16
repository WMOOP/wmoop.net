using Wmoop.Core;
using Wmoop.Nature;
using Wmoop.Transport;

// A minimal, dependency-free test runner. Each Check records a pass/fail; the process
// exits non-zero if anything failed, so CI can treat it as a gate (invariant #6).

var t = new TestRunner();

// ---- Kernel value types -------------------------------------------------------------
t.Check("ConceptId rejects blank", () =>
{
    try { _ = new ConceptId("  "); return false; }
    catch (ArgumentException) { return true; }
});

t.Check("ConceptId equality is by value", () =>
    new ConceptId("wm:x/y") == new ConceptId("wm:x/y"));

t.Check("Quantity requires a unit", () =>
{
    try { _ = new Quantity(1, ""); return false; }
    catch (ArgumentException) { return true; }
});

t.Check("Quantity renders value and unit", () =>
    new Quantity(49, "cc").ToString() == "49 cc");

// ---- Inheritance (shallow, honest) --------------------------------------------------
t.Check("Moped is-a Vehicle (polymorphism)", () =>
{
    Vehicle v = new Moped("wm:transport/moped/x", "X", new Quantity(45, "km/h"), new Quantity(49, "cc"));
    return v is Motorcycle and RoadVehicle;
});

t.Check("Classification overrides down the chain", () =>
    new Moped("wm:transport/moped/x", "X", new Quantity(45, "km/h"), new Quantity(49, "cc"))
        .Classification == "moped");

t.Check("Person is-a Animal with 2 legs", () =>
{
    Animal a = new Person("wm:nature/person/p", "P", 2000);
    return a.LegCount == 2 && a.Taxon == "Homo sapiens";
});

// ---- Roles over composition (the cross-domain crux) ---------------------------------
t.Check("Person starts with no roles", () =>
    new Person("wm:nature/person/p", "P", 2000).Roles.Count == 0);

t.Check("Person can hold Driver and Owner at once", () =>
{
    var p = new Person("wm:nature/person/p", "P", 1990);
    var moped = new Moped("wm:transport/moped/x", "X", new Quantity(45, "km/h"), new Quantity(49, "cc"));
    p.AddRole(new DriverRole("L1", new[] { "AM" }, new DateOnly(2020, 1, 1), new DateOnly(2030, 1, 1)));
    p.AddRole(new OwnerRole(moped, new DateOnly(2021, 1, 1)));
    return p.HasRole<DriverRole>() && p.HasRole<OwnerRole>() && p.Roles.Count == 2;
});

t.Check("AddRole binds the role's Subject back to the person", () =>
{
    var p = new Person("wm:nature/person/p", "P", 1990);
    var role = p.AddRole(new DriverRole("L1", new[] { "B" }, new DateOnly(2020, 1, 1), new DateOnly(2030, 1, 1)));
    return ReferenceEquals(role.Subject, p);
});

t.Check("GetRole returns null when absent", () =>
    new Person("wm:nature/person/p", "P", 1990).GetRole<DriverRole>() is null);

t.Check("DriverRole validity window and category are honoured", () =>
{
    var d = new DriverRole("L1", new[] { "AM", "B" }, new DateOnly(2020, 1, 1), new DateOnly(2030, 1, 1));
    return d.CanDrive("am", new DateOnly(2026, 1, 1))      // case-insensitive, in window
        && !d.CanDrive("C", new DateOnly(2026, 1, 1))      // category not held
        && !d.CanDrive("B", new DateOnly(2031, 1, 1));     // expired
});

t.Check("A role cannot be attached to two subjects", () =>
{
    var p1 = new Person("wm:nature/person/p1", "P1", 1990);
    var p2 = new Person("wm:nature/person/p2", "P2", 1991);
    var role = new DriverRole("L1", new[] { "B" }, new DateOnly(2020, 1, 1), new DateOnly(2030, 1, 1));
    p1.AddRole(role);
    try { p2.AddRole(role); return false; }
    catch (InvalidOperationException) { return true; }
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
