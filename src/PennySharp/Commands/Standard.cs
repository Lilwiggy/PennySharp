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

        // For those server stats my doggy dude man
        [Command("serverinfo")]
        public async Task ServerInfo(CommandContext ctx)
        {
            var total = ctx.Guild.Members.Where(x => !x.IsBot).Count();
            var online = ctx.Guild.Members.Where(x => !x.IsBot && x.Presence?.Status == UserStatus.Online).Count();
            var dnd = ctx.Guild.Members.Where(x => !x.IsBot && x.Presence?.Status == UserStatus.DoNotDisturb).Count();
            var idle = ctx.Guild.Members.Where(x => !x.IsBot && x.Presence?.Status == UserStatus.Idle).Count();
            var offline = total - online - dnd - idle;
            await ctx.Message.Channel.SendMessageAsync("", embed: new DiscordEmbedBuilder()
            {
                Title = ctx.Guild.Name,
                ThumbnailUrl = ctx.Guild.IconUrl,
                Color = new DiscordColor("89ff89")
            }.AddField("Guild ID", ctx.Guild.Id.ToString(), true)
            .AddField("Member count", $"Total users: {total}\n" +
            $"<:online:499784465145397258> {online}\n" +
            $"<:dnd:499778040147083264> {dnd}\n" +
            $"<:idle:499784448334888960> {idle}\n" +
            $"<:invisible:499784436276133889> {offline}\n" +
            $"<:bot:499786409348038686> {ctx.Guild.Members.Where(x => x.IsBot).Count()}", true)
            .AddField("Owner", $"{ctx.Guild.Owner.Mention} | {ctx.Guild.Owner.Id}")
            .WithFooter($"PennyBot | Lilwiggy {DateTime.UtcNow.Year}"));
        }

    }
}
