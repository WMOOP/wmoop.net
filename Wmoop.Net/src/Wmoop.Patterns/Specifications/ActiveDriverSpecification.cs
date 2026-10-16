using Wmoop.Nature;
using Wmoop.Transport;

namespace Wmoop.Patterns.Specifications;

/// <summary>
/// A concrete <b>Specification</b>: satisfied when a <see cref="Person"/> currently
/// holds a <see cref="DriverRole"/> valid for a given licence category on a given date.
/// It reads a ROLE (<c>has_role</c>), never a subclass — the cross-domain facet stays
/// composed, so this rule works without Nature ever knowing about licences.
/// </summary>
public sealed class ActiveDriverSpecification : ISpecification<Person>
{
    private readonly string _category;
    private readonly DateOnly _on;

    public ActiveDriverSpecification(string category, DateOnly on)
    {
        _category = category;
        _on = on;
    }

    public bool IsSatisfiedBy(Person candidate)
    {
        var licence = candidate.GetRole<DriverRole>();
        return licence is not null && licence.CanDrive(_category, _on);
    }
}
