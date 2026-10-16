namespace Wmoop.Core;

/// <summary>
/// WM meta-model capability: <b>Role-bearing entity</b>. An entity that can have
/// facets attached to it by composition (invariant #3) implements this interface,
/// typically by delegating to a <see cref="RoleContainer"/>.
/// <para>
/// This is the contract through which the rest of the system asks questions such as
/// "is this person currently a driver?" (<see cref="HasRole{T}"/>) or "give me their
/// driver details" (<see cref="GetRole{T}"/>) — without the entity's own type
/// needing to know about any particular domain's roles.
/// </para>
/// </summary>
public interface IHasRoles
{
    /// <summary>All roles currently attached to this entity.</summary>
    IReadOnlyCollection<IRole> Roles { get; }

    /// <summary>Grants a role to this entity and binds the role's subject to it.</summary>
    /// <returns>The same role instance, for fluent use.</returns>
    T AddRole<T>(T role) where T : Role;

    /// <summary>Returns <c>true</c> if this entity currently plays a role of type <typeparamref name="T"/>.</summary>
    bool HasRole<T>() where T : class, IRole;

    /// <summary>Returns the first role of type <typeparamref name="T"/>, or <c>null</c> if the entity does not play it.</summary>
    T? GetRole<T>() where T : class, IRole;
}
