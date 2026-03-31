# Philiprehberger.EmbeddedResource

[![CI](https://github.com/philiprehberger/dotnet-embedded-resource/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-embedded-resource/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.EmbeddedResource.svg)](https://www.nuget.org/packages/Philiprehberger.EmbeddedResource)
[![Last updated](https://img.shields.io/github/last-commit/philiprehberger/dotnet-embedded-resource)](https://github.com/philiprehberger/dotnet-embedded-resource/commits/main)

Fluent access to embedded resources with string, stream, byte array, and JSON deserialization support.

## Installation

```bash
dotnet add package Philiprehberger.EmbeddedResource
```

## Usage

### Reading Resources

```csharp
using Philiprehberger.EmbeddedResource;

// Read as string
string html = EmbeddedResource.ReadString("Templates/welcome.html");

// Read as bytes
byte[] data = EmbeddedResource.ReadBytes("Assets/logo.png");

// Open as stream
using var stream = EmbeddedResource.OpenStream("Data/config.xml");
```

### JSON Deserialization

```csharp
using Philiprehberger.EmbeddedResource;

// Deserialize a JSON resource directly into a typed object
var settings = EmbeddedResource.ReadJson<AppSettings>("Config/defaults.json");

Console.WriteLine(settings.Theme);   // "dark"
Console.WriteLine(settings.MaxRetries); // 3
```

### Listing and Filtering Resources

```csharp
using Philiprehberger.EmbeddedResource;

// List all embedded resource names in the calling assembly
string[] all = EmbeddedResource.List();

// Filter by pattern
string[] templates = EmbeddedResource.List("*.html");

// Read from a specific assembly
var assembly = typeof(MyPlugin).Assembly;
string text = EmbeddedResource.ReadString("plugin-config.json", assembly);
```

## API

### EmbeddedResource

| Method | Description |
|--------|-------------|
| `ReadString(name, assembly?)` | Read an embedded resource as a string. |
| `ReadBytes(name, assembly?)` | Read an embedded resource as a byte array. |
| `OpenStream(name, assembly?)` | Open an embedded resource as a stream. |
| `ReadJson<T>(name, assembly?)` | Read and deserialize a JSON embedded resource. |
| `List(pattern?, assembly?)` | List available embedded resource names. |

Name resolution tries an exact match first, then falls back to suffix matching. For example, `"Templates/welcome.html"` will match `"MyApp.Templates.welcome.html"`.

### ResourceNotFoundException

Thrown when a resource cannot be found. Includes the list of available resource names in the exception message for easier debugging.

## Development

```bash
dotnet build src/Philiprehberger.EmbeddedResource.csproj --configuration Release
```

## Support

If you find this project useful:

⭐ [Star the repo](https://github.com/philiprehberger/dotnet-embedded-resource)

🐛 [Report issues](https://github.com/philiprehberger/dotnet-embedded-resource/issues?q=is%3Aissue+is%3Aopen+label%3Abug)

💡 [Suggest features](https://github.com/philiprehberger/dotnet-embedded-resource/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement)

❤️ [Sponsor development](https://github.com/sponsors/philiprehberger)

🌐 [All Open Source Projects](https://philiprehberger.com/open-source-packages)

💻 [GitHub Profile](https://github.com/philiprehberger)

🔗 [LinkedIn Profile](https://www.linkedin.com/in/philiprehberger)

## License

[MIT](LICENSE)
