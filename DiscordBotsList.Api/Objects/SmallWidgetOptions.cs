using System.Collections.Generic;

namespace DiscordBotsList.Api.Objects
{
	public class SmallWidgetOptions
	{
		SmallWidgetType Type;
		bool? UseAvatar;
		int? AvatarBackgroundColor;
		int? LeftColor;
		int? RightColor;
		int? LeftTextColor;
		int? RightTextColor;

		public SmallWidgetOptions SetType(SmallWidgetType t)
		{
			Type = t;
			return this;
		}

		public SmallWidgetOptions SetAvatar(bool useAvatar)
		{
			UseAvatar = useAvatar;
			return this;
		}

		public SmallWidgetOptions SetAvatarBackgroundColor(int r, int g, int b)
		{
			AvatarBackgroundColor = Utils.FromColor(r, g, b);
			return this;
		}
		public SmallWidgetOptions SetLeftColor(int r, int g, int b)
		{
			LeftColor = Utils.FromColor(r, g, b);
			return this;
		}
		public SmallWidgetOptions SetRightColor(int r, int g, int b)
		{
			RightColor = Utils.FromColor(r, g, b);
			return this;
		}
		public SmallWidgetOptions SetLeftTextColor(int r, int g, int b)
		{
			LeftTextColor = Utils.FromColor(r, g, b);
			return this;
		}
		public SmallWidgetOptions SetRightTextColor(int r, int g, int b)
		{
			RightTextColor = Utils.FromColor(r, g, b);
			return this;
		}

		/// <summary>
		/// Builds and returns a value.
		/// </summary>
		/// <param name="botId">Id of the bot</param>
		/// <returns>Widget url</returns>
		public string Build(ulong botId)
		{
			string query = $"https://top.gg/api/widget/{ Type.ToString().ToLower() }/{ botId }.svg";

			List<string> args = new List<string>();

			if (AvatarBackgroundColor != null)
				args.Add($"avatarbg={AvatarBackgroundColor.Value.ToString("X")}");

			if (LeftColor != null)
				args.Add($"leftcolor={LeftColor.Value.ToString("X")}");

			if (RightColor != null)
				args.Add($"rightcolor={RightColor.Value.ToString("X")}");

			if (LeftTextColor != null)
				args.Add($"lefttextcolor={LeftTextColor.Value.ToString("X")}");

			if (RightTextColor != null)
				args.Add($"righttextcolor={RightTextColor.Value.ToString("X")}");

			return Utils.CreateQuery(query, args.ToArray());
		}
	}
}
