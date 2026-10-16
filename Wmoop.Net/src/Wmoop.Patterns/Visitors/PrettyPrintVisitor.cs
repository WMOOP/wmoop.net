using System.Text;
using Wmoop.Geography;

namespace Wmoop.Patterns.Visitors;

/// <summary>
/// <b>Visitor</b> that renders an administrative tree as indented text. It demonstrates
/// the principle: this is real behaviour (traversal + formatting) living entirely
/// OUTSIDE the <see cref="AdministrativeArea"/> data classes, which stay pure state.
/// </summary>
public sealed class PrettyPrintVisitor : IAreaVisitor
{
    private readonly StringBuilder _sb = new();
    private int _depth;

    /// <summary>The accumulated rendering produced while visiting.</summary>
    public string Result => _sb.ToString();

    public void VisitCountry(Country country) => Descend(country);
    public void VisitCounty(County county) => Descend(county);
    public void VisitCity(City city) => Line(city);

    private void Line(AdministrativeArea area) =>
        _sb.AppendLine($"{new string(' ', _depth * 2)}- {area.Name} ({area.AreaKind}, pop {area.ResidentPopulation})");

    private void Descend(AdministrativeArea area)
    {
        Line(area);
        _depth++;
        foreach (var child in area.Children)
            child.Accept(this);
        _depth--;
    }
}
