using Wmoop.Core;

namespace Wmoop.Geography;

/// <summary>
/// WM concept <c>wm:geography/County</c> — a <b>Composite</b> node that contains
/// cities via <c>part_of</c>.
/// </summary>
public sealed class County : AdministrativeArea
{
    public County(ConceptId id, string name, int residentPopulation = 0)
        : base(id, name, residentPopulation) { }

    public override string AreaKind => "county";

    public override void Accept(IAreaVisitor visitor) => visitor.VisitCounty(this);
}
