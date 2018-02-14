using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotsList.Api
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
