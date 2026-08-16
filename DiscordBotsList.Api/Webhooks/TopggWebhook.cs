using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DiscordBotsList.Api.Internal;

namespace DiscordBotsList.Api
{
    /// <summary>
    ///     Helpers for consuming Top.gg v1 webhooks. Every webhook request carries an
    ///     <c>x-topgg-signature</c> header in the format <c>t={unix timestamp},v1={signature}</c>,
    ///     where the signature is the HMAC SHA-256 digest of <c>{timestamp}.{rawBody}</c>
    ///     computed with your webhook secret (prefixed with <c>whs_</c>) as the key.
    ///     Never process a webhook payload without first verifying its signature.
    /// </summary>
    public static class TopggWebhook
    {
        /// <summary>The header containing the request signature.</summary>
        public const string SignatureHeaderName = "x-topgg-signature";

        /// <summary>The header containing a trace ID used when debugging deliveries.</summary>
        public const string TraceHeaderName = "x-topgg-trace";

        /// <summary>
        ///     Verifies the HMAC SHA-256 signature of a webhook request. Use this before
        ///     processing any payload received on your endpoint.
        /// </summary>
        /// <param name="webhookSecret">Your webhook secret (the value prefixed with <c>whs_</c>).</param>
        /// <param name="rawBody">The raw request body as a string. Do not parse or reformat it first.</param>
        /// <param name="signatureHeader">The value of the <c>x-topgg-signature</c> header.</param>
        /// <param name="maxAge">
        ///     When provided, requests whose signature timestamp is older than this
        ///     duration are rejected, protecting against replay attacks. Defaults to
        ///     <c>null</c>, which disables the freshness check.
        /// </param>
        /// <returns><c>true</c> when the signature is valid and originates from Top.gg.</returns>
        public static bool VerifySignature(string webhookSecret, string rawBody, string signatureHeader, TimeSpan? maxAge = null)
        {
            if (string.IsNullOrEmpty(webhookSecret) || rawBody is null || string.IsNullOrEmpty(signatureHeader))
            {
                return false;
            }

            string timestamp = null;
            string signature = null;

            foreach (var part in signatureHeader.Split(','))
            {
                var pair = part.Split('=', 2);
                if (pair.Length != 2)
                {
                    continue;
                }

                if (pair[0] == "t")
                {
                    timestamp = pair[1];
                }
                else if (pair[0] == "v1")
                {
                    signature = pair[1];
                }
            }

            if (string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(signature))
            {
                return false;
            }

            if (maxAge.HasValue && IsTooOld(timestamp, maxAge.Value))
            {
                return false;
            }

            var expected = ComputeSignature(webhookSecret, timestamp, rawBody);
            var expectedBytes = Encoding.ASCII.GetBytes(expected);
            var receivedBytes = Encoding.ASCII.GetBytes(signature);

            return expectedBytes.Length == receivedBytes.Length
                   && CryptographicOperations.FixedTimeEquals(expectedBytes, receivedBytes);
        }

        /// <summary>
        ///     Verifies the HMAC SHA-256 signature of a webhook request given a set of
        ///     request headers. The signature header is matched case-insensitively.
        /// </summary>
        public static bool VerifySignature(string webhookSecret, string rawBody, IEnumerable<KeyValuePair<string, string>> headers, TimeSpan? maxAge = null)
        {
            var header = headers?.FirstOrDefault(h => h.Key.Equals(SignatureHeaderName, StringComparison.OrdinalIgnoreCase)).Value;
            return header is not null && VerifySignature(webhookSecret, rawBody, header, maxAge);
        }

        /// <summary>
        ///     Verifies the request signature and, on success, deserializes the payload.
        ///     Returns <c>null</c> when the signature is invalid.
        /// </summary>
        public static TopggWebhookEvent ParseAndVerify(string webhookSecret, string rawBody, string signatureHeader, TimeSpan? maxAge = null)
        {
            if (!VerifySignature(webhookSecret, rawBody, signatureHeader, maxAge))
            {
                return null;
            }

            return Deserialize(rawBody);
        }

        /// <summary>
        ///     Deserializes a webhook payload without verifying its signature. Prefer
        ///     <see cref="ParseAndVerify" /> or verify separately before calling this.
        /// </summary>
        public static TopggWebhookEvent Deserialize(string rawBody)
        {
            if (string.IsNullOrEmpty(rawBody))
            {
                throw new ArgumentException("The webhook payload is empty.", nameof(rawBody));
            }

            return JsonSerializer.Deserialize<TopggWebhookEvent>(rawBody, JsonDefaults.Options)
                   ?? throw new JsonException("The webhook payload could not be parsed.");
        }

        /// <summary>
        ///     Computes the expected HMAC SHA-256 signature for a raw body and timestamp.
        ///     Exposed for testing and for tooling that constructs signed requests.
        /// </summary>
        public static string ComputeSignature(string webhookSecret, string timestamp, string rawBody)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(webhookSecret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes($"{timestamp}.{rawBody}"));
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        private static bool IsTooOld(string timestamp, TimeSpan maxAge)
        {
            if (!long.TryParse(timestamp, out var unixSeconds))
            {
                return true;
            }

            var time = DateTimeOffset.FromUnixTimeSeconds(unixSeconds);
            var age = DateTimeOffset.UtcNow - time;
            return age < TimeSpan.Zero || age > maxAge;
        }
    }
}
