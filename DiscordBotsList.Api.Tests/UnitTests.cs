using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DiscordBotsList.Api.Tests
{
	public class Credentials
	{
		public ulong BotId;
		public string Token;

		public static Credentials LoadFromFile(string filePath)
		{
			if (!File.Exists(filePath))
			{
				StreamWriter sw = new StreamWriter(filePath);
				sw.Write(JsonConvert.SerializeObject(new Credentials()));
				sw.Flush();
				sw.Close();
				return null;
			}

			StreamReader sr = new StreamReader(filePath);
			var cred = JsonConvert.DeserializeObject<Credentials>(sr.ReadToEnd());
			return cred;
		}
	}

    public class UnitTests
    {
        readonly Credentials _cred;
        readonly AuthDiscordBotListApi _api;

		public UnitTests()
		{
			_cred = Credentials.LoadFromFile("./settings.json");
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
            Assert.NotNull(await _api.GetBotAsync(423593006436712458));
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
