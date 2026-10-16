using Wmoop.Core;

namespace Wmoop.Transport;

/// <summary>
/// WM cross-domain concept <c>wm:transport/role/Owner</c> — another Transport facet
/// a <see cref="Wmoop.Nature.Person"/> can hold, <em>at the same time</em> as being
/// a driver. Its existence alongside <see cref="DriverRole"/> is exactly why roles
/// beat inheritance: a single <c>Person</c> object carries several independent
/// facets, which a single-rooted <c>Driver : Person</c> / <c>Owner : Person</c>
/// hierarchy could not express without multiple inheritance.
/// </summary>
public sealed class OwnerRole : Role
{
    /// <summary>Stable identifier of the Owner role type.</summary>
    public static readonly ConceptId TypeId = new("wm:transport/role/Owner");

    public OwnerRole(Vehicle asset, DateOnly ownedSince)
    {
        Asset = asset ?? throw new ArgumentNullException(nameof(asset));
        OwnedSince = ownedSince;
    }

    /// <inheritdoc />
    public override ConceptId RoleType => TypeId;

    /// <summary>The vehicle owned (a link to another domain entity, by reference).</summary>
    public Vehicle Asset { get; }

    public DateOnly OwnedSince { get; }
}
