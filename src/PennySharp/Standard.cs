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
    // Extends BaseCommandModule so you can load in commands from the main file
   public class StandardCommands : BaseCommandModule
    {
        // For your own avatar ;)
        [Command("avatar")]
        public async Task Avatar(CommandContext ctx)
        {
                await ctx.Message.Channel.SendMessageAsync("", embed: new DiscordEmbedBuilder()
                {
                    Title = "Your avatar",
                    ImageUrl = ctx.Member.AvatarUrl,
                    Color = ctx.Member.Color
                });
        }
        // For That other guy's avatar or whatever
        [Command("avatar")]
        public async Task Avatar(CommandContext ctx, [RemainingText] DiscordMember member)
        {
            await ctx.Message.Channel.SendMessageAsync("", embed: new DiscordEmbedBuilder()
            {
                Title = $"{member.Username}'s avatar",
                ImageUrl = member.AvatarUrl,
                Color = member.Color
            });
        }

    }
}
