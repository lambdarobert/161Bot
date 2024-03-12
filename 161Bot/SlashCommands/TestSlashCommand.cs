using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.SlashCommands
{
    [ChaoCommand("testslashcommand", "testing")]
    [ChaoPermissions(new ulong[] { }, 796444864177766400)]
    public class TestSlashCommand : ChaoSlashCommand
    {
        public async Task Run(SocketSlashCommand cmd, [ChaoParameter("ketamine", "testing param")] string ketamine)
        {
            await cmd.RespondAsync("ketamine", ephemeral: true);
        }
    }
}
