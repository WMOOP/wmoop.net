using Wmoop.Geography;

namespace Wmoop.Patterns.Visitors;

/// <summary>
/// <b>Visitor</b> that totals population and counts cities by walking the Composite.
/// The algorithm lives here, not on the data model — a new report is a new visitor
/// (Open/Closed), never an edit to a generated class.
/// </summary>
public sealed class PopulationSumVisitor : IAreaVisitor
{
    /// <summary>Running total of resident population across every visited area.</summary>
    public long Total { get; private set; }

    /// <summary>Number of leaf cities visited.</summary>
    public int CityCount { get; private set; }

    public void VisitCountry(Country country) => Recurse(country);
    public void VisitCounty(County county) => Recurse(county);

    public void VisitCity(City city)
    {
        Total += city.ResidentPopulation;
        CityCount++;
    }

    private void Recurse(AdministrativeArea area)
    {
        Total += area.ResidentPopulation;
        foreach (var child in area.Children)
            child.Accept(this);
    }
}
