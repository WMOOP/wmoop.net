// Generated from module wmoop.nature v0.7.0. DO NOT EDIT — regenerate with wmoop.tools.
using System;
using System.Collections.Generic;
using Wmoop.Generated.Runtime;

namespace Wmoop.Generated.Nature;

public abstract class AnatomicalStructure : Entity
{
    public const string WmId = "wmoop.nature/AnatomicalStructure";
    protected AnatomicalStructure(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Organ : AnatomicalStructure
{
    public new const string WmId = "wmoop.nature/Organ";
    public Organ(ConceptId id, string name) : base(id, name)
    {
    }
}

public class OrganSystem : Entity
{
    public const string WmId = "wmoop.nature/OrganSystem";
    public OrganSystem(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Heart : Organ
{
    public new const string WmId = "wmoop.nature/Heart";
    public Heart(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Brain : Organ
{
    public new const string WmId = "wmoop.nature/Brain";
    public Brain(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Lung : Organ
{
    public new const string WmId = "wmoop.nature/Lung";
    public Lung(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Liver : Organ
{
    public new const string WmId = "wmoop.nature/Liver";
    public Liver(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Kidney : Organ
{
    public new const string WmId = "wmoop.nature/Kidney";
    public Kidney(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Stomach : Organ
{
    public new const string WmId = "wmoop.nature/Stomach";
    public Stomach(ConceptId id, string name) : base(id, name)
    {
    }
}

public abstract class Animal : Entity
{
    public const string WmId = "wmoop.nature/Animal";
    protected Animal(ConceptId id, string name, int legCount) : base(id, name)
    {
        LegCount = legCount;
    }
    public int LegCount { get; }
}

public abstract class Human : Animal
{
    public new const string WmId = "wmoop.nature/Human";
    protected Human(ConceptId id, string name, int legCount) : base(id, name, legCount)
    {
    }
}

public class Person : Human, IHasRoles
{
    public new const string WmId = "wmoop.nature/Person";
    private readonly RoleContainer _roles;
    public Person(ConceptId id, string name, int legCount, string fullName, DateOnly dateOfBirth) : base(id, name, legCount)
    {
        _roles = new RoleContainer(this);
        FullName = fullName;
        DateOfBirth = dateOfBirth;
    }
    public string FullName { get; }
    public DateOnly DateOfBirth { get; }
    public IReadOnlyCollection<IRole> Roles => _roles.Roles;
    public T AddRole<T>(T role) where T : Role => _roles.AddRole(role);
    public bool HasRole<T>() where T : class, IRole => _roles.HasRole<T>();
    public T? GetRole<T>() where T : class, IRole => _roles.GetRole<T>();
}
