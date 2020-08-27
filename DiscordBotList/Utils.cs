using System;

namespace DiscordBotList
{
    internal static class Utils
    {
        public static int GetCeiling(int numerator, int denominator)
            => (int) Math.Ceiling(numerator / (double) denominator);

        public static int GetColorValue(string hex)
        {
            hex = hex.TrimStart('#');

            if (!ParseHex(hex, out byte r, out byte g, out byte b))
                return 0x000000;

            return FromColor(r, g, b);
        }

        private static bool ParseHex(string h, out byte r, out byte g, out byte b)
        {
			r = g = b = 0;

            if (string.IsNullOrWhiteSpace(h) || h.Length != 6)
                return false;

            const string format = "0x{0}";

            return byte.TryParse(string.Format(format, h[0] + h[1]), out r)
                   && byte.TryParse(string.Format(format, h[2] + h[3]), out g)
                   && byte.TryParse(string.Format(format, h[4] + h[5]), out b);
        }

		public static int FromColor(float r, float g, float b)
			=> FromColor((int)(r * 255), (int)(g * 255), (int)(b * 255));

		public static int FromColor(int r, int g, int b)
			=> (255 << 24) | ((byte)r << 16) | ((byte)g << 8) | ((byte)b << 0);


	}
}
