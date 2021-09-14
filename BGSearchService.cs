using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

class BGSearchService {
    private Token _token;
    static readonly HttpClient _client = new HttpClient();

    private async Task<string> GetToken()
    {
        /* Eventual flow:
         *   Check if we have a non-expired token, return it if we do.
         *   If not, use Client ID and Client Secret to fetch one and return that.
         *   Wrap all web requests here and have token management be automatic and private.
         */
        
        // Attach our username:password (the ID and Secret).
        string clientID = Environment.GetEnvironmentVariable("BobBotBlizzardId");
        string clientSecret = Environment.GetEnvironmentVariable("BobBotBlizzardSecret");

        var byteArray = Encoding.ASCII.GetBytes($"{clientID}:{clientSecret}");
        var encodedAuth = Convert.ToBase64String(byteArray);
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedAuth);

        // Add grant_type=client_credentials to post body.
        var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>{new KeyValuePair<string, string>("grant_type", "client_credentials")});

        // Post to battle.net.
        var response = await _client.PostAsync("https://us.battle.net/oauth/token", content);

        // Turn response into a usable object.
        var jsonString = await response.Content.ReadAsStringAsync();
        _token = JsonSerializer.Deserialize<Token>(jsonString);

        return _token.AccessToken;
    }
    
    public async Task<List<Card>> SearchCardsAsync(string searchText)
    {
        Console.WriteLine($"Searching Battlegrounds card list for {searchText}");
        string token = await GetToken();

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var jsonString = await _client.GetStringAsync($"https://us.api.blizzard.com/hearthstone/cards?textFilter={searchText}&locale=en_us&gameMode=battlegrounds&type=minion");
        SearchResults searchResults = JsonSerializer.Deserialize<SearchResults>(jsonString);

        return searchResults.Cards;
    }
}