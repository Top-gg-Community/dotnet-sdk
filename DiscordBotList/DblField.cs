namespace DiscordBotList
{
    /// <summary>
    /// Defines search fields for the Discord Bot List API.
    /// </summary>
    public enum DblField
    {
        /// <inheritdoc cref="Models.IDblEntity.Id" />
        Id = 1,

        /// <inheritdoc cref="Models.IDblEntity.Username" />
        Username = 2,

        /// <inheritdoc cref="Models.IDblEntity.Discriminator" />
        Discriminator = 3,

        /// <inheritdoc cref="Models.IDblEntity.AvatarUrl" />
        Avatar = 4,

        /// <inheritdoc cref="Models.IDblEntity.DefaultAvatarHash" />
        DefAvatar = 5,

        /// <inheritdoc cref="Models.IDblBot.LibraryUsed" />
        Lib = 6,

        /// <inheritdoc cref="Models.IDblBot.PrefixUsed" />
        Prefix = 7,

        /// <inheritdoc cref="Models.IDblBot.ShortDescription" />
        ShortDesc = 8,

        /// <inheritdoc cref="Models.IDblBot.LongDescription" />
        LongDesc = 9,

        /// <inheritdoc cref="Models.IDblBot.Tags" />
        Tags = 10,

        /// <inheritdoc cref="Models.IDblBot.WebsiteUrl" />
        Website = 11,

        /// <inheritdoc cref="Models.IDblBot.SupportInviteUrl" />
        Support = 12,

        /// <inheritdoc cref="Models.IDblBot.GitHubUrl" />
        Github = 13,

        /// <inheritdoc cref="Models.IDblBot.OwnerIds" />
        Owners = 14,

        /// <inheritdoc cref="Models.IDblBot.FeaturedGuildIds" />
        Guilds = 15,

        /// <inheritdoc cref="Models.IDblBot.InviteUrl" />
        Invite = 16,

        /// <inheritdoc cref="Models.IDblBot.ApprovedAt" />
        Date = 17,

        /// <inheritdoc cref="Models.IDblBot.IsCertified" />
        CertifiedBot = 18,

        /// <inheritdoc cref="Models.IDblBot.VanityUrl" />
        Vanity = 19,

        /// <inheritdoc cref="Models.IDblBot.Points" />
        Points = 20,

        /// <inheritdoc cref="Models.IDblBot.MonthlyPoints" />
        MonthlyPoints = 21,

        /// <inheritdoc cref="Models.IDblBot.DonateBotGuildId" />
        DonateBotGuildId = 22
    }
}
