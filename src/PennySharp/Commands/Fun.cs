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
    public class FunCommands : BaseCommandModule
    {
        static Dictionary<ulong, List<string>> WaifuList = new Dictionary<ulong, List<string>>();
        static Dictionary<ulong, List<string>> NSFWList = new Dictionary<ulong, List<string>>();
        static HttpClient httpClient = new HttpClient();
        static int failedAttempts = 0;
        Random rnd = new Random();
        // All my waifus in one place.
        [Command("waifu"), Description("Posts a random waifu!")]
        public async Task Waifu(CommandContext ctx)
        {
            List<string> Waifus = new List<string>();
            string[] emotes = { "<:kawaii:459008931281371156>", "<:blushu:458688271917121537>", "<:gasm:500019753411411969>" };
            using (StreamReader r = new StreamReader("JSON/images.json"))
            {
                string json = await r.ReadToEndAsync();
                dynamic arr = JsonConvert.DeserializeObject(json);
                foreach (string waifu in arr.waifu)
                {
                    Waifus.Add(waifu);
                }
            }
            try
            {
                string img = RandomImage(WaifuList, ctx, Waifus);
                Stream image = await httpClient.GetStreamAsync(img);
                await ctx.Message.Channel.SendFileAsync("waifuImage.png", image, emotes[rnd.Next(emotes.Length)]);
                failedAttempts = 0;

            }
            catch (HttpRequestException)
            {
                if (failedAttempts >= 3)
                {
                    await ctx.Message.Channel.SendMessageAsync("<:sadness:405061263362752523> I failed to get an image 3 times. Sorry about that.");
                    failedAttempts = 0;
                    return;
                }
                failedAttempts++;
                await Waifu(ctx);
            }
        }

        // I know what you're doing here ( ͠° ͟ʖ ͠°)
        [Command("nsfw"), Description("Posts a random NSFW image. Only works in NSFW labeled chats.")]
        public async Task NSFW(CommandContext ctx)
        {
            if (ctx.Channel.IsNSFW)
            {
                List<string> nsfw = new List<string>();
                using (StreamReader r = new StreamReader("JSON/images.json"))
                {
                    string json = await r.ReadToEndAsync();
                    dynamic arr = JsonConvert.DeserializeObject(json);
                    foreach (string waifu in arr.nsfw)
                    {
                        nsfw.Add(waifu);
                    }
                }
                try
                {
                    string img = RandomImage(NSFWList, ctx, nsfw);
                    Stream image = await httpClient.GetStreamAsync(img);
                    await ctx.Message.Channel.SendFileAsync("nsfwImage.png", image);
                    failedAttempts = 0;

                }
                catch (HttpRequestException)
                {
                    if (failedAttempts >= 3)
                    {
                        await ctx.Message.Channel.SendMessageAsync("<:sadness:405061263362752523> I failed to get an image 3 times. Sorry about that.");
                        failedAttempts = 0;
                        return;
                    }
                    failedAttempts++;
                    await NSFW(ctx);
                }
            }
            else
            {
               await ctx.Channel.SendMessageAsync("This can only be done in an NSFW labled chat.");
            }
        }

        [Command("complain"), Description("Complain about Penny.\n\"I see all of these messages so have fun :)\" - Lilwiggy")]
        public async Task Complain(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Please leave a complaint next time.");
        }

        [Command("complain"), Description("Complain about Penny.\n\"I see all of these messages so have fun :)\" - Lilwiggy")]
        public async Task Complain(CommandContext ctx, [RemainingText] string complaint)
        {
            DiscordChannel Channel = await ctx.Client.GetChannelAsync(396008624289349634);
            var embed = new DiscordEmbedBuilder
            {
                Title = "New complaint",
                Color = new DiscordColor("#80ff8a"),
                ThumbnailUrl = ctx.User.AvatarUrl

            };
            embed.AddField($"From {ctx.User.Username} on {ctx.Guild.Name}", $"Channel name: {ctx.Channel.Name}\n" +
                $"Channel ID: {ctx.Channel.Id}\n" +
                $"Author ID: {ctx.User.Id}");
            embed.AddField("Complain:", complaint);
            await Channel.SendMessageAsync("", false, embed);
            await ctx.Channel.SendMessageAsync("Thank you for your complaint. It has been reported to the proper authorities.");
        }

        private string RandomImage(Dictionary<ulong, List<string>> WaifuList, CommandContext ctx, List<string> Waifus)
        {
            string img = Waifus[rnd.Next(Waifus.Count)];
            if (WaifuList.ContainsKey(ctx.Guild.Id))
            {
                if (WaifuList.Count == Waifus.Count)
                {
                    WaifuList.Clear();
                    WaifuList[ctx.Guild.Id].Add(img);
                    return img;
                }
                else if (WaifuList[ctx.Guild.Id].Contains(img))
                {
                    RandomImage(WaifuList, ctx, Waifus);
                }
                else {
                    WaifuList[ctx.Guild.Id].Add(img);
                    return img;
                }
            }
            else
            {
                WaifuList.Add(ctx.Guild.Id, new List<string>());
                WaifuList[ctx.Guild.Id].Add(img);
                return img;
            } return "";
        }
    }
}
