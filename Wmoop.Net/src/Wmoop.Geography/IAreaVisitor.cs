using Wmoop.Core;

namespace Wmoop.Geography;

/// <summary>
/// <b>Visitor pattern</b> — the behaviour hook. It is declared in the domain so the
/// Composite can expose <see cref="AdministrativeArea.Accept"/>, but concrete visitors
/// live OUTSIDE the data model (in <c>Wmoop.Patterns</c>). This is exactly how WMOOP
/// answers the question "if the data classes are generated from the schema, where does
/// behaviour live?": operations over the model are visitors, never methods bolted onto
/// the generated classes.
/// </summary>
public interface IAreaVisitor
{
    void VisitCountry(Country country);
    void VisitCounty(County county);
    void VisitCity(City city);
}
