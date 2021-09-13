using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Collections.Generic;

public class CardSearchModule : ModuleBase<SocketCommandContext>
{
    [Command("bg")]
    [Summary("Doesn't do much.")]
    public async Task SayAsync([Remainder] [Summary("Search Battleground minions for cards matching searchText.")] string searchText)
    {
        var searchService = new BGSearchService();
        List<Card> results = await searchService.SearchCardsAsync(searchText);
        var joinedList = String.Join(", ", results);
        await Context.Channel.SendMessageAsync($"Cards matching '{searchText}': {joinedList}.");
    }
}