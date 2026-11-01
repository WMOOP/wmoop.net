// Generated from module wmoop.org v0.1.0. DO NOT EDIT — regenerate with wmoop.tools.
using System;
using System.Collections.Generic;
using Wmoop.Generated.Runtime;

namespace Wmoop.Generated.Org;

public class Organization : Entity
{
    public const string WmId = "wmoop.org/Organization";
    public Organization(ConceptId id, string name, string legalName, DateOnly foundingDate) : base(id, name)
    {
        LegalName = legalName;
        FoundingDate = foundingDate;
    }
    public string LegalName { get; }
    public DateOnly FoundingDate { get; }
}

public sealed class Employee : Role
{
    public const string WmId = "wmoop.org/Employee";
    public const string WmPlayedBy = "wmoop.nature/Person";
    public Employee(string employeeId, DateOnly hiredOn)
    {
        EmployeeId = employeeId;
        HiredOn = hiredOn;
    }
    public override ConceptId RoleType => new ConceptId(WmId);
    public string EmployeeId { get; }
    public DateOnly HiredOn { get; }
}
