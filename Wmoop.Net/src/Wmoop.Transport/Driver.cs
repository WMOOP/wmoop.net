using System;
using System.Linq;

namespace Wmoop.Transport;

/// <summary>The hand-written half of the generated <c>Driver</c> role: what the licence allows.</summary>
public partial class Driver
{
    /// <summary>
    /// True when the licence covers <paramref name="category"/> and has not expired on
    /// <paramref name="on"/>. The model records no issue date, and a licence whose expiry is not
    /// recorded counts as valid.
    /// </summary>
    public bool CanDrive(LicenceCategory category, DateOnly on) =>
        LicenceCategories.Contains(category) && (LicenceExpiry is not { } expiry || on <= expiry);
}
