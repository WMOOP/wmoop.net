using Wmoop.Core;

namespace Wmoop.Transport;

/// <summary>
/// WM cross-domain concept <c>wm:transport/role/Driver</c> — the Transport facet of
/// a <see cref="Wmoop.Nature.Person"/>.
/// <para>
/// This is the crux of the architecture. A driver is a person who <em>currently
/// holds a driving licence</em>. That is contextual, temporary and non-exclusive,
/// so it is modelled as a <see cref="Role"/> attached by composition — never as
/// <c>Driver : Person</c>. The role lives in the Transport domain (which may depend
/// on Nature), so biology stays free of any knowledge of licences.
/// </para>
/// </summary>
public sealed class DriverRole : Role
{
    /// <summary>Stable identifier of the Driver role type.</summary>
    public static readonly ConceptId TypeId = new("wm:transport/role/Driver");

    public DriverRole(
        string licenceNumber,
        IEnumerable<string> categories,
        DateOnly issueDate,
        DateOnly expiryDate)
    {
        if (string.IsNullOrWhiteSpace(licenceNumber))
            throw new ArgumentException("A driver role requires a licence number.", nameof(licenceNumber));
        if (expiryDate < issueDate)
            throw new ArgumentException("Expiry date cannot precede issue date.", nameof(expiryDate));

        LicenceNumber = licenceNumber;
        Categories = categories?.ToArray() ?? throw new ArgumentNullException(nameof(categories));
        IssueDate = issueDate;
        ExpiryDate = expiryDate;
    }

    /// <inheritdoc />
    public override ConceptId RoleType => TypeId;

    /// <summary>The licence number (meta carried by the role, not by the person).</summary>
    public string LicenceNumber { get; }

    /// <summary>Licence categories held, e.g. <c>A1</c>, <c>AM</c>, <c>B</c>.</summary>
    public IReadOnlyList<string> Categories { get; }

    public DateOnly IssueDate { get; }
    public DateOnly ExpiryDate { get; }

    /// <summary>
    /// True if, on the given date, the holder is licensed for <paramref name="category"/>
    /// and the licence is within its validity window.
    /// </summary>
    public bool CanDrive(string category, DateOnly on) =>
        on >= IssueDate &&
        on <= ExpiryDate &&
        Categories.Contains(category, StringComparer.OrdinalIgnoreCase);
}
