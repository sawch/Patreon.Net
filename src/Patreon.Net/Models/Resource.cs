using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Patreon.Net.Models
{
    public sealed class ResourceData<T, U>
    {
        /// <summary>
        /// The attributes of the resource being returned, which is the resource itself.
        /// </summary>
        [JsonProperty("attributes")]
        public T Attributes { get; set; }
        /// <summary>
        /// The resource's relationships which may be null, have null values or be populated depending on the <see cref="Includes"/> specified in the request that got this resource.
        /// </summary>
        [JsonProperty("relationships")]
        public U Relationships { get; set; }
        /// <summary>
        /// The ID of the resource.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// The type of the resource, such as "user".
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    /// <summary>
    /// The root object of a JSON response from the API.
    /// </summary>
    /// <typeparam name="T">The Resource that is being returned, such as <see cref="Member"/> or <see cref="Campaign"/>.</typeparam>
    /// <typeparam name="U">The relationship that is being returned, such as <see cref="Member.Relationships"/> or <see cref="Campaign.Relationships"/>.</typeparam>
    public sealed class Resource<T, U>
    {
        [JsonProperty("data")]
        public ResourceData<T, U> Data { get; set; }
    }

    /// <summary>
    /// The root object of a JSON response from the API.
    /// </summary>
    /// <typeparam name="T">The Resource that is being returned, such as <see cref="Member"/> or <see cref="Campaign"/>.</typeparam>
    /// <typeparam name="U">The relationship that is being returned, such as <see cref="Member.Relationships"/> or <see cref="Campaign.Relationships"/>.</typeparam>
    public sealed class ResourceArray<T, U>
    {
        [JsonProperty("data")]
        public ResourceData<T, U>[] Data { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }

    public sealed class Meta
    {
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
        [JsonProperty("next")]
        public string Next { get; set; }
    }
}
