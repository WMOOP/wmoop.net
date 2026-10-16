using Wmoop.Core;

namespace Wmoop.Transport;

/// <summary>
/// WM domain concept <c>wm:transport/Moped</c> — a light <see cref="Motorcycle"/>
/// (the founder's own example: <c>Vehicle → AutoVehicle → Moped</c>, rendered here
/// as <c>Vehicle → RoadVehicle → Motorcycle → Moped</c>). Adds
/// <see cref="EngineDisplacement"/> and overrides the polymorphic
/// <see cref="Vehicle.Classification"/>. Sealed: it is a leaf of the hierarchy.
/// </summary>
public sealed class Moped : Motorcycle
{
    public Moped(ConceptId id, string name, Quantity maxSpeed, Quantity engineDisplacement)
        : base(id, name, maxSpeed, hasSidecar: false)
    {
        EngineDisplacement = engineDisplacement;
    }

    /// <summary>Engine displacement, unit-bearing (e.g. 49 cc).</summary>
    public Quantity EngineDisplacement { get; }

    /// <inheritdoc />
    public override string Classification => "moped";

    /// <inheritdoc />
    public override string Describe() =>
        base.Describe() + $" Engine: {EngineDisplacement}.";
}
