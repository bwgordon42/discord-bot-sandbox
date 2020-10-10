using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DevLifeBot.Handlers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace DevLifeBot
{
    class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private DiscordSocketClient _client;

        public Startup(string[] args)
        {
        }

        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Debug });
            _client.Log += LogAsync;

            var Initialize = new Configuration(null);
            string discordToken = Initialize.GetConfig("tokens:discord");   // reads bot token from _config.yml
            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception("Please enter your bot's token into the `config.yaml` file found in the applications root directory.");
            
            await _client.LoginAsync(TokenType.Bot, discordToken);     // Login to discord
            await _client.StartAsync();

            CommandService commandService = new CommandService(new CommandServiceConfig { DefaultRunMode = RunMode.Async });
            commandService.Log += LogAsync;
            Handlers.CommandHandler commandHandler = new Handlers.CommandHandler(_client, commandService, '&');
            commandHandler.AddModule<Modules.CommandModule>();
            commandHandler.InstallCommands();

            ReactionAddedHandler reactionAddedHandler = new ReactionAddedHandler(_client);
            reactionAddedHandler.InstallReactionAddedHandler();

            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            Debug.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }
    }
}
