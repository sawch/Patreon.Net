using System;
using Newtonsoft.Json;

namespace Patreon.Net.Models
{
    /// <summary>
    /// A patron's shipping address.
    /// </summary>
    [PatreonResource("address")]
    public class Address : PatreonResource<AddressRelationships>
    {
        /// <summary>
        /// The full recipient name. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("addressee")]
        public string Addressee { get; set; }
        /// <summary>
        /// The city this address is in.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }
        /// <summary>
        /// The country this address is in.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }
        /// <summary>
        /// The time this address was first created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        /// <summary>
        /// First line of street address. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("line_1")]
        public string Line1 { get; set; }
        /// <summary>
        /// Second line of street address. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("line_2")]
        public string Line2 { get; set; }
        /// <summary>
        /// Telephone number. Specified for non-US addresses. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Postal or zip code. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
        /// <summary>
        /// State or province name. Can be <see langword="null"/>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }
    }

    public class AddressRelationships
    {
        /// <summary>
        /// The campaigns that have access to the address.
        /// </summary>
        [JsonProperty("campaigns")]
        public Campaign[] Campaigns { get; set; }
        /// <summary>
        /// The user this address belongs to.
        /// </summary>
        [JsonProperty("user")]
        public User User { get; set; }
    }
}
