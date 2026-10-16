namespace Wmoop.Core;

/// <summary>
/// Base class for concrete <see cref="IRole"/> implementations. Domain role types
/// (e.g. <c>DriverRole</c>, <c>OwnerRole</c>) derive from this and add the meta
/// that makes the facet meaningful (licence number, ownership date, ...).
/// <para>
/// A role holds a back-reference to its <see cref="Subject"/>, which is populated
/// exactly once when the role is granted through <see cref="RoleContainer"/>. The
/// setter is <c>internal</c> so only the kernel can establish the link, keeping
/// subject/role consistency invariant from outside the assembly.
/// </para>
/// </summary>
public abstract class Role : IRole
{
    /// <inheritdoc />
    public abstract ConceptId RoleType { get; }

    /// <inheritdoc />
    public Entity? Subject { get; private set; }

    /// <summary>
    /// Binds this role instance to the entity that now plays it. Called by
    /// <see cref="RoleContainer.AddRole{T}"/>; not part of the public surface so a
    /// role cannot be silently re-pointed at a different subject.
    /// </summary>
    internal void AttachTo(Entity subject)
    {
        if (Subject is not null)
            throw new InvalidOperationException(
                $"Role '{RoleType}' is already attached to '{Subject.Name}' and cannot be reassigned.");
        Subject = subject ?? throw new ArgumentNullException(nameof(subject));
    }
}
