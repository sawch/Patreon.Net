using System;
using System.Threading.Tasks;

namespace Patreon.Net.Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string accessToken = "";
            string refreshToken = "";
            string clientId = "";
            using var client = new PatreonClient(accessToken, refreshToken, clientId);
            Console.WriteLine($"Patreon.Net {PatreonClient.Version}\n\n");

            // Get Identity
            var user = await client.GetIdentityAsync(Includes.All).ConfigureAwait(false);
            Console.WriteLine($"Access token identity: {user.Attributes.FullName} (ID {user.Id})"); Console.WriteLine("\n\n");

            // Get Campaigns
            var campaigns = await client.GetCampaignsAsync(Includes.All).ConfigureAwait(false);
            string campaignId = null;
            Console.WriteLine($"{campaigns.Data.Length} campaigns across {campaigns.Meta.Pagination.Total} pages");
            if (campaigns.Data.Length > 0)
            {
                foreach (var campaign in campaigns.Data)
                {
                    Console.WriteLine($"\tID {campaign.Id}, {campaign.Attributes.CreationName}, {campaign.Attributes.PatronCount} patrons");
                    if (campaignId == null)
                        campaignId = campaign.Id;
                }
                Console.WriteLine("\n\n");
            }
            else
            {
                Console.WriteLine("No campaigns found, exiting"); 
                return;
            }

            // Get Campaign by ID
            var singleCampaign = (await client.GetCampaignAsync(campaignId, Includes.All).ConfigureAwait(false));
            Console.WriteLine($"Campaign created at {singleCampaign.Attributes.CreatedAt}, {singleCampaign.Attributes.PledgeUrl}, created by {singleCampaign.Relationships.Creator.Data.Attributes.FullName}");
            var tiers = singleCampaign.Relationships.Tiers;
            if (tiers != null && tiers.Data.Length > 0)
            {
                var tierData = tiers.Data;
                for (int i = 0; i < tierData.Length; i++)
                {
                    var tier = tierData[i];
                    Console.WriteLine($"\tTier {tier.Id}, titled {tier.Attributes.Title}, worth {tier.Attributes.AmountCents} cents, has {tier.Attributes.PatronCount} patrons");
                }
            }
            Console.WriteLine("\n\n");

            // Get Campaign Members
            var members = await client.GetCampaignMembersAsync(campaignId, Includes.All).ConfigureAwait(false);
            string memberId = null;
            if (members.Data.Length > 0)
            {
                Console.WriteLine($"{members.Data.Length} members in campaign {campaignId} across {members.Meta.Pagination.Total} pages");
                foreach (var member in members.Data)
                {
                    Console.WriteLine($"\tID {member.Id}, {member.Attributes.FullName}, {member.Attributes.Email}, {member.Attributes.LifetimeSupportCents} paid, {member.Attributes.PatronStatus}");
                    if (memberId == null)
                        memberId = member.Id;
                }
                Console.WriteLine("\n\n");
            }
            else
            {
                Console.WriteLine("No members found, exiting");
                return;
            }

            // Get Member by ID
            var singleMember = await client.GetMemberAsync(memberId, Includes.All).ConfigureAwait(false);
            Console.WriteLine($"Got single member {singleMember.Attributes.FullName}, {singleMember.Attributes.PatronStatus} has contributed {singleMember.Attributes.CampaignLifetimeSupportCents} cents total, entitled to {singleMember.Relationships.Tiers?.Data.Length.ToString() ?? "null"} tiers");
            var entitledTiers = singleMember.Relationships.Tiers;
            if (entitledTiers != null)
            {
                var data = entitledTiers.Data;
                for(int i=0; i<data.Length; i++)
                {
                    var tier = data[i];
                    Console.WriteLine($"Tier {tier.Id}");
                    Console.WriteLine($"{tier.Attributes.AmountCents} cents, titled {tier.Attributes.Title}");
                }
            }
            else
            {
                Console.WriteLine("No tiers");
                return;
            }
        }
    }
}
