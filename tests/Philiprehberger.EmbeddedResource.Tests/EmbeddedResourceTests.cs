using Xunit;
using System.Reflection;

namespace Philiprehberger.EmbeddedResource.Tests;

public class EmbeddedResourceTests
{
    [Fact]
    public void List_NoPattern_ReturnsAllResources()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var resources = EmbeddedResource.List(assembly: assembly);

        Assert.NotNull(resources);
        Assert.IsType<string[]>(resources);
    }

    [Fact]
    public void List_WithPattern_FiltersResults()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var resources = EmbeddedResource.List("*.nonexistent", assembly);

        Assert.Empty(resources);
    }

    [Fact]
    public void ReadString_NonExistentResource_ThrowsResourceNotFoundException()
    {
        var assembly = Assembly.GetExecutingAssembly();

        Assert.Throws<ResourceNotFoundException>(
            () => EmbeddedResource.ReadString("nonexistent.resource", assembly));
    }

    [Fact]
    public void ReadBytes_NonExistentResource_ThrowsResourceNotFoundException()
    {
        var assembly = Assembly.GetExecutingAssembly();

        Assert.Throws<ResourceNotFoundException>(
            () => EmbeddedResource.ReadBytes("nonexistent.resource", assembly));
    }

    [Fact]
    public void OpenStream_NonExistentResource_ThrowsResourceNotFoundException()
    {
        var assembly = Assembly.GetExecutingAssembly();

        Assert.Throws<ResourceNotFoundException>(
            () => EmbeddedResource.OpenStream("nonexistent.resource", assembly));
    }

    [Fact]
    public void ReadJson_NonExistentResource_ThrowsResourceNotFoundException()
    {
        var assembly = Assembly.GetExecutingAssembly();

        Assert.Throws<ResourceNotFoundException>(
            () => EmbeddedResource.ReadJson<object>("nonexistent.resource", assembly));
    }
}
