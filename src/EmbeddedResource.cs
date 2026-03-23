using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Philiprehberger.EmbeddedResource;

/// <summary>
/// Provides fluent access to embedded resources with support for reading as string,
/// byte array, stream, and JSON deserialization.
/// </summary>
public static class EmbeddedResource
{
    /// <summary>
    /// Reads an embedded resource as a string.
    /// </summary>
    /// <param name="name">The resource name. Supports exact match or suffix match.</param>
    /// <param name="assembly">The assembly to search. Defaults to the calling assembly.</param>
    /// <returns>The resource content as a string.</returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the resource is not found.</exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string ReadString(string name, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        using var stream = ResolveStream(name, assembly);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Reads an embedded resource as a byte array.
    /// </summary>
    /// <param name="name">The resource name. Supports exact match or suffix match.</param>
    /// <param name="assembly">The assembly to search. Defaults to the calling assembly.</param>
    /// <returns>The resource content as a byte array.</returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the resource is not found.</exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static byte[] ReadBytes(string name, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        using var stream = ResolveStream(name, assembly);
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// Opens an embedded resource as a readable stream.
    /// The caller is responsible for disposing the returned stream.
    /// </summary>
    /// <param name="name">The resource name. Supports exact match or suffix match.</param>
    /// <param name="assembly">The assembly to search. Defaults to the calling assembly.</param>
    /// <returns>A readable stream for the resource.</returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the resource is not found.</exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static Stream OpenStream(string name, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        return ResolveStream(name, assembly);
    }

    /// <summary>
    /// Reads and deserializes a JSON embedded resource.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="name">The resource name. Supports exact match or suffix match.</param>
    /// <param name="assembly">The assembly to search. Defaults to the calling assembly.</param>
    /// <returns>The deserialized object.</returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the resource is not found.</exception>
    /// <exception cref="JsonException">Thrown when JSON deserialization fails.</exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static T? ReadJson<T>(string name, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        using var stream = ResolveStream(name, assembly);
        return JsonSerializer.Deserialize<T>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    /// <summary>
    /// Lists available embedded resource names in the specified assembly.
    /// </summary>
    /// <param name="pattern">
    /// An optional glob-like pattern to filter resource names (e.g., "*.html").
    /// Supports * as a wildcard. When null, all resources are returned.
    /// </param>
    /// <param name="assembly">The assembly to search. Defaults to the calling assembly.</param>
    /// <returns>An array of matching resource names.</returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string[] List(string? pattern = null, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        var names = assembly.GetManifestResourceNames();

        if (string.IsNullOrWhiteSpace(pattern))
        {
            return names;
        }

        var regexPattern = "^" + Regex.Escape(pattern)
            .Replace("\\*", ".*")
            .Replace("\\?", ".") + "$";

        var regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
        return names.Where(n => regex.IsMatch(n)).ToArray();
    }

    private static Stream ResolveStream(string name, Assembly assembly)
    {
        var resourceNames = assembly.GetManifestResourceNames();

        // Try exact match first
        var stream = assembly.GetManifestResourceStream(name);
        if (stream is not null)
        {
            return stream;
        }

        // Try suffix match: normalize separators to dots for comparison
        var normalizedName = name.Replace('/', '.').Replace('\\', '.');

        var match = resourceNames.FirstOrDefault(r =>
            r.EndsWith(normalizedName, StringComparison.OrdinalIgnoreCase));

        if (match is not null)
        {
            stream = assembly.GetManifestResourceStream(match);
            if (stream is not null)
            {
                return stream;
            }
        }

        throw new ResourceNotFoundException(name, resourceNames);
    }
}
