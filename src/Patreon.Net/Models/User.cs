using System;
using Newtonsoft.Json;

namespace Patreon.Net.Models
{
    /// <summary>
    /// A Patreon user, which can be both patron and creator.
    /// </summary>
    [PatreonResource("user")]
    public class User : PatreonResource<UserRelationships>
    {
        /// <summary>
        /// The user's about text, which appears on their profile. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("about")]
        public string About { get; set; }
        /// <summary>
        /// Whether or not this user can view NSFW content. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("can_see_nsfw")]
        public bool? CanSeeNsfw { get; set; }
        /// <summary>
        /// The time of this user's account creation.
        /// </summary>
        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }
        /// <summary>
        /// The user's email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
        /// <summary>
        /// First name. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        /// <summary>
        /// Combined first and last name.
        /// </summary>
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        /// <summary>
        /// Whether or not the user has chosen to keep private which creators they pledge to. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("hide_pledges")]
        public bool? HidePledges { get; set; }
        /// <summary>
        /// The user's profile picture URL, scaled to width 400px.
        /// </summary>
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        /// <summary>
        /// Whether or not the user has confirmed their email.
        /// </summary>
        [JsonProperty("is_email_verified")]
        public bool IsEmailVerified { get; set; }
        /// <summary>
        /// Last name. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        /// <summary>
        /// How many posts this user has liked.
        /// </summary>
        [JsonProperty("like_count")]
        public int LikeCount { get; set; }
        /// <summary>
        /// Mapping from user's connected app names to external user Id on the respective app.<para/>
        /// This type lacks official documentation so most unknown types have been defaulted to <see cref="object"/>.
        /// </summary>
        [JsonProperty("social_connections")]
        public SocialConnections SocialConnections { get; set; }
        /// <summary>
        /// The user's profile picture URL, scaled to a square of size 100x100px.
        /// </summary>
        [JsonProperty("thumb_url")]
        public string ThumbUrl { get; set; }
        /// <summary>
        /// URL of this user's creator or patron profile.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
        /// <summary>
        /// Deprecated: The public "username" of the user. Non-creator users might not have a vanity. [Deprecated! use campaign.vanity] Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("vanity")]
        public string Vanity { get; set; }
    }

    public class UserRelationships
    {
        /// <summary>
        /// Undocumented in the official documentation. 
        /// </summary>
        [JsonProperty("campaign")]
        public Campaign Campaign { get; set; }
        /// <summary>
        /// Usually a zero or one-element array with the user's membership to the token creator's campaign, if they are a member.
        /// </summary>
        [JsonProperty("memberships")]
        public Member[] Memberships { get; set; }
    }
}
