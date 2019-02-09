using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;


namespace PennySharp
{
    class ModerationCommands : BaseCommandModule
    {
        static HttpClient httpClient = new HttpClient();

        [Command("ban"), Description("Bans a user")]
        public async Task Ban(CommandContext ctx)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.BanMembers))
            await ctx.Channel.SendMessageAsync("Please mention a valid user.");
            else
            await ctx.Channel.SendMessageAsync("This command is restricted to server mods.");
        }

        [Command("ban"), Description("Bans a user")]
        public async Task Ban(CommandContext ctx, DiscordMember member)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.BanMembers))
            {
                if (member.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
                {
                    await ctx.Channel.SendMessageAsync("I could not ban that user.");
                }
                else
                {
                    Stream image = await httpClient.GetStreamAsync("https://cdn.discordapp.com/attachments/310125753209454593/520794127009447956/tumblr_p2i5iyZYMf1s846hwo1_500.gif");
                    await member.BanAsync();
                    await ctx.Message.Channel.SendFileAsync("ban.gif", image, $"**{member.Username}** was just banned.");
                }
            }
            else {
                await ctx.Channel.SendMessageAsync("This command is restricted to server mods.");
            }

        }

        [Command("kick"), Description("Kicks a user")]
        public async Task Kick(CommandContext ctx)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.BanMembers))
                await ctx.Channel.SendMessageAsync("Please mention a valid user.");
            else
                await ctx.Channel.SendMessageAsync("This command is restricted to server mods.");
        }

        [Command("kick"), Description("Kicks a user")]
        public async Task Kick(CommandContext ctx, DiscordMember member)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.KickMembers))
            {
                if (member.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
                {
                    await ctx.Channel.SendMessageAsync("I could not kick that user.");
                }
                else
                {
                    Stream image = await httpClient.GetStreamAsync("https://1.bp.blogspot.com/-ntNTXe-I8JA/WJJIdY02ndI/AAAAAAAAG68/PUV4C7r4rCEAo-6mXj502Aw71-X2onjtQCLcB/s1600/rwby-coco.gif");
                    await member.RemoveAsync();
                    await ctx.Message.Channel.SendFileAsync("kick.gif", image, $"**{member.Username}** was just kicked.");
                }
            }
            else
            {
                await ctx.Channel.SendMessageAsync("This command is restricted to server mods.");
            }

        }
    }
}
