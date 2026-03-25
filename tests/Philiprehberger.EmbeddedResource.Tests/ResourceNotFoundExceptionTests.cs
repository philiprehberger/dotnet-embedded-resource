using Xunit;
namespace Philiprehberger.EmbeddedResource.Tests;

public class ResourceNotFoundExceptionTests
{
    [Fact]
    public void Constructor_SetsResourceName()
    {
        var exception = new ResourceNotFoundException("test.txt", Array.Empty<string>());

        Assert.Equal("test.txt", exception.ResourceName);
    }

    [Fact]
    public void Constructor_SetsAvailableResources()
    {
        var available = new[] { "file1.txt", "file2.txt" };

        var exception = new ResourceNotFoundException("missing.txt", available);

        Assert.Equal(available, exception.AvailableResources);
    }

    [Fact]
    public void Message_NoAvailableResources_IndicatesEmpty()
    {
        var exception = new ResourceNotFoundException("test.txt", Array.Empty<string>());

        Assert.Contains("test.txt", exception.Message);
        Assert.Contains("No embedded resources are available", exception.Message);
    }

    [Fact]
    public void Message_WithAvailableResources_ListsThem()
    {
        var available = new[] { "resource1.txt", "resource2.html" };

        var exception = new ResourceNotFoundException("missing.txt", available);

        Assert.Contains("missing.txt", exception.Message);
        Assert.Contains("resource1.txt", exception.Message);
        Assert.Contains("resource2.html", exception.Message);
    }

    [Fact]
    public void IsException_DerivedFromException()
    {
        var exception = new ResourceNotFoundException("test.txt", Array.Empty<string>());

        Assert.IsAssignableFrom<Exception>(exception);
    }
}
