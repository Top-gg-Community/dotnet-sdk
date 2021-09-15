using System;
using System.Linq;
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
            var cred = new Credentials();
            cred.BotId = ulong.Parse(Environment.GetEnvironmentVariable("BOT_ID"));
            cred.Token = Environment.GetEnvironmentVariable("API_KEY");
            return cred;
        }
    }

    public class UnitTests
    {
        private readonly AuthDiscordBotListApi _api;
        private readonly Credentials _cred;

        public UnitTests()
        {
            _cred = Credentials.LoadFromEnv();
            _api = new AuthDiscordBotListApi(_cred.BotId, _cred.Token);
        }

        [Fact]
        public void GetUserTest()
        {
            Assert.NotNull(_api.GetMeAsync());
            Assert.NotNull(_api.GetUserAsync(_cred.BotId));
        }

        [Fact]
        public async Task HasVotedTestAsync()
        {
            Assert.False(await _api.HasVoted(0));
        }

        [Fact]
        public async Task TaskIsWeekendTestAsync()
        {
            await _api.IsWeekendAsync();
        }

        [Fact]
        public async Task TaskGetVotersTestAsync()
        {
            Assert.NotNull(await _api.GetVotersAsync());
        }

        [Fact]
        public async Task GetUserTestAsync()
        {
            Assert.NotNull(await _api.GetUserAsync(181514288278536193));
        }

        [Fact]
        public async Task GetBotTestAsync()
        {
            var botId = 423593006436712458U;
            var bot = await _api.GetBotAsync(botId);
            Assert.NotNull(bot);
            Assert.Equal(botId, bot.Id);
        }

        [Fact]
        public async Task GetMeTestAsync()
        {
            Assert.NotNull(await _api.GetMeAsync());
        }

        [Fact]
        public async Task GetUsersGetStatsTest()
        {
            var bots = await _api.GetBotsAsync();

            Assert.NotNull(bots);
            Assert.NotEmpty(bots.Items);

            var firstBot = bots.Items.First();

            var stats = await firstBot.GetStatsAsync();

            Assert.NotNull(stats);
        }
    }
}
