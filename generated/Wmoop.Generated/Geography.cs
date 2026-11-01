// Generated from module wmoop.geography v0.2.0. DO NOT EDIT — regenerate with wmoop.tools.
using System;
using System.Collections.Generic;
using Wmoop.Generated.Runtime;

namespace Wmoop.Generated.Geography;

public abstract class Place : Entity
{
    public const string WmId = "wmoop.geography/Place";
    protected Place(ConceptId id, string name) : base(id, name)
    {
    }
}

public abstract class AdministrativeArea : Place
{
    public new const string WmId = "wmoop.geography/AdministrativeArea";
    protected AdministrativeArea(ConceptId id, string name, int residentPopulation) : base(id, name)
    {
        ResidentPopulation = residentPopulation;
    }
    public int ResidentPopulation { get; }
}

public class Country : AdministrativeArea
{
    public new const string WmId = "wmoop.geography/Country";
    public Country(ConceptId id, string name, int residentPopulation) : base(id, name, residentPopulation)
    {
    }
}

public class County : AdministrativeArea
{
    public new const string WmId = "wmoop.geography/County";
    public County(ConceptId id, string name, int residentPopulation) : base(id, name, residentPopulation)
    {
    }
}

public class City : AdministrativeArea
{
    public new const string WmId = "wmoop.geography/City";
    public City(ConceptId id, string name, int residentPopulation) : base(id, name, residentPopulation)
    {
    }
}
