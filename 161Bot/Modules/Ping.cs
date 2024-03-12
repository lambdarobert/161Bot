using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace _161Bot.Commands
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Outputs 'pong'.")]
        public async Task Run()
        {
            await ReplyAsync(messageReference: new MessageReference(Context.Message.Id), message: "Pong!");

        }
    }
}
