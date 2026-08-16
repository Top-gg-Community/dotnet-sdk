using System;
using System.Threading.Tasks;
using Xunit;

namespace DiscordBotsList.Api.Tests
{
    public class Credentials
    {
        public ulong BotId { get; set; }
        public string Token { get; set; }

        public static Credentials LoadFromEnv()
        {
            return new Credentials()
            {
                BotId = ulong.Parse(Environment.GetEnvironmentVariable("BOT_ID")),
                Token = Environment.GetEnvironmentVariable("API_KEY")
            };
        }
    }

    /// <summary>
    ///     Live integration tests. They require the BOT_ID and API_KEY environment
    ///     variables (a legacy v0 token) and are skipped when they are not present.
    /// </summary>
    public class UnitTests
    {
        private readonly AuthDiscordBotListApi _api;
        private readonly bool _available;

        public UnitTests()
        {
            var botId = Environment.GetEnvironmentVariable("BOT_ID");
            var apiKey = Environment.GetEnvironmentVariable("API_KEY");

            _available = !string.IsNullOrEmpty(botId) && !string.IsNullOrEmpty(apiKey);
            if (_available)
            {
                _api = new AuthDiscordBotListApi(ulong.Parse(botId), apiKey);
            }
        }

        [Fact]
        public async Task HasVotedTestAsync()
        {
            if (!_available) return;
            Assert.False(await _api.HasVoted(0));
        }

        [Fact]
        public async Task TaskIsWeekendTestAsync()
        {
            if (!_available) return;
            await _api.IsWeekendAsync();
        }

        [Fact]
        public async Task TaskGetVotersTestAsync()
        {
            if (!_available) return;
            Assert.NotNull(await _api.GetVotersAsync());
        }

        [Fact]
        public async Task GetBotTestAsync()
        {
            if (!_available) return;
            var botId = 1026525568344264724U;
            var bot = await _api.GetBotAsync(botId);
            Assert.NotNull(bot);
            Assert.Equal(botId, bot.Id);
        }

        [Fact]
        public async Task GetMeTestAsync()
        {
            if (!_available) return;
            Assert.NotNull(await _api.GetMeAsync());
        }

        [Fact]
        public async Task GetUsersGetStatsTest()
        {
            if (!_available) return;
            var bots = await _api.GetBotsAsync();

            Assert.NotNull(bots);
            Assert.NotEmpty(bots.Items);
        }
    }
}
