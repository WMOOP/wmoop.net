using Wmoop.Core;

namespace Wmoop.Transport;

/// <summary>
/// WM domain concept <c>wm:transport/Motorcycle</c> — a concrete
/// <see cref="RoadVehicle"/>. Adds <see cref="HasSidecar"/>. Wheel and seat counts
/// are derived from that intrinsic choice and passed up to the base, demonstrating
/// that a subclass refines, and never contradicts, its parent's contract.
/// </summary>
public class Motorcycle : RoadVehicle
{
    public Motorcycle(ConceptId id, string name, Quantity maxSpeed, bool hasSidecar = false)
        : base(id, name,
               wheelCount: hasSidecar ? 3 : 2,
               maxSpeed: maxSpeed,
               seatCount: hasSidecar ? 2 : 1)
    {
        HasSidecar = hasSidecar;
    }

    /// <summary>Whether a sidecar is fitted — intrinsic to this kind.</summary>
    public bool HasSidecar { get; }

    /// <inheritdoc />
    public override string Classification => "motorcycle";
}
