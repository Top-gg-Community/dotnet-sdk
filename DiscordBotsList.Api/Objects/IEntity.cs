using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotsList.Api
{
    public interface IEntity
	{
		ulong Id { get; }

		string Username { get; }

		string Discriminator { get; }

		string AvatarUrl { get; }
	}
}
