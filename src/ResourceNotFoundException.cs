namespace Philiprehberger.EmbeddedResource;

/// <summary>
/// Exception thrown when an embedded resource cannot be found.
/// Includes the list of available resource names in the message for debugging.
/// </summary>
public sealed class ResourceNotFoundException : Exception
{
    /// <summary>
    /// Gets the name of the resource that was not found.
    /// </summary>
    public string ResourceName { get; }

    /// <summary>
    /// Gets the list of available resource names in the assembly.
    /// </summary>
    public IReadOnlyList<string> AvailableResources { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceNotFoundException"/> class.
    /// </summary>
    /// <param name="resourceName">The name of the resource that was not found.</param>
    /// <param name="availableResources">The list of available resource names.</param>
    public ResourceNotFoundException(string resourceName, IReadOnlyList<string> availableResources)
        : base(BuildMessage(resourceName, availableResources))
    {
        ResourceName = resourceName;
        AvailableResources = availableResources;
    }

    private static string BuildMessage(string resourceName, IReadOnlyList<string> availableResources)
    {
        var message = $"Embedded resource '{resourceName}' was not found.";

        if (availableResources.Count == 0)
        {
            return message + " No embedded resources are available in the assembly.";
        }

        var resourceList = string.Join(Environment.NewLine + "  - ", availableResources);
        return message + $" Available resources:{Environment.NewLine}  - {resourceList}";
    }
}
