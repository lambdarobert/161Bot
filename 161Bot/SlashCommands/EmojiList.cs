using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.SlashCommands
{

    [ChaoCommand("wtfemojis", "See a list of available emojis on this server.")]
    public class EmojiList : ChaoSlashCommand
    {
        public async Task Run(SocketSlashCommand cmd)
        {
            StringBuilder sb = new StringBuilder("**Emojis in this Guild**: \n");
            var guild = (cmd.Channel as SocketGuildChannel).Guild;
            foreach (var emote in guild.Emotes)
            {
                sb.Append(emote.Name + " with ID " + emote.Id + "\n");
            }
            await cmd.RespondAsync(sb.ToString(), ephemeral: true);
        }
    }
}
