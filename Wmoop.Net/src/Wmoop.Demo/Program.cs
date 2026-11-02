using Wmoop.Core;
using Wmoop.Nature;
using Wmoop.Transport;

// =====================================================================================
// WMOOP .NET binding — runnable walkthrough.
//
// The data classes (Person, Moped, Driver, Owner, …) are GENERATED from the WML model in
// wmoop.doc (see generated/); the behaviour used here (AgeOn, CanDrive, Describe) is the
// hand-written half of those partial classes, under Wmoop.Net/src/.
//
// It demonstrates the two things the architecture insists on:
//   1. A genuine, shallow is-a hierarchy with polymorphism (Vehicle family).
//   2. The cross-domain intersection (Person-as-Driver / Person-as-Owner) modelled
//      as ROLES by COMPOSITION, not by inheritance.
// =====================================================================================

Console.WriteLine("=== WMOOP .NET demo ===\n");

// -------------------------------------------------------------------------------------
// Part 1 — Cross-domain roles over composition.
// -------------------------------------------------------------------------------------
Console.WriteLine("-- Part 1: a Person and the roles they play --");

// A Person is an intrinsic biological entity (Wmoop.Nature). Nothing here knows about cars.
var today = new DateOnly(2026, 10, 16);
var ada = new Person
{
    Id = "wmoop.nature/person/ada-lovelace",
    Name = "Ada",
    FullName = "Ada Lovelace",
    LegCount = 2,
    DateOfBirth = new DateOnly(1990, 1, 1),
};
Console.WriteLine($"{ada.Name}: legs={ada.LegCount}, age on {today:yyyy-MM-dd}={ada.AgeOn(today)}");

var moped = new Moped
{
    Id = "wmoop.transport/moped/piaggio-ciao",
    Name = "Piaggio Ciao",
    WheelCount = 2,
    MaxSpeed = new Quantity(45, "km/h"),
    EngineDisplacement = new Quantity(49, "cm3"),
};

// Grant two Transport facets to the SAME person, at the same time, via composition.
ada.AddRole(new Driver
{
    LicenceNumber = "RO-AB-123456",
    LicenceCategories = new[] { LicenceCategory.AM, LicenceCategory.A1, LicenceCategory.B },
    LicenceExpiry = new DateOnly(2028, 6, 1),
});
var owner = ada.AddRole(new Owner { OwnedSince = new DateOnly(2022, 3, 15) });
owner.Owns.Add(moped); // a generated relation: the moped's OwnedBy follows

// Query roles polymorphically through the IHasRoles capability.
Console.WriteLine($"Is {ada.Name} a driver? {ada.HasRole<Driver>()}");
Console.WriteLine($"Is {ada.Name} an owner? {ada.HasRole<Owner>()}");
Console.WriteLine($"{ada.Name} holds {ada.Roles.Count} role(s):");
foreach (var role in ada.Roles)
    Console.WriteLine($"   - {role.RoleType} (on {role.Subject?.Name})");

// Use the meta carried by a role.
var licence = ada.GetRole<Driver>()!;
Console.WriteLine($"Licence {licence.LicenceNumber}: can ride a moped (AM) today? {licence.CanDrive(LicenceCategory.AM, today)}");
Console.WriteLine($"Can ride an unrestricted motorcycle (A) today? {licence.CanDrive(LicenceCategory.A, today)}");
Console.WriteLine($"Owns '{string.Join(", ", owner.Owns.Select(v => v.Name))}' since {owner.OwnedSince:yyyy-MM-dd}");
Console.WriteLine($"'{moped.Name}' is owned by {string.Join(", ", moped.OwnedBy.Select(o => o.Subject?.Name))}\n");

// -------------------------------------------------------------------------------------
// Part 2 — Shallow is-a hierarchy with polymorphism.
// -------------------------------------------------------------------------------------
Console.WriteLine("-- Part 2: the Vehicle hierarchy, polymorphically --");
var fleet = new List<Vehicle>
{
    moped,
    new Motorcycle
    {
        Id = "wmoop.transport/motorcycle/bmw-r1250gs",
        Name = "BMW R1250 GS",
        WheelCount = 2,
        MaxSpeed = new Quantity(200, "km/h"),
        EngineDisplacement = new Quantity(1254, "cm3"),
    },
    new Motorcycle
    {
        Id = "wmoop.transport/motorcycle/ural-gear-up",
        Name = "Ural Gear-Up",
        WheelCount = 3, // a sidecar outfit
        MaxSpeed = new Quantity(110, "km/h"),
        EngineDisplacement = new Quantity(749, "cm3"),
    },
};
foreach (var vehicle in fleet)
    Console.WriteLine($"   {vehicle.Describe()}");

Console.WriteLine("\n=== demo complete ===");
