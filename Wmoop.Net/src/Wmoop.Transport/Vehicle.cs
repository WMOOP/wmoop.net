namespace Wmoop.Transport;

/// <summary>
/// The hand-written half of the generated <c>Vehicle</c> (the generation gap): how a vehicle
/// describes itself. The data — <c>WheelCount</c>, <c>MaxSpeed</c>, the subclasses — is generated
/// from <c>wmoop.transport</c>.
/// </summary>
public abstract partial class Vehicle
{
    /// <summary>A short kind name, overridden down the hierarchy.</summary>
    public virtual string Classification => "vehicle";

    /// <summary>Polymorphic description — overridden in subclasses.</summary>
    public virtual string Describe() =>
        $"{Name} — a {Classification} with {WheelCount} wheels, top speed {MaxSpeed?.ToString() ?? "unknown"}.";
}
