namespace Wmoop.Core;

/// <summary>
/// WM meta-model concept: <b>Role</b> — a context-dependent facet that an
/// <see cref="Entity"/> can play, acquire, or lose over time, without changing
/// what the entity fundamentally <em>is</em> (invariant #3).
/// <para>
/// The canonical example: a human being is intrinsically a <c>Person</c>
/// (biology) for their whole life, but is a <c>Driver</c> only while they hold a
/// licence, and may simultaneously be an <c>Owner</c>, an <c>Employee</c>, and so
/// on. Modelling <c>Driver : Person</c> by inheritance is wrong because the facet
/// is neither permanent nor exclusive. Instead a role is a separate object,
/// attached to its subject by composition and queried through
/// <see cref="IHasRoles"/>.
/// </para>
/// </summary>
public interface IRole
{
    /// <summary>The stable identifier of this role <em>type</em> (e.g. <c>wm:transport/role/Driver</c>).</summary>
    ConceptId RoleType { get; }

    /// <summary>
    /// The entity this role instance is attached to, or <c>null</c> if it has not
    /// yet been granted to any subject. Set when the role is added via
    /// <see cref="IHasRoles.AddRole{T}"/>.
    /// </summary>
    Entity? Subject { get; }
}
