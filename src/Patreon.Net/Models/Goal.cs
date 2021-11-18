using System;
using Newtonsoft.Json;

namespace Patreon.Net.Models
{
    /// <summary>
    /// A funding goal in USD set by a creator on a campaign.
    /// </summary>
    [PatreonResource("goal")]
    public class Goal : PatreonResource<GoalRelationships>
    {
        /// <summary>
        /// Goal amount in USD cents.
        /// </summary>
        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }
        /// <summary>
        /// Equal to (pledge_sum/goal amount)*100.
        /// </summary>
        [JsonProperty("completed_percentage")]
        public int CompletedPercentage { get; set; }
        /// <summary>
        /// When the goal was created for the campaign.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        /// <summary>
        /// Goal description. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// When the campaign reached the goal. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("reached_at")]
        public DateTimeOffset? ReachedAt { get; set; }
        /// <summary>
        /// Goal title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class GoalRelationships
    {
        /// <summary>
        /// The campaign trying to reach the goal
        /// </summary>
        [JsonProperty("campaign")]
        public Campaign Campaign { get; set; }
    }
}
