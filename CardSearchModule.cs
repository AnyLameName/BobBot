using System;
using System.Threading.Tasks;
using Discord.Commands;

public class CardSearchModule : ModuleBase<SocketCommandContext>
{
    [Command("bg")]
    [Summary("Doesn't do much.")]
    public async Task SayAsync([Remainder] [Summary("It's just going to say this is cool.")] string echo)
    {
        await Context.Channel.SendMessageAsync($"{echo} is a cool guy.");
    }
}