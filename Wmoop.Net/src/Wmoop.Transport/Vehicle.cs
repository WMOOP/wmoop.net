using Wmoop.Core;

namespace Wmoop.Transport;

/// <summary>
/// WM domain concept <c>wm:transport/Vehicle</c> — the root of a genuine, shallow
/// is-a hierarchy (<c>Vehicle → RoadVehicle → Motorcycle → Moped</c>). Each level
/// is a real "kind-of" refinement that only <em>adds</em> intrinsic properties and
/// never violates a base guarantee (Liskov; invariant #3).
/// </summary>
public abstract class Vehicle : Entity
{
    /// <param name="id">Stable identifier of this vehicle.</param>
    /// <param name="name">Human-readable name / model.</param>
    /// <param name="wheelCount">Dimensionless count — plain int.</param>
    /// <param name="maxSpeed">A unit-bearing <see cref="Quantity"/> (e.g. km/h).</param>
    protected Vehicle(ConceptId id, string name, int wheelCount, Quantity maxSpeed)
        : base(id, name)
    {
        if (wheelCount < 0)
            throw new ArgumentOutOfRangeException(nameof(wheelCount));
        WheelCount = wheelCount;
        MaxSpeed = maxSpeed;
    }

    /// <summary>Number of wheels — intrinsic, dimensionless.</summary>
    public int WheelCount { get; }

    /// <summary>Top speed, carried with its unit (invariant #4).</summary>
    public Quantity MaxSpeed { get; }

    /// <summary>
    /// A short classification label for the concrete kind. Overridden down the
    /// hierarchy; this is the polymorphic hook the demo iterates over.
    /// </summary>
    public abstract string Classification { get; }

    /// <summary>Uniform, human-readable description built from polymorphic parts.</summary>
    public virtual string Describe() =>
        $"{Name} — a {Classification} with {WheelCount} wheels, top speed {MaxSpeed}.";
}
