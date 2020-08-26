using System;

namespace DiscordBotsList.Api.Objects
{
    internal class QueryParamAttribute : Attribute
    {
        internal QueryParamAttribute(string name = null, string format = null, bool toLower = true)
        {
            Name = name;
            Format = format;
            ToLower = toLower;
        }

        internal string Name { get; }
        internal string Format { get; }
        internal bool ToLower { get; }
    }
}
