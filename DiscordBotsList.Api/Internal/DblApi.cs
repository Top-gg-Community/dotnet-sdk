using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Internal
{
    /// <summary>
    /// Represents a helper class for retrieving endpoints related to the Discord Bot List API.
    /// </summary>
    internal static class DblApi
    {
        internal static readonly string BaseUrl = "https://top.gg/api/";

        internal static string GetVanityUrl(string vanity)
            => $"https://top.gg/bot/{vanity}";

        public static string GetDefaultVanityUrl(ulong id)
            => $"https://top.gg/bot/{id}";

        public static string GetDefaultInviteUrl(ulong id)
            => $"https://discordapp.com/oauth2/authorize?&client_id={id}&scope=bot";

        private static string GetQueryParam(PropertyInfo property, object obj)
        {
            var param = property.GetCustomAttribute<QueryParamAttribute>();

            if (param == null)
                throw new ArgumentException("Expected to find QueryParamAttribute but returned null");

            string name = param.Name ?? property.Name.ToLower();
            object value = property.GetValue(obj);
            string key;

            if (param.Format != null && value is IFormattable f)
                key = f.ToString("##,0", null);
            else
                key = value.ToString();

            if (param.ToLower)
                key = key.ToLower();

            return $"{name}={key}";
        }

        internal static string SetQueryParams(string url, object obj)
        {
            var query = new StringBuilder(url);

            string args = string.Join("&",
                obj.GetType()
                .GetTypeInfo()
                .GetProperties()
                .Where(x => x.GetCustomAttribute<QueryParamAttribute>() != null)
                .Select(x => GetQueryParam(x, obj)));

            query.Append(args);

            return query.ToString();
        }

        internal static string CreateQuery(string url, in IEnumerable<string> args)
            => $"{url}?{(args.Any() ? string.Join("&", args) : "")}";

        private static string GetBaseWidgetUrl(ulong id, WidgetImageFormat format,  SmallWidgetType? type = null)
        {
            return $"{BaseUrl}{(type.HasValue ? $"{type.ToString().ToLower()}/" : "")}{id}.{format.ToString().ToLower()}";
        }

        public static string GetWidgetUrl(ulong id, SmallWidgetOptions options)
        {
            return SetQueryParams(GetBaseWidgetUrl(id, options.Format, options.Type), options);
        }

        public static string GetWidgetUrl(ulong id, LargeWidgetOptions options)
        {
            return SetQueryParams(GetBaseWidgetUrl(id, options.Format), options);
        }

        public static string GetWidgetUrl(ulong id, WidgetImageFormat format = WidgetImageFormat.Svg,
            int? topColor = null, int? middleColor = null, int? usernameColor = null, int? certifiedColor = null,
            int? dataColor = null, int? labelColor = null, int? highlightColor = null)
        {
            string url = GetBaseWidgetUrl(id, format);

            var args = new List<string>();

            if (topColor != null)
                args.Add($"topcolor={topColor.Value:X}");

            if (middleColor != null)
                args.Add($"middlecolor={middleColor.Value:X}");

            if (usernameColor != null)
                args.Add($"usernamecolor={usernameColor.Value:X}");

            if (certifiedColor != null)
                args.Add($"certifiedcolor={certifiedColor.Value:X}");

            if (dataColor != null)
                args.Add($"datacolor={dataColor.Value:X}");

            if (labelColor != null)
                args.Add($"labelcolor={labelColor.Value:X}");

            if (highlightColor != null)
                args.Add($"highlightcolor={highlightColor.Value:X}");

            return CreateQuery(url, args);
        }

        public static string GetWidgetUrl(ulong id, SmallWidgetType type, WidgetImageFormat format = WidgetImageFormat.Svg, int? avatarBackgroundColor = null,
            int? leftColor = null, int? rightColor = null, int? leftTextColor = null, int? rightTextColor = null, bool useAvatar = true)
        {
            string url = GetBaseWidgetUrl(id, format, type);

            var args = new List<string>();

            if (!useAvatar)
                args.Add($"noavatar=true");

            if (avatarBackgroundColor != null)
                args.Add($"avatarbg={avatarBackgroundColor.Value:X}");

            if (leftColor != null)
                args.Add($"leftcolor={leftColor.Value:X}");

            if (rightColor != null)
                args.Add($"rightcolor={rightColor.Value:X}");

            if (leftTextColor != null)
                args.Add($"lefttextcolor={leftTextColor.Value:X}");

            if (rightTextColor != null)
                args.Add($"righttextcolor={rightTextColor.Value:X}");

            return CreateQuery(url, args);
        }
    }
}