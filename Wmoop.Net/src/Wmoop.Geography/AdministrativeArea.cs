using Wmoop.Core;

namespace Wmoop.Geography;

/// <summary>
/// WM domain concept <c>wm:geography/AdministrativeArea</c> and the <b>Composite</b>
/// pattern's component. Administrative containment (a country HAS counties, a county
/// HAS cities) is a <c>part_of</c> whole–part relation — NOT inheritance: a city is
/// not a <em>kind of</em> county. Leaf and composite share one uniform type so clients
/// can treat "a city" and "a whole country" alike.
/// </summary>
public abstract class AdministrativeArea : Entity
{
    private readonly List<AdministrativeArea> _children = new();

    protected AdministrativeArea(ConceptId id, string name, int residentPopulation)
        : base(id, name)
    {
        if (residentPopulation < 0)
            throw new ArgumentOutOfRangeException(nameof(residentPopulation), "Population cannot be negative.");
        ResidentPopulation = residentPopulation;
    }

    /// <summary>Population resident directly in this area, excluding its sub-areas.</summary>
    public int ResidentPopulation { get; }

    /// <summary>The contained sub-areas — the <c>part_of</c> children.</summary>
    public IReadOnlyList<AdministrativeArea> Children => _children;

    /// <summary>A short label for the kind of area (country/county/city).</summary>
    public abstract string AreaKind { get; }

    /// <summary>Adds a sub-area. Leaves (e.g. <see cref="City"/>) reject this.</summary>
    /// <returns>This area, for fluent chaining.</returns>
    public virtual AdministrativeArea Add(AdministrativeArea child)
    {
        ArgumentNullException.ThrowIfNull(child);
        _children.Add(child);
        return this;
    }

    /// <summary>Total population of this area and every sub-area, computed recursively.</summary>
    public long TotalPopulation
    {
        get
        {
            long total = ResidentPopulation;
            foreach (var child in _children)
                total += child.TotalPopulation;
            return total;
        }
    }

    /// <summary>Double-dispatch hook for an <see cref="IAreaVisitor"/> (Visitor pattern).</summary>
    public abstract void Accept(IAreaVisitor visitor);
}
