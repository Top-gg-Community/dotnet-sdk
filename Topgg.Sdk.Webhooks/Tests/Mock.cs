using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Topgg.Sdk.Webhooks.Tests;

internal class Mock
{
    internal static readonly string Secret = "testsecret1234";

    internal static readonly string Prefix = "Topgg.Sdk.Webhooks.Tests.Mocks.";

    internal static string[] Names = [.. typeof(Mock).Assembly.GetManifestResourceNames().Where(name => name.StartsWith(Prefix)).Select(name => name[Prefix.Length..(name.Length - 5)])];

    internal static string ReadJson(string name)
    {
        using var stream = typeof(Mock).Assembly.GetManifestResourceStream($"{Prefix}{name}.json");
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    internal static string Signature(string body)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var hash = Convert.ToHexString(HMACSHA256.HashData(Encoding.UTF8.GetBytes(Secret), Encoding.UTF8.GetBytes($"{timestamp}.{body}"))).ToLowerInvariant();

        return $"t={timestamp},v1={hash}";
    }
}