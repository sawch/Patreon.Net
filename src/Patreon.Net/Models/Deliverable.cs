using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Patreon.Net.Models
{
    /// <summary>
    /// The record of whether or not a patron has been delivered the benefit they are owed because of their member tier.
    /// </summary>
    public class Deliverable : PatreonResource<DeliverableRelationships>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum DeliveryStatusValue
        {
            [EnumMember(Value = "delivered")]
            Delivered,
            [EnumMember(Value = "not_delivered")]
            NotDelivered,
            [EnumMember(Value = "wont_deliver")]
            WontDeliver
        }

        /// <summary>
        /// When the creator marked the deliverable as completed or fulfilled to the patron. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("completed_at")]
        public DateTimeOffset? CompletedAt { get; set; }
        [JsonProperty("delivery_status")]
        public DeliveryStatusValue DeliveryStatus { get; set; }
        /// <summary>
        /// When the deliverable is due to the patron.
        /// </summary>
        [JsonProperty("due_at")]
        public DateTimeOffset DueAt { get; set; }
    }

    public class DeliverableRelationships
    {
        /// <summary>
        /// The Benefit the Deliverables were generated for.
        /// </summary>
        [JsonProperty("benefit")]
        public Benefit Benefit { get; set; }
        /// <summary>
        /// The Campaign the Deliverables were generated for.
        /// </summary>
        [JsonProperty("campaign")]
        public Campaign Campaign { get; set; }
        /// <summary>
        /// The member who has been granted the deliverable.
        /// </summary>
        [JsonProperty("member")]
        public Member Member { get; set; }
        /// <summary>
        /// The user who has been granted the deliverable. This user is the same as the member user.
        /// </summary>
        [JsonProperty("user")]
        public User User { get; set; }
    }
}
