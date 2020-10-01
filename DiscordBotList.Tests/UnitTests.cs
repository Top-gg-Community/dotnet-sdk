using System;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DiscordBotList.Models;
using Xunit;
using Xunit.Abstractions;

namespace DiscordBotList.Tests
{
	public class Credentials
	{
		public ulong BotId;
		public string Token;

		public static Credentials LoadFromFile(string filePath)
		{
			if (!File.Exists(filePath))
			{
                using (var writer = new StreamWriter(filePath))
                {
                    writer.Write(JsonConvert.SerializeObject(new Credentials()));
                    writer.Flush();
                }

                return null;
			}

            using (var reader = new StreamReader(filePath))
				return JsonConvert.DeserializeObject<Credentials>(reader.ReadToEnd());
        }
	}

    public class UnitTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Credentials _cred;
        private readonly BaseDblClient _client;

		public UnitTests(ITestOutputHelper testOutputHelper)
		{
            _testOutputHelper = testOutputHelper;
            _cred = Credentials.LoadFromFile("./settings.json");
            if (_cred == null)
            {
				_client = new BaseDblClient();
            }
            else
            {
                _client = new DblClient(_cred.BotId, _cred.Token);
            }
        }

		[Fact]
        public async Task Test_SelfNotNull()
        {
            IDblUser user = await _client.GetUserAsync(181605794159001601);
            Assert.NotNull(user);
            _testOutputHelper.WriteLine($"{user.ToString()}");
        }

        [Fact]
        public async Task Test_InvalidVoteIsFalseAsync()
        {
            if (_cred != null)
            {
                Assert.False(await ((DblClient)_client).HasVotedAsync(0));
            }
        }

        [Fact]
        public async Task Test_GetWeekendAsync()
        {
			bool result = await _client.IsWeekendAsync();
            _testOutputHelper.WriteLine($"{result}");
        }

        [Fact]
        public async Task Test_GetVotersAsync()
        {
            if (_cred != null)
                Assert.NotNull(await ((DblClient)_client).GetVotersAsync());
        }

        [Fact]
		public async Task Test_UserNotNullAsync()
		{
			Assert.NotNull(await _client.GetUserAsync(181514288278536193));
		}

        [Fact]
        public async Task Test_BotNotNullAsync()
        {
            Assert.NotNull(await _client.GetBotAsync(423593006436712458));
        }

		[Fact]
		public async Task Test_SelfNotNullAsync()
		{
            if (_cred != null)
                Assert.NotNull(await ((DblClient)_client).GetSelfAsync());
		}

		[Fact]
		public async Task Test_DefaultSearchNotNullAsync()
		{
			ISearchResult<IDblBot> bots = await _client.GetBotsAsync();

			Assert.NotNull(bots);
			Assert.NotEmpty(bots.Values);

			IDblBot firstBot = bots.Values.First();
            IDblBotStats stats = await firstBot.GetStatsAsync();

			Assert.NotNull(stats);
		}
	}
}
