// Generated from module wmoop.transport v0.3.0. DO NOT EDIT — regenerate with wmoop.tools.
using System;
using System.Collections.Generic;
using Wmoop.Generated.Runtime;

namespace Wmoop.Generated.Transport;

public enum LicenceCategory { AM, A1, A2, A, B }

public abstract class Vehicle : Entity
{
    public const string WmId = "wmoop.transport/Vehicle";
    protected Vehicle(ConceptId id, string name, int wheelCount, Quantity maxSpeed) : base(id, name)
    {
        WheelCount = wheelCount;
        MaxSpeed = maxSpeed;
    }
    public int WheelCount { get; }
    public Quantity MaxSpeed { get; }
}

public abstract class RoadVehicle : Vehicle
{
    public new const string WmId = "wmoop.transport/RoadVehicle";
    protected RoadVehicle(ConceptId id, string name, int wheelCount, Quantity maxSpeed, bool roadLegal) : base(id, name, wheelCount, maxSpeed)
    {
        RoadLegal = roadLegal;
    }
    public bool RoadLegal { get; }
}

public class Motorcycle : RoadVehicle
{
    public new const string WmId = "wmoop.transport/Motorcycle";
    public Motorcycle(ConceptId id, string name, int wheelCount, Quantity maxSpeed, bool roadLegal, Quantity engineDisplacement) : base(id, name, wheelCount, maxSpeed, roadLegal)
    {
        EngineDisplacement = engineDisplacement;
    }
    public Quantity EngineDisplacement { get; }
}

public class Moped : Motorcycle
{
    public new const string WmId = "wmoop.transport/Moped";
    public Moped(ConceptId id, string name, int wheelCount, Quantity maxSpeed, bool roadLegal, Quantity engineDisplacement) : base(id, name, wheelCount, maxSpeed, roadLegal, engineDisplacement)
    {
    }
}

public sealed class Driver : Role
{
    public const string WmId = "wmoop.transport/Driver";
    public const string WmPlayedBy = "wmoop.nature/Person";
    public Driver(string licenceNumber, IReadOnlyList<LicenceCategory> licenceCategories, DateOnly licenceExpiry)
    {
        LicenceNumber = licenceNumber;
        LicenceCategories = licenceCategories;
        LicenceExpiry = licenceExpiry;
    }
    public override ConceptId RoleType => new ConceptId(WmId);
    public string LicenceNumber { get; }
    public IReadOnlyList<LicenceCategory> LicenceCategories { get; }
    public DateOnly LicenceExpiry { get; }
}

public sealed class Owner : Role
{
    public const string WmId = "wmoop.transport/Owner";
    public const string WmPlayedBy = "wmoop.nature/Person";
    public Owner(DateOnly ownedSince)
    {
        OwnedSince = ownedSince;
    }
    public override ConceptId RoleType => new ConceptId(WmId);
    public DateOnly OwnedSince { get; }
}
