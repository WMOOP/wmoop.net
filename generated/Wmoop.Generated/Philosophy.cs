// Generated from module wmoop.philosophy v0.1.0. DO NOT EDIT — regenerate with wmoop.tools.
using System;
using System.Collections.Generic;
using Wmoop.Generated.Runtime;

namespace Wmoop.Generated.Philosophy;

public enum CategoricalForm { UNIVERSAL_AFFIRMATIVE, UNIVERSAL_NEGATIVE, PARTICULAR_AFFIRMATIVE, PARTICULAR_NEGATIVE }

public abstract class PhilosophicalConcept : Entity
{
    public const string WmId = "wmoop.philosophy/PhilosophicalConcept";
    protected PhilosophicalConcept(ConceptId id, string name) : base(id, name)
    {
    }
}

public abstract class Proposition : Entity
{
    public const string WmId = "wmoop.philosophy/Proposition";
    protected Proposition(ConceptId id, string name, string content) : base(id, name)
    {
        Content = content;
    }
    public string Content { get; }
}

public class CategoricalProposition : Proposition
{
    public new const string WmId = "wmoop.philosophy/CategoricalProposition";
    public CategoricalProposition(ConceptId id, string name, string content, CategoricalForm form) : base(id, name, content)
    {
        Form = form;
    }
    public CategoricalForm Form { get; }
}

public abstract class Argument : Entity
{
    public const string WmId = "wmoop.philosophy/Argument";
    protected Argument(ConceptId id, string name, object premiseCount) : base(id, name)
    {
        PremiseCount = premiseCount;
    }
    public object PremiseCount { get; }
}

public class DeductiveArgument : Argument
{
    public new const string WmId = "wmoop.philosophy/DeductiveArgument";
    public DeductiveArgument(ConceptId id, string name, object premiseCount, bool valid) : base(id, name, premiseCount)
    {
        Valid = valid;
    }
    public bool Valid { get; }
}

public class InductiveArgument : Argument
{
    public new const string WmId = "wmoop.philosophy/InductiveArgument";
    public InductiveArgument(ConceptId id, string name, object premiseCount, bool strong) : base(id, name, premiseCount)
    {
        Strong = strong;
    }
    public bool Strong { get; }
}

public class Syllogism : DeductiveArgument
{
    public new const string WmId = "wmoop.philosophy/Syllogism";
    public Syllogism(ConceptId id, string name, object premiseCount, bool valid, object figure) : base(id, name, premiseCount, valid)
    {
        Figure = figure;
    }
    public object Figure { get; }
}

public abstract class PhilosophicalPosition : Entity
{
    public const string WmId = "wmoop.philosophy/PhilosophicalPosition";
    protected PhilosophicalPosition(ConceptId id, string name) : base(id, name)
    {
    }
}

public class AristotelianCategory : Entity
{
    public const string WmId = "wmoop.philosophy/AristotelianCategory";
    public AristotelianCategory(ConceptId id, string name, string greekTerm) : base(id, name)
    {
        GreekTerm = greekTerm;
    }
    public string GreekTerm { get; }
}

public class Branch : Entity
{
    public const string WmId = "wmoop.philosophy/Branch";
    public Branch(ConceptId id, string name, string centralQuestion) : base(id, name)
    {
        CentralQuestion = centralQuestion;
    }
    public string CentralQuestion { get; }
}
