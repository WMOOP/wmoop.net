// Generated from module wmoop.education v0.1.0. DO NOT EDIT — regenerate with wmoop.tools.
using System;
using System.Collections.Generic;
using Wmoop.Generated.Runtime;
using Wmoop.Generated.Architecture;
using Wmoop.Generated.Org;

namespace Wmoop.Generated.Education;

public enum ISCEDLevel { EARLY_CHILDHOOD, PRIMARY, LOWER_SECONDARY, UPPER_SECONDARY, POST_SECONDARY_NON_TERTIARY, SHORT_CYCLE_TERTIARY, BACHELOR_OR_EQUIVALENT, MASTER_OR_EQUIVALENT, DOCTORAL_OR_EQUIVALENT }

public enum GradingScale { ROMANIAN_1_10, PERCENTAGE_0_100, GPA_0_4 }

public abstract class EducationalInstitution : Organization
{
    public new const string WmId = "wmoop.education/EducationalInstitution";
    protected EducationalInstitution(ConceptId id, string name, string legalName, DateOnly foundingDate, IReadOnlyList<ISCEDLevel> iscedLevels) : base(id, name, legalName, foundingDate)
    {
        IscedLevels = iscedLevels;
    }
    public IReadOnlyList<ISCEDLevel> IscedLevels { get; }
}

public class School : EducationalInstitution
{
    public new const string WmId = "wmoop.education/School";
    public School(ConceptId id, string name, string legalName, DateOnly foundingDate, IReadOnlyList<ISCEDLevel> iscedLevels) : base(id, name, legalName, foundingDate, iscedLevels)
    {
    }
}

public class SchoolBuilding : Building
{
    public new const string WmId = "wmoop.education/SchoolBuilding";
    public SchoolBuilding(ConceptId id, string name, int floorCount) : base(id, name, floorCount)
    {
    }
}

public class Classroom : Room
{
    public new const string WmId = "wmoop.education/Classroom";
    public Classroom(ConceptId id, string name, Quantity floorArea, object seatingCapacity) : base(id, name, floorArea)
    {
        SeatingCapacity = seatingCapacity;
    }
    public object SeatingCapacity { get; }
}

public class Laboratory : Room
{
    public new const string WmId = "wmoop.education/Laboratory";
    public Laboratory(ConceptId id, string name, Quantity floorArea, string discipline) : base(id, name, floorArea)
    {
        Discipline = discipline;
    }
    public string Discipline { get; }
}

public class CurriculumSubject : Entity
{
    public const string WmId = "wmoop.education/CurriculumSubject";
    public CurriculumSubject(ConceptId id, string name, string iscedfField) : base(id, name)
    {
        IscedfField = iscedfField;
    }
    public string IscedfField { get; }
}

public class Mark : Entity
{
    public const string WmId = "wmoop.education/Mark";
    public Mark(ConceptId id, string name, object value, GradingScale scale, DateOnly awardedOn) : base(id, name)
    {
        Value = value;
        Scale = scale;
        AwardedOn = awardedOn;
    }
    public object Value { get; }
    public GradingScale Scale { get; }
    public DateOnly AwardedOn { get; }
}

public class StudentClass : Entity
{
    public const string WmId = "wmoop.education/StudentClass";
    public StudentClass(ConceptId id, string name, string label, object studyYear) : base(id, name)
    {
        Label = label;
        StudyYear = studyYear;
    }
    public string Label { get; }
    public object StudyYear { get; }
}

public sealed class Student : Role
{
    public const string WmId = "wmoop.education/Student";
    public const string WmPlayedBy = "wmoop.nature/Person";
    public Student(string enrolmentId, object studyYear)
    {
        EnrolmentId = enrolmentId;
        StudyYear = studyYear;
    }
    public override ConceptId RoleType => new ConceptId(WmId);
    public string EnrolmentId { get; }
    public object StudyYear { get; }
}

public sealed class Teacher : Role
{
    public const string WmId = "wmoop.education/Teacher";
    public const string WmPlayedBy = "wmoop.nature/Person";
    public Teacher(string staffId)
    {
        StaffId = staffId;
    }
    public override ConceptId RoleType => new ConceptId(WmId);
    public string StaffId { get; }
}

public sealed class FormTutor : Role
{
    public const string WmId = "wmoop.education/FormTutor";
    public const string WmPlayedBy = "wmoop.nature/Person";
    public FormTutor()
    {
    }
    public override ConceptId RoleType => new ConceptId(WmId);
}
