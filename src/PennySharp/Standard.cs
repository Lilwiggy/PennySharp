using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using PennySharp.Helpers;
using System.Drawing;
using System.IO;

namespace PennySharp
{
    // Extends BaseCommandModule so you can load in commands from the main file
    public class StandardCommands : BaseCommandModule
    {
        // For your own avatar ;)
        [Command("avatar"), Description("Shows your or someone else's avatar")]
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
        [Command("avatar"), Description("Shows your or someone else's avatar")]
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
        [Command("serverinfo"), Description("Nice server stats and info")]
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

        [Command("help"), Description("Exactly what you think it does.")]
        public async Task Help(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();
            int i = 0;
            List<string> Cmds = new List<string>();
            List<List<string>> c = new List<List<string>>();

            var Commands = ctx.Client.GetCommandsNext();
            foreach (KeyValuePair<string, Command> cmd in Commands.RegisteredCommands)
            {
                if (!cmd.Value.IsHidden)
                    Cmds.Add(cmd.Key);
            }
            for (int j = 0; j < Cmds.Count; j += 5)
                c.Add(Cmds.Skip(j).Take(j + 5).ToList());
            var Pages = Math.Ceiling(Cmds.Count / 5.0);
            var embed = new DiscordEmbedBuilder
            {
                Title = "Official server",
                Url = "https://discord.gg/kwcd9dq",
                Color = new DiscordColor(9043849)
            }.WithAuthor($"Page {i + 1}/{Pages}", null, ctx.Client.CurrentUser.AvatarUrl)
             .WithFooter($"PennyBot | Lilwiggy {DateTime.UtcNow.Year}");
            foreach (string s in c[i])
            {
               // Console.WriteLine(s);
               // Console.WriteLine(c[i]);
                embed.AddField(s, Commands.RegisteredCommands.Values.ToList().Find(x => x.Name == s).Description);
            }

            var m = await ctx.Channel.SendMessageAsync("", embed: embed);
            await m.CreateReactionAsync(DiscordEmoji.FromUnicode("⬅"));
            await m.CreateReactionAsync(DiscordEmoji.FromUnicode("➡"));

            var help = new HelpCommandHelper();

            while (true) {
                var em = await interactivity.WaitForReactionAsync(e => e == DiscordEmoji.FromUnicode("➡") || e == DiscordEmoji.FromUnicode("⬅"), ctx.User, TimeSpan.FromMinutes(3));
                if (em == null) break;
                if (em.Emoji == DiscordEmoji.FromUnicode("➡"))
                {
                    if (i == Pages - 1)
                        i = 0;
                    else
                        i++;
                }
                else if (em.Emoji == DiscordEmoji.FromUnicode("⬅"))
                {
                    if (i == 0)
                        i = Convert.ToInt32(Pages) - 1;
                    else
                        i--;
                }
                Console.WriteLine("hi");
                await m.DeleteReactionAsync(em.Emoji, ctx.User);
                var emb = help.GetCommandsEmbed(ctx, i, m);
                Console.WriteLine(emb.Fields[0].Value);
                await m.ModifyAsync("", emb);
            };


        }


        [Command("color"), Description("View a user's color.")]
        public async Task Color(CommandContext ctx)
        {
            var c = ctx.Member.Color;
            await ctx.Channel.SendMessageAsync($"Your role color is **#{c.R.ToString("X2")}{c.G.ToString("X2")}{c.B.ToString("X2")}**");
        }

        [Command("color"), Description("View a user's color.")]
        public async Task Color(CommandContext ctx, [RemainingText] DiscordMember member)
        {
            var c = member.Color;
            await ctx.Channel.SendMessageAsync($"{member.Username}'s role color is **#{c.R.ToString("X2")}{c.G.ToString("X2")}{c.B.ToString("X2")}**");
        }

        [Command("color"), Description("View a user's color.")]
        public async Task Color(CommandContext ctx, string hex)
        {
            Image img = new Bitmap(100, 100);
            Graphics drawing = Graphics.FromImage(img);
            if (hex.StartsWith("#"))
            {
                try
                {
                    drawing.Clear(ColorTranslator.FromHtml(hex));
                }
                catch (Exception)
                {
                    await ctx.Channel.SendMessageAsync("Please use a valid hex code.");
                    return;
                }
            }
            else
            {
                try
                {
                    drawing.Clear(ColorTranslator.FromHtml($"#{hex}"));
                }
                catch (Exception)
                {
                    await ctx.Channel.SendMessageAsync("Please use a valid hex code.");
                    return;
                }
            }

            drawing.Save();
            drawing.Dispose();
            Stream s = new MemoryStream();
            img.Save(s, System.Drawing.Imaging.ImageFormat.Png);
            s.Position = 0;
            await ctx.Channel.SendFileAsync("color.png", s);
        }

        [Command("invite"), Description("Gives an invite link to Penny.")]
        public async Task Invite(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("I'm combat ready! <https://discordapp.com/oauth2/authorize?client_id=309531399789215744&scope=bot&permissions=36809798>");
        }

        [Command("listening"), Description("Shows what you're listening to.")]
        public async Task Listening(CommandContext ctx)
        {
            if (ctx.User.Presence.Activity.ActivityType != ActivityType.ListeningTo && ctx.User.Presence.Activity.Name != "Spotify")
            {
                await ctx.Channel.SendMessageAsync("You aren't listening to anything.");
                return;
            }

                var embed = new DiscordEmbedBuilder
                {
                    Title = ctx.User.Presence.Activity.RichPresence.Details,
                    Description = $"By: {ctx.User.Presence.Activity.RichPresence.State}\nAlbum: {ctx.User.Presence.Activity.RichPresence.LargeImageText}",
                    Color = new DiscordColor("#1ed760"),
                    ThumbnailUrl = ctx.User.Presence.Activity.RichPresence.LargeImage.Url.ToString()
                };
                GetListening(ctx, embed, ctx.Member);
                await ctx.Channel.SendMessageAsync("", false, embed);
        }

        [Command("listening"), Description("Shows what you're listening to.")]
        public async Task Listening(CommandContext ctx, DiscordMember Member)
        {
            if (ctx.User.Presence.Activity.ActivityType != ActivityType.ListeningTo && ctx.User.Presence.Activity.Name != "Spotify")
            {
                await ctx.Channel.SendMessageAsync($"{Member.Username} isn't listening to anything.");
                return;
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = ctx.User.Presence.Activity.RichPresence.Details,
                Description = $"By: {ctx.User.Presence.Activity.RichPresence.State}\nAlbum: {ctx.User.Presence.Activity.RichPresence.LargeImageText}",
                Color = new DiscordColor("#1ed760"),
                ThumbnailUrl = ctx.User.Presence.Activity.RichPresence.LargeImage.Url.ToString()
            };
            GetListening(ctx, embed, Member);
            await ctx.Channel.SendMessageAsync("", false, embed);
        }

        private static void GetListening(CommandContext ctx, DiscordEmbedBuilder embed, DiscordMember Member)
        {
            List<string> album = new List<string>();
            List<string> artist = new List<string>();
            List<string> listening = new List<string>();

            DiscordRichPresence userPresence = Member.Presence.Activity.RichPresence;

            foreach (DiscordMember member in ctx.Guild.Members)
            {
                if (member.Presence == null || member.Presence.Activity == null || member.Presence.Activity.RichPresence == null)
                    continue;
                DiscordRichPresence presence = member.Presence.Activity.RichPresence;

                if (member.Presence.Activity.ActivityType != ActivityType.ListeningTo || member.IsBot || member.Id == Member.Id)
                    continue;

                if (userPresence.Details == presence.Details)
                    listening.Add(member.Username);
                else if (userPresence.LargeImageText == presence.LargeImageText)
                    album.Add(member.Username);
                else if (userPresence.State == presence.State)
                    artist.Add(member.Username);
            }

            if (listening.Count > 0)
            {
                if (listening.Count == 1)
                    embed.AddField($"Listening with {listening.Count} other.", string.Join(", ", listening));
                else
                    embed.AddField($"Listening with {listening.Count} others.", string.Join(", ", listening));
            }
            else if (album.Count > 0)
            {
                if (album.Count > 1)
                    embed.AddField($"{album.Count} others are listening to this album.", string.Join(", ", album));
                else
                    embed.AddField($"{album.Count} other is listening to this album.", string.Join(", ", album));
            }
            else if (artist.Count > 0)
            {
                if (artist.Count > 1)
                    embed.AddField($"{artist.Count} others are listening to this artist.", string.Join(", ", artist));
                else
                    embed.AddField($"{artist.Count} other is listening to this artist.", string.Join(", ", artist));
            }
        }
    }
}
