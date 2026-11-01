// Generated runtime kernel for the WMOOP C# binding. DO NOT EDIT.
using System;
using System.Collections.Generic;

namespace Wmoop.Generated.Runtime;

public readonly struct ConceptId : IEquatable<ConceptId>
{
    public string Value { get; }
    public ConceptId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("A ConceptId must be non-empty.", nameof(value));
        Value = value;
    }
    public static implicit operator ConceptId(string value) => new(value);
    public bool Equals(ConceptId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
    public override bool Equals(object? obj) => obj is ConceptId o && Equals(o);
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value ?? string.Empty);
    public override string ToString() => Value ?? string.Empty;
    public static bool operator ==(ConceptId a, ConceptId b) => a.Equals(b);
    public static bool operator !=(ConceptId a, ConceptId b) => !a.Equals(b);
}

public readonly struct Quantity
{
    public double Value { get; }
    public string Unit { get; }
    public Quantity(double value, string unit)
    {
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("A Quantity must carry a unit.", nameof(unit));
        Value = value;
        Unit = unit;
    }
    public override string ToString() => $"{Value:0.###} {Unit}";
}

public abstract class Entity
{
    protected Entity(ConceptId id, string name)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
    public ConceptId Id { get; }
    public string Name { get; }
    public override string ToString() => $"{Name} [{Id}]";
}

public interface IRole
{
    ConceptId RoleType { get; }
    Entity? Subject { get; }
}

public abstract class Role : IRole
{
    public abstract ConceptId RoleType { get; }
    public Entity? Subject { get; private set; }
    internal void AttachTo(Entity subject)
    {
        if (Subject is not null)
            throw new InvalidOperationException("Role is already attached to a subject.");
        Subject = subject ?? throw new ArgumentNullException(nameof(subject));
    }
}

public interface IHasRoles
{
    IReadOnlyCollection<IRole> Roles { get; }
    T AddRole<T>(T role) where T : Role;
    bool HasRole<T>() where T : class, IRole;
    T? GetRole<T>() where T : class, IRole;
}

public sealed class RoleContainer : IHasRoles
{
    private readonly Entity _subject;
    private readonly List<IRole> _roles = new();
    public RoleContainer(Entity subject) => _subject = subject ?? throw new ArgumentNullException(nameof(subject));
    public IReadOnlyCollection<IRole> Roles => _roles;
    public T AddRole<T>(T role) where T : Role
    {
        role.AttachTo(_subject);
        _roles.Add(role);
        return role;
    }
    public bool HasRole<T>() where T : class, IRole => GetRole<T>() is not null;
    public T? GetRole<T>() where T : class, IRole
    {
        foreach (var r in _roles)
            if (r is T t) return t;
        return null;
    }
}
