using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotsList.Api
{
	public enum WidgetType
	{ STATUS, SERVERS, LIB, UPVOTES, OWNER }

	public class SmallWidgetOptions
	{
		WidgetType Type;

		int? AvatarBackgroundColor;
		int? LeftColor;
		int? RightColor;
		int? LeftTextColor;
		int? RightTextColor;

		public SmallWidgetOptions SetType(WidgetType t)
		{
			Type = t;
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
			string query = $"https://discordbots.org/api/widget/{ Type.ToString().ToLower() }/{ botId }.svg";

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
	public class LargeWidgetOptions
	{
		int? TopColor;
		int? MiddleColor;	
		int? UsernameColor;
		int? CertifiedColor;
		int? DataColor;
		int? LabelColor;
		int? HighlightColor;

		public LargeWidgetOptions SetTopColor(int r, int g, int b)
		{
			TopColor = Utils.FromColor(r, g, b);
			return this;
		}
		public LargeWidgetOptions SetMiddleColor(int r, int g, int b)
		{
			MiddleColor = Utils.FromColor(r, g, b);
			return this;
		}
		public LargeWidgetOptions SetUsernameColor(int r, int g, int b)
		{
			UsernameColor = Utils.FromColor(r, g, b);
			return this;
		}
		public LargeWidgetOptions SetCertifiedColor(int r, int g, int b)
		{
			CertifiedColor = Utils.FromColor(r, g, b);
			return this;
		}
		public LargeWidgetOptions SetDataColor(int r, int g, int b)
		{
			DataColor = Utils.FromColor(r, g, b);
			return this;
		}
		public LargeWidgetOptions SetLabelColor(int r, int g, int b)
		{
			LabelColor = Utils.FromColor(r, g, b);
			return this;
		}
		public LargeWidgetOptions SetHighlightColor(int r, int g, int b)
		{
			HighlightColor = Utils.FromColor(r, g, b);
			return this;
		}

		/// <summary>
		/// Builds and returns a value.
		/// </summary>
		/// <param name="botId">Id of the bot</param>
		/// <returns>Widget url</returns>
		public string Build(ulong botId)
		{
			string query = $"https://discordbots.org/api/widget/{ botId }.svg";

			List<string> args = new List<string>();

			if (TopColor != null)
				args.Add($"topcolor={TopColor.Value.ToString("X")}");

			if (MiddleColor != null)
				args.Add($"middlecolor={MiddleColor.Value.ToString("X")}");

			if (UsernameColor != null)
				args.Add($"usernamecolor={UsernameColor.Value.ToString("X")}");

			if (CertifiedColor != null)
				args.Add($"certifiedcolor={CertifiedColor.Value.ToString("X")}");

			if (DataColor != null)
				args.Add($"datacolor={DataColor.Value.ToString("X")}");
			
			if (LabelColor != null)
				args.Add($"labelcolor={LabelColor.Value.ToString("X")}");

			if (HighlightColor != null)
				args.Add($"highlightcolor={HighlightColor.Value.ToString("X")}");

			return Utils.CreateQuery(query, args.ToArray());
		}
	}

	internal static class Utils
	{
		public static int FromColor(float r, float g, float b)
			=> FromColor((int)(r * 255), (int)(g * 255), (int)(b * 255));
		public static int FromColor(int r, int g, int b)
			=> (255 << 24) | ((byte)r << 16) | ((byte)g << 8) | ((byte)b << 0);

		/// <summary>
		/// Creates rest parameters
		/// </summary>
		/// <param name="baseUrl">url</param>
		/// <param name="args">arguments</param>
		/// <returns>baseUrl?argument[0]&argument[1]&...</returns>
		public static string CreateQuery(string baseUrl, params string[] args)
		{
			if (args.Length > 0)
			{
				return $"{baseUrl}?{string.Join("&", args)}";
			}
			return baseUrl;
		}
	}
}
