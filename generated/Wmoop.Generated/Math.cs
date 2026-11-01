// Generated from module wmoop.math v0.1.0. DO NOT EDIT — regenerate with wmoop.tools.
using System;
using System.Collections.Generic;
using Wmoop.Generated.Runtime;

namespace Wmoop.Generated.Math;

public abstract class MathematicalObject : Entity
{
    public const string WmId = "wmoop.math/MathematicalObject";
    protected MathematicalObject(ConceptId id, string name) : base(id, name)
    {
    }
}

public abstract class Collection : MathematicalObject
{
    public new const string WmId = "wmoop.math/Collection";
    protected Collection(ConceptId id, string name, bool isEmpty) : base(id, name)
    {
        IsEmpty = isEmpty;
    }
    public bool IsEmpty { get; }
}

public class Set : Collection
{
    public new const string WmId = "wmoop.math/Set";
    public Set(ConceptId id, string name, bool isEmpty, bool isFinite) : base(id, name, isEmpty)
    {
        IsFinite = isFinite;
    }
    public bool IsFinite { get; }
}

public abstract class Number : MathematicalObject
{
    public new const string WmId = "wmoop.math/Number";
    protected Number(ConceptId id, string name) : base(id, name)
    {
    }
}

public class NaturalNumber : Number
{
    public new const string WmId = "wmoop.math/NaturalNumber";
    public NaturalNumber(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Integer : Number
{
    public new const string WmId = "wmoop.math/Integer";
    public Integer(ConceptId id, string name) : base(id, name)
    {
    }
}

public class RationalNumber : Number
{
    public new const string WmId = "wmoop.math/RationalNumber";
    public RationalNumber(ConceptId id, string name) : base(id, name)
    {
    }
}

public class RealNumber : Number
{
    public new const string WmId = "wmoop.math/RealNumber";
    public RealNumber(ConceptId id, string name) : base(id, name)
    {
    }
}

public abstract class GeometricShape : MathematicalObject
{
    public new const string WmId = "wmoop.math/GeometricShape";
    protected GeometricShape(ConceptId id, string name) : base(id, name)
    {
    }
}

public abstract class Polygon : GeometricShape
{
    public new const string WmId = "wmoop.math/Polygon";
    protected Polygon(ConceptId id, string name, int sideCount, bool isRegular) : base(id, name)
    {
        SideCount = sideCount;
        IsRegular = isRegular;
    }
    public int SideCount { get; }
    public bool IsRegular { get; }
}

public class Triangle : Polygon
{
    public new const string WmId = "wmoop.math/Triangle";
    public Triangle(ConceptId id, string name, int sideCount, bool isRegular) : base(id, name, sideCount, isRegular)
    {
    }
}

public class Quadrilateral : Polygon
{
    public new const string WmId = "wmoop.math/Quadrilateral";
    public Quadrilateral(ConceptId id, string name, int sideCount, bool isRegular) : base(id, name, sideCount, isRegular)
    {
    }
}

public abstract class AlgebraicStructure : MathematicalObject
{
    public new const string WmId = "wmoop.math/AlgebraicStructure";
    protected AlgebraicStructure(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Group : AlgebraicStructure
{
    public new const string WmId = "wmoop.math/Group";
    public Group(ConceptId id, string name, Quantity order, bool isAbelian) : base(id, name)
    {
        Order = order;
        IsAbelian = isAbelian;
    }
    public Quantity Order { get; }
    public bool IsAbelian { get; }
}
