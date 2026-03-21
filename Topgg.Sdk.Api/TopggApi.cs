#nullable enable

using Topgg.Sdk.Api.Data;
using Topgg.Sdk.Api.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Topgg.Sdk.Api;

/// <summary>Interact with Top.gg API v1's endpoints.</summary>
public class TopggApi(HttpClient httpClient)
{
    internal static readonly string BaseURL = "https://top.gg/api/v1";
    private readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new ULongToStringConverter(), new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
    private readonly HttpClient Http = httpClient;

    /// <summary>Creates a new client instance.</summary>
    /// <param name="token">The API token to use.</param>
    public TopggApi(string token) : this(new HttpClient()
    {
        DefaultRequestHeaders = { { "Authorization", $"Bearer {token}" } }
    })
    { }

    /// <summary>Tries to get your project's information.</summary>
    /// <returns>Your project's information.</returns>
    public async Task<Project> GetSelfAsync() => (await GetAsync<Project>("/projects/@me"))!;

    /// <summary>Tries to update the application commands list in your Discord bot's Top.gg page.</summary>
    /// <typeparam name="T">Serializable list of Discord application commands.</typeparam>
    /// <param name="commands">A list of your Discord bot's application commands in the form of Discord API's raw JSON format.</param>
    public async Task PostCommandsAsync<T>(T commands) => await PostAsync<T, string>("/projects/@me/commands", commands);

    /// <summary>Tries to get the latest vote information of a user on your project. Returns null if the user has not voted.</summary>
    /// <param name="id">The user's ID.</param>
    /// <param name="source">The user's source.</param>
    /// <returns>The latest vote information of a user on your project or null if the user has not voted.</returns>
    public async Task<Vote?> GetVoteAsync(ulong id, UserSource source = UserSource.Discord)
    {
        try
        {
            return await GetAsync<Vote>($"/projects/@me/votes/{id}?source={source.ToString().ToLower()}");
        }
        catch (HttpRequestException error)
        {
            if (error.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            throw;
        }
    }


    /// <summary>Tries to get a cursor-based paginated list of votes for your project, ordered by creation date.</summary>
    /// <param name="since">The earliest possible date for all votes.</param>
    /// <returns>A cursor-based paginated list of votes for your project, ordered by creation date.</returns>
    public async Task<PaginatedVotes> GetVotesAsync(DateTime since)
    {
        var votes = (await GetAsync<PaginatedVotes>($"/projects/@me/votes?startDate={Uri.EscapeDataString(since.ToString("yyyy-MM-ddTHH:mm:ss.fffK"))}"))!;

        votes.Client = this;

        return votes;
    }

    internal async Task<PaginatedVotes> GetVotesAsync(string cursor)
    {
        var votes = (await GetAsync<PaginatedVotes>($"/projects/@me/votes?cursor={cursor}"))!;

        votes.Client = this;

        return votes;
    }

    private async Task<T?> ProcessResponse<T>(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();

        if (typeof(T) == typeof(string))
        {
            return (T)(object)await response.Content.ReadAsStringAsync();
        }
        else
        {
            return await response.Content.ReadFromJsonAsync<T>(SerializerOptions);
        }
    }

    private async Task<T?> GetAsync<T>(string url) => await ProcessResponse<T>(await Http.GetAsync(BaseURL + url));

    private async Task<T?> PostAsync<B, T>(string url, B body)
    {
        StringContent httpContent;

        if (typeof(B) == typeof(string))
        {
            httpContent = new StringContent((string)(object)body!, Encoding.UTF8, "application/json");
        }
        else
        {
            var json = JsonSerializer.Serialize(body);

            httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return await ProcessResponse<T>(await Http.PostAsync(BaseURL + url, httpContent));
    }
}