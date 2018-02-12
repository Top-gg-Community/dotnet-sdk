using Newtonsoft.Json;
using System;
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
		Credentials cred;
		AuthDiscordBotListApi api;

		public UnitTests()
		{
			cred = Credentials.LoadFromFile("./settings.json");
			api = new AuthDiscordBotListApi(cred.BotId, cred.Token);
		}

		[Fact]
        public void GetUserTest()
        {
			Assert.NotNull(api.GetMeAsync());
			Assert.NotNull(api.GetUserAsync(cred.BotId));
        }

		[Fact]
		public async Task GetUserTestAsync()
		{
			Assert.NotNull(await api.GetUserAsync(160105994217586689));
		}

		[Fact]
		public async Task GetMeTestAsync()
		{
			Assert.NotNull(await api.GetMeAsync());
		}

		[Fact]
		public async Task GetUsersGetStatsTest()
		{
			Assert.NotNull(await (await api.GetBotsAsync()).Items.First().GetStatsAsync());
		}
	}
}
