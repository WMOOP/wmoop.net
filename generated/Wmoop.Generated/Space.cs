// Generated from module wmoop.space v0.1.0. DO NOT EDIT — regenerate with wmoop.tools.
using System;
using System.Collections.Generic;
using Wmoop.Generated.Runtime;

namespace Wmoop.Generated.Space;

public abstract class CelestialBody : Entity
{
    public const string WmId = "wmoop.space/CelestialBody";
    protected CelestialBody(ConceptId id, string name, Quantity mass, Quantity equatorialRadius) : base(id, name)
    {
        Mass = mass;
        EquatorialRadius = equatorialRadius;
    }
    public Quantity Mass { get; }
    public Quantity EquatorialRadius { get; }
}

public class Planet : CelestialBody
{
    public new const string WmId = "wmoop.space/Planet";
    public Planet(ConceptId id, string name, Quantity mass, Quantity equatorialRadius, Quantity orbitalPeriod, Quantity numberOfMoons) : base(id, name, mass, equatorialRadius)
    {
        OrbitalPeriod = orbitalPeriod;
        NumberOfMoons = numberOfMoons;
    }
    public Quantity OrbitalPeriod { get; }
    public Quantity NumberOfMoons { get; }
}

public class DwarfPlanet : CelestialBody
{
    public new const string WmId = "wmoop.space/DwarfPlanet";
    public DwarfPlanet(ConceptId id, string name, Quantity mass, Quantity equatorialRadius) : base(id, name, mass, equatorialRadius)
    {
    }
}

public class Star : CelestialBody
{
    public new const string WmId = "wmoop.space/Star";
    public Star(ConceptId id, string name, Quantity mass, Quantity equatorialRadius) : base(id, name, mass, equatorialRadius)
    {
    }
}

public class NaturalSatellite : CelestialBody
{
    public new const string WmId = "wmoop.space/NaturalSatellite";
    public NaturalSatellite(ConceptId id, string name, Quantity mass, Quantity equatorialRadius) : base(id, name, mass, equatorialRadius)
    {
    }
}

public class PlanetarySystem : Entity
{
    public const string WmId = "wmoop.space/PlanetarySystem";
    public PlanetarySystem(ConceptId id, string name, Quantity numberOfPlanets) : base(id, name)
    {
        NumberOfPlanets = numberOfPlanets;
    }
    public Quantity NumberOfPlanets { get; }
}
