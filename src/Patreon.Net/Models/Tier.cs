using System;
using Newtonsoft.Json;

namespace Patreon.Net.Models
{
    /// <summary>
    /// A membership level on a campaign, which can have benefits attached to it.
    /// </summary>
    [PatreonResource("tier")]
    public class Tier
    {
        public class Relationships
        {
            /// <summary>
            /// The benefits attached to the tier, which are used for generating deliverables.
            /// </summary>
            [JsonProperty("benefits")]
            public ResourceArray<Benefit, Benefit.Relationships> Benefits { get; set; }
            /// <summary>
            /// The campaign the tier belongs to.
            /// </summary>
            [JsonProperty("campaign")]
            public Campaign Campaign { get; set; }
            /// <summary>
            /// The image file associated with the tier.
            /// </summary>
            [JsonProperty("tier_image")]
            public Media TierImage { get; set; }
        }

        /// <summary>
        /// Monetary amount associated with this tier (in U.S. cents).
        /// </summary>
        [JsonProperty("amount_cents")]
        public string AmountCents { get; set; }
        /// <summary>
        /// The time this tier was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        /// <summary>
        /// The tier display description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// The Discord role IDs granted by this tier. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("discord_role_ids")]
        public object DiscordRoleIds { get; set; }
        /// <summary>
        /// The time the tier was last modified.
        /// </summary>
        [JsonProperty("edited_at")]
        public DateTimeOffset EditedAt { get; set; }
        /// <summary>
        /// Full qualified image URL associated with this tier. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        /// <summary>
        /// Number of patrons currently registered for this tier.
        /// </summary>
        [JsonProperty("patron_count")]
        public int PatronCount { get; set; }
        /// <summary>
        /// Number of posts published to this tier. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("post_count")]
        public int? PostCount { get; set; }
        /// <summary>
        /// Whether or not the tier is currently published.
        /// </summary>
        [JsonProperty("published")]
        public bool Published { get; set; }
        /// <summary>
        /// The time this tier was last published. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("published_at")]
        public DateTimeOffset? PublishedAt { get; set; }
        /// <summary>
        /// Remaining number of patrons who may subscribe, if there is a <see cref="UserLimit"/>. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("remaining")]
        public int? Remaining { get; set; }
        /// <summary>
        /// Whether or not this tier requires a shipping address from patrons.
        /// </summary>
        [JsonProperty("requires_shipping")]
        public bool RequiresShipping { get; set; }
        /// <summary>
        /// The tier display title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// The time the tier was unpublished, while applicable. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("unpublished_at")]
        public DateTimeOffset? UnpublishedAt { get; set; }
        /// <summary>
        /// Fully qualified URL associated with this tier.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
        /// <summary>
        /// Maximum number of patrons this tier is limited to, if applicable. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("user_limit")]
        public int? UserLimit { get; set; }
    }
}
