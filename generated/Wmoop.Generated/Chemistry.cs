// Generated from module wmoop.chemistry v0.1.0. DO NOT EDIT — regenerate with wmoop.tools.
using System;
using System.Collections.Generic;
using Wmoop.Generated.Runtime;

namespace Wmoop.Generated.Chemistry;

public enum StateOfMatter { SOLID, LIQUID, GAS }

public abstract class ChemicalSubstance : Entity, IHasRoles
{
    public const string WmId = "wmoop.chemistry/ChemicalSubstance";
    private readonly RoleContainer _roles;
    protected ChemicalSubstance(ConceptId id, string name, StateOfMatter standardState, Quantity meltingPoint) : base(id, name)
    {
        _roles = new RoleContainer(this);
        StandardState = standardState;
        MeltingPoint = meltingPoint;
    }
    public StateOfMatter StandardState { get; }
    public Quantity MeltingPoint { get; }
    public IReadOnlyCollection<IRole> Roles => _roles.Roles;
    public T AddRole<T>(T role) where T : Role => _roles.AddRole(role);
    public bool HasRole<T>() where T : class, IRole => _roles.HasRole<T>();
    public T? GetRole<T>() where T : class, IRole => _roles.GetRole<T>();
}

public class ChemicalElement : ChemicalSubstance
{
    public new const string WmId = "wmoop.chemistry/ChemicalElement";
    public ChemicalElement(ConceptId id, string name, StateOfMatter standardState, Quantity meltingPoint, object atomicNumber, string symbol, object standardAtomicWeight) : base(id, name, standardState, meltingPoint)
    {
        AtomicNumber = atomicNumber;
        Symbol = symbol;
        StandardAtomicWeight = standardAtomicWeight;
    }
    public object AtomicNumber { get; }
    public string Symbol { get; }
    public object StandardAtomicWeight { get; }
}

public class ChemicalCompound : ChemicalSubstance
{
    public new const string WmId = "wmoop.chemistry/ChemicalCompound";
    public ChemicalCompound(ConceptId id, string name, StateOfMatter standardState, Quantity meltingPoint, string chemicalFormula) : base(id, name, standardState, meltingPoint)
    {
        ChemicalFormula = chemicalFormula;
    }
    public string ChemicalFormula { get; }
}

public class Atom : Entity
{
    public const string WmId = "wmoop.chemistry/Atom";
    public Atom(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Molecule : Entity
{
    public const string WmId = "wmoop.chemistry/Molecule";
    public Molecule(ConceptId id, string name) : base(id, name)
    {
    }
}

public class ChemicalReaction : Entity
{
    public const string WmId = "wmoop.chemistry/ChemicalReaction";
    public ChemicalReaction(ConceptId id, string name, bool isReversible) : base(id, name)
    {
        IsReversible = isReversible;
    }
    public bool IsReversible { get; }
}

public sealed class Catalyst : Role
{
    public const string WmId = "wmoop.chemistry/Catalyst";
    public const string WmPlayedBy = "wmoop.chemistry/ChemicalSubstance";
    public Catalyst()
    {
    }
    public override ConceptId RoleType => new ConceptId(WmId);
}
