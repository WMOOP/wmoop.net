namespace Wmoop.Core;

/// <summary>
/// WM meta-model concept: <b>Entity</b> — the root of everything that can be
/// identified and talked about in the World Model.
/// <para>
/// An <see cref="Entity"/> is anything that bears a stable <see cref="Id"/>
/// (invariant #4) and a human-readable <see cref="Name"/>. Concrete domain types
/// — a <c>Person</c> in <c>Wmoop.Nature</c>, a <c>Vehicle</c> in
/// <c>Wmoop.Transport</c> — derive from it only where there is a genuine, stable
/// <em>is-a-kind-of</em> relationship (invariant #3). Cross-cutting facets a thing
/// can gain or lose (being a driver, an owner, an employee) are NOT subclasses;
/// they are <see cref="IRole"/>s attached by composition.
/// </para>
/// </summary>
public abstract class Entity
{
    /// <summary>Creates an entity with its stable identifier and human name.</summary>
    /// <param name="id">The stable global identifier (see <see cref="ConceptId"/>).</param>
    /// <param name="name">A human-readable name; never used for identity or lookup.</param>
    protected Entity(ConceptId id, string name)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>The stable global identifier that defines this entity's identity.</summary>
    public ConceptId Id { get; }

    /// <summary>A human-readable label. Mutable in intent but kept read-only here for simplicity.</summary>
    public string Name { get; }

    public override string ToString() => $"{Name} [{Id}]";
}
