using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Web;

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
            //await Context.Channel.SendMessageAsync($"Found a match! You card: '{results[0]}'");
            var card = results[0];
            var encodedName = HttpUtility.UrlEncode(card.Name);
            var blizzUrl = $"https://playhearthstone.com/en-us/battlegrounds?textFilter={encodedName}&type=minion";
            var embed = new EmbedBuilder {
                Title = card.Name,
                ImageUrl = card.battlegroundsData.Image,
            };
            embed.Url = blizzUrl;
            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }
        else
        {
            await Context.Channel.SendMessageAsync($"Sorry, but I couldn't find any matches for '{searchText}'");
        }
    }
}