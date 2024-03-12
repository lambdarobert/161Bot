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
    [ChaoCommand("roleplay", "Does a variety of things")]
    public class CommandRolePlay : ChaoSlashCommand
    {
        public async Task Run(SocketSlashCommand cmd, [ChaoParameter("action", "Which action to perform"), ChaoMultipleChoice("bonk=bonk")] string action, [ChaoParameter("victim", "The victim of your action")] IGuildUser victim)
        {
            switch(action)
            {
                case "bonk":
                    await cmd.RespondAsync(cmd.User.Mention + " bonks " + victim.Mention + ".");
                    break;
                default:
                    await cmd.RespondAsync(embed: QuickEmbeds.Error("Could not find this action."), ephemeral: true);
                    break;
            }
        }
    }
}
