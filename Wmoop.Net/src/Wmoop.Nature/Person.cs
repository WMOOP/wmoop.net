using System;

namespace Wmoop.Nature;

/// <summary>The hand-written half of the generated <c>Person</c>.</summary>
public partial class Person
{
    /// <summary>Age in whole years on <paramref name="on"/>, or null when the date of birth is not recorded.</summary>
    public int? AgeOn(DateOnly on)
    {
        if (DateOfBirth is not { } born)
            return null;
        var age = on.Year - born.Year;
        return on < born.AddYears(age) ? age - 1 : age;
    }
}
