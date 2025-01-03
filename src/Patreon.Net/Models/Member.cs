using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Patreon.Net.Models
{
    /// <summary>
    /// The record of a user's membership to a campaign.
    /// </summary>
    [PatreonResource("member")]
    public class Member : PatreonResource<MemberRelationships>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PatronStatusValue
        {
            NeverPledged,
            [EnumMember(Value = "active_patron")]
            ActivePatron,
            [EnumMember(Value = "declined_patron")]
            DeclinedPatron,
            [EnumMember(Value = "former_patron")]
            FormerPatron
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum ChargeStatusValue
        {
            NeverCharged,
            [EnumMember(Value = "Paid")]
            Paid,
            [EnumMember(Value = "Declined")]
            Declined,
            [EnumMember(Value = "Deleted")]
            Deleted,
            [EnumMember(Value = "Pending")]
            Pending,
            [EnumMember(Value = "Refunded")]
            Refunded,
            [EnumMember(Value = "Fraud")]
            Fraud,
            [EnumMember(Value = "Refunded by Patreon")]
            RefundedByPatreon,
            [EnumMember(Value = "Other")]
            Other,
            [EnumMember(Value = "Partially Refunded")]
            PartiallyRefunded,
            [EnumMember(Value = "Free Trial")]
            FreeTrial
        }

        /// <summary>
        /// The total amount that the member has ever paid to the campaign in campaign's currency. 0 if never paid.
        /// </summary>
        [JsonProperty("campaign_lifetime_support_cents")]
        public int CampaignLifetimeSupportCents { get; set; }
        /// <summary>
        /// The amount in cents that the member is entitled to. This includes a current pledge, or payment that covers the current payment period.
        /// </summary>
        [JsonProperty("currently_entitled_amount_cents")]
        public int CurrentlyEntitledAmountCents { get; set; }
        /// <summary>
        /// The member's email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
        /// <summary>
        /// Full name of the member user.
        /// </summary>
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        /// <summary>
        /// The user is not a pledging patron but has subscribed to updates about public posts.
        /// </summary>
        [JsonProperty("is_follower"), Obsolete("This will always be false, following has been replaced by free membership.")]
        public bool IsFollower { get; set; }
        /// <summary>
        /// Whether the user is in a free trial period.
        /// </summary>
        [JsonProperty("is_free_trial")]
        public bool IsFreeTrial { get; set; }
        /// <summary>
        /// Whether the user's membership is from a free gift.
        /// </summary>
        [JsonProperty("is_gifted")]
        public bool IsGifted { get; set; }
        /// <summary>
        /// The time of last attempted charge. Can be <see langword="null"/> if never charged.
        /// </summary>
        [JsonProperty("last_charge_date")]
        public DateTimeOffset? LastChargeDate { get; set; }
        /// <summary>
        /// The result of the last attempted charge. The only successful status is <see cref="ChargeStatusValue.Paid"/>.
        /// </summary>
        [JsonProperty("last_charge_status", DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(ChargeStatusValue.NeverCharged)]
        public ChargeStatusValue LastChargeStatus { get; set; }
        /// <summary>
        /// The total amount that the member has ever paid to the campaign. 0 if never paid.
        /// </summary>
        [JsonProperty("lifetime_support_cents")]
        public int LifetimeSupportCents { get; set; }
        /// <summary>
        /// The time of next charge. Can be <see langword="null"/> in case of annual pledge downgrade.
        /// </summary>
        [JsonProperty("next_charge_date")]
        public DateTimeOffset? NextChargeDate { get; set; }
        /// <summary>
        /// The creator's notes on the member.
        /// </summary>
        [JsonProperty("note")]
        public string Note { get; set; }
        /// <summary>
        /// The current status of the member.
        /// </summary>
        [JsonProperty("patron_status", DefaultValueHandling = DefaultValueHandling.Populate), DefaultValue(PatronStatusValue.NeverPledged)]
        public PatronStatusValue PatronStatus { get; set; }
        /// <summary>
        /// The number of months between charges.
        /// </summary>
        [JsonProperty("pledge_cadence")]
        public int PledgeCadence { get; set; }
        /// <summary>
        /// The time marking the beginning of the most recent pledge chain from this member to the campaign. Pledge updates do not change this value. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("pledge_relationship_start")]
        public DateTimeOffset? PledgeRelationshipStart { get; set; }
        /// <summary>
        /// The amount in cents the user will pay at the next pay cycle.
        /// </summary>
        [JsonProperty("will_pay_amount_cents")]
        public int WillPayAmountCents { get; set; }
    }

    public class MemberRelationships
    {
        /// <summary>
        /// The member's shipping address that they entered for the campaign.
        /// </summary>
        [JsonProperty("address")]
        public Address Address { get; set; }
        /// <summary>
        /// The campaign that the membership is for.
        /// </summary>
        [JsonProperty("campaign")]
        public Campaign Campaign { get; set; }
        /// <summary>
        /// The tiers that the member is entitled to. This includes a current pledge, or payment that covers the current payment period.
        /// </summary>
        [JsonProperty("currently_entitled_tiers")]
        public Tier[] Tiers { get; set; }
        /// <summary>
        /// The pledge history of the member.
        /// </summary>
        [JsonProperty("pledge_history")]
        public PledgeEvent[] PledgeHistory { get; set; }
        /// <summary>
        /// The user who is pledging to the campaign.
        /// </summary>
        [JsonProperty("user")]
        public User User { get; set; }
    }
}
