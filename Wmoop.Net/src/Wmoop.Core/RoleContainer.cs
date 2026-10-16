namespace Wmoop.Core;

/// <summary>
/// Reusable implementation of <see cref="IHasRoles"/>. A role-bearing entity holds
/// one of these and forwards its <see cref="IHasRoles"/> members to it — classic
/// composition: "a Person HAS-A set of roles", not "a Person IS-A driver".
/// <para>
/// Keeping this logic in one kernel type means every domain entity (Person, and
/// later Organization, Place, ...) gains correct, consistent role semantics for
/// free, with no duplication (invariant #1: write once).
/// </para>
/// </summary>
public sealed class RoleContainer : IHasRoles
{
    private readonly Entity _subject;
    private readonly List<IRole> _roles = new();

    /// <summary>Creates a container bound to the entity whose roles it manages.</summary>
    public RoleContainer(Entity subject) =>
        _subject = subject ?? throw new ArgumentNullException(nameof(subject));

    /// <inheritdoc />
    public IReadOnlyCollection<IRole> Roles => _roles;

    /// <inheritdoc />
    public T AddRole<T>(T role) where T : Role
    {
        ArgumentNullException.ThrowIfNull(role);
        role.AttachTo(_subject);
        _roles.Add(role);
        return role;
    }

    /// <inheritdoc />
    public bool HasRole<T>() where T : class, IRole => GetRole<T>() is not null;

    /// <inheritdoc />
    public T? GetRole<T>() where T : class, IRole
    {
        foreach (var role in _roles)
            if (role is T match)
                return match;
        return null;
    }
}
