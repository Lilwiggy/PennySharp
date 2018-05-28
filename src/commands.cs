using Discord.Commands;
using System.Threading.Tasks;

public class Tests : ModuleBase
{
    [Command("test"), Summary("Just test shit")]
    public async Task Test()
    {
        await ReplyAsync("Sup faggot");
    }

    [Command("ping"), Summary("Just test shit")]
    public async Task Pong()
    {
        await ReplyAsync("Fuck you");
    }

    [Command("danganronpa"), Summary("Yes")]
    public async Task Danganronpa()
    {
        Discord.Embed emb = new Discord.EmbedBuilder() {
            Author = new Discord.EmbedAuthorBuilder() {
                IconUrl = "https://cdn.discordapp.com/avatars/309531399789215744/5d27d9ce0a36adf52d103f8c7c3a9cfa.png?size=2048",
                Name = "Geoff",
                Url = "https://discord.gg/kwcd9dq"
            },

            ImageUrl = "https://vignette.wikia.nocookie.net/danganronpa/images/3/32/DR3_Blue_Ray_Box_002.png/revision/latest?cb=20161127185631"

        };
        await ReplyAsync("", false, emb);
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

