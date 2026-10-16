using Wmoop.Core;

namespace Wmoop.Patterns.Registries;

/// <summary>
/// <b>Registry</b> + <b>Flyweight</b>: canonicalizes <see cref="ConceptId"/> values so
/// equal ids resolve to one shared instance, and holds L2 individuals for lookup by id.
/// A minimal, single-threaded reference implementation of the L2 instance store.
/// </summary>
public sealed class ConceptRegistry
{
    private readonly Dictionary<string, ConceptId> _ids = new(StringComparer.Ordinal);
    private readonly Dictionary<string, Entity> _entities = new(StringComparer.Ordinal);

    /// <summary>Returns a shared, canonical <see cref="ConceptId"/> for the value (Flyweight).</summary>
    public ConceptId Intern(string value)
    {
        if (!_ids.TryGetValue(value, out var id))
        {
            id = new ConceptId(value);
            _ids[value] = id;
        }
        return id;
    }

    /// <summary>Registers an entity under its id (Registry).</summary>
    public T Register<T>(T entity) where T : Entity
    {
        ArgumentNullException.ThrowIfNull(entity);
        _entities[entity.Id.Value] = entity;
        return entity;
    }

    /// <summary>Resolves a registered entity by id, or <c>null</c> if unknown.</summary>
    public Entity? Resolve(ConceptId id) =>
        _entities.TryGetValue(id.Value, out var entity) ? entity : null;

    /// <summary>Number of registered entities.</summary>
    public int Count => _entities.Count;
}
