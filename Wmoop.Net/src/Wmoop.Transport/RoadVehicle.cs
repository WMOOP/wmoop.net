using Wmoop.Core;

namespace Wmoop.Transport;

/// <summary>
/// WM domain concept <c>wm:transport/RoadVehicle</c> — a <see cref="Vehicle"/>
/// designed for use on public roads. Adds the intrinsic property
/// <see cref="SeatCount"/>. Still abstract: "road vehicle" is a category, not a
/// thing you can instantiate on its own.
/// </summary>
public abstract class RoadVehicle : Vehicle
{
    protected RoadVehicle(ConceptId id, string name, int wheelCount, Quantity maxSpeed, int seatCount)
        : base(id, name, wheelCount, maxSpeed)
    {
        if (seatCount < 1)
            throw new ArgumentOutOfRangeException(nameof(seatCount), "A road vehicle seats at least one.");
        SeatCount = seatCount;
    }

    /// <summary>Number of seats — added at this level of the hierarchy.</summary>
    public int SeatCount { get; }
}
