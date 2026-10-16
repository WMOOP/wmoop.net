using Wmoop.Core;
using Wmoop.Nature;
using Wmoop.Transport;

namespace Wmoop.Patterns.Builders;

/// <summary>
/// <b>Builder</b> + <b>Fluent Interface</b> for assembling a <see cref="Person"/>
/// together with the roles they play. Construction stays readable, and adding a facet
/// is naturally expressed as attaching a role (<c>has_role</c>) by composition.
/// </summary>
public sealed class PersonBuilder
{
    private ConceptId _id;
    private bool _idSet;
    private string _name = "Unnamed";
    private int _birthYear = 1970;
    private readonly List<Func<Role>> _roles = new();

    public PersonBuilder WithId(ConceptId id) { _id = id; _idSet = true; return this; }
    public PersonBuilder Named(string name) { _name = name; return this; }
    public PersonBuilder BornIn(int year) { _birthYear = year; return this; }

    public PersonBuilder WithDriverLicence(string number, IEnumerable<string> categories, DateOnly issue, DateOnly expiry)
    {
        _roles.Add(() => new DriverRole(number, categories, issue, expiry));
        return this;
    }

    public PersonBuilder WithOwnership(Vehicle asset, DateOnly since)
    {
        _roles.Add(() => new OwnerRole(asset, since));
        return this;
    }

    public Person Build()
    {
        if (!_idSet)
            throw new InvalidOperationException("A person needs a ConceptId; call WithId() first.");

        var person = new Person(_id, _name, _birthYear);
        foreach (var make in _roles)
            person.AddRole(make());
        return person;
    }
}
