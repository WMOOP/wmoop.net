using Wmoop.Geography;

namespace Wmoop.Patterns.Strategies;

/// <summary><b>Strategy</b> pattern: a swappable algorithm for formatting an area label.</summary>
public interface IAreaFormatStrategy
{
    string Format(AdministrativeArea area);
}

/// <summary>Compact one-line label — a concrete <b>Strategy</b>.</summary>
public sealed class CompactAreaFormat : IAreaFormatStrategy
{
    public string Format(AdministrativeArea area) => $"{area.Name} ({area.AreaKind})";
}

/// <summary>Census-style label with aggregate population — a concrete <b>Strategy</b>.</summary>
public sealed class CensusAreaFormat : IAreaFormatStrategy
{
    public string Format(AdministrativeArea area) =>
        $"{area.Name} [{area.AreaKind}] pop={area.TotalPopulation:N0} across {area.Children.Count} sub-area(s)";
}
