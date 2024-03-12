using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace _161Bot.Commands
{
    [ChaoCommand("ping", "Outputs 'pong'.")]
    public class Ping : ChaoSlashCommand
    {
        public async Task Run(SocketSlashCommand cmd)
        {
            await cmd.RespondAsync("Pong!");

        }
    }
}
