using System;
using Newtonsoft.Json;

namespace Patreon.Net.Models
{
    /// <summary>
    /// An OAuth2 token used for authentication with the Patreon API.
    /// </summary>
    public class OAuthToken
    {
        /// <summary>
        /// The access token. Use this in construction of a new <see cref="PatreonClient"/> in the future.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        /// <summary>
        /// The time in seconds in which this token will expire. Use this to calculate a <see cref="DateTimeOffset"/> for construction of a new <see cref="PatreonClient"/> in the future.
        /// </summary>
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
        /// <summary>
        /// The type of this token.
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        /// <summary>
        /// The scope of this token.
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }
        /// <summary>
        /// The refresh token. Use this in construction of a new <see cref="PatreonClient"/> in the future.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        /// <summary>
        /// The version of this token.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
