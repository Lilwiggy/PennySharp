using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord;

public class Tests : ModuleBase
{
    public string RandomImage(string[] images)
    {
        Random rnd = new Random();

        return images[rnd.Next(images.Length)];
    }

    [Command("test"), Summary("Just test shit")]
    public async Task Test()
    {
        await ReplyAsync("Sup faggot");
    }

    [Command("avatar"), Summary("Yip Yip!")]
    public async Task Avatar([Remainder, Summary("Avatars")] IUser user = null)
    {
        var person = user ?? Context.Message.Author;
        string name = "Your";

        if (person != Context.Message.Author)
            name = $"{person.Username}'s";

        await ReplyAsync("", false, new EmbedBuilder() {
            Title = $"{name} avatar",
            ImageUrl = person.GetAvatarUrl(),
            Color = new Color(128, 255, 138)
        });
    }

    [Command("ping"), Summary("Just test shit")]
    public async Task Pong()
    {
        await ReplyAsync("Fuck you");
    }

    [Command("danganronpa"), Summary("Yes")]
    public async Task Danganronpa()
    {
        string[] imgs = {
    "https://cdn.discordapp.com/attachments/151760749918683137/450802650946994176/DANGANRONPA2GD.png",
    "https://cdn.discordapp.com/attachments/349619800147886081/450803294739365908/39e21d46656c19e182d4722160a0d8e1c4eba66f_hq.png",
    "https://is3-ssl.mzstatic.com/image/thumb/Music118/v4/42/bc/66/42bc66a3-d5b5-1041-66ad-ab8a1b469ea4/4580327260159_cover.jpg/268x0w.jpg",
    "https://cdn3.dualshockers.com/wp-content/uploads/2016/10/Danganronpa-1-2-Reload-6.png",
    "https://vignette.wikia.nocookie.net/danganronpa/images/3/32/DR3_Blue_Ray_Box_002.png/revision/latest?cb=20161127185631",
    "https://pop-wrapped.s3-us-west-1.amazonaws.com/articles/93350/danganronpa-the-despair-arc-review-1-lg.jpg"
};
        await ReplyAsync("", false, new Discord.EmbedBuilder()
        {
            Title = "Muh name geoff",

            ImageUrl = RandomImage(imgs)

        });
    }
}

    [Group("dang")]
public class Things : ModuleBase
{
    [Command("man"), Summary("ron")]
    public async Task Pa()
    {
        await ReplyAsync("ron pa");
    }

    [Command("man ron"), Summary("ron")]
    public async Task Ron()
    {
        await ReplyAsync("pa");
    }
}

