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
            var card = results[0];
            // Go get the golden one.
            var goldenCardTask = searchService.GetByIdAsync(card.battlegroundsData.UpgradeId);

            // While that's fetching, send the standard card to the channel.
            await SendCardToChannel(card);

            // Now wait for the golden card to come back, so we can send it to the channel.
            try
            {
                var goldenCard = await goldenCardTask;
                await SendCardToChannel(goldenCard);
            }
            catch(Exception e)
            {
                await Context.Channel.SendMessageAsync("Sorry, something went wrong pulling the golden card. You can probably draw your own conclusions, though.");
            }
        }
        else
        {
            await Context.Channel.SendMessageAsync($"Sorry, but I couldn't find any matches for '{searchText}'");
        }
    }

    private async Task SendCardToChannel(Card card)
    {
        var encodedName = HttpUtility.UrlEncode(card.Name);
        var blizzUrl = $"https://playhearthstone.com/en-us/battlegrounds/{card.Slug}";
        // UpgradeId 0 means no upgrade, so this IS the upgrade. Show it in gold.
        var imageUrl = (card.battlegroundsData.UpgradeId == 0) ? card.battlegroundsData.ImageGold : card.battlegroundsData.Image;
        var name = (card.battlegroundsData.UpgradeId == 0) ? $"{card.Name} (Upgraded)" : card.Name;

        var embed = new EmbedBuilder {
            Title = name,
            ImageUrl = imageUrl,
            Url = blizzUrl,
        };
        await Context.Channel.SendMessageAsync(embed: embed.Build());
    }
}