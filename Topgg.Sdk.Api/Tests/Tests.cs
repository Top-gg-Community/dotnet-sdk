using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Topgg.Sdk.Api.Data;
using Xunit;

namespace Topgg.Sdk.Api.Tests;

public class Tests
{
    public static IEnumerable<TheoryDataRow<UserSource>> UserSources => Enum.GetValues<UserSource>().Select(source => new TheoryDataRow<UserSource>(source));
    public static IEnumerable<TheoryDataRow<Platform, ProjectType>> PlatformsAndProjectTypes => Enum.GetValues<Platform>().SelectMany(platform => Enum.GetValues<ProjectType>(), (platform, projectType) => new TheoryDataRow<Platform, ProjectType>(platform, projectType));

    private readonly TopggApi Client = new(new HttpClient(new Mock()));

    [Fact]
    public async Task GetSelfAsync() => await Client.GetSelfAsync();

    [Fact]
    public async Task PostCommandsAsync() => await Client.PostCommandsAsync(Mock.ReadJson("PostCommands"));

    [Theory]
    [MemberData(nameof(UserSources))]
    public async Task GetVoteAsync(UserSource source)
    {
        await Client.GetVoteAsync(661200758510977084, source);
    }

    [Fact]
    public async Task GetVotesAsync()
    {
        var since = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var firstPage = await Client.GetVotesAsync(since);
        await firstPage.Next();
    }

    [Theory]
    [MemberData(nameof(PlatformsAndProjectTypes))]
    public void Widgets(Platform platform, ProjectType projectType)
    {
        Widget.Large(platform, projectType, 123456);
        Widget.Votes(platform, projectType, 123456);
        Widget.Owner(platform, projectType, 123456);
        Widget.Social(platform, projectType, 123456);
    }
}