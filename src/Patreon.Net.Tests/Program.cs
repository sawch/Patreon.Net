using System;
using System.Threading.Tasks;

namespace Patreon.Net.Tests
{
    class Program
    {
        static async Task Main()
        {
            string accessToken = "";
            string refreshToken = "";
            string clientId = "";
            using var client = new PatreonClient(accessToken, refreshToken, clientId);

            Console.WriteLine($"Patreon.Net {PatreonClient.Version}\n\n");

            // Get Identity
            var user = await client.GetIdentityAsync(Includes.All).ConfigureAwait(false);
            Console.WriteLine($"Access token identity: {user.FirstName} {user.LastName} ({user.FullName}) (ID {user.Id})"); Console.WriteLine("\n\n");

            // Get Campaigns
            var campaigns = await client.GetCampaignsAsync(Includes.All).ConfigureAwait(false);
            string campaignId = null;
            if (campaigns != null)
            {
                Console.WriteLine($"Total of {campaigns.Meta.Pagination.Total} campaigns");
                await foreach (var campaign in campaigns)
                {
                    Console.WriteLine($"\tID {campaign.Id}, {campaign.CreationName}, {campaign.PatronCount} patrons");
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
            var singleCampaign = await client.GetCampaignAsync(campaignId, Includes.All).ConfigureAwait(false);
            Console.WriteLine($"Campaign {singleCampaign.PledgeUrl}: created at {singleCampaign.CreatedAt}, created by {singleCampaign.Relationships.Creator.FullName}");
            var tiers = singleCampaign.Relationships.Tiers;
            if (tiers != null && tiers.Length > 0)
            {
                foreach (var tier in tiers)
                    Console.WriteLine($"\tTier {tier.Id}: titled {tier.Title}, worth {tier.AmountCents} cents, has {tier.PatronCount} patrons");

                Console.WriteLine("\n\n");
            }

            // Get Campaign Members
            var members = await client.GetCampaignMembersAsync(campaignId, Includes.All).ConfigureAwait(false);
            string memberId = null;
            if (members != null)
            {
                Console.WriteLine($"Total of {members.Meta.Pagination.Total} members in campaign {campaignId}");
                await foreach (var member in members)
                {
                    Console.WriteLine($"\tMember \"{member.Id}\" {member.FullName} ({member.Email}): paid {member.LifetimeSupportCents} cents total, status {member.PatronStatus}, is free member? {member.IsFreeTrial}");
                    memberId ??= member.Id;
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
            Console.WriteLine($"Member \"{singleMember.Id}\" {singleMember.FullName} ({singleMember.Email}): paid {singleMember.CampaignLifetimeSupportCents} campaign currency, is free member? {singleMember.IsFreeTrial}, status ({singleMember.PatronStatus}), entitled to {singleMember.Relationships.Tiers?.Length.ToString() ?? "null"} tier(s)");
            var entitledTiers = singleMember.Relationships.Tiers;
            if (entitledTiers != null)
            {
                foreach (var tier in entitledTiers)
                    Console.WriteLine($"\tTier {tier.Id}: Titled {tier.Title} worth {tier.AmountCents} cents");
            }
            else
                Console.WriteLine("\tNo tiers");
        }
    }
}
