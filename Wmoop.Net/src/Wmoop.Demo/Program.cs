using Wmoop.Core;
using Wmoop.Nature;
using Wmoop.Transport;

// =====================================================================================
// WMOOP .NET reference implementation — runnable walkthrough.
//
// It demonstrates the two things the architecture insists on:
//   1. A genuine, shallow is-a hierarchy with polymorphism (Vehicle family).
//   2. The cross-domain intersection (Person-as-Driver / Person-as-Owner) modelled
//      as ROLES by COMPOSITION, not by inheritance.
// =====================================================================================

Console.WriteLine("=== WMOOP .NET reference demo ===\n");

// -------------------------------------------------------------------------------------
// Part 1 — Cross-domain roles over composition.
// -------------------------------------------------------------------------------------
Console.WriteLine("-- Part 1: a Person and the roles they play --");

// A Person is an intrinsic biological entity (Wmoop.Nature). Nothing here knows about cars.
var ada = new Person(id: "wm:nature/person/ada-lovelace", name: "Ada", birthYear: 1990);
Console.WriteLine($"{ada.Name}: taxon={ada.Taxon}, legs={ada.LegCount}, age in 2026={ada.AgeAt(2026)}");

// Build a Moped first so the owner role can point at it.
var moped = new Moped(
    id: "wm:transport/moped/piaggio-ciao",
    name: "Piaggio Ciao",
    maxSpeed: new Quantity(45, "km/h"),
    engineDisplacement: new Quantity(49, "cc"));

// Grant two Transport facets to the SAME person, at the same time, via composition.
ada.AddRole(new DriverRole(
    licenceNumber: "RO-AB-123456",
    categories: new[] { "AM", "A1", "B" },
    issueDate: new DateOnly(2018, 6, 1),
    expiryDate: new DateOnly(2028, 6, 1)));

ada.AddRole(new OwnerRole(asset: moped, ownedSince: new DateOnly(2022, 3, 15)));

// Query roles polymorphically through the IHasRoles capability.
Console.WriteLine($"Is {ada.Name} a driver? {ada.HasRole<DriverRole>()}");
Console.WriteLine($"Is {ada.Name} an owner? {ada.HasRole<OwnerRole>()}");
Console.WriteLine($"{ada.Name} holds {ada.Roles.Count} role(s):");
foreach (var role in ada.Roles)
    Console.WriteLine($"   - {role.RoleType} (on {role.Subject?.Name})");

// Use the meta carried by a role.
var licence = ada.GetRole<DriverRole>()!;
var today = new DateOnly(2026, 10, 16);
bool canRideMoped = licence.CanDrive("AM", today);
bool canDriveTruck = licence.CanDrive("C", today);
Console.WriteLine($"Licence {licence.LicenceNumber}: can ride a moped (AM) today? {canRideMoped}");
Console.WriteLine($"Can drive a truck (C) today? {canDriveTruck}");

var ownership = ada.GetRole<OwnerRole>()!;
Console.WriteLine($"Owns '{ownership.Asset.Name}' since {ownership.OwnedSince}\n");

// -------------------------------------------------------------------------------------
// Part 2 — Shallow is-a hierarchy with polymorphism.
// -------------------------------------------------------------------------------------
Console.WriteLine("-- Part 2: the Vehicle hierarchy, polymorphically --");

var fleet = new List<Vehicle>
{
    moped,
    new Motorcycle(
        id: "wm:transport/motorcycle/bmw-r1250",
        name: "BMW R1250 GS",
        maxSpeed: new Quantity(200, "km/h")),
    new Motorcycle(
        id: "wm:transport/motorcycle/ural-sidecar",
        name: "Ural Gear-Up",
        maxSpeed: new Quantity(110, "km/h"),
        hasSidecar: true),
};

foreach (var vehicle in fleet)
    Console.WriteLine($"   {vehicle.Describe()}");

Console.WriteLine("\n=== demo complete ===");
