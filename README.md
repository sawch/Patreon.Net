## Patreon.Net
A .NET implementation of the Patreon V2 API, targeting **.NET 5** and **.NET 6**.

## Usage
1. Create an application on the [Patreon developer portal](https://www.patreon.com/portal/registration/register-clients) and grab your access token, refresh token and client ID.
2. Create a `PatreonClient` with your tokens and client ID:
```csharp
var client = new PatreonClient("access token", "refresh token", "client id");

var campaigns = await client.GetCampaignsAsync(Includes.All);
foreach(var campaign in campaigns.Data)
    Console.WriteLine($"{campaign.Id} has {campaign.Attributes.PatronCount} patrons");

client.Dispose();
```

## Notes
This implementation isn't fully complete yet but is in a working state (see implemented endpoints below).

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
Certain endpoints return arrays of resources which can be paged, such as `PatreonClient.GetCampaignMembersAsync()`. As I do not yet have a Patreon campaign with enough users to return more than 1 page, I cannot say with 100% certainty that the page cursor implementation is correct at this time.

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
