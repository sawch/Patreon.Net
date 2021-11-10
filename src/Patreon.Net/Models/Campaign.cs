using System;
using Newtonsoft.Json;
using System.Reflection;

namespace Patreon.Net.Models
{
    /// <summary>
    /// The creator's page, and the top-level object for accessing lists of members, tiers, etc.
    /// </summary>
    [PatreonResource("campaign")]
    public class Campaign
    {
        public class Relationships
        {
            /// <summary>
            /// The campaign's benefits.
            /// </summary>
            [JsonProperty("benefits")]
            public ResourceArray<Benefit, Benefit.Relationships> Benefits { get; set; }
            /// <summary>
            /// Undocumented.
            /// </summary>
            //[JsonProperty("campaign_installations")]
            //public object[] CampaignInstallations { get; set; }
            /// <summary>
            /// The campaign's categories.
            /// </summary>
            //[JsonProperty("categories")]
            //public object[] Categories { get; set; }
            /// <summary>
            /// The campaign owner.
            /// </summary>
            [JsonProperty("creator")]
            public Resource<User, User.Relationships> Creator { get; set; }
            /// <summary>
            /// The campaign's goals.
            /// </summary>
            [JsonProperty("goals")]
            public ResourceArray<Goal, Goal.Relationships> Goals { get; set; }
            /// <summary>
            /// The campaign's tiers.
            /// </summary>
            [JsonProperty("tiers")]
            public ResourceArray<Tier, Tier.Relationships> Tiers { get; set; }
        }
        /// <summary>
        /// The time that the creator first began the campaign creation process. See <seealso cref="PublishedAt"/> for when it was published.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        /// <summary>
        /// The type of content the creator is creating, as in "vanity is creating creation_name". Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("creation_name")]
        public string CreationName { get; set; }
        /// <summary>
        /// The ID of the external Discord server that is linked to this campaign. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("discord_server_id")]
        public string DiscordServerId { get; set; }
        /// <summary>
        /// The ID of the Google Analytics tracker that the creator wants metrics to be sent to. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("google_analytics_id")]
        public string GoogleAnalyticsId { get; set; }
        /// <summary>
        /// Whether this user has opted-in to RSS feeds.
        /// </summary>
        [JsonProperty("has_rss")]
        public bool HasRss { get; set; }
        /// <summary>
        /// Whether or not the creator has sent a one-time RSS notification email.
        /// </summary>
        [JsonProperty("has_sent_rss_notify")]
        public bool HasSentRssNotify { get; set; }
        /// <summary>
        /// The URL for the campaign's profile image.
        /// </summary>
        [JsonProperty("image_small_url")]
        public string ImageSmallUrl { get; set; }
        /// <summary>
        /// The banner image URL for the campaign.
        /// </summary>
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        /// <summary>
        /// Whether or not the campaign charges upfront. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("is_charged_immediately")]
        public bool? IsChargedImmediately { get; set; }
        /// <summary>
        /// Whether or not campaign charges per month. If <see langword="false"/>, the campaign charges per-post.
        /// </summary>
        [JsonProperty("is_monthly")]
        public bool IsMonthly { get; set; }
        /// <summary>
        /// Whether or not the creator has marked the campaign as containing NSFW content.
        /// </summary>
        [JsonProperty("is_nsfw")]
        public bool IsNsfw { get; set; }
        /// <summary>
        /// Undocumented. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("main_video_embed")]
        public string MainVideoEmbed { get; set; }
        /// <summary>
        /// Undocumented. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("main_video_url")]
        public string MainVideoUrl { get; set; }
        /// <summary>
        /// Pithy one-liner for this campaign, displayed on the creator page. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("one_liner")]
        public string OneLiner { get; set; }
        /// <summary>
        /// The number of patrons pledging to this creator.
        /// </summary>
        [JsonProperty("patron_count")]
        public int PatronCount { get; set; }
        /// <summary>
        /// The thing which patrons are paying per, as in "vanity is making $1000 per <see cref="PayPerName"/>". Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("pay_per_name")]
        public string PayPerName { get; set; }
        /// <summary>
        /// Relative (to patreon.com) URL for the pledge checkout flow for this campaign.
        /// </summary>
        [JsonProperty("pledge_url")]
        public string PledgeUrl { get; set; }
        /// <summary>
        /// The time that the creator most recently published (made publicly visible) the campaign. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("published_at")]
        public DateTimeOffset? PublishedAt { get; set; }
        /// <summary>
        /// The URL for the RSS album artwork. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("rss_artwork_url")]
        public string RssArtworkUrl { get; set; }
        /// <summary>
        /// The title of the campaigns RSS feed.
        /// </summary>
        [JsonProperty("rss_feed_title")]
        public string RssFeedTitle { get; set; }
        /// <summary>
        /// Whether or not the campaign's total earnings are shown publicly.
        /// </summary>
        [JsonProperty("show_earnings")]
        public bool ShowEarnings { get; set; }
        /// <summary>
        /// The creator's summary of their campaign. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("summary")]
        public string Summary { get; set; }
        /// <summary>
        /// Undocumented. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("thanks_embed")]
        public string ThanksEmbed { get; set; }
        /// <summary>
        /// Thank you message shown to patrons after they pledge to this campaign. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("thanks_msg")]
        public string ThanksMsg { get; set; }
        /// <summary>
        /// The URL for the video shown to patrons after they pledge to this campaign. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("thanks_video_url")]
        public string ThanksVideoUrl { get; set; }
        /// <summary>
        /// The URL to access this campaign on patreon.com.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
        /// <summary>
        /// The campaign's vanity. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("vanity")]
        public string Vanity { get; set; }
    }
}
