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
    private string? _fullName;
    private DateOnly? _dateOfBirth;
    private readonly List<Func<Role>> _roles = new();

    public PersonBuilder WithId(ConceptId id) { _id = id; _idSet = true; return this; }

    public PersonBuilder Named(string name, string? fullName = null) { _name = name; _fullName = fullName; return this; }

    public PersonBuilder BornOn(DateOnly dateOfBirth) { _dateOfBirth = dateOfBirth; return this; }

    public PersonBuilder WithDriverLicence(string number, IEnumerable<LicenceCategory> categories, DateOnly? expiry = null)
    {
        _roles.Add(() => new Driver { LicenceNumber = number, LicenceCategories = categories.ToArray(), LicenceExpiry = expiry });
        return this;
    }

    public PersonBuilder WithOwnership(DateOnly since, params Vehicle[] vehicles)
    {
        _roles.Add(() =>
        {
            var owner = new Owner { OwnedSince = since };
            foreach (var vehicle in vehicles)
                owner.Owns.Add(vehicle);
            return owner;
        });
        return this;
    }

    public Person Build()
    {
        if (!_idSet)
            throw new InvalidOperationException("A person needs a ConceptId; call WithId() first.");
        var person = new Person { Id = _id, Name = _name, FullName = _fullName ?? _name, DateOfBirth = _dateOfBirth };
        foreach (var make in _roles)
            person.AddRole(make());
        return person;
    }
}
