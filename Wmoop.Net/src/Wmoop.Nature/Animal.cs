using Wmoop.Core;

namespace Wmoop.Nature;

/// <summary>
/// WM domain concept <c>wm:nature/Animal</c> — an L1 class in the biology branch.
/// <para>
/// This is a genuine, stable <em>is-a-kind-of</em> abstraction (invariant #3):
/// everything that is an animal is one for its whole existence. We therefore model
/// it with inheritance, and keep the hierarchy shallow and Liskov-clean — a
/// subclass only ever <em>adds</em> intrinsic biological properties, never removes
/// or contradicts a base guarantee.
/// </para>
/// </summary>
public abstract class Animal : Entity
{
    /// <param name="id">Stable identifier of this individual animal.</param>
    /// <param name="name">Human-readable name.</param>
    /// <param name="legCount">An intrinsic, dimensionless count — a plain int, not a <see cref="Quantity"/>.</param>
    protected Animal(ConceptId id, string name, int legCount)
        : base(id, name)
    {
        if (legCount < 0)
            throw new ArgumentOutOfRangeException(nameof(legCount), "Leg count cannot be negative.");
        LegCount = legCount;
    }

    /// <summary>Number of legs — a shared intrinsic property of the Animal abstraction.</summary>
    public int LegCount { get; }

    /// <summary>
    /// The biological taxon of the concrete kind of animal (polymorphic). Each
    /// concrete subclass answers for itself; callers can treat any
    /// <see cref="Animal"/> uniformly.
    /// </summary>
    public abstract string Taxon { get; }
}
