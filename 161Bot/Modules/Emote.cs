using Discord;
using Discord.Commands;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.Modules
{
    public class EmoteCommand : ModuleBase<SocketCommandContext>
    {
        [Command("emote")]
        [Summary("Do an emote. Usage: !emote <id>")]
        public async Task Run(ulong emoteId)
        {
            if(await Context.Guild.GetEmoteAsync(emoteId) != null)
            {
                var emote = await Context.Guild.GetEmoteAsync(emoteId);
                await ReplyAsync(emote.ToString());
                await Context.Message.DeleteAsync();
            }
            else
            {
                await ReplyAsync(embed: QuickEmbeds.Error("Not a valid emote ID. Use !wtfemojis to see the list of valids."));
                return;
            }
        }
    }
}
