using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace BobBot
{
    public class Program
    {   
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        private Program()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
            });

            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false,
            });

            _client.Log += Log;
            _commands.Log += Log;
        }

        private Task Log(LogMessage msg)
        {
            // Totally ignoring severity for now. We want to see everything anyway.
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task MainAsync()
        {

            var token = Environment.GetEnvironmentVariable("BobBotDiscordToken");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Log(new LogMessage(LogSeverity.Verbose, "Bot", "Creating CommandHandler"));
            var _handler = new CommandHandler(_client, _commands);
            await _handler.InstallCommands();

            await Task.Delay(-1);
        }
    }
}
