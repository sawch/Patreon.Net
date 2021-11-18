namespace Patreon.Net
{
    internal static class Endpoints
    {
        internal const string Hostname = "https://www.patreon.com";

        internal static class Token
        {
            private const string Url = "/api/oauth2/token";

            public static string RefreshToken(string refreshToken, string clientId) => Url + $"?grant_type=refresh_token&refresh_token={refreshToken}&client_id={clientId}";
        }
        internal static class Identity
        {
            private const string Url = "/api/oauth2/v2/identity";
            private const Includes SupportedIncludes = Includes.Memberships | Includes.Campaign;

            public static string GetIdentity(Includes includes) => Url + "?" + UrlHelper.Generate(typeof(Models.User), includes & SupportedIncludes);
        }
        internal static class Campaigns
        {
            private const string Url = "/api/oauth2/v2/campaigns";
            private const Includes SupportedIncludesCampaigns = Includes.Tiers | Includes.Creator | Includes.Benefits | Includes.Goals;
            private const Includes SupportedIncludesCampaignMembers = Includes.Address | Includes.Campaign | Includes.CurrentlyEntitledTiers | Includes.User;

            public static string GetCampaigns(Includes includes) => 
                Url + "?" + UrlHelper.Generate(typeof(Models.Campaign), includes & SupportedIncludesCampaigns);
            
            public static string GetCampaign(string campaignId, Includes includes) => 
                Url + "/" + campaignId + "?" + UrlHelper.Generate(typeof(Models.Campaign), includes & SupportedIncludesCampaigns);
            
            public static string GetCampaignMembers(string campaignId, Includes includes) => 
                Url + "/" + campaignId + "/members?" + UrlHelper.Generate(typeof(Models.Member), includes & SupportedIncludesCampaignMembers);
        }
        internal static class Members
        {
            private const string Url = "/api/oauth2/v2/members/";
            private const Includes SupportedIncludes = Includes.Address | Includes.Campaign | Includes.CurrentlyEntitledTiers | Includes.User;

            public static string GetMember(string memberId, Includes includes) => Url + memberId + "?" + UrlHelper.Generate(typeof(Models.Member), includes & SupportedIncludes);
        }

        /// <summary>
        /// Appends an endpoint URL with a page cursor.
        /// </summary>
        /// <param name="url">An endpoint URL, like <see cref="Campaigns.AllCampaignFields"/></param>
        /// <param name="pageCursor">The next page cursor, found in <see cref="Models.PatreonResourceArray{T}.Meta"/>.</param>
        internal static string Page(string url, string pageCursor) => url + "&page%5Bcursor%5D=" + pageCursor;
    }
}
