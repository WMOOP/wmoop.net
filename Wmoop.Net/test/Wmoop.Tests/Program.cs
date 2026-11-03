using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Wmoop.Core;

// Runs the shared conformance vectors of wmoop.doc against this binding: the vectors are written
// once (external/wmoop.doc/conformance/, format in its README.md) and wmoop.py runs the same ones.
// It reads the bundle suite.json and drives the generated classes by reflection, through their
// model ids (WmId) and the model's member names; the behaviour cases check the hand-written halves
// of the partial classes. The process exits non-zero when an active case fails, so CI can treat it
// as a gate (invariant #6).
//
//   dotnet run --project Wmoop.Net/test/Wmoop.Tests                    # every case
//   dotnet run --project Wmoop.Net/test/Wmoop.Tests -- conf.transport  # ids with a prefix

var suite = JsonNode.Parse(File.ReadAllText(Conformance.FindSuite()))!.AsObject();
if ((int?)suite["format"] != Conformance.Format)
{
    Console.WriteLine($"suite.json has format {suite["format"]}; this runner reads format {Conformance.Format}");
    return 2;
}

int passed = 0, failed = 0, pending = 0;
foreach (var node in suite["cases"]!.AsArray())
{
    var testCase = node!.AsObject();
    var id = (string)testCase["id"]!;
    if (args.Length > 0 && !args.Any(prefix => id.StartsWith(prefix, StringComparison.Ordinal)))
        continue;

    string? problem = null;
    try { new CaseRun(testCase).Run(); }
    catch (Mismatch m) { problem = m.Message; }
    catch (Exception ex) { problem = $"{ex.GetType().Name}: {ex.Message}"; } // one crash must not hide the others

    if ((string?)testCase["status"] == "pending")
    {
        pending++;
        Console.WriteLine($"[PENDING] {id}: {(problem is null ? "passes now: make it active in wmoop.doc" : (string)testCase["reason"]!)}");
    }
    else if (problem is null) { passed++; Console.WriteLine($"[PASS] {id}"); }
    else { failed++; Console.WriteLine($"[FAIL] {id}: {problem}"); }
}

Console.WriteLine($"\n{passed} passed, {failed} failed, {pending} pending.");
return failed == 0 ? 0 : 1;

/// <summary>The binding did not do what the case says.</summary>
internal sealed class Mismatch(string message) : Exception(message);

/// <summary>
/// A rule C# enforces before any binding code runs (a <c>required</c> member, an abstract class),
/// reported with the model's code for it.
/// </summary>
internal sealed class LanguageRule(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}

internal static class Conformance
{
    public const int Format = 1;

    public static readonly JsonSerializerOptions Json = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

    /// <summary>external/wmoop.doc/conformance/suite.json, above the working directory or this program.</summary>
    public static string FindSuite()
    {
        foreach (var start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
            for (var dir = new DirectoryInfo(start); dir is not null; dir = dir.Parent)
            {
                var path = Path.Combine(dir.FullName, "external", "wmoop.doc", "conformance", "suite.json");
                if (File.Exists(path))
                    return path;
            }
        throw new FileNotFoundException("external/wmoop.doc/conformance/suite.json not found (run: git submodule update --init)");
    }
}

/// <summary>The generated types by model id, and their members by model name.</summary>
internal static class Model
{
    private static readonly Lazy<Dictionary<string, Type>> Types = new(Index);

    /// <summary>The generated type whose <c>WmId</c> is <paramref name="id"/>: resolved through metadata, not a naming rule.</summary>
    public static Type Type(string id) =>
        Types.Value.TryGetValue(id, out var type) ? type : throw new Mismatch($"no generated type has WmId {id}");

    public static string? WmId(Type type) =>
        type.GetField("WmId", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly) is { IsLiteral: true } field
            ? field.GetRawConstantValue() as string
            : null;

    /// <summary>The model types of <paramref name="obj"/>, its own type first.</summary>
    public static List<string> Chain(object obj)
    {
        var chain = new List<string>();
        for (var t = obj.GetType(); t is not null; t = t.BaseType)
            if (WmId(t) is { } id)
                chain.Add(id);
        return chain;
    }

    /// <summary>The public property that is the model member <paramref name="name"/>: equal ignoring case and underscores.</summary>
    public static PropertyInfo Property(Type type, string name)
    {
        var found = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => Key(p.Name) == Key(name))
            .OrderByDescending(p => Depth(p.DeclaringType)) // a member hidden with `new`: the most derived one
            .ToList();
        if (found.Count == 0 || found.Select(p => p.Name).Distinct().Count() > 1)
            throw new Mismatch($"{type.Name} has {(found.Count == 0 ? "no" : "several")} member(s) for '{name}'");
        return found[0];
    }

    public static MethodInfo Method(Type type, string name, int arity)
    {
        var found = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => Key(m.Name) == Key(name) && m.GetParameters().Length == arity)
            .ToList();
        return found.Count == 1 ? found[0] : throw new Mismatch($"{type.Name} has {(found.Count == 0 ? "no" : "several")} method(s) {name}/{arity}");
    }

    private static string Key(string name) => name.Replace("_", "", StringComparison.Ordinal).ToLowerInvariant();

    private static int Depth(Type? type)
    {
        var depth = 0;
        for (; type is not null; type = type.BaseType)
            depth++;
        return depth;
    }

    private static Dictionary<string, Type> Index()
    {
        // Every generated project is referenced, so its assembly sits next to this program.
        foreach (var dll in Directory.GetFiles(AppContext.BaseDirectory, "Wmoop.*.dll"))
            Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(dll)));
        var index = new Dictionary<string, Type>(StringComparer.Ordinal);
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            foreach (var type in assembly.GetTypes())
                if (WmId(type) is { } id)
                    index[id] = type;
        return index;
    }
}

/// <summary>Converts between the vectors' value encoding and this binding's own types.</summary>
internal static class Values
{
    /// <summary>A vector value as an instance of <paramref name="target"/> (<c>object</c>: inferred from its shape).</summary>
    public static object? In(JsonNode? node, Type target, Dictionary<string, object> names)
    {
        var underlying = Nullable.GetUnderlyingType(target);
        if (node is null)
            return target.IsValueType && underlying is null ? throw new Mismatch($"null is not a {target.Name}") : null;
        target = underlying ?? target;

        if (node is JsonObject obj)
        {
            if (obj["ref"] is { } name)
                return names.TryGetValue((string)name!, out var bound) ? bound : throw new Mismatch($"'{name}' is not bound");
            if (obj["date"] is { } date)
                return DateOnly.ParseExact((string)date!, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (obj.ContainsKey("unit"))
                return new Quantity(Number(obj["value"]!.AsValue()), (string)obj["unit"]!);
            if (obj["id"] is { } id)
                return new ConceptId((string)id!);
            throw new Mismatch($"not a value: {obj.ToJsonString()}");
        }

        if (node is JsonArray array)
        {
            var item = ItemType(target);
            var items = Array.CreateInstance(item, array.Count);
            for (var i = 0; i < array.Count; i++)
                items.SetValue(In(array[i], item, names), i);
            return items;
        }

        var value = node.AsValue();
        switch (value.GetValueKind())
        {
            case JsonValueKind.True or JsonValueKind.False:
                return value.GetValue<bool>();
            case JsonValueKind.String when target.IsEnum:
                // C# cannot even write a value outside an enum, so that is the language's rule.
                return Enum.GetNames(target).Contains((string)value!)
                    ? Enum.Parse(target, (string)value!)
                    : throw new LanguageRule("WM-CONSTRAINT-ENUM", $"'{value}' is not a {target.Name}");
            case JsonValueKind.String:
                return (string)value!;
            case JsonValueKind.Number when target == typeof(object):
                return value.ToJsonString().Contains('.') ? Number(value) : value.GetValue<long>();
            case JsonValueKind.Number:
                return Convert.ChangeType(target == typeof(double) || target == typeof(decimal) ? Number(value) : value.GetValue<long>(),
                    target, CultureInfo.InvariantCulture);
            default:
                throw new Mismatch($"not a value: {value.ToJsonString()}");
        }
    }

    /// <summary>A result in the vectors' encoding, so it can be compared with <c>equals</c>.</summary>
    public static JsonNode? Out(object? value, Dictionary<string, object> names) => value switch
    {
        null => null,
        bool b => JsonValue.Create(b),
        string s => JsonValue.Create(s),
        Enum e => JsonValue.Create(e.ToString()),
        DateOnly d => new JsonObject { ["date"] = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) },
        Quantity q => new JsonObject { ["value"] = q.Value, ["unit"] = q.Unit },
        ConceptId id => new JsonObject { ["id"] = id.Value },
        Entity or IRole => new JsonObject { ["ref"] = names.FirstOrDefault(n => ReferenceEquals(n.Value, value)).Key ?? throw new Mismatch($"a query returned {value}, which no step bound") },
        sbyte or byte or short or ushort or int or uint or long => JsonValue.Create(Convert.ToInt64(value, CultureInfo.InvariantCulture)),
        float or double or decimal => JsonValue.Create(Convert.ToDouble(value, CultureInfo.InvariantCulture)),
        IEnumerable items => new JsonArray(items.Cast<object?>().Select(item => Out(item, names)).ToArray()),
        _ => throw new Mismatch($"cannot compare a {value.GetType().Name}"),
    };

    /// <summary>Equality of two vector values: numbers by value, but a boolean is never a number.</summary>
    public static bool Same(JsonNode? a, JsonNode? b) => (a, b) switch
    {
        (null, null) => true,
        (JsonArray x, JsonArray y) => x.Count == y.Count && x.Zip(y).All(p => Same(p.First, p.Second)),
        (JsonObject x, JsonObject y) => x.Count == y.Count && x.All(p => y.ContainsKey(p.Key) && Same(p.Value, y[p.Key])),
        (JsonValue x, JsonValue y) when x.GetValueKind() == JsonValueKind.Number && y.GetValueKind() == JsonValueKind.Number =>
            Number(x) == Number(y),
        (JsonValue x, JsonValue y) => x.GetValueKind() == y.GetValueKind() && x.ToJsonString() == y.ToJsonString(),
        _ => false,
    };

    public static string Show(JsonNode? node) => node?.ToJsonString(Conformance.Json) ?? "null";

    private static double Number(JsonValue value) => double.Parse(value.ToJsonString(), NumberStyles.Float, CultureInfo.InvariantCulture);

    private static Type ItemType(Type target) =>
        target.IsArray ? target.GetElementType()!
        : target.IsGenericType && typeof(IEnumerable).IsAssignableFrom(target) ? target.GetGenericArguments()[0]
        : typeof(object);
}

/// <summary>One case: its steps on a fresh set of names, then its outcome.</summary>
internal sealed class CaseRun(JsonObject testCase)
{
    private readonly Dictionary<string, object> _names = new(StringComparer.Ordinal);

    public void Run()
    {
        var steps = testCase["given"]?.AsArray() ?? new JsonArray();
        var expect = testCase["expect"]!;
        var fails = expect is JsonObject error ? (string)error["error_code"]! : null;
        for (var i = 0; i < steps.Count; i++)
        {
            var (kind, args) = Single(steps[i]!.AsObject());
            var where = $"given[{i + 1}].{kind}";
            try
            {
                Step(kind, args);
            }
            catch (Exception ex)
            {
                var inner = Unwrap(ex);
                if (inner is Mismatch mismatch)
                    throw new Mismatch($"{where}: {mismatch.Message}");
                var code = inner switch { IWmoopError w => w.Code, LanguageRule r => r.Code, _ => null };
                if (code is null)
                    throw new Mismatch($"{where} threw {inner.GetType().Name} without an error code: {inner.Message}");
                if (fails is not null && i == steps.Count - 1)
                {
                    if (code != fails)
                        throw new Mismatch($"{where} failed with {code}, expected {fails}: {inner.Message}");
                    return;
                }
                throw new Mismatch($"{where} failed with {code}: {inner.Message}");
            }
        }
        if (fails is not null)
            throw new Mismatch($"every step succeeded, expected {fails}");
        var assertions = expect.AsArray();
        for (var j = 0; j < assertions.Count; j++)
            Check(assertions[j]!.AsObject(), $"expect[{j + 1}]");
    }

    private static (string Key, JsonNode Args) Single(JsonObject obj)
    {
        var (key, value) = obj.Single(p => p.Key != "equals");
        return (key, value!);
    }

    private static Exception Unwrap(Exception ex)
    {
        while (ex is TargetInvocationException { InnerException: { } inner })
            ex = inner;
        return ex;
    }

    private object Named(JsonNode? name) =>
        _names.TryGetValue((string)name!, out var obj) ? obj : throw new Mismatch($"'{name}' is not bound");

    private void Bind(JsonNode? name, object obj)
    {
        if (name is not null)
            _names[(string)name!] = obj;
    }

    // -- steps -------------------------------------------------------------------------------
    private void Step(string kind, JsonNode args)
    {
        var a = args.AsObject();
        switch (kind)
        {
            case "construct": Construct(a); break;
            case "attach_role": AttachRole(a); break;
            case "link": Link(a, add: true); break;
            case "unlink": Link(a, add: false); break;
            case "assert": Check(a, "assert"); break;
            default: throw new Mismatch($"unknown step {kind}");
        }
    }

    private void Construct(JsonObject a)
    {
        var typeId = (string)a["type"]!;
        var type = Model.Type(typeId);
        object obj;
        if (typeof(Role).IsAssignableFrom(type))
            obj = Build(type, a["with"]?.AsObject(), identity: null);
        else
        {
            var fallback = (string?)a["bind"] ?? typeId[(typeId.IndexOf('/') + 1)..];
            obj = Build(type, a["with"]?.AsObject(), ((string?)a["id"] ?? $"conformance/{fallback}", (string?)a["name"] ?? fallback));
        }
        Bind(a["bind"], obj);
    }

    private void AttachRole(JsonObject a)
    {
        var subject = (Entity)Named(a["to"]);
        object role;
        if (a["ref"] is { } name)
            role = Named(name);
        else
        {
            role = Build(Model.Type((string)a["role"]!), a["with"]?.AsObject(), identity: null);
            Bind(a["bind"], role);
        }
        typeof(Entity).GetMethod(nameof(Entity.AddRole))!.MakeGenericMethod(role.GetType()).Invoke(subject, [role]);
    }

    /// <summary>
    /// Builds an instance from the case's model properties. C# checks <c>required</c> members and
    /// abstract classes at compile time, so the runner reports those from the declarations.
    /// </summary>
    private object Build(Type type, JsonObject? given, (string Id, string Name)? identity)
    {
        if (type.IsAbstract)
            throw new LanguageRule("WM-ABSTRACT-INSTANTIATION", $"{type.Name} is abstract, so C# cannot construct it");
        var values = new List<(PropertyInfo Property, object? Value)>();
        if (identity is var (id, name))
        {
            values.Add((Model.Property(type, "id"), new ConceptId(id)));
            values.Add((Model.Property(type, "name"), name));
        }
        foreach (var (member, value) in given ?? new JsonObject())
        {
            var property = Model.Property(type, member);
            values.Add((property, Values.In(value, property.PropertyType, _names)));
        }
        var missing = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.IsDefined(typeof(RequiredMemberAttribute)) && values.All(v => v.Property.Name != p.Name))
            .Select(p => p.Name)
            .ToList();
        if (missing.Count > 0)
            throw new LanguageRule("WM-REQUIRED-MISSING", $"{type.Name} requires {string.Join(", ", missing)}");
        var obj = Activator.CreateInstance(type)!;
        foreach (var (property, value) in values)
            property.SetValue(obj, value); // an init accessor runs its validation here
        return obj;
    }

    private void Link(JsonObject a, bool add)
    {
        var obj = Named(a["subject"]);
        var other = Named(a[add ? "to" : "from"]);
        var end = Model.Property(obj.GetType(), (string)a["relation"]!);
        if (end.PropertyType.IsGenericType && end.PropertyType.GetGenericTypeDefinition() == typeof(Links<>))
            end.PropertyType.GetMethod(add ? "Add" : "Remove")!.Invoke(end.GetValue(obj), [other]);
        else if (add)
            end.SetValue(obj, other);
        else if (ReferenceEquals(end.GetValue(obj), other))
            end.SetValue(obj, null);
    }

    // -- queries -----------------------------------------------------------------------------
    private void Check(JsonObject assertion, string where)
    {
        var (query, args) = Single(assertion);
        var got = Values.Out(Query(query, args), _names);
        if (!Values.Same(got, assertion["equals"]))
            throw new Mismatch($"{where}: {query} returned {Values.Show(got)}, expected {Values.Show(assertion["equals"])}");
    }

    private object? Query(string query, JsonNode args)
    {
        if (query == "same_value")
        {
            var pair = args.AsArray();
            var (a, b) = (Values.In(pair[0], typeof(object), _names), Values.In(pair[1], typeof(object), _names));
            return Equals(a, b) && a?.GetHashCode() == b?.GetHashCode();
        }
        var q = args.AsObject();
        if (query == "role_subject")
            return ((Role)Named(q["role"])).Subject;
        if (query == "text" && q.ContainsKey("value"))
            return Values.In(q["value"], typeof(object), _names)?.ToString();

        var subject = Named(q["subject"]);
        return query switch
        {
            "type_chain" => Model.Chain(subject),
            "is_a" => ((Entity)subject).IsA((string)q["type"]!),
            "get" => Model.Property(subject.GetType(), (string)q["property"]!).GetValue(subject),
            "call" => Call(subject, (string)q["operation"]!, q["args"]?.AsArray() ?? new JsonArray()),
            "text" => (q.ContainsKey("property") ? Model.Property(subject.GetType(), (string)q["property"]!).GetValue(subject) : subject)?.ToString(),
            "roles" => ((Entity)subject).Roles.Select(r => r.RoleType.Value).ToList(),
            "has_role" => RoleQuery(nameof(Entity.HasRole), subject, q),
            "get_role" => RoleQuery(nameof(Entity.GetRole), subject, q),
            _ => throw new Mismatch($"this binding does not support {query} yet"),
        };
    }

    private object? Call(object subject, string operation, JsonArray args)
    {
        var method = Model.Method(subject.GetType(), operation, args.Count);
        var parameters = method.GetParameters();
        var values = args.Select((arg, i) => Values.In(arg, parameters[i].ParameterType, _names)).ToArray();
        return method.Invoke(subject, values);
    }

    private static object? RoleQuery(string method, object subject, JsonObject q) =>
        typeof(Entity).GetMethod(method)!.MakeGenericMethod(Model.Type((string)q["role"]!)).Invoke(subject, null);
}
