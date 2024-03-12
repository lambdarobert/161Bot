using Discord;
using Discord.Commands;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.Commands
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("cmds")]
        [Alias("help")]
        [Summary("Shows the list of bot commands.")]
        public async Task Run()
        {
            CommandService cmdService = Program.GetServices().GetService(typeof(CommandService)) as CommandService;
            var helpMessage = new StringBuilder();
            foreach(var cmd in cmdService.Commands)
            {
                helpMessage.Append("**").Append(cmd.Name).Append("**").Append(": ").Append(cmd.Summary == null ? "No description exists at this time." : cmd.Summary).Append("\n");
            }
            await ReplyAsync(messageReference: new MessageReference(Context.Message.Id), embed : new EmbedBuilder().WithTitle("Commands").WithDescription(helpMessage.ToString()).Build());
        }
    }
}
