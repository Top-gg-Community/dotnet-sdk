using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Topgg.Sdk.Webhooks.Tests;

public class Tests
{
    public static IEnumerable<TheoryDataRow<string>> Payloads => Mock.Names.Select(payload => new TheoryDataRow<string>(payload));

    private static readonly string Trace = "trace";
    private readonly HttpClient Http;

    public Tests()
    {
        var server = new TestServer(new WebHostBuilder().ConfigureServices(services =>
        {
            services.AddRouting();
        }).Configure(app =>
        {
            app.UseRouting();
            app.UseEndpoints(builder =>
                {
                    var webhooks = new CustomWebhooks();

                    builder.MapPost("/webhook", webhooks.Handler);
                });
        }));

        Http = server.CreateClient();
    }

    [Theory]
    [MemberData(nameof(Payloads))]
    public async Task Test(string payload)
    {
        var body = Mock.ReadJson(payload);

        var response = await Http.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/webhook")
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
            Headers = {
                    {"x-topgg-signature", Mock.Signature(body)},
                    {"x-topgg-trace", Trace}
                }
        }, TestContext.Current.CancellationToken);

        Assert.StrictEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal($"{payload},{Trace}", await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
    }
}