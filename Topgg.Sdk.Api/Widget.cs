using Topgg.Sdk.Api.Data;

namespace Topgg.Sdk.Api;

public static class Widget
{
    /// <summary>Generates a large widget URL.</summary>
    /// <param name="platform">The project's platform.</param>
    /// <param name="projectType">The project's type.</param>
    /// <param name="id">The project ID.</param>
    /// <returns>The widget URL.</returns>
    public static string Large(Platform platform, ProjectType projectType, ulong id) => $"{TopggApi.BaseURL}/widgets/large/{platform.ToString().ToLower()}/{projectType.ToString().ToLower()}/{id}";

    /// <summary>Generates a small widget URL for displaying votes.</summary>
    /// <param name="platform">The project's platform.</param>
    /// <param name="projectType">The project's type.</param>
    /// <param name="id">The project ID.</param>
    /// <returns>The widget URL.</returns>
    public static string Votes(Platform platform, ProjectType projectType, ulong id) => $"{TopggApi.BaseURL}/widgets/small/votes/{platform.ToString().ToLower()}/{projectType.ToString().ToLower()}/{id}";

    /// <summary>Generates a small widget URL for displaying a project's owner.</summary>
    /// <param name="platform">The project's platform.</param>
    /// <param name="projectType">The project's type.</param>
    /// <param name="id">The project ID.</param>
    /// <returns>The widget URL.</returns>
    public static string Owner(Platform platform, ProjectType projectType, ulong id) => $"{TopggApi.BaseURL}/widgets/small/owner/{platform.ToString().ToLower()}/{projectType.ToString().ToLower()}/{id}";

    /// <summary>Generates a small widget URL for displaying social stats.</summary>
    /// <param name="platform">The project's platform.</param>
    /// <param name="projectType">The project's type.</param>
    /// <param name="id">The project ID.</param>
    /// <returns>The widget URL.</returns>
    public static string Social(Platform platform, ProjectType projectType, ulong id) => $"{TopggApi.BaseURL}/widgets/small/social/{platform.ToString().ToLower()}/{projectType.ToString().ToLower()}/{id}";
}