using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Collections.Generic;

public class CardSearchModule : ModuleBase<SocketCommandContext>
{
    [Command("bg")]
    [Summary("Doesn't do much.")]
    public async Task CardSearchAsync([Remainder] [Summary("Search Battleground minions for cards matching searchText.")] string searchText)
    {
        var searchService = new BGSearchService();
        List<Card> results;
        using (Context.Channel.EnterTypingState())
        {
            results = await searchService.SearchCardsAsync(searchText);
        }

        if(results.Count > 1)
        {
            var joinedList = String.Join(", ", results);
            await Context.Channel.SendMessageAsync($"Cards matching '{searchText}': {joinedList}.");
        }
        else if(results.Count == 1)
        {
            await Context.Channel.SendMessageAsync($"Found a match! You card: '{results[0]}'");
        }
        else
        {
            await Context.Channel.SendMessageAsync($"Sorry, but I couldn't find any matches for '{searchText}'");
        }
    }
}