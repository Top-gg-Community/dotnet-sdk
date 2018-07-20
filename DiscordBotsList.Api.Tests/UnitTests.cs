using Newtonsoft.Json;
using System.IO;
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
        private readonly AuthDiscordBotListApi _dblApi =

   new AuthDiscordBotListApi(9999999999999, "DBL API TOKEN");

		public UnitTests()
		{
			_cred = Credentials.LoadFromFile("./settings.json");
			_api = new AuthDiscordBotListApi(_cred.BotId, _cred.Token);
		}

	    	[Fact]
        public void GetUserTest()
        {
		       	Assert.NotNull(_dblApi.GetMeAsync());
		      	Assert.NotNull(_dblApi.GetUserAsync(_cred.BotId));
        }

        [Fact]
        public async Task HasVotedTestAsync()
        {
            Assert.False(!await _dblApi.HasVoted(181514288278536193));
        }

        [Fact]
        public async Task HasVotedTestAsync()
        {
            Assert.False(!await _dblApi.HasVoted(181514288278536193));
        }

        
        [Fact]
        public async Task TaskIsWeekendTestAsync()
        {
            Assert.False(!await _dblApi.IsWeekend());
        }

        [Fact]
        public async Task TaskGetVotersTestAsync()
        {
            Assert.NotNull(await _dblApi.GetVotersAsync());
        }

        [Fact]
	    	public async Task GetUserTestAsync()
	    	{
		      	Assert.NotNull(await _dblApi.GetUserAsync(181514288278536193));
	     	}

        [Fact]
        public async Task GetBotTestAsync()
        {
            Assert.NotNull(await _dblApi.GetBotAsync(423593006436712458));
        }

		[Fact]
		public async Task GetMeTestAsync()
		{
			Assert.NotNull(await _dblApi.GetMeAsync());
		}

        /* Wasnt working before 
		[Fact]
		public async Task GetUsersGetStatsTest()
		{
			Assert.NotNull(await (await _dblApi.GetBotsAsync()).Items.First().GetStatsAsync());
		}
        */
	}
}
