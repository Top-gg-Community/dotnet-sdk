using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DiscordBotList.Widgets;

namespace DiscordBotList.Internal
{
    /// <summary>
    /// Represents a helper class for retrieving endpoints related to the Discord Bot List API.
    /// </summary>
    public static class DblApi
    {
        internal static readonly string BaseUrl = "https://top.gg/api/";
        internal static readonly string BotEndpoint = "bots";
        internal static readonly string UserEndpoint = "users";
        internal static readonly string WeekendEndpoint = "weekend";

        public static string GetDefaultVanityUrl(ulong id)
            => $"https://top.gg/bot/{id}";

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

        public static string GetYouTubeUrl(string channelId)
            => GetSocialUrl("https://www.youtube.com/channel/", channelId);

        public static string GetRedditUrl(string name)
            => GetSocialUrl("https://www.reddit.com/user/", name);

        public static string GetTwitterUrl(string name)
            => GetSocialUrl("https://twitter.com/", name);

        public static string GetInstagramUrl(string name)
            => GetSocialUrl("https://www.instagram.com/", name);

        public static string GetGitHubUrl(string name)
            => GetSocialUrl("https://github.com/", name);

        internal static string GetInviteUrl(string inviteCode)
            => $"https://discord.gg/{inviteCode}";

        internal static string GetDefaultInviteUrl(ulong id)
            => $"https://discordapp.com/oauth2/authorize?&client_id={id}&scope=bot";

        internal static string GetWidgetUrl(ulong id, WidgetImageFormat format, SmallWidgetBuilder builder)
        {
            return SetQueryParams(GetBaseWidgetUrl(id, format, builder.Type), builder);
        }

        internal static string GetWidgetUrl(ulong id, WidgetImageFormat format, LargeWidgetBuilder builder)
        {
            return SetQueryParams(GetBaseWidgetUrl(id, format), builder);
        }

        internal static string GetVanityUrl(string vanity, ulong id)
            => string.IsNullOrWhiteSpace(vanity) ? GetDefaultVanityUrl(id) : $"https://top.gg/bot/{vanity}";

        internal static string GetUserUrl(ulong id)
            => $"https://top.gg/user/{id}";

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

        internal static string GetAvatarUrl(ulong id, string avatarId, string discriminator)
        {
            if (string.IsNullOrWhiteSpace(discriminator))
                return null;

            if (string.IsNullOrWhiteSpace(avatarId))
                return GetDefaultAvatarUrl(ushort.Parse(discriminator));

            return $"https://cdn.discordapp.com/{id}/{avatarId}.png";
        }

        internal static string GetDefaultAvatarUrl(ushort discriminator)
        {
            return $"https://cdn.discordapp.com/embed/avatars/{((int)discriminator % 5)}.png";
        }

        internal static string CreateQuery(string url, in IEnumerable<string> args)
            => $"{url}?{(args.Any() ? string.Join("&", args) : "")}";

        internal static string GetUserEndpoint(ulong id)
            => $"{UserEndpoint}/{id}";

        internal static string GetBotEndpoint(ulong id)
            => $"{BotEndpoint}/{id}";

        internal static string GetStatsEndpoint(ulong id)
            => $"{GetBotEndpoint(id)}/stats";

        internal static string GetVotesEndpoint(ulong id)
            => $"{GetBotEndpoint(id)}/votes";

        internal static string GetVoteCheckEndpoint(ulong id)
            => $"{GetBotEndpoint(id)}/check";

        internal static string GetVoteCheckEndpoint(ulong id, ulong userId)
            => $"{GetVoteCheckEndpoint(id)}?userId={userId}";

        internal static string GetBaseWidgetUrl(ulong id, WidgetImageFormat format,  SmallWidgetType? type = null)
        {
            return $"{BaseUrl}{(type.HasValue ? $"{type.ToString().ToLower()}/" : "")}{id}.{format.ToString().ToLower()}";
        }

        internal static string GetSearchEndpoint()
            => BotEndpoint;

        internal static string GetSearchEndpoint(BotSearchParams searchParams)
        {
            if (searchParams == null)
                return GetSearchEndpoint();

            string url = BotEndpoint;
            var args = new List<string>();

            if (searchParams.Limit > 0)
                args.Add($"limit={ClampLimit(searchParams.Limit)}");

            if (searchParams.Offset > 0)
                args.Add($"offset={searchParams.Offset}");

            if (searchParams.SearchFilters?.Any() ?? false)
                args.Add(GetSearchParam(searchParams.SearchFilters));

            if (searchParams.Sort != null)
                args.Add(GetSortParam(searchParams.Sort));

            if (searchParams.Fields?.Any() ?? false)
                args.Add(GetFieldsParam(searchParams.Fields));

            return CreateQuery(url, args);
        }

        internal static string GetSearchUrl(BotSearchParams searchParams)
            => $"{BaseUrl}{GetSearchEndpoint(searchParams)}";

        internal static string FormatField(DblField field)
        {
            if (field > 0)
            {
                string value = field.ToString();
                return $"{char.ToLower(value[0])}{value.Substring(1)}";
            }

            return field.ToString();
        }

        private static string GetSortParam(FieldSort sort)
            => GetSortParam(sort.Field, sort.Reverse);

        private static string GetSortParam(DblField field, bool reverse)
        {
            return $"sort={(reverse ? "-" : "")}{FormatField(field)}";
        }

        private static string GetFieldsParam(IEnumerable<DblField> fields)
        {
            return $"fields={string.Join(",", fields.Select(FormatField))}";
        }

        private static string GetSearchParam(IEnumerable<FieldSearch> searches)
        {
            return $"search={string.Join(" ", searches.Select(x => $"{FormatField(x.Field)}: {x.Value}"))}";
        }

        private static int ClampLimit(int limit)
            => limit > BotSearchParams.MaxLimit
                ? BotSearchParams.MaxLimit
                : limit < 0 ? 0 : limit;

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

        private static string GetSocialUrl(string baseUrl, string id)
            => string.IsNullOrWhiteSpace(id) ? null : $"{baseUrl}{id}";
    }
}
