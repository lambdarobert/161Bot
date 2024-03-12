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
    [ChaoCommand("updateterm", "This is used for the daysremaining command")]
    [ChaoPermissions(new ulong[] { }, 796444864177766400)]
    public class UpdateTerm : ChaoSlashCommand
    {
        public async Task Run(SocketSlashCommand cmd, [ChaoParameter("epoch", "The epoch time for the term to end at")] long epoch, [ChaoParameter("term", "The term to update daysremaining for"), ChaoMultipleChoice("Fall Term=Fall", "Winter Term=Winter", "Spring Term=Spring", "Summer Term=Summer")] string term)
        {
            BotConfig.GetCachedConfig().CurrentTerm = term + " Term";
            BotConfig.GetCachedConfig().CurrentTermEnds = (ulong)epoch;
            BotConfig.SaveConfig(BotConfig.GetCachedConfig());

            await cmd.RespondAsync(embed: new EmbedBuilder().WithColor(Color.Green).WithTitle("Term Info Updated").Build());
        }
    }
}
