## Patreon.Net
[![Nuget](https://img.shields.io/nuget/v/Patreon.Net?color=%23ff424d)](https://www.nuget.org/packages/Patreon.Net)

A .NET implementation of the Patreon V2 API, targeting **.NET 5** and **.NET 6**.

## Usage
1. Create an application on the [Patreon developer portal](https://www.patreon.com/portal/registration/register-clients) and grab your access token, refresh token and client ID.
2. Create a `PatreonClient` with your tokens and client ID:
```csharp
var client = new PatreonClient("access token", "refresh token", "client id");

var campaigns = await client.GetCampaignsAsync(Includes.All);
await foreach(var campaign in campaigns)
    Console.WriteLine($"{campaign.CreationName} has {campaign.PatronCount} patrons");

client.Dispose();
```

## Notes
This implementation doesn't cover 100% of the API at this time but is in a fully working state (see implemented endpoints below).
Basically, everything but webhooks and getting creator's posts is fully implemented.

### Token refreshing
Patreon.Net automatically handles token refreshing and exposes an event containing the newly generated access and refresh tokens to store for later, in case you need to restart your application:
```csharp
client.TokensRefreshedAsync += OnTokensRefreshedAsync;

private async Task OnTokensRefreshedAsync(OAuthToken token)
{
    await SaveNewTokensForLater(token);
}
```
In case of tokens expiring while your application is offline, you'll have to manually grab them from the website again.

### Paged resources
Certain endpoints return arrays of resources which can be paged if there are too many resources to fit in a single request, such as `PatreonClient.GetCampaignMembersAsync()`. You can iterate across all pages by using an `await foreach` on the returned resource arrays:
```csharp
var members = await client.GetCampaignMembersAsync(campaignId);
if(members != null)
{
    await foreach(var member in members)
        // your work here
}
```
If you want to manually handle requesting the next pages (for rate limiting requests, for example), you can use the overloads on `PatreonClient` that take page cursors found in the `Meta` property:
```csharp
var members = await client.GetCampaignMembersAsync(campaignId);
if(members != null)
{
    do
    {
        //your work here
        
        string nextPageCursor = members.Meta.Pagination.Cursor?.Next;
        if (nextPageCursor != null)
            members = await client.GetCampaignMembersAsync(campaignId, nextPageCursor);
        else
            members = null;
    }
    while(members != null);
}
```

## Implemented Endpoints

- [ ] Resources
  - [x] /identity
  - [x] /campaigns
  - [x] /campaigns/{campaign_id}
  - [x] /campaigns/{campaign_id}/members
  - [x] /members/{id}
  - [ ] /campaigns/{campaign_id}/posts
  - [ ] /posts/{id}
- [ ] Webhooks
  - [ ] /webhooks 
  - [ ] /webhooks/{id}
