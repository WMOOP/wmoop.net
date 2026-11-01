// Generated from module wmoop.music v0.1.0. DO NOT EDIT — regenerate with wmoop.tools.
using System;
using System.Collections.Generic;
using Wmoop.Generated.Runtime;

namespace Wmoop.Generated.Music;

public class PitchClass : Entity
{
    public const string WmId = "wmoop.music/PitchClass";
    public PitchClass(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Interval : Entity
{
    public const string WmId = "wmoop.music/Interval";
    public Interval(ConceptId id, string name, object semitones) : base(id, name)
    {
        Semitones = semitones;
    }
    public object Semitones { get; }
}

public class Scale : Entity
{
    public const string WmId = "wmoop.music/Scale";
    public Scale(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Chord : Entity
{
    public const string WmId = "wmoop.music/Chord";
    public Chord(ConceptId id, string name) : base(id, name)
    {
    }
}

public class MusicalWork : Entity
{
    public const string WmId = "wmoop.music/MusicalWork";
    public MusicalWork(ConceptId id, string name, string title) : base(id, name)
    {
        Title = title;
    }
    public string Title { get; }
}

public abstract class MusicalInstrument : Entity
{
    public const string WmId = "wmoop.music/MusicalInstrument";
    protected MusicalInstrument(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Idiophone : MusicalInstrument
{
    public new const string WmId = "wmoop.music/Idiophone";
    public Idiophone(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Membranophone : MusicalInstrument
{
    public new const string WmId = "wmoop.music/Membranophone";
    public Membranophone(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Chordophone : MusicalInstrument
{
    public new const string WmId = "wmoop.music/Chordophone";
    public Chordophone(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Aerophone : MusicalInstrument
{
    public new const string WmId = "wmoop.music/Aerophone";
    public Aerophone(ConceptId id, string name) : base(id, name)
    {
    }
}

public class Electrophone : MusicalInstrument
{
    public new const string WmId = "wmoop.music/Electrophone";
    public Electrophone(ConceptId id, string name) : base(id, name)
    {
    }
}

public class MusicalPerformance : Entity
{
    public const string WmId = "wmoop.music/MusicalPerformance";
    public MusicalPerformance(ConceptId id, string name) : base(id, name)
    {
    }
}

public class NoteOccurrence : Entity
{
    public const string WmId = "wmoop.music/NoteOccurrence";
    public NoteOccurrence(ConceptId id, string name, Quantity duration) : base(id, name)
    {
        Duration = duration;
    }
    public Quantity Duration { get; }
}

public sealed class Performer : Role
{
    public const string WmId = "wmoop.music/Performer";
    public const string WmPlayedBy = "wmoop.nature/Person";
    public Performer()
    {
    }
    public override ConceptId RoleType => new ConceptId(WmId);
}
