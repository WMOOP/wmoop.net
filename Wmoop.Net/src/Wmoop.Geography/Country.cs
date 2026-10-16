using Wmoop.Core;

namespace Wmoop.Geography;

/// <summary>
/// WM concept <c>wm:geography/Country</c> — a <b>Composite</b> node that contains
/// counties via <c>part_of</c>.
/// </summary>
public sealed class Country : AdministrativeArea
{
    public Country(ConceptId id, string name, int residentPopulation = 0)
        : base(id, name, residentPopulation) { }

    public override string AreaKind => "country";

    public override void Accept(IAreaVisitor visitor) => visitor.VisitCountry(this);
}
