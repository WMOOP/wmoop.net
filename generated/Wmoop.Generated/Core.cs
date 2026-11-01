// Generated from module wmoop.core v1.1.0. DO NOT EDIT — regenerate with wmoop.tools.
using System;
using System.Collections.Generic;
using Wmoop.Generated.Runtime;

namespace Wmoop.Generated.Core;

public enum ProvenanceKind { sourced, definitional, convention }

public class Provenance : Entity
{
    public const string WmId = "wmoop.core/Provenance";
    public Provenance(ConceptId id, string name, string source, ProvenanceKind kind, string @ref, string url, string note) : base(id, name)
    {
        Source = source;
        Kind = kind;
        Ref = @ref;
        Url = url;
        Note = note;
    }
    public string Source { get; }
    public ProvenanceKind Kind { get; }
    public string Ref { get; }
    public string Url { get; }
    public string Note { get; }
}

public class QuantityKind : Entity
{
    public const string WmId = "wmoop.core/QuantityKind";
    public QuantityKind(ConceptId id, string name, object dimL, object dimM, object dimT, object dimI, object dimTheta, object dimN, object dimJ, object canonicalUnit, string derivation) : base(id, name)
    {
        DimL = dimL;
        DimM = dimM;
        DimT = dimT;
        DimI = dimI;
        DimTheta = dimTheta;
        DimN = dimN;
        DimJ = dimJ;
        CanonicalUnit = canonicalUnit;
        Derivation = derivation;
    }
    public object DimL { get; }
    public object DimM { get; }
    public object DimT { get; }
    public object DimI { get; }
    public object DimTheta { get; }
    public object DimN { get; }
    public object DimJ { get; }
    public object CanonicalUnit { get; }
    public string Derivation { get; }
}

public class Unit : Entity
{
    public const string WmId = "wmoop.core/Unit";
    public Unit(ConceptId id, string name, string symbol, object quantityKind, object factor, object offset) : base(id, name)
    {
        Symbol = symbol;
        QuantityKind = quantityKind;
        Factor = factor;
        Offset = offset;
    }
    public string Symbol { get; }
    public object QuantityKind { get; }
    public object Factor { get; }
    public object Offset { get; }
}
