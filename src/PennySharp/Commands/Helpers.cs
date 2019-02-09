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

namespace PennySharp.Helpers
{
    class HelpCommandHelper
    {
        public async void HelpCommand(CommandContext ctx, int i, DiscordMessage m)
        {
            var interactivity = ctx.Client.GetInteractivity();
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
            var em = await interactivity.WaitForReactionAsync(e => e == DiscordEmoji.FromUnicode("➡") || e == DiscordEmoji.FromUnicode("⬅"), ctx.User, TimeSpan.FromMinutes(3));
            if (em != null)
            {
                await m.DeleteReactionAsync(em.Emoji, ctx.User);

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
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Official server",
                    Url = "https://discord.gg/kwcd9dq",
                    Color = new DiscordColor(9043849)
                }.WithAuthor($"Page {i + 1}/{Pages}", null, ctx.Client.CurrentUser.AvatarUrl)
                 .WithFooter($"PennyBot | Lilwiggy {DateTime.UtcNow.Year}");
                Console.WriteLine(c.Count);
                Console.WriteLine(i);
                foreach (string s in c[i])
                    embed.AddField(s, Commands.RegisteredCommands.Values.ToList().Find(x => x.Name == s).Description);

                await m.ModifyAsync("", embed: embed.Build());
                HelpCommand(ctx, i, m);
            }
        }
    }
}
