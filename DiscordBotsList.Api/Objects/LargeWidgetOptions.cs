using System.Collections.Generic;

namespace DiscordBotsList.Api.Objects
{
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
			string query = $"https://top.gg/api/widget/{ botId }.svg";

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
}
