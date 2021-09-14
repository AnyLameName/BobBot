using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using System.Threading.Tasks;
using System;

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;

    public CommandHandler(DiscordSocketClient client, CommandService commands)
    {
        _client = client;
        _commands = commands;
    }

    public async Task InstallCommands()
    {
        _client.MessageReceived += HandleMessageReceived;

        // Apparently we should check out the Dependency Injection Guide for Discord.Net and replace this null with something. We'll come back to that.
        await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
    }

    private async Task HandleMessageReceived(SocketMessage messageParam)
    {
        // Don't process the message if it's a system message. This appears to be an attempt to cast and we check if it works? Feels like we could do better.
        var msg = messageParam as SocketUserMessage;
        if (msg == null) return;

        int argPos = 0;

        // Make sure this is being addressed to us and not from another bot.
        if(!(msg.HasCharPrefix('!', ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
           msg.Author.IsBot)
        {
           return;
        }

        // If we get here then we actually want to process this message as a command. We need a context for it.
        var context = new SocketCommandContext(_client, msg);

        await _commands.ExecuteAsync(context: context, argPos: argPos, services: null);
    }
}