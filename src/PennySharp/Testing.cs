using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace PennySharp
{
    public class TestingCommands : BaseCommandModule
    {
        [Command("test")]
        public async Task Test(CommandContext ctx)
        {
            await ctx.Message.Channel.SendMessageAsync($"Sup bitch ass {ctx.User.Mention}");
        }

        [Command("embed")]
        public async Task Embed(CommandContext ctx)
        {
            string[] args = ctx.Message.Content.Split(' ');
            string name = "";
            if (args.Length == 1)
                name = ctx.Member.Username;
            else
                name = args[1];
            var embed = new DiscordEmbedBuilder
            {
                Title = name,
                Description = "Turns out your name gay",
                ImageUrl = ctx.Member.AvatarUrl
            };
            await ctx.Message.Channel.SendMessageAsync("", embed: embed);
        }
    }
}