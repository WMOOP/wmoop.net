using Wmoop.Core;

namespace Wmoop.Geography;

/// <summary>
/// WM concept <c>wm:geography/City</c> — a <b>leaf</b> of the Composite. A city has no
/// administrative sub-areas, so <see cref="Add"/> is rejected: containment is a real
/// <c>part_of</c> structure, not a free-for-all.
/// </summary>
public sealed class City : AdministrativeArea
{
    public City(ConceptId id, string name, int residentPopulation)
        : base(id, name, residentPopulation) { }

    public override string AreaKind => "city";

    public override AdministrativeArea Add(AdministrativeArea child) =>
        throw new InvalidOperationException("A city is a leaf area and cannot contain sub-areas.");

    public override void Accept(IAreaVisitor visitor) => visitor.VisitCity(this);
}
