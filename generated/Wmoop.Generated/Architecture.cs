// Generated from module wmoop.architecture v0.1.0. DO NOT EDIT — regenerate with wmoop.tools.
using System;
using System.Collections.Generic;
using Wmoop.Generated.Runtime;

namespace Wmoop.Generated.Architecture;

public class Building : Entity
{
    public const string WmId = "wmoop.architecture/Building";
    public Building(ConceptId id, string name, int floorCount) : base(id, name)
    {
        FloorCount = floorCount;
    }
    public int FloorCount { get; }
}

public class Room : Entity
{
    public const string WmId = "wmoop.architecture/Room";
    public Room(ConceptId id, string name, Quantity floorArea) : base(id, name)
    {
        FloorArea = floorArea;
    }
    public Quantity FloorArea { get; }
}
