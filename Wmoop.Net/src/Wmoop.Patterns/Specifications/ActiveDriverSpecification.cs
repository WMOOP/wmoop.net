using Wmoop.Nature;
using Wmoop.Transport;

namespace Wmoop.Patterns.Specifications;

/// <summary>
/// A concrete <b>Specification</b>: satisfied when a <see cref="Person"/> currently
/// holds a <see cref="Driver"/> role valid for a given licence category on a given date.
/// It reads a ROLE (<c>has_role</c>), never a subclass — the cross-domain facet stays
/// composed, so this rule works without Nature ever knowing about licences.
/// </summary>
public sealed class ActiveDriverSpecification : ISpecification<Person>
{
    private readonly LicenceCategory _category;
    private readonly DateOnly _on;

    public ActiveDriverSpecification(LicenceCategory category, DateOnly on)
    {
        _category = category;
        _on = on;
    }

    public bool IsSatisfiedBy(Person candidate) =>
        candidate.GetRole<Driver>()?.CanDrive(_category, _on) == true;
}
