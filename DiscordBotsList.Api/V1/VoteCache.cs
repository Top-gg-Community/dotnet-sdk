using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBotsList.Api
{
    /// <summary>
    ///     A single cached vote record. The user can vote again once the vote expires.
    /// </summary>
    public sealed class CachedVote
    {
        internal CachedVote(ulong userId, ulong platformId, int weight, DateTimeOffset createdAt, DateTimeOffset expiresAt)
        {
            UserId = userId;
            PlatformId = platformId;
            Weight = weight;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
        }

        internal static CachedVote FromVote(Vote vote)
        {
            return new CachedVote(vote.UserId, vote.PlatformId, vote.Weight, vote.CreatedAt, vote.ExpiresAt);
        }

        internal static CachedVote FromWebhook(VoteWebhookData data)
        {
            var user = data.User;
            return new CachedVote(user?.Id ?? 0, user?.PlatformId ?? 0, data.Weight, data.CreatedAt, data.ExpiresAt);
        }

        /// <summary>The voter's Top.gg Snowflake ID.</summary>
        public ulong UserId { get; }

        /// <summary>The voter's platform-specific ID (for example their Discord user ID).</summary>
        public ulong PlatformId { get; }

        /// <summary>
        ///     The number of votes this single action counted for (for example <c>2</c>
        ///     on weekends for Discord bots).
        /// </summary>
        public int Weight { get; }

        /// <summary>ISO 8601 timestamp of when the user voted.</summary>
        public DateTimeOffset CreatedAt { get; }

        /// <summary>ISO 8601 timestamp of when the user is eligible to vote again.</summary>
        public DateTimeOffset ExpiresAt { get; }

        /// <summary>Whether this vote is still active at the given time.</summary>
        public bool IsActiveAt(DateTimeOffset now)
        {
            return now < ExpiresAt;
        }

        /// <summary>Whether this vote is currently active.</summary>
        public bool IsActive => IsActiveAt(DateTimeOffset.UtcNow);
    }

    /// <summary>
    ///     An in-memory cache of recent votes, keyed by both the Top.gg and the
    ///     platform-specific user ID. Populate it from vote webhooks with
    ///     <see cref="HandleWebhookEvent" /> and it answers <see cref="HasActiveVote" />
    ///     instantly, without a single API call. This keeps reward checks well below the
    ///     documented rate limits (100 requests/second globally, 60 per minute on
    ///     bot routes) that block tokens for an hour.
    /// </summary>
    public sealed class VoteCache
    {
        private readonly ConcurrentDictionary<string, CachedVote> _votes = new(StringComparer.Ordinal);

        /// <summary>The number of cached votes, including expired entries.</summary>
        public int Count => _votes.Count;

        /// <summary>Records a vote event if it is a <c>vote.create</c> webhook event.</summary>
        public void HandleWebhookEvent(TopggWebhookEvent webhookEvent)
        {
            if (webhookEvent.IsVoteCreate)
            {
                HandleVote(webhookEvent.GetVoteData());
            }
        }

        /// <summary>Records the user's vote from a <c>vote.create</c> webhook payload.</summary>
        public void HandleVote(VoteWebhookData vote)
        {
            if (vote?.User is null)
            {
                return;
            }

            Upsert(CachedVote.FromWebhook(vote));
        }

        /// <summary>Stores a vote under both its Top.gg and platform-specific keys.</summary>
        public void Upsert(CachedVote vote)
        {
            if (vote is null)
            {
                return;
            }

            if (vote.UserId != 0)
            {
                _votes[Key(VoteSource.Topgg, vote.UserId)] = vote;
            }

            if (vote.PlatformId != 0)
            {
                _votes[Key(VoteSource.Discord, vote.PlatformId)] = vote;
            }
        }

        /// <summary>Removes the cached vote for the given user.</summary>
        public void Remove(string userId, VoteSource source = VoteSource.Topgg)
        {
            _votes.TryRemove(Key(source, userId), out _);
        }

        /// <summary>Removes the cached vote for the given user.</summary>
        public void Remove(ulong userId, VoteSource source)
        {
            _votes.TryRemove(Key(source, userId), out _);
        }

        /// <summary>Gets the cached vote for a Top.gg user ID, or <c>null</c>.</summary>
        public CachedVote Get(string userId, VoteSource source = VoteSource.Topgg)
        {
            return _votes.TryGetValue(Key(source, userId), out var vote) ? vote : null;
        }

        /// <summary>Gets the cached vote for a platform-specific user ID, or <c>null</c>.</summary>
        public CachedVote Get(ulong userId, VoteSource source)
        {
            return _votes.TryGetValue(Key(source, userId), out var vote) ? vote : null;
        }

        /// <summary>Gets the cached vote for a Discord user ID, or <c>null</c>.</summary>
        public CachedVote Get(ulong discordUserId)
        {
            return Get(discordUserId, VoteSource.Discord);
        }

        /// <summary>Whether the Discord user currently has an active vote.</summary>
        public bool HasActiveVote(ulong discordUserId)
        {
            return HasActiveVote(discordUserId, VoteSource.Discord);
        }

        /// <summary>Whether the user currently has an active vote.</summary>
        public bool HasActiveVote(ulong userId, VoteSource source)
        {
            return Get(userId, source)?.IsActive ?? false;
        }

        /// <summary>Whether the user currently has an active vote.</summary>
        public bool HasActiveVote(string userId, VoteSource source = VoteSource.Topgg)
        {
            return Get(userId, source)?.IsActive ?? false;
        }

        /// <summary>Drops expired votes and returns how many were removed.</summary>
        public int Prune(DateTimeOffset? now = null)
        {
            var cutoff = now ?? DateTimeOffset.UtcNow;
            var removed = 0;

            foreach (var pair in _votes)
            {
                if (pair.Value.IsActiveAt(cutoff))
                {
                    continue;
                }

                if (_votes.TryRemove(new KeyValuePair<string, CachedVote>(pair.Key, pair.Value)))
                {
                    removed++;
                }
            }

            return removed;
        }

        /// <summary>Removes every cached vote.</summary>
        public void Clear()
        {
            _votes.Clear();
        }

        /// <summary>Backfills the cache with the project's recent votes from the v1 API.</summary>
        /// <param name="api">A v1 API client authenticated with the project token.</param>
        /// <param name="startDate">Start of the vote history to load (not older than one year).</param>
        /// <returns>The number of votes loaded.</returns>
        public async Task<int> SyncAsync(TopggV1Client api, DateTimeOffset startDate, CancellationToken cancellationToken = default)
        {
            var count = 0;

            await foreach (var vote in api.GetAllVotesAsync(startDate, cancellationToken))
            {
                Upsert(CachedVote.FromVote(vote));
                count++;
            }

            return count;
        }

        /// <summary>
        ///     Refreshes a single user's vote status from the v1 API and updates the cache.
        ///     When the API reports no active vote, the user's cached vote is removed.
        /// </summary>
        public async Task<CachedVote> RefreshUserAsync(TopggV1Client api, string userId, VoteSource source = VoteSource.Topgg, CancellationToken cancellationToken = default)
        {
            var status = await api.GetVoteStatusAsync(userId, source, cancellationToken);
            if (status is null)
            {
                Remove(userId, source);
                return null;
            }

            var vote = new CachedVote(
                source == VoteSource.Topgg && ulong.TryParse(userId, out var topggId) ? topggId : 0,
                source == VoteSource.Discord && ulong.TryParse(userId, out var platformId) ? platformId : 0,
                status.Weight,
                status.CreatedAt,
                status.ExpiresAt);

            _votes[Key(source, userId)] = vote;
            Upsert(vote);
            return vote;
        }

        private static string Key(VoteSource source, string userId)
        {
            return source == VoteSource.Discord ? "d:" + userId : "t:" + userId;
        }

        private static string Key(VoteSource source, ulong userId)
        {
            return Key(source, userId.ToString());
        }
    }
}
