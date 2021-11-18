using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Patreon.Net.Models
{
    /// <summary>
    /// A Patreon resource that is identifiable by ID and type.
    /// </summary>
    public abstract class PatreonResource
    {
        /// <summary>
        /// The ID of the resource.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// The type of the resource, such as "user" or "member".
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    /// <summary>
    /// A Patreon resource that is identifiable by ID and type and may contain relationships.
    /// </summary>
    /// <typeparam name="TRelationships">The relationships of the resource that is being returned, such as <see cref="MemberRelationships"/> or <see cref="CampaignRelationships"/>.</typeparam>
    public abstract class PatreonResource<TRelationships> where TRelationships : class
    {
        /// <summary>
        /// The ID of the resource.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// The type of the resource, such as "user" or "member".
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The resource's relationships which may be <see langword="null"/>, have <see langword="null"/> values or be populated depending on the <see cref="Includes"/> specified in the request that got this resource.<para/>
        /// This will always be <see langword="null"/> if you are accessing this from inside a resource that is already nested in another <see cref="Relationships"/> property.
        /// </summary>
        [JsonProperty("relationships")]
        public TRelationships Relationships { get; set; }
    }

    /// <summary>
    /// An array of Patreon resources that are identifiable by ID and type and may contain relationships, including additional information used for traversing pages.
    /// </summary>
    /// <typeparam name="TResource">The resource that is being returned, such as <see cref="Member"/> or <see cref="Campaign"/>.</typeparam>
    /// <typeparam name="TRelationships">The relationships of the resource that is being returned, such as <see cref="MemberRelationships"/> or <see cref="CampaignRelationships"/>.</typeparam>
    public sealed class PatreonResourceArray<TResource, TRelationships> where TResource : PatreonResource<TRelationships> where TRelationships : class
    {
        /// <summary>
        /// The array of resources that were returned.
        /// </summary>
        [JsonProperty("data")]
        public TResource[] Resources { get; set; }

        /// <summary>
        /// Additional information returned with the request, which at this time is only used for pagination.
        /// </summary>
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }

    public sealed class Meta
    {
        /// <summary>
        /// Information used for traversing pages.
        /// </summary>
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public sealed class Pagination
    {
        [JsonProperty("cursors")]
        public Cursor Cursor { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public sealed class Cursor
    {
        /// <summary>
        /// The cursor for the next page. Use this in subsequent requests to fetch additional pages of a resource.
        /// </summary>
        [JsonProperty("next")]
        public string Next { get; set; }
    }
}
