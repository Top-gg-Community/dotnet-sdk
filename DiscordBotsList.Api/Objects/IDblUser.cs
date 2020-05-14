namespace DiscordBotsList.Api.Objects
{
	public interface IDblUser : IDblEntity
    {
		string Biography { get; }

		string BannerUrl { get; }

		SocialConnections Connections { get; }

		string Color { get; }

		bool IsSupporter { get; }

		bool IsCertified { get; }

		bool IsModerator { get; }

		bool IsWebModerator { get; }

		bool IsAdmin { get; }
    }
}
