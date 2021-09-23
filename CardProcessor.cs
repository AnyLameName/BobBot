using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Web;
class CardProcessor
{
    private SocketCommandContext _context;
    private BGSearchService _search;

    public CardProcessor(SocketCommandContext context)
    {
        _context = context;
        _search = new BGSearchService();
    }
    public async Task SendCard(Card card)
    {
        switch(card.CardType)
        {
            case 3: // Hero
                await SendHero(card);
                break;
            case 4: // Minion
                await SendMinion(card);
                break;
        }
    }

    private async Task SendHero(Card hero)
    {
        // TODO: Currently just assumes we got something useful in the hero power search.
        Card heroPower = new Card();
        foreach(int childID in hero.Children)
        {
            // TODO: Fetch all children async. No need to go one at a time.
            var child =  await _search.GetByIdAsync(childID);
            if(child.CardType == 10)
            {
                heroPower = child;
                break;
            }
        }
        Embed embed = EmbedHero(hero, heroPower);
        await _context.Channel.SendMessageAsync(embed: embed);
    }

    private async Task SendMinion(Card minion)
    {
        Embed embed = EmbedMinion(minion);

        try
        {
            var goldenMinion =  await _search.GetByIdAsync(minion.BgData.UpgradeId);

            // While that's fetching, send the standard card to the channel.
            var goldenEmbed = EmbedMinion(goldenMinion);
            await _context.Channel.SendMessageAsync(embed: embed);
            await _context.Channel.SendMessageAsync(embed: goldenEmbed);
        }
        catch
        {
            await _context.Channel.SendMessageAsync("Sorry, something went wrong pulling the golden card. You can probably draw your own conclusions, though.");
        }
    }

    private Embed EmbedHero(Card hero, Card heroPower)
    {
        var encodedName = HttpUtility.UrlEncode(hero.Name);
        var imageUrl = heroPower.BgData.Image;
        var name = hero.Name;

        var builder = new EmbedBuilder {
            Title = name,
            ImageUrl = imageUrl,
            Url = $"https://playhearthstone.com/en-us/battlegrounds/{hero.Slug}",
        };

        return builder.Build();
    }

    private Embed EmbedMinion(Card minion)
    {
        var encodedName = HttpUtility.UrlEncode(minion.Name);
        var imageUrl = (minion.IsUpgradedMinion) ? minion.BgData.ImageGold : minion.BgData.Image;
        var name = (minion.IsUpgradedMinion) ? $"{minion.Name} (Upgraded)" : minion.Name;

        var builder = new EmbedBuilder {
            Title = name,
            ImageUrl = imageUrl,
        };
        if(!minion.IsUpgradedMinion)
        {
            builder.Url = $"https://playhearthstone.com/en-us/battlegrounds/{minion.Slug}";
        }

        return builder.Build();
    }
}