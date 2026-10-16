using Wmoop.Core;

namespace Wmoop.Nature;

/// <summary>
/// WM domain concept <c>wm:nature/Person</c> (a <c>Homo sapiens</c> individual),
/// an L1 class that is-a-kind-of <see cref="Animal"/>. The lineage is deliberately
/// shallow: <c>Animal → Person</c>. In the fuller WM taxonomy the intermediate
/// biological ranks (Chordate, Mammal, Primate, <c>Homo sapiens</c>) exist as
/// classification facts, but inheritance is reserved for the ranks that earn their
/// keep in code; the rest are carried as categories/tags on the concept
/// (invariant #3: multiple/contested classification is not forced into one tree).
/// <para>
/// A <see cref="Person"/> carries only <em>intrinsic biological</em> state here
/// (leg count via <see cref="Animal"/>, birth year). Everything a person can
/// additionally <em>do</em> or <em>be</em> in some context — driver, owner,
/// employee — is attached as an <see cref="IRole"/> through <see cref="IHasRoles"/>,
/// which this class satisfies by composing a <see cref="RoleContainer"/>.
/// </para>
/// </summary>
public sealed class Person : Animal, IHasRoles
{
    // HAS-A set of roles (composition) — the heart of the cross-domain design.
    private readonly RoleContainer _roles;

    /// <summary>Creates a person. Humans have two legs; that intrinsic fact is fixed here.</summary>
    public Person(ConceptId id, string name, int birthYear)
        : base(id, name, legCount: 2)
    {
        BirthYear = birthYear;
        _roles = new RoleContainer(this);
    }

    /// <summary>Year of birth (Gregorian). Intrinsic biographical fact.</summary>
    public int BirthYear { get; }

    /// <inheritdoc />
    public override string Taxon => "Homo sapiens";

    /// <summary>Age in whole years as of the given calendar year.</summary>
    public int AgeAt(int year) => year - BirthYear;

    // ---- IHasRoles, forwarded to the composed container (no inheritance of facets) ----

    /// <inheritdoc />
    public IReadOnlyCollection<IRole> Roles => _roles.Roles;

    /// <inheritdoc />
    public T AddRole<T>(T role) where T : Role => _roles.AddRole(role);

    /// <inheritdoc />
    public bool HasRole<T>() where T : class, IRole => _roles.HasRole<T>();

    /// <inheritdoc />
    public T? GetRole<T>() where T : class, IRole => _roles.GetRole<T>();
}
