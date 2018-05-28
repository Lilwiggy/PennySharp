using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PennySharp
{
    class Program
    {
        private CommandService commands;
        private CommandServiceConfig config;
        private DiscordSocketClient client;
        private IServiceProvider services;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync ()
        {

            client = new DiscordSocketClient();
            config = new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                ThrowOnError = true,
                DefaultRunMode = RunMode.Async
            };
            commands = new CommandService(config);

            // Events

            client.Log += Log;
            client.Ready += Ready;

            services = new ServiceCollection()
            .BuildServiceProvider();

            await InstallCommands();

            await client.LoginAsync(TokenType.Bot, "Heck you bro");
            await client.StartAsync();

            await Task.Delay(-1);

        }
        
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        private Task Ready()
        {
            client.SetStatusAsync(UserStatus.DoNotDisturb);
            client.SetGameAsync("Not combat ready.");
            return Task.CompletedTask;
        }


        public async Task InstallCommands()
        {
            client.MessageReceived += HandleCommand;

            await commands.AddModulesAsync(Assembly.GetEntryAssembly());

        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            var msg = messageParam as SocketUserMessage;
            if (msg == null) return;

            int argsPos = 0;

            if (!(msg.HasCharPrefix('!', ref argsPos) || msg.HasMentionPrefix(client.CurrentUser, ref argsPos))) return;

            var ctx = new CommandContext(client, msg);

            var res = await commands.ExecuteAsync(ctx, argsPos, services);
            if (!res.IsSuccess)
                Console.WriteLine(res.ErrorReason);
        }


    }
}
