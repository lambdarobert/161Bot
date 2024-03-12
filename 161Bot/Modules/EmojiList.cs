using Discord.Commands;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.Modules
{
    public class EmojiList : ModuleBase<SocketCommandContext>
    {
        [Command("wtfemojis")]
        [Summary("See a list of available emojis on this server.")]
        public async Task Run()
        {
            StringBuilder sb = new StringBuilder("**Emojis in this Guild**: \n");
            var guild = Context.Guild;
            foreach (var emote in guild.Emotes)
            {
                sb.Append(emote.Name + " with ID " + emote.Id + "\n");
            }
            await ReplyAsync(sb.ToString());
        }
    }
}
