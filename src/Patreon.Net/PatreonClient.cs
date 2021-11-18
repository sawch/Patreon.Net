using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Patreon.Net.Models;

namespace Patreon.Net
{
    /// <summary>
    /// Provides a class for interacting with the Patreon V2 API using an <see cref="HttpClient"/>.
    /// </summary>
    public class PatreonClient : IDisposable
    {
        private readonly JsonSerializer jsonSerializer;
        private readonly HttpClient httpClient;
        private readonly string clientId;
        private OAuthToken oAuthToken;
        private DateTimeOffset oAuthTokenExpirationDate;

        /// <summary>
        /// Occurs when a new OAuth token has been acquired. The previous access and refresh tokens are invalidated and replaced prior to this event.
        /// </summary>
        public event TokenRefreshedAsyncEvent TokensRefreshedAsync;
        public delegate Task TokenRefreshedAsyncEvent(OAuthToken token);

        /// <summary>
        /// The version of the Patreon.Net library, such as "0.9.0".
        /// </summary>
        public static string Version { get; } = typeof(PatreonClient).Assembly.GetName().Version.ToString(3) ?? "Unknown";

        /// <summary>
        /// Creates a new <see cref="PatreonClient"/> with an unknown token expiration date.
        /// </summary>
        /// <param name="accessToken">The access token of the API client, typically the Creator's Access Token in the Patreon developer portal.</param>
        /// <param name="refreshToken">The refresh token of the API client, typically the Creator's Refresh Token in the Patreon developer portal.</param>
        /// <param name="clientId">The client ID of the API client found in the Patreon developer portal.</param>
        public PatreonClient(string accessToken, string refreshToken, string clientId) : this(accessToken, refreshToken, clientId, DateTimeOffset.UtcNow.AddDays(31.0)) { }

        /// <summary>
        /// Creates a new <see cref="PatreonClient"/> with a known token expiration date.
        /// </summary>
        /// <param name="accessToken">The access token of the API client, typically the Creator's Access Token in the Patreon developer portal.</param>
        /// <param name="refreshToken">The refresh token of the API client, typically the Creator's Refresh Token in the Patreon developer portal.</param>
        /// <param name="clientId">The client ID of the API client found in the Patreon developer portal.</param>
        /// <param name="tokenExpirationDate">The expiration date of the current access token.</param>
        public PatreonClient(string accessToken, string refreshToken, string clientId, DateTimeOffset tokenExpirationDate)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentNullException(nameof(accessToken));

            jsonSerializer = new JsonSerializer()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            httpClient = new HttpClient(new SocketsHttpHandler() { UseCookies = false, AllowAutoRedirect = false, }, true)
            {
                BaseAddress = new Uri(Endpoints.Hostname),
            };

            oAuthToken = new OAuthToken() { AccessToken = accessToken, RefreshToken = refreshToken };
            oAuthTokenExpirationDate = tokenExpirationDate;

            this.clientId = clientId;
            var headers = httpClient.DefaultRequestHeaders;
            headers.Add("Authorization", "Bearer " + accessToken);
            headers.Add("Accept", "application/json");
            headers.UserAgent.Add(new ProductInfoHeaderValue("Patreon.Net", Version));
        }

        private async Task<bool> RefreshTokenAsync()
        {
            string requestUri = Endpoints.Token.RefreshToken(oAuthToken.RefreshToken, clientId);
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            var statusCodeNumber = (int)response.StatusCode;
            if (statusCodeNumber >= 200 && statusCodeNumber < 300)
            {
                var httpContent = response.Content;
                if (httpContent != null)
                {
                    string content = await httpContent.ReadAsStringAsync().ConfigureAwait(false);
                    if (content != null && content.Length > 0)
                    {
                        OAuthToken newToken = JObject.Parse(content).ToObject<OAuthToken>(jsonSerializer);
                        oAuthToken = newToken;
                        oAuthTokenExpirationDate = DateTimeOffset.UtcNow.AddSeconds(newToken.ExpiresIn);

                        var headers = httpClient.DefaultRequestHeaders;
                        headers.Remove("Authorization");
                        headers.Add("Authorization", "Bearer " + newToken.AccessToken);

                        if (TokensRefreshedAsync != null)
                            _ = TokensRefreshedAsync(newToken);
                        return true;
                    }
                }
            }
            return false;
        }

        private async Task<T> GetAsync<T>(string requestUri, bool isRetry = false) where T : class
        {
            if(!isRetry && DateTimeOffset.UtcNow >= oAuthTokenExpirationDate && !string.IsNullOrEmpty(oAuthToken.RefreshToken))
            {
                if(!await RefreshTokenAsync().ConfigureAwait(false))
                    throw new PatreonApiException("Failed to refresh OAuth token. Please verify that you have supplied a valid token.");
                isRetry = true; // Consider this a retry if we just refreshed the token, to avoid attempting refresh twice
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            var statusCodeNumber = (int)response.StatusCode;
            if (statusCodeNumber >= 200 && statusCodeNumber < 300)
            {
                var httpContent = response.Content;
                if (httpContent != null)
                {
                    string content = await httpContent.ReadAsStringAsync().ConfigureAwait(false);
                    if (content != null && content.Length > 0)
                    {
                        if (JsonPostprocessor.Process(JObject.Parse(content), out JToken dataToken))
                            return dataToken.ToObject<T>(jsonSerializer);
                    }
                }
                throw new PatreonApiException($"Patreon returned an indecipherable response format (\"{requestUri}\")");
            }
            else if (!isRetry && await ShouldAttemptRetryOrThrowAsync(response).ConfigureAwait(false))
            {
                return await GetAsync<T>(requestUri, true).ConfigureAwait(false);
            }
            return null;
        }

        private async Task<bool> ShouldAttemptRetryOrThrowAsync(HttpResponseMessage response)
        {
            int statusCodeNumber = (int)response.StatusCode;
            switch (statusCodeNumber)
            {
                case 401: { return await RefreshTokenAsync().ConfigureAwait(false); }
                case 404: { return false; }
                case 410: { throw new PatreonApiException("Gone -- The resource requested has been removed from our servers."); }
                case 429: { throw new PatreonApiException("Too Many Requests -- Slow down!"); }
                case 503: { throw new PatreonApiException("Patreon is temporarily offline for maintenance. Please try again later."); }
                default:
                    {
                        var httpContent = response.Content;
                        if (httpContent != null)
                        {
                            string content = await httpContent.ReadAsStringAsync().ConfigureAwait(false);
                            if (content != null && content.Length > 0)
                            {
                                try
                                {
                                    var apiErrors = JObject.Parse(content).ToObject<ApiErrors>(jsonSerializer);
                                    if (apiErrors.Error != null)
                                    {
                                        throw new PatreonApiException($"Patreon returned an error: {apiErrors.Error}");
                                    }
                                    else if (apiErrors.Errors != null)
                                    {
                                        var errors = apiErrors.Errors;
                                        int totalLength = 0;
                                        for (int i = 0; i < errors.Length; i++)
                                        {
                                            var error = errors[i];
                                            totalLength += error.Status.Length + error.Detail.Length + 1; // + space
                                        }
                                        if (totalLength > 0)
                                        {
                                            StringBuilder errorMessages = new(totalLength);
                                            for (int i = 0; i < errors.Length; i++)
                                            {
                                                var error = errors[i];
                                                errorMessages.Append(error.Status); errorMessages.Append(' ');
                                                errorMessages.Append(error.Detail); errorMessages.Append('\n');
                                            }
                                            throw new PatreonApiException($"Patreon returned error(s): {errorMessages}");
                                        }
                                    }
                                }
                                catch (JsonReaderException) { throw new PatreonApiException($"Patreon returned an unsuccessful status code: {response.StatusCode} ({statusCodeNumber}), {content}"); }
                            }
                        }
                        throw new PatreonApiException($"Patreon returned an unsuccessful status code: {response.StatusCode} ({statusCodeNumber})");
                    }
            }
        }

        /// <summary>
        /// Gets the user resource of the account who owns the access token used to create this <see cref="PatreonClient"/>.
        /// </summary>
        /// <param name="includes">The desired resources to be included on the returned <see cref="User"/>.</param>
        /// <returns>A user resource.</returns>
        /// <exception cref="PatreonApiException"/>
        public async Task<User> GetIdentityAsync(Includes includes = Includes.None)
        {
            return await GetAsync<User>(Endpoints.Identity.GetIdentity(includes)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the campaigns owned by the authorized user. If more than 1 total pages in the resource array's <see cref="Meta"/> property, get next pages via <see cref="GetCampaignsAsync(string, Includes)"/>.
        /// </summary>
        /// <param name="includes">The desired resources to be included on the <see cref="Campaign"/> objects in the returned array.</param>
        /// <returns>A resource array of campaigns, or <see langword="null"/> if none is found.</returns>
        /// <exception cref="PatreonApiException"/>
        public async Task<PatreonResourceArray<Campaign, CampaignRelationships>> GetCampaignsAsync(Includes includes = Includes.None)
        {
            return await GetAsync<PatreonResourceArray<Campaign, CampaignRelationships>>(Endpoints.Campaigns.GetCampaigns(includes)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a specific page of campaigns owned by the authorized user. Use <see cref="GetCampaignsAsync(Includes)"/> first, then this if the initial response has more than 1 page.
        /// </summary>
        /// <param name="nextPageCursor">The page cursor found in the <see cref="Meta.Pagination"/> property found on the previous <see cref="PatreonResourceArray{T, U}"/> of <see cref="Campaign"/> objects.</param>
        /// <param name="includes">The desired resources to be included on the <see cref="Campaign"/> objects in the returned array.</param>
        /// <returns>A resource array of campaigns, or <see langword="null"/> if none is found.</returns>
        /// <exception cref="PatreonApiException"/>
        public async Task<PatreonResourceArray<Campaign, CampaignRelationships>> GetCampaignsAsync(string nextPageCursor, Includes includes = Includes.None)
        {
            if (string.IsNullOrWhiteSpace(nextPageCursor))
                throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(nextPageCursor));

            return await GetAsync<PatreonResourceArray<Campaign, CampaignRelationships>>(Endpoints.Page(Endpoints.Campaigns.GetCampaigns(includes), nextPageCursor)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a specific campaign owned by the authorized user, by ID.
        /// </summary>
        /// <param name="campaignId">The ID of the campaign to fetch.</param>
        /// <param name="includes">The desired resources to be included on the returned <see cref="Campaign"/>.</param>
        /// <returns>The campaign, or <see langword="null"/> if none is found.</returns>
        /// <exception cref="PatreonApiException"/>
        public async Task<Campaign> GetCampaignAsync(string campaignId, Includes includes = Includes.None)
        {
            if (string.IsNullOrWhiteSpace(campaignId))
                throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(campaignId));

            return (await GetAsync<Campaign>(Endpoints.Campaigns.GetCampaign(campaignId, includes)).ConfigureAwait(false));
        }

        /// <summary>
        /// Gets the members for a given campaign owned by the authorized user. If more than 1 total pages in the resource array's <see cref="Meta"/> property, get next pages via <see cref="GetCampaignMembersAsync(string, string, Includes)"/>.
        /// </summary>
        /// <param name="campaignId">The ID of the campaign to fetch the members of.</param>
        /// <param name="includes">The desired resources to be included on the <see cref="Member"/> objects in the returned array.</param>
        /// <returns>A resource array of members, or <see langword="null"/> if no campaign with the given <paramref name="campaignId"/> is found.</returns>
        /// <exception cref="PatreonApiException"/>
        public async Task<PatreonResourceArray<Member, MemberRelationships>> GetCampaignMembersAsync(string campaignId, Includes includes = Includes.None)
        {
            if (string.IsNullOrWhiteSpace(campaignId))
                throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(campaignId));

            return await GetAsync<PatreonResourceArray<Member, MemberRelationships>>(Endpoints.Campaigns.GetCampaignMembers(campaignId, includes)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the members for a given campaign owned by the authorized user. Use <see cref="GetCampaignMembersAsync(string, Includes)"/> first, then this if the initial response has more than 1 page.
        /// </summary>
        /// <param name="campaignId">The ID of the campaign to fetch the members of.</param>
        /// <param name="nextPageCursor">The page cursor found in the <see cref="Meta.Pagination"/> property found on the previous <see cref="PatreonResourceArray{T, U}"/> of <see cref="Member"/> objects.</param>
        /// <param name="includes">The desired resources to be included on the <see cref="Member"/> objects in the returned array.</param>
        /// <returns>A resource array of members, or <see langword="null"/> if no campaign with the given <paramref name="campaignId"/> is found.</returns>
        /// <exception cref="PatreonApiException"/>
        public async Task<PatreonResourceArray<Member, MemberRelationships>> GetCampaignMembersAsync(string campaignId, string nextPageCursor, Includes includes = Includes.None)
        {
            if (string.IsNullOrWhiteSpace(campaignId))
                throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(campaignId));

            return await GetAsync<PatreonResourceArray<Member, MemberRelationships>>(Endpoints.Page(Endpoints.Campaigns.GetCampaignMembers(campaignId, includes), nextPageCursor)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a specific member by ID, belonging to any campaign owned by the authorized user.
        /// </summary>
        /// <param name="memberId">The ID of the member to fetch.</param>
        /// <param name="includes">The desired resources to be included on the returned <see cref="Member"/>.</param>
        /// <returns>A member resource, or <see langword="null"/> if none is found.</returns>
        /// <exception cref="PatreonApiException"/>
        public async Task<Member> GetMemberAsync(string memberId, Includes includes = Includes.None)
        {
            if (string.IsNullOrWhiteSpace(memberId))
                throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(memberId));

            return (await GetAsync<Member>(Endpoints.Members.GetMember(memberId, includes)).ConfigureAwait(false));
        }

        #region IDisposable Implementation

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    httpClient.Dispose();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion // IDisposable Implementation
    }
}
