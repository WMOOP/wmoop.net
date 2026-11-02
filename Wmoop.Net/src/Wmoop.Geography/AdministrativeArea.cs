using System.Linq;

namespace Wmoop.Geography;

/// <summary>
/// The hand-written half of the generated <c>AdministrativeArea</c> (the generation gap). The data,
/// including the <c>partOf</c> / <c>Contains</c> composite, is generated from
/// <c>wmoop.geography</c>; this half only adds behaviour over it.
/// </summary>
public abstract partial class AdministrativeArea
{
    /// <summary>A short kind name for display: <c>country</c>, <c>county</c>, <c>city</c>.</summary>
    public virtual string AreaKind => GetType().Name.ToLowerInvariant();

    /// <summary>
    /// This area's own residents plus those of every area it contains, recursively. An area whose
    /// resident population is not recorded counts as 0 here.
    /// </summary>
    public long TotalPopulation => (ResidentPopulation ?? 0) + Contains.Sum(area => area.TotalPopulation);

    /// <summary>Double dispatch for <see cref="IAreaVisitor"/>.</summary>
    public abstract void Accept(IAreaVisitor visitor);
}
