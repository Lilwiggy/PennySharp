using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;

namespace PennySharp
{
    class Program
    {
        // Our little baby boy
        static DiscordClient client;
        // Our commands
        static CommandsNextExtension commands;
        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        static async Task MainAsync(string[] args)
        {
            client = new DiscordClient(new DiscordConfiguration
            {
                // Please for the love of all that is holy don't include this in the GitHub
                Token = "Get your own token :(",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                // LogLevel.Debug for Debugguging info, LogLevel.Error for errors
                LogLevel = LogLevel.Debug
            });

            client.Ready += async e =>
            {
                Console.WriteLine("Awaiting despair...");
                // Set the Activity
                await client.UpdateStatusAsync(new DSharpPlus.Entities.DiscordActivity {
                    Name = "despair",
                    ActivityType = DSharpPlus.Entities.ActivityType.Watching
                }, DSharpPlus.Entities.UserStatus.DoNotDisturb);
            };

            // Our prefixes!
            string[] prefixes = new string[] { "!!" };
            commands = client.UseCommandsNext(new CommandsNextConfiguration
            {
                // Heck off DMs >:(
                EnableDms = false,
                // Mention the bot for commands as well
                EnableMentionPrefix = true,
                // Those prefixes from earlier
                StringPrefixes = prefixes
            });
            // Car salesman: *slaps roof of Client* This bad boy can fit so many fucking command modules in it
            commands.RegisterCommands<TestingCommands>();
            commands.RegisterCommands<StandardCommands>();
            // Connect
            await client.ConnectAsync();

            // I'LL BE WAITING FOR YOU *I'll be waiting I'll be waiting*
            await Task.Delay(-1);
        }
    }
}
